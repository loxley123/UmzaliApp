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
            try
            {
                SqlCommand cmd = new SqlCommand("MajorPlantsInsert", conn);
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@plantNo", plantNo);
                cmd.Parameters.AddWithValue("@serialNo", serial);
                cmd.Parameters.AddWithValue("@machineMake", mach);
                cmd.Parameters.AddWithValue("@model", model);
                cmd.Parameters.AddWithValue("@description", desc);
                cmd.Parameters.AddWithValue("@tireSizeFront", tireFront);
                cmd.Parameters.AddWithValue("@quantityFront", quanFront);
                cmd.Parameters.AddWithValue("@tireSizeRear", tireRear);
                cmd.Parameters.AddWithValue("@quantityRear", quanRear);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
            }
        }
        public void CreateSmallPlant(string plantNo, string serial, string mach, string model, string desc) //Creates a new entry in the small plants table
        {
            try
            {
                conn.Open();
                cmd = new SqlCommand("SmallPlantsInsert", conn);
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@plantNo", plantNo);
                cmd.Parameters.AddWithValue("@serialNo", serial);
                cmd.Parameters.AddWithValue("@machineMake", mach);
                cmd.Parameters.AddWithValue("@model", model);
                cmd.Parameters.AddWithValue("@description", desc);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
            }
        }
        public void CreateOilEntry(string oilType, DateTime date, double liters, string orderNo, string artisan)
        {
            try
            {
                conn.Open();
                cmd = new SqlCommand("OilDetailsInsert", conn);
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@oilType", oilType);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@liters", liters);
                cmd.Parameters.AddWithValue("@orderNo", orderNo);
                cmd.Parameters.AddWithValue("@artisan", artisan);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
            }
        }

        public void CreateNewOrder(int orderID, string companyID, string plantNo, DateTime dateRequired, string requestedBy, string desc, int machTime, string type, double total, string completedBy)
        {
            try
            {
                conn.Open();
                cmd = new SqlCommand("OrderInsert", conn);
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@orderID", orderID);
                cmd.Parameters.AddWithValue("@CompanyID", companyID);
                cmd.Parameters.AddWithValue("@plantNo", plantNo);
                cmd.Parameters.AddWithValue("@dateRequired", dateRequired);
                cmd.Parameters.AddWithValue("@requestedBy", requestedBy);
                cmd.Parameters.AddWithValue("@description", desc);
                cmd.Parameters.AddWithValue("@machineTime", machTime);
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@total", total);
                cmd.Parameters.AddWithValue("@completedBy", completedBy);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
            }

        }

        public void CreateNewOrderDetails(int orderID, string partID, string desc, double amount, int quantity) //Creates a new entry in the order details table
        {
            try
            {
                conn.Open();
                cmd = new SqlCommand("OrderDetailsInsert", conn);
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@orderID", orderID);
                cmd.Parameters.AddWithValue("@partID", partID);
                cmd.Parameters.AddWithValue("@description", desc);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@quantity", quantity);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
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
