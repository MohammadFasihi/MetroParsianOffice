using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA
{
    public class SqlAdoClass
    {
        static SqlConnection con;
        static SqlCommand cmd;

        static DataTable dt;
        static SqlDataAdapter da;
        public static string ConnectionString { get; set; }

        public static void OpenConnection()
        {
            con = new SqlConnection(ConnectionString);
            con.Open();
        }


        public static void CloseConnection()
        {
            con.Close();
            con.Dispose();
        }

        public static void Command(string Query, CommandType Type)
        {
            cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = Query;
            cmd.CommandType = Type;
        }

        public static void Command(string Query, CommandType Type,string Date)
        {
            cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = Query;
            cmd.Parameters.AddWithValue("@date", Date);
            cmd.CommandType = Type;
        }

        public static void Command(string Query, CommandType Type, int? ID)
        {
            cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = Query;
            cmd.Parameters.AddWithValue("@vt_ID", ID);
            cmd.CommandType = Type;
        }

        public static void Command(string Query, CommandType Type, int? ID,string ParameterName)
        {
            cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = Query;
            cmd.Parameters.AddWithValue(ParameterName, ID);
            cmd.CommandType = Type;
        }
        public static DataTable ExecuteQuery()
        {
            dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            return dt;
        }

        public static DataTable ExecuteQuery(string tableName)
        {
            dt = new DataTable(tableName);
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            return dt;
        }
    }
}
