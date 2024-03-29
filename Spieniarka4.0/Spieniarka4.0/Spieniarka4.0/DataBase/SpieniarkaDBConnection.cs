﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace SpieniarkaApplication.DataBase
{
    public class SpieniarkaDBConnection
    {
     
            public NpgsqlConnection session;
            private static string serverAddress;
            private static string port;
            private static string login;
            private static string password;
            private static string dataBaseName;
            private readonly ISpieniarkaLogger myLogger;

            public SpieniarkaDBConnection(ISpieniarkaLogger myLogger)
            {
                this.myLogger = myLogger;
            }
                          
            public void OpenSession()
            {
                try
                {
                    serverAddress = ConfigurationManager.AppSettings["Address"];
                    port = ConfigurationManager.AppSettings["Port"];
                    login = ConfigurationManager.AppSettings["Login"];
                    password = ConfigurationManager.AppSettings["Password"];
                    dataBaseName = ConfigurationManager.AppSettings["DataBaseName"];
                    var connectionString = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};", serverAddress, port, login, password,
                        dataBaseName);
                    myLogger.logger.DebugFormat("ConnectionString to DB: {0}", connectionString);
                        session = new NpgsqlConnection(connectionString);
                        session.Open();
            }
                catch (Exception e)
                {
                    myLogger.logger.ErrorFormat("You have a problem with connection to your DB or your connection configuration to DB is wrong: {0}", e.Message);
                    throw;
                }
            }
        }
    
}
