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
        private String connectionString = "Data Source = DESKTOP-EJ3OT5A\\SQLEXPRESS; Initial Catalog = testSSDB; Integrated Security = True";

        public DataAccess()
        {
            //conn = new SqlConnection(connectionString);
        }


        public void createMajorPlant(String plantNo, String serial, String mach, String model, String desc, String tireFront, int quanFront, String tireRear, int quanRear)
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
        public void createSmallPlant(String plantNo, String serial, String mach, String model, String desc) //Creates a new entry in the small plants table
        {
            Console.WriteLine(conn.ConnectionString);
            //conn = new SqlConnection(@"Data Source = DESKTOP-EJ3OT5A\SQLEXPRESS; Initial Catalog = testSSDB; Integrated Security = True");

            using (conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    cmd = new SqlCommand("SmallPlantsInsert", conn);
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@plantNo", SqlDbType.NVarChar).Value = plantNo;
                    cmd.Parameters.Add("@serialNo", SqlDbType.NVarChar).Value = serial;
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

        public void createNewOrder(int orderID, String companyID, String plantNo, DateTime dateRequired, String requestedBy, String desc, int machTime, String type, double total, String completedBy)
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
        
        public void createNewOrderDetails(int orderID, String partID, String desc, double amount, int quantity) //Creates a new entry in the order details table
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

        public int getLastOrderID()//Get the last (highest number) in the order id column
        {
            DataTable dt = getDataTable("OrdersSelectLastID");
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

        public void setupComboWithColNames(String spString, ComboBox combo) //Populate the combobox with the column names of the SELECT'ed table      
        {
            DataTable dt = getDataTable(spString);
            var columnNames = new BindingList<KeyValuePair<int, string>>();
            int count = 0;

            foreach(DataColumn column in dt.Columns)
            {
                String columnName = column.ColumnName;
                columnNames.Add(new KeyValuePair<int, string>(count, columnName));
                count++;
            }
            combo.DataSource = columnNames;            
            combo.ValueMember = "Key";
            combo.DisplayMember = "Value";            
        }
        

        public DataTable getDataTable(String spString) //Calls a 'SELECT' stored procedure using the string as the name of the stored procedure
        {
            DataTable dt = new DataTable();
            using (conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (cmd = conn.CreateCommand())
                {
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = spString;
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dt);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
                conn.Close();
            }
            return dt;
        }

        public DataTable searchMajorPlantsTable(String spString, String textBoxString, int index)
        {
            DataTable dt = new DataTable();
            using (conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (cmd = conn.CreateCommand())
                {
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = spString;
                        String parStr = "";

                        //Needs cleanup. 2 switch statements probably not necessary

                        switch(index)
                        {
                            case 0:
                                parStr = "@plantNo";
                                break;
                            case 1:
                                parStr = "@serialNo";
                                break;
                            case 2:
                                parStr = "@machineMake";
                                break;
                            case 3:
                                parStr = "@model";
                                break;
                            case 4:
                                parStr = "@description";
                                break;
                            case 5:
                                parStr = "@tyreSizeFront";
                                break;
                            case 6:
                                parStr = "@quantityFront";
                                break;
                            case 7:
                                parStr = "@tyreSizeRear";
                                break;
                            case 8:
                                parStr = "@quantityRear";
                                break;
                        }
                        switch(index)
                        {
                            case 6:
                            case 8:
                                //quantityFront and quantityRear are smallints
                                cmd.Parameters.Add(parStr, SqlDbType.SmallInt).Value = Int32.Parse(textBoxString);
                                break;
                            default:
                                //The rest are nvarchar
                                cmd.Parameters.Add(parStr, SqlDbType.NVarChar).Value = textBoxString;
                                break;
                        }                                            
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dt);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }

                }
            }
            return dt;
        }

        public DataTable searchSmallPlantsTable(String spString, String textBoxString, int index)
        {
            DataTable dt = new DataTable();
            using (conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (cmd = conn.CreateCommand())
                {
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = spString;
                        String parStr = "";

                        //Needs cleanup. 2 switch statements probably not necessary

                        switch (index)
                        {
                            case 0:
                                parStr = "@plantNo";
                                break;
                            case 1:
                                parStr = "@serialNo";
                                break;
                            case 2:
                                parStr = "@machineMake";
                                break;
                            case 3:
                                parStr = "@model";
                                break;
                            case 4:
                                parStr = "@description";
                                break;
                        }
                        cmd.Parameters.Add(parStr, SqlDbType.NVarChar).Value = textBoxString;
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dt);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }

            }
            return dt;
        }

        public DataTable searchTableInt(String spString, int textBoxInt)
        {
            DataTable dt = new DataTable();
            using (conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (cmd = conn.CreateCommand())
                {
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = spString;
                        cmd.Parameters.Add("@par1", SqlDbType.SmallInt).Value = textBoxInt;                        
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dt);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }

                }
            }
            return dt;
        }

    }

    


}
