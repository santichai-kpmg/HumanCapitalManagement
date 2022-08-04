using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
//using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace HumanCapitalManagement.Models.OldTable
{
    public class DatabaseConnectionSQLBase
    {
        private SqlConnection Conn;
        private SqlCommand Cmd;// = new SqlCommand();

        public DatabaseConnectionSQLBase()
        {
            Cmd = new SqlCommand();
            Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ToString());
            Cmd.Connection = Conn;
        }
        public DatabaseConnectionSQLBase(string strConn)
        {
            Cmd = new SqlCommand();
            Conn = new SqlConnection(strConn);
            Cmd.Connection = Conn;
        }

        public string ConnectionString
        {
            get { return Cmd.Connection.ConnectionString; }
            set { Cmd.Connection.ConnectionString = value; }
        }

        public void AddParameter(string var, string val, SqlDbType datatype)
        {
            Cmd.Parameters.Add(var, datatype).Value = val;
        }

        public int ExecuteNonQuery(string sql)
        {
            try
            {
                Cmd.Connection.Open();
                Cmd.CommandText = sql;
                int rowsAffect = Cmd.ExecuteNonQuery();
                Cmd.Connection.Close();
                return rowsAffect;
            }
            catch//(Exception e)
            {
                return -1;
            }
        }

        public SqlDataReader getDataReader(string sql)
        {
            try
            {
                SqlDataReader reader = null;
                Cmd.Connection.Open();
                Cmd.CommandText = sql;
                reader = Cmd.ExecuteReader();
                Cmd.Connection.Close();
                return reader;
            }
            catch
            {
                return null;
            }
        }

        public DataSet getDataSet(string sql)
        {
            try
            {
                Cmd.Connection.Open();
                SqlDataAdapter da = new SqlDataAdapter(sql, Cmd.Connection);
                DataSet ds = new DataSet();
                da.Fill(ds);
                Cmd.Connection.Close();
                return ds;
            }
            catch
            {
                return null;
            }
        }

        public object getDataScalar(string sql)
        {
            try
            {
                Cmd.CommandText = sql;
                Cmd.Connection.Open();
                object val = Cmd.ExecuteScalar();
                Cmd.Connection.Close();
                return val;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}