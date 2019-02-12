using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SQLite;
using System.IO;

namespace S7_Dimat.Class
{
    public class DBLite
    {
        public SQLiteConnection Connection = new SQLiteConnection("Data Source = " + Path.Combine(Global.ExeDirectory, Global.LocalDBName) + "; Version = 3;");
        public SQLiteCommand Command;
        private List<SQLiteParameter> Parameters = new List<SQLiteParameter>();
        public SQLiteDataReader dr;

        public string query;

        public DBLite(string query)
        {
            this.query = query;
            this.Command = new SQLiteCommand(this.query, this.Connection);
        }

        public void AddParameter(string name, object value, DbType type)
        {
            SQLiteParameter parameter = new SQLiteParameter("@" + name, type);
            parameter.Value = value;
            this.Parameters.Add(parameter);
        }

        public void SendParameters()
        {
            if (this.Parameters.Count > 0)
            {
                this.Command.Parameters.AddRange(this.Parameters.ToArray());
            }
        }

        public void Exec()
        {
            SendParameters();
            using (SQLiteConnection c = this.Connection)
            {
                c.Open();
                using (SQLiteCommand cmd = this.Command)
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public SQLiteDataReader ExecReader()
        {
            SendParameters();
            this.Connection.Open();
            return this.Command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public DataTable ExecTable()
        {
            SendParameters();
            using (SQLiteConnection c = this.Connection)
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(this.query, c))
                {
                    DataTable dt = new DataTable();
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    da.Fill(dt);
                    return dt;
                }
            }
        }
    }
}