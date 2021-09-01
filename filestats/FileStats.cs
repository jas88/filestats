using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using Microsoft.Data.Sqlite;

namespace fileStats
{
    static class FileStats
    {
        public static void Scan(FileSystem fs = null)
        {
            fs ??= new FileSystem();
            var cwd = fs.Directory.GetCurrentDirectory();

            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "fileStats", "cache.db");
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            var dbs = new SqliteConnectionStringBuilder
            {
                DataSource = path
            };
            using var db = new SqliteConnection(dbs.ConnectionString);
            db.Open();
            using (var ct = new SqliteCommand("CREATE TABLE IF NOT EXISTS dirCache (parent STRING,name STRING,num INT,vol INT,seen INT,PRIMARY KEY(parent,name))", db))
            {
                ct.ExecuteNonQuery();
            }

            using var trans = db.BeginTransaction(IsolationLevel.Serializable);
            using (var setAllSeen = new SqliteCommand("UPDATE dirCache SET seen=0 WHERE parent=?", db, trans))
                setAllSeen.Parameters.AddWithValue(null, cwd);
            using var setSeen = new SqliteCommand("UPDATE dirCache SET seen=1 WHERE parent=@parent AND name=@name", db, trans);
            using var populate = new SqliteCommand("INSERT INTO dirCache (parent,name,num,vol,seen) VALUES (@parent,@name,@num,@vol,1)", db, trans);
            setSeen.Parameters.AddWithValue("@parent", cwd);
            setSeen.Parameters.Add("@name", SqliteType.Text);
            populate.Parameters.AddWithValue("@parent", cwd);
            populate.Parameters.Add("@name", SqliteType.Text);
            populate.Parameters.Add("@num", SqliteType.Integer);
            populate.Parameters.Add("@vol", SqliteType.Integer);
            foreach (var dir in fs.Directory.GetDirectories(cwd))
            {
                setSeen.Parameters["@name"] = new SqliteParameter("@name",dir);
                if (setSeen.ExecuteNonQuery() != 0) continue;
                long num=0,vol=0;

                // Cache miss: find the size the hard way, then cache it
                foreach (var f in fs.Directory.EnumerateFiles(Path.Combine(cwd, dir), "*", SearchOption.AllDirectories))
                {
                    var info = fs.FileInfo.FromFileName(f);
                    num++;
                    vol += info.Length;
                }

                populate.Parameters["@name"] = new SqliteParameter("@name", dir);
                populate.Parameters["@num"] = new SqliteParameter("@num", num);
                populate.Parameters["@vol"] = new SqliteParameter("@vol", vol);
                populate.ExecuteNonQuery();
            }

            // Now expire old entries and report the totals:
            using (var cleanup = new SqliteCommand("DELETE FROM dirCache WHERE parent=@parent AND seen=0",db,trans))
            {
                cleanup.Parameters.AddWithValue("@parent", cwd);
                Console.WriteLine($"Expired {cleanup.ExecuteNonQuery()} old cache entries");
            }

            using (var stats =
                new SqliteCommand("SELECT SUM(num),SUM(vol) FROM dirCache WHERE parent=@parent", db, trans))
            {
                stats.Parameters.AddWithValue("@parent", cwd);
                var r=stats.ExecuteReader();
                r.Read();
                Console.WriteLine($"Found {r.GetInt64(0)} files, total {r.GetInt64(1)} bytes ({BytesToString(r.GetInt64(1))})");
            }
            trans.Commit();
        }

        static string BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            var bytes = Math.Abs(byteCount);
            var place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            var num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return $"{(Math.Sign(byteCount) * num).ToString(CultureInfo.InvariantCulture)}{suf[place]}";
        }

        static void Main(string[] args)
        {
            Scan();
        }
    }
}
