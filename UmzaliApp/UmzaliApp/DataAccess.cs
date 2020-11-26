using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UmzaliApp
{
    class DataAccess
    {
        private SqlConnection conn;
        private SqlCommand cmd;
        private SqlDataAdapter adap;
        private OptionToParamModel _model;
        private string connectionString = "Data Source = DESKTOP-EJ3OT5A\\SQLEXPRESS; Initial Catalog = testSSDB; Integrated Security = True";

        public DataAccess()
        {
            _model = new OptionToParamModel();
            conn = new SqlConnection(connectionString);
        }


        public void CreateMajorPlant(string plantNo, string serial, string mach, string model, string desc, string tireFront, int quanFront, string tireRear, int quanRear)
        {
            using (conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("MajorPlantsInsert", conn);
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@plantNo", SqlDbType.NVarChar).Value = plantNo;
                    cmd.Parameters.Add("@serialNo", SqlDbType.NVarChar).Value = serial;
                    cmd.Parameters.Add("@machineMake", SqlDbType.NVarChar).Value = mach;
                    cmd.Parameters.Add("@model", SqlDbType.NVarChar).Value = model;
                    cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = desc;
                    cmd.Parameters.Add("@tireSizeFront", SqlDbType.NVarChar).Value = tireFront;
                    cmd.Parameters.Add("@quantityFront", SqlDbType.Int).Value = quanFront;
                    cmd.Parameters.Add("@tireSizeRear", SqlDbType.NVarChar).Value = tireRear;
                    cmd.Parameters.Add("@quantityRear", SqlDbType.Int).Value = quanRear;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
        public void CreateSmallPlant(string plantNo, string serial, string mach, string model, string desc) //Creates a new entry in the small plants table
        {

            //conn = new SqlConnection(@"Data Source = DESKTOP-EJ3OT5A\SQLEXPRESS; Initial Catalog = testSSDB; Integrated Security = True");

            using (conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    cmd = new SqlCommand("SmallPlantsInsert", conn);
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@plantNo", plantNo);
                    cmd.Parameters.AddWithValue("@serialNo", serial);
                    cmd.Parameters.Add("@machineMake", SqlDbType.NVarChar).Value = mach;
                    cmd.Parameters.Add("@model", SqlDbType.NVarChar).Value = model;
                    cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = desc;

                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
        public void CreateOilEntry(string oilType, DateTime date, double liters, string orderNo, string artisan)
        {
            using (conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    cmd = new SqlCommand("OilDetailsInsert", conn);
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@oilType", SqlDbType.NVarChar).Value = oilType;
                    cmd.Parameters.Add("@date", SqlDbType.Date).Value = date;
                    cmd.Parameters.Add("@liters", SqlDbType.Float).Value = liters;
                    cmd.Parameters.Add("@orderNo", SqlDbType.NVarChar).Value = orderNo;
                    cmd.Parameters.Add("@artisan", SqlDbType.NVarChar).Value = artisan;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

        public void CreateNewOrder(int orderID, string companyID, string plantNo, DateTime dateRequired, string requestedBy, string desc, int machTime, string type, double total, string completedBy)
        {
            using (conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    cmd = new SqlCommand("OrderInsert", conn);
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@orderID", SqlDbType.SmallInt).Value = orderID;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.NVarChar).Value = companyID;
                    cmd.Parameters.Add("@plantNo", SqlDbType.NVarChar).Value = plantNo;
                    cmd.Parameters.Add("@dateRequired", SqlDbType.Date).Value = dateRequired;
                    cmd.Parameters.Add("@requestedBy", SqlDbType.NVarChar).Value = requestedBy;
                    cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = desc;
                    cmd.Parameters.Add("@machineTime", SqlDbType.Time).Value = machTime;
                    cmd.Parameters.Add("@type", SqlDbType.NVarChar).Value = type;
                    cmd.Parameters.Add("@total", SqlDbType.Money).Value = total;
                    cmd.Parameters.Add("@completedBy", SqlDbType.NVarChar).Value = completedBy;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

        public void CreateNewOrderDetails(int orderID, string partID, string desc, double amount, int quantity) //Creates a new entry in the order details table
        {
            using (conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    cmd = new SqlCommand("OrderDetailsInsert", conn);
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@orderID", SqlDbType.SmallInt).Value = orderID;
                    cmd.Parameters.Add("@partID", SqlDbType.NVarChar).Value = partID;
                    cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = desc;
                    cmd.Parameters.Add("@amount", SqlDbType.Money).Value = amount;
                    cmd.Parameters.Add("@quantity", SqlDbType.SmallInt).Value = quantity;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

        public int GetLastOrderID()//Get the last (highest number) in the order id column
        {
            DataTable dt = GetDataTable("OrdersSelectLastID");
            int id = 0;
            try
            {
                id = Convert.ToInt32(dt.Rows[0][0]);
            }
            catch
            {
                id = 1;
            }
            return id;
        }

        public void SetupComboWithColNames(string spString, ComboBox combo) //Populate the combobox with the column names of the SELECT'ed table      
        {
            DataTable dt = GetDataTable(spString);
            var columnNames = new BindingList<KeyValuePair<int, string>>();
            int count = 0;
            foreach (DataColumn column in dt.Columns)
            {
                string columnName = column.ColumnName;
                columnNames.Add(new KeyValuePair<int, string>(count, columnName));
                count++;
            }
            combo.DataSource = columnNames;
            combo.ValueMember = "Key";
            combo.DisplayMember = "Value";
        }

        public DataTable GetDataTable(string spString) //Calls a 'SELECT' stored procedure using the string as the name of the stored procedure
        {
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                cmd = conn.CreateCommand();
                adap = new SqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = spString;
                adap.Fill(dt);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                adap.Dispose();
                cmd.Dispose();
                conn.Close();
            }
            return dt;
        }

        public DataTable SearchTable(string spString, string textBoxString, int index, string tableName)
        {
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                cmd = conn.CreateCommand();
                adap = new SqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = spString;
                string parStr = _model.GetParamForIndex(tableName, index);
                cmd.Parameters.AddWithValue(parStr, textBoxString);
                adap.Fill(dt);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
                adap.Dispose();
                cmd.Dispose();
            }
            return dt;
        }
    }




}
