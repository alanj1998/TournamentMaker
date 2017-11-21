using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;

namespace AlanJLibraries
{
    /*
     *  Abstract class used for Database Connection
     *  It only requires the ErrorMessage query method to be implemented
     *  To implement the method:
     *      -Check for these messages and react:
     *      1)noQry - You have not added the Query to the object Dictionary, add and proceed
     *      2)deleteQry - Can't delete the query as it doesn't exist in the Dictionary! No action needed
     *  
     *  Copyright(C) Alan Jachimczak
     */
    public abstract class Database_Connection
    {
        private string ConnectionString;
        private Dictionary<string, string> Queries = new Dictionary<string, string>();

        //On creation provide the connection string
        public Database_Connection(string ConnectionString)
        {
            this.ConnectionString = ConnectionString;
        }

        /*
         *  The query used to run queries.
         *  If the query is a select it will return a datatable with the results
         *  If it's an insert, delete or update it will return null.
         *  The query MUST be added prior to running it
         */
        public DataTable RunQuery(string queryName = "")
        {
            DataTable data = new DataTable();

            using (SqlConnection conn = new SqlConnection(this.ConnectionString))
            {
                conn.Open();
                
                if(queryName != "")
                {
                    if (Queries.ContainsKey(queryName))
                    {
                        SqlCommand qry = new SqlCommand(Queries[queryName], conn);
                        SqlDataReader reader = null;
                        reader = qry.ExecuteReader();

                        if(reader.HasRows == true)
                        {
                            data.Load(reader);
                            return data;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        ErrorMessage("noQry");
                        return null;
                    }
                }
                else
                {
                    ErrorMessage("noQry");
                    return null;
                }
            }
        }

        /*
         *  Method used to add queries
         *  Must provide name and query
         */
        public void AddQuery(string name, string query)
        {
            Queries.Add(name, query);
        }

        /* 
         *  Deleting queries in the Dictionary
         */
        public void DeleteQuery(string name)
        {
            if (Queries.ContainsKey(name))
            {
                Queries.Remove(name);
            }
            else
            {
                ErrorMessage("deleteQry");
            }
        }

        /*
         *  Error message method that is to be implemented in your class
         *   -Check for these messages and react(switch, if statements etc):
         *      1)noQry - You have not added the Query to the object Dictionary, add and proceed
         *      2)deleteQry - Can't delete the query as it doesn't exist in the Dictionary! No action needed 
         */
        public abstract void ErrorMessage(string ErrorType);
    }
}
