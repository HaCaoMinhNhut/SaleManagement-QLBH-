﻿using System.Configuration;
using System.Data.SqlClient;

namespace DAL
{
    public class DBConnect
    {
        static string connstr = ConfigurationManager.ConnectionStrings["SalesManagement"].ToString();
        protected SqlConnection _conn = new SqlConnection(connstr);
    }
}
