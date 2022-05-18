using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace SchoolDataSet.Classes
{
    public class Library
    {
        public static string valueFromDataTable(DataTable dataTable, string fieldName)
        {
            try
            {
                string value = dataTable.Rows[0][fieldName].ToString();
                if (string.IsNullOrEmpty(value))
                {
                    return string.Empty;
                }
                else
                {
                    return value;
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }


        public static DataTable DataSetToDataTable(DataSet datasetResult)
        {
            DataTable dtResult = null;

            try
            {
                dtResult = datasetResult.Tables[0].Copy();
                return dtResult;
            }
            catch (Exception)
            {
                return dtResult = null;
            }
        }


        public static DataTable DataSetToDataTable(DataSet datasetResult, string tableName)
        {
            DataTable dtResult = null;

            try
            {
                dtResult = datasetResult.Tables[0].Copy();
                dtResult.TableName = tableName;
                return dtResult;
            }
            catch (Exception)
            {
                return dtResult = null;
            }
        }


        public static DataTable DataSetToDataTable(DataSet datasetResult, int tableIndex)
        {
            DataTable dtResult = null;

            try
            {
                dtResult = datasetResult.Tables[tableIndex].Copy();
                return dtResult;
            }
            catch (Exception)
            {
                return dtResult = null;
            }
        }


        public static DataSet CombineDataTables(params DataTable[] dataTables)
        {
            DataSet resultDataSet = new DataSet();

            foreach (DataTable dt in dataTables)
            {
                resultDataSet.Tables.Add(dt);
            }

            resultDataSet.AcceptChanges();
            return resultDataSet;
        }


        public static string ValueFromDataSet(DataSet dataset, int tableIndex, string fieldValue)
        {
            DataTable table = DataSetToDataTable(dataset, tableIndex);
            string valueRetrieved = valueFromDataTable(table, fieldValue);

            return valueRetrieved;
        }

        public static DataTable CreateDataTable(string[] columnNames, string[] values)
        {
            DataTable dataTable = new DataTable();
            int rowsToAdd = values.Length / columnNames.Length;
            dataTable.Clear();

            if (columnNames.Length == (values.Length / rowsToAdd))
            {
                foreach (var columns in columnNames)
                {
                    dataTable.Columns.Add(columns);
                }

                int valueIndex = 0;

                for (int rowIndex = 0; rowIndex < rowsToAdd; rowIndex++)
                {
                    DataRow newRow = dataTable.NewRow();

                    for (int columnIndex = 0; columnIndex < columnNames.Length; columnIndex++)
                    {
                        newRow[columnNames[columnIndex]] = values[valueIndex];
                    }

                    dataTable.Rows.Add(newRow);
                }

                return dataTable;
            }
            else
            {
                return null;
            }
        }

        public static DateTime YearValidation(DateTime date)
        {
            string newDate = string.Empty;
            string stringDate;
            int yearTwoDigit = Convert.ToInt32(Convert.ToString(date.Year).Substring(0, 2));
            int CurrentYear = Convert.ToInt32(DateTime.Now.Year.ToString().Substring(0, 2));

            if (yearTwoDigit >= CurrentYear)
            {
                stringDate = date.ToShortDateString();
                newDate = stringDate.Replace(stringDate.Substring(5, 2), "20");
                return Convert.ToDateTime(stringDate);
            }
            else
            {
                stringDate = date.ToShortDateString();
                newDate = stringDate.Replace(stringDate.Substring(5, 2), "19");
                return Convert.ToDateTime(stringDate);
            }
        }

        public static DataSet CreateDataSet(string[] headers, string[] values)
        {
            DataTable dataTable = CreateDataTable(headers, values);
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);

            return dataSet;
        }


        public static string ValueFromDataRow(DataRow row, string fieldName)
        {
            string value;

            try
            {
                value = row[fieldName].ToString();

                if (value.Equals(null))
                {
                    return "";
                }
            }
            catch (Exception)
            {
                return "";
            }

            return value;
        }

        public static int CountLetters(string stringText)
        {
            int count = 0;
            string currentChar = "";

            for (int c = 0; c < stringText.Length; c++)
            {
                currentChar = stringText.Substring(c, 1);

                if (Regex.IsMatch(currentChar, @"^[a-zA-Z]+$"))
                {
                    count++;
                }
            }

            return count;
        }
    }
}