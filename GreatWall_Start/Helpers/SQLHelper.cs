﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;

namespace LionKing.Helpers
{
    public class SQLHelper
    {
        private const string strDBServer = "Server=tcp:greatwall-db-server1.database.windows.net,1433;" +
                                    "Initial Catalog=GreatWallDB;" +
                                    "Persist Security Info=False;" +
                                    "User ID=minsu;" +
                                    "Password=rkdzxzx123!;" +
                                    "MultipleActiveResultSets=False;" +
                                    "Encrypt=True;" +
                                    "TrustServerCertificate=False;" +
                                    "Connection Timeout=30;";

        public static DataSet RunSQL(string strQuery)
        {
            SqlConnection DB_CON = new SqlConnection(strDBServer);
            SqlCommand DB_Query = new SqlCommand(strQuery, DB_CON);
            SqlDataAdapter DB_Adapter = new SqlDataAdapter(DB_Query);

            DataSet DB_DS = new DataSet();
            DB_Adapter.Fill(DB_DS);

            return DB_DS;
        }

        public static void ExecuteNonQuery(string strQuery, SqlParameter[] para)
        {
            SqlConnection DB_CON = new SqlConnection(strDBServer);
            SqlCommand DB_Query = new SqlCommand(strQuery, DB_CON);
            DataSet DB_DS = new DataSet();

            DB_Query.Parameters.Clear();

            foreach(SqlParameter p in para)
            {
                DB_Query.Parameters.Add(p);
            }

            DB_CON.Open();
            DB_Query.ExecuteNonQuery();
            DB_CON.Close();
        }
    }
}