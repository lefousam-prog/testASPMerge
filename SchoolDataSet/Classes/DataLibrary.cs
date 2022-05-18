using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace SchoolDataSet.Classes
{
    public class DataLibrary
    {
        public static bool TestConnection()
        {
            string strConnection = "Data Source=LAPTOP-CHRMOO8V\\SQLEXPRESS; Initial Catalog=StudentApi; User=sa; Password=123456;";
            SqlConnection con = new SqlConnection(strConnection);
            bool status;

            try
            {
                con.Open();
                con.Close();
                status = true;
            }
            catch (Exception)
            {

                status = false;
            }
            finally
            {
                con.Dispose();
            }

            return status;
        }

        public static SqlConnection ConnectionSQL()
        {
            string strConnection = "Data Source=LAPTOP-CHRMOO8V\\SQLEXPRESS; Initial Catalog=StudentApi; User=sa; Password=123456;";
            SqlConnection con = new SqlConnection(strConnection);

            try
            {
                con.Open();

            }
            catch (Exception)
            {
                con.Dispose();
                return null;

            }

            return con;
        }

        public static DataSet GetData(string sqlQuery)
        {
            DataSet getInfo = new DataSet();

            bool isValid = AllowTransaction(sqlQuery);
            if (!isValid)
            {
                return null;
            }

            if (isValid)
            {
                SqlConnection con = Classes.DataLibrary.ConnectionSQL();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(sqlQuery, con);

                try
                {
                    adapter.Fill(getInfo);
                    adapter.Dispose();
                }
                catch (Exception)
                {

                    getInfo = null;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            else
            {
                getInfo = null;
                return getInfo;
            }

            return getInfo;
        }

        public static bool InsertUpdateData(string sqlQuery, bool testMode=false)
        {

            SqlConnection con = Classes.DataLibrary.ConnectionSQL();
            string isProcessed = "";

            if (testMode)
            {
                sqlQuery = " begin tran " + sqlQuery + "rollback tran;";
            }
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                isProcessed = Convert.ToString(cmd.ExecuteNonQuery());
                cmd.Dispose();

                if (isProcessed.Trim() != "")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                return false;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

        public static string GetScalar(string sqlQuery)
        {
            SqlConnection con = Classes.DataLibrary.ConnectionSQL();
            string result = "";


            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                result = Convert.ToString(cmd.ExecuteScalar());
                con.Dispose();
            }
            catch (Exception)
            {

                result = null;
            }
            finally
            {
                con.Close();
            }

            return result;
        }


        public static bool DeleteObject(string stringDelete, bool testMode = false)
        {

            if (testMode)
            {
                stringDelete = " begin tran " + stringDelete + "rollback tran;";
            }

            SqlConnection con = Classes.DataLibrary.ConnectionSQL();
            string result = "";
            try
            {
                
                if (stringDelete.ToLower().Contains("delete from") && stringDelete.ToLower().Contains("where"))
                {
                    SqlCommand cmd = new SqlCommand(stringDelete, con);
                    result = Convert.ToString(cmd.ExecuteNonQuery());
                    cmd.Dispose();
                }
                else
                {
                    result = "";
                }
                if (result.Trim() != "")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                return false;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

        public static bool ExecuteProcedure(string procedureName, int timeOut)
        {
            int rowAffected = 0;

            using (SqlConnection con = ConnectionSQL())
            {
                using (SqlCommand cmd = new SqlCommand(procedureName,con))
                {
                    cmd.CommandTimeout = timeOut;
                    cmd.CommandType = CommandType.StoredProcedure;
                    rowAffected = cmd.ExecuteNonQuery();
                }

                con.Close();
            }

            if (rowAffected != 0) return true; return false;
        }



        public static string ExecuteProcedure(string procedureName, int timeOut, SqlParameter[] sqlParameters)
        {
            int rowAffected = 0;
            string returnedValue = "";

            using (SqlConnection con = ConnectionSQL())
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, con))
                {
                    cmd.CommandTimeout = timeOut;
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (sqlParameters != null)
                    {
                        foreach (SqlParameter parameter in sqlParameters)
                        {
                            cmd.Parameters.Add(parameter);
                        }
                    }
                    rowAffected = cmd.ExecuteNonQuery();

                    foreach (SqlParameter parameter in sqlParameters)
                    {
                        if (parameter.Direction == ParameterDirection.Output)
                        {
                            returnedValue = parameter.Value.ToString();
                        }
                    }
                }

                con.Close();
            }

            if (string.IsNullOrWhiteSpace(returnedValue))
            {
                if (rowAffected != 0) return "true"; return "false";
            }
            else
            {
                return returnedValue;
            }

        }


        public static DataSet GetExecuteProcedure(string procedureName, int timeOut)
        {
            DataSet getInfo = new DataSet();

            using (SqlConnection con = ConnectionSQL())
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, con))
                {
                    cmd.CommandTimeout = timeOut;
                    cmd.CommandType = CommandType.StoredProcedure;



                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;

                    try
                    {
                        adapter.Fill(getInfo);
                        con.Dispose();
                    }
                    catch (Exception)
                    {

                        getInfo = null;
                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                    }

                }

                con.Close();
            }

            return getInfo;
        }


        public static DataSet GetExecuteProcedure(string procedureName, int timeOut, SqlParameter[] sqlParameters)
        {
            DataSet getInfo = new DataSet();

            using (SqlConnection con = ConnectionSQL())
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, con))
                {
                    cmd.CommandTimeout = timeOut;
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (sqlParameters != null)
                    {
                        foreach (SqlParameter parameter in sqlParameters)
                        {
                            cmd.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);
                        }
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;

                    try
                    {
                        adapter.Fill(getInfo);
                        con.Dispose();
                    }
                    catch (Exception)
                    {

                        getInfo = null;
                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                    }

                }

                con.Close();
            }

            return getInfo;
        }


        public static bool AllowTransaction(string sqlString)
        {
            string[] reservedWords = new string[] { "insert ", "update ", "exec ", "drop ", "alter ", "--", "create " };
            bool isValid = true;

            foreach (var item in reservedWords)
            {
                if (sqlString.ToLower().Contains(item))
                {
                    isValid = false;
                    break;
                }
            }

            return isValid;
        }






    }
}