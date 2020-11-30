using System;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ParsianOffice
{
    public class ConnectionStringClass
    {

        public static string ServerName;
        public static string InctancName;
        public static string DatabaseName;
        public static string Auth;
        public static string User;
        public static string Pass;

        public static bool IsConnected;

        public static string GetConnectionString()
        {
            try
            {
                SqlConnectionStringBuilder conStr = new SqlConnectionStringBuilder();
                EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();

                string user = "NULL";
                string pass = "NULL";
                string appStartPath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

                string serverName = new AMS.Profile.Ini(appStartPath + "\\Connection Configs.ini").GetValue("Database", "ServerName").ToString();
                string inctancName = new AMS.Profile.Ini(appStartPath + "\\Connection Configs.ini").GetValue("Database", "InctancName").ToString();
                string databaseName = new AMS.Profile.Ini(appStartPath + "\\Connection Configs.ini").GetValue("Database", "DatabaseName").ToString();
                string auth = new AMS.Profile.Ini(appStartPath + "\\Connection Configs.ini").GetValue("Database", "Auth").ToString();

                string connectionString = "";

                if (auth == "Windows Authentication")
                {
                    conStr.DataSource = serverName + "\\" + inctancName;
                    conStr.InitialCatalog = databaseName;
                    conStr.IntegratedSecurity = true;
                    conStr.ConnectTimeout = 5;
                    conStr.LoadBalanceTimeout = 5;
                }
                else
                {
                    user = new AMS.Profile.Ini(appStartPath + "\\Connection Configs.ini").GetValue("Database", "User").ToString();
                    pass = new AMS.Profile.Ini(appStartPath + "\\Connection Configs.ini").GetValue("Database", "Password").ToString();

                    conStr.DataSource = serverName + "\\" + inctancName;
                    conStr.InitialCatalog = databaseName;
                    conStr.IntegratedSecurity = false;
                    conStr.UserID = user;
                    conStr.Password = pass;
                }

                entityBuilder.Provider = "System.Data.SqlClient";
                entityBuilder.ProviderConnectionString = conStr.ToString();
                entityBuilder.Metadata = @"res://*/DatabaseModel.csdl|res://*/DatabaseModel.ssdl|res://*/DatabaseModel.msl";

                ServerName = serverName;
                InctancName = inctancName;
                DatabaseName = databaseName;
                Auth = auth;
                User = user;
                Pass = pass;

                connectionString = entityBuilder.ToString();
                return connectionString;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string GetADOConnectionString()
        {
            string conStr = "";

            GetConnectionString();

            conStr = "Data Source=" + ConnectionStringClass.ServerName + "\\" + ConnectionStringClass.InctancName + ";Initial Catalog=" + ConnectionStringClass.DatabaseName;

            if (ConnectionStringClass.Auth == "SQL Authentication")
            {
                conStr += ";User ID=" + ConnectionStringClass.User + ";Password=" + ConnectionStringClass.Pass;
            }
            else
            {
                conStr += ";Integrated Security=True";
            }

            conStr += ";timeout=3;";

            return conStr;
        }

        public static string GetADOConnectionString(int Timing)
        {
            string conStr = "";

            GetConnectionString();

            conStr = "Data Source=" + ConnectionStringClass.ServerName + "\\" + ConnectionStringClass.InctancName + ";Initial Catalog=" + ConnectionStringClass.DatabaseName+ "; MultipleActiveResultSets=true";

            if (ConnectionStringClass.Auth == "SQL Authentication")
            {
                conStr += ";User ID=" + ConnectionStringClass.User + ";Password=" + ConnectionStringClass.Pass;
            }
            else
            {
                conStr += ";Integrated Security=True";
            }

            conStr += ";timeout=" + Timing + ";";

            return conStr;
        }

        public static void SetConnectionString(string Authentication, string ServerName, string InstancName, string DatabaseName, string Username, string Password)
        {
            string appStartPath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

            new AMS.Profile.Ini(appStartPath + "\\Connection Configs.ini").SetValue("Database", "User", Username);
            new AMS.Profile.Ini(appStartPath + "\\Connection Configs.ini").SetValue("Database", "Password", Password);
            new AMS.Profile.Ini(appStartPath + "\\Connection Configs.ini").SetValue("Database", "Auth", Authentication);
            new AMS.Profile.Ini(appStartPath + "\\Connection Configs.ini").SetValue("Database", "ServerName", ServerName);
            new AMS.Profile.Ini(appStartPath + "\\Connection Configs.ini").SetValue("Database", "InctancName", InstancName);
            new AMS.Profile.Ini(appStartPath + "\\Connection Configs.ini").SetValue("Database", "DatabaseName", DatabaseName);
        }

        public static bool TestConnection()
        {
            try
            {
                GetConnectionString();

                string conStr = "Data Source=" + ConnectionStringClass.ServerName + "\\" + ConnectionStringClass.InctancName + ";Initial Catalog=" + ConnectionStringClass.DatabaseName;

                if (ConnectionStringClass.Auth == "SQL Authentication")
                {
                    conStr += ";User ID=" + ConnectionStringClass.User + ";Password=" + ConnectionStringClass.Pass;
                }
                else
                {
                    conStr += ";Integrated Security=True";
                }

                conStr += ";timeout=3;";

                SqlConnection con = new SqlConnection(conStr);
                con.Open();
                con.Close();
                con.Dispose();

                IsConnected = true;

                return true;
            }
            catch (Exception ex)
            {
                IsConnected = false;
                return false;
            }
        }
        public static bool TestConnection(string serverName, string Inctance, string databaseName, string Auth, string User, string Pass)
        {
            try
            {
                GetConnectionString();

                string conStr = "Data Source=" + serverName + "\\" + Inctance + ";Initial Catalog=" + databaseName;

                if (Auth == "SQL Authentication")
                {
                    conStr += ";User ID=" + User + ";Password=" + Pass;
                }
                else
                {
                    conStr += ";Integrated Security=True";
                }

                conStr += ";timeout=3;";

                SqlConnection con = new SqlConnection(conStr);
                con.Open();
                con.Close();
                con.Dispose();

                IsConnected = true;

                return true;
            }
            catch (Exception ex)
            {
                IsConnected = false;
                return false;
            }
        }
    }
}