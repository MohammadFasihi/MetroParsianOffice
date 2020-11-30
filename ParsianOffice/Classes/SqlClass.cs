using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsianOffice.Classes
{
    class SqlClass
    {
        private SqlConnection con;
        private SqlCommand cmd;

        private DataTable dt;
        private SqlDataAdapter da;
        public string ConnectionString { get; set; }
        
        public void OpenConnection()
        {
            con = new SqlConnection(ConnectionString);
            con.Open();
        }


        public void CloseConnection()
        {
            con.Close();
            con.Dispose();
        }

        public void Command(string Query, CommandType Type)
        {
            cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = Query;
            cmd.CommandType = Type;
        }

        public void Command(string Query, CommandType Type, string Date)
        {
            cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = Query;
            cmd.Parameters.AddWithValue("@date", Date);
            cmd.CommandType = Type;
        }

        public void Command(string Query, CommandType Type, int? ID)
        {
            cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = Query;
            cmd.Parameters.AddWithValue("@vt_ID", ID);
            cmd.CommandType = Type;
        }

        public void Command(string Query, CommandType Type, int? ID, string ParameterName)
        {
            cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = Query;
            cmd.Parameters.AddWithValue(ParameterName, ID);
            cmd.CommandType = Type;
        }
        public DataTable ExecuteQuery()
        {
            dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            return dt;
        }

        public DataTable ExecuteQuery(string tableName)
        {
            dt = new DataTable(tableName);
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            return dt;
        }
    }
}
