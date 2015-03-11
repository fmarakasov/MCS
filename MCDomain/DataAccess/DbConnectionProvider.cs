using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Devart.Data.Oracle;
using MCDomain.DataAccess;
using MCDomain.Common;
using CommonBase;

namespace MCDomain.DataAccess
{

    public class OracleDataSource
    {
        public  string InstanceName{ get; private set;}
        public  string ServerName{ get; private set;}
        public  string ServiceName{ get; private set;}
        public  string Protocol{ get; private set;}
        public  string Port{ get; private set;}

        public OracleDataSource(string instance, string server, string service, string protocol, string port)
        {
            InstanceName = instance;
            ServerName = server;
            ServiceName = service;
            Protocol = protocol;
            Port = port;
        }
    
    }

    public class OracleConnectionProvider: ILoginProvider
    {        
        public static readonly OracleConnectionProvider Instance = new OracleConnectionProvider();

        private OracleConnection _connection;

        public IQueryLoginProvider QueryLoginProvider { get; set; }

        public OracleConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = new OracleConnection();
                    
                }
                return _connection;
            }
        }

        

        /// <summary>
        /// Полукает коллекцию клиентов Oracle
        /// </summary>
        public OracleHomeCollection OracleHomes
        {
            get { return OracleConnection.Homes; }
        }

        /// <summary>
        /// Получает клиента Oracle по умолчанию
        /// </summary>
        public OracleHome OracleDefaultHome
        {
            get
            {                
                Contract.Requires(OracleHomes != null, "Свойство OracleHomes не должно быть null.");                
                return OracleHomes.DefaultHome;
            }
        }

        /// <summary>
        /// Получает источники данных клиента по умолчанию Oracle
        /// </summary>        
        public IEnumerable<OracleDataSource> OracleDataSources
        {
            get
            {
                OracleHome home = OracleDefaultHome;
                Contract.Assert(home != null, "Свойство OracleDefaultHome не должно быть null.");                
                return GetOracleDataSources(home.Name);
            }
        }
        /// <summary>
        /// Получает коллекцию источников данных для заданного клиента Oracle
        /// </summary>
        /// <param name="homeName">Имя клиента Oracle</param>
        /// <returns>Коллекция источников данных</returns>        
        public IEnumerable<OracleDataSource> GetOracleDataSources(string homeName)
        {
            Contract.Requires(!string.IsNullOrEmpty(homeName), "Праметр homeName не может быть null.");            

            var dsEnum = new OracleDataSourceEnumerator();
            
            DataRowCollection rows = dsEnum.GetDataSources(homeName).Rows;
            
            Contract.Assume(rows != null);

            var result = (from DataRow row in rows
                          select new OracleDataSource(row["InstanceName"].ToString(), row["ServerName"].ToString(), row["ServiceName"].ToString(), row["Protocol"].ToString(), row["Port"].ToString())).ToList();
            
            return result.AsReadOnly();

        }
    

        public bool Connect(string userName, string password, string serverName, out Exception exception)
        {
            Connection.UserId = userName;
            Connection.Password = password;
            Connection.Server = serverName;
            try
            {
                Connection.Open();
                exception = null;
                return true;
            }
            catch (Exception e)
            {
                exception = e;
                return false;
            }
            
        }
    }
}
