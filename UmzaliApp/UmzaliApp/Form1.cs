using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UmzaliApp
{
    public partial class Form1 : Form
    {
        DataAccess da;
        double vat = 1.15;

        public Form1()
        {
            InitializeComponent();
            da = new DataAccess();
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            String companyNamesSP = "CreditorsSelectCompanyNames";
            DataTable dt = da.GetDataTable(companyNamesSP);
            companyNameComboBox.DataSource = dt;
            companyNameComboBox.DisplayMember = "Name";
            companyNameComboBox.ValueMember = "CompanyID";

            String orderTypesSP = "OrderTypesSelect";
            dt = da.GetDataTable(orderTypesSP);
            orderFormTypeComboBox.DataSource = dt;
            orderFormTypeComboBox.DisplayMember = "OrderType";
            orderFormTypeComboBox.ValueMember = "OrderType";
            String MPSelect = "MajorPlantsSelect";
            String SPSelect = "SmallPlantsSelect";
            String LabourSelect = "JobCardsSelect";
            String OilSelect = "OilDetailsSelect";

            da.SetupComboWithColNames(MPSelect, reportMPCombo);
            da.SetupComboWithColNames(SPSelect, reportSPCombo);
            da.SetupComboWithColNames(LabourSelect, reportLabourCombo);
            da.SetupComboWithColNames(OilSelect, reportOilCombo);
            
            orderFormOrderNoText.Text = (da.GetLastOrderID()).ToString();

            tabControl.DrawItem += new DrawItemEventHandler(tabControl_DrawItem);
            tabControl.Click += new EventHandler(tabControl_SelectedIndexChanged);
            removeReportTabs();

            //DataTable majorPlantsReport = da.getDataTable("majorPlantsSelect");
            majorPlantsDataView.DataSource = da.GetDataTable(MPSelect);
            smallPlantsDataView.DataSource = da.GetDataTable(SPSelect);
            labourDataView.DataSource = da.GetDataTable(LabourSelect);
            oilDataView.DataSource = da.GetDataTable(OilSelect);
            

        }
        

        private void tabControl_SelectedIndexChanged(Object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == reportsTab)
            {
                if (tabControl.TabPages.Contains(majorPlantReportTab))
                {
                    removeReportTabs();
                }
                else if (!tabControl.TabPages.Contains(majorPlantReportTab))
                {
                    addReportTabs();
                }
            }
            
            if(tabControl.SelectedTab == newMajorPlantTab || tabControl.SelectedTab == newSmallPlantTab || tabControl.SelectedTab == newOrderTab || tabControl.SelectedTab == jobCardsTab || tabControl.SelectedTab == oilTab || tabControl.SelectedTab == archiveTab)
            {                
                removeReportTabs();
            }
            
        }
        private void removeReportTabs()
        {
            tabControl.TabPages.Remove(majorPlantReportTab);
            tabControl.TabPages.Remove(smallPlantReportTab);
            tabControl.TabPages.Remove(labourReportTab);
            tabControl.TabPages.Remove(oilReportTab);
            tabControl.TabPages.Remove(tyresReportTab);
            tabControl.TabPages.Remove(orderReportTab);
        }
        private void addReportTabs()
        {
            tabControl.TabPages.Insert(1, majorPlantReportTab);
            tabControl.TabPages.Insert(2, smallPlantReportTab);
            tabControl.TabPages.Insert(3, labourReportTab);
            tabControl.TabPages.Insert(4, oilReportTab);
            tabControl.TabPages.Insert(5, tyresReportTab);
            tabControl.TabPages.Insert(6, orderReportTab);
        }
        private void tabControl_DrawItem(Object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush _textBrush;

            // Get the item from the collection.
            TabPage _tabPage = tabControl.TabPages[e.Index];

            // Get the real bounds for the tab rectangle.
            Rectangle _tabBounds = tabControl.GetTabRect(e.Index);

            if (e.State == DrawItemState.Selected)
            {

                // Draw a different background color, and don't paint a focus rectangle.
                _textBrush = new SolidBrush(Color.Black);
                //g.FillRectangle(Brushes.LightGray, e.Bounds);
                
                
                if(tabControl.TabPages[e.Index] == reportsTab)
                {
                    g.FillRectangle(Brushes.DarkCyan, e.Bounds);
                }
                if (tabControl.TabPages[e.Index] == newMajorPlantTab)
                {
                    g.FillRectangle(Brushes.DarkCyan, e.Bounds);
                }
                if (tabControl.TabPages[e.Index] == newSmallPlantTab)
                {
                    g.FillRectangle(Brushes.DarkCyan, e.Bounds);
                }
                if (tabControl.TabPages[e.Index] == newOrderTab)
                {
                    g.FillRectangle(Brushes.DarkCyan, e.Bounds);
                }
                if (tabControl.TabPages[e.Index] == jobCardsTab)
                {
                    g.FillRectangle(Brushes.DarkCyan, e.Bounds);
                }
                if (tabControl.TabPages[e.Index] == oilTab)
                {
                    g.FillRectangle(Brushes.DarkCyan, e.Bounds);
                }
                if (tabControl.TabPages[e.Index] == archiveTab)
                {
                    g.FillRectangle(Brushes.DarkCyan, e.Bounds);
                }


            }
            else
            {
                _textBrush = new System.Drawing.SolidBrush(e.ForeColor);
                e.DrawBackground();
                //g.FillRectangle(Brushes.AliceBlue, e.Bounds);
                
                if (tabControl.TabPages[e.Index] == reportsTab)
                {
                    g.FillRectangle(Brushes.LightBlue, e.Bounds);
                }
                if (tabControl.TabPages[e.Index] == newMajorPlantTab)
                {
                    g.FillRectangle(Brushes.LightBlue, e.Bounds);
                }
                if (tabControl.TabPages[e.Index] == newSmallPlantTab)
                {
                    g.FillRectangle(Brushes.LightBlue, e.Bounds);
                }
                if (tabControl.TabPages[e.Index] == newOrderTab)
                {
                    g.FillRectangle(Brushes.LightBlue, e.Bounds);
                }
                if (tabControl.TabPages[e.Index] == jobCardsTab)
                {
                    g.FillRectangle(Brushes.LightBlue, e.Bounds);
                }
                if (tabControl.TabPages[e.Index] == oilTab)
                {
                    g.FillRectangle(Brushes.LightBlue, e.Bounds);
                }
                if (tabControl.TabPages[e.Index] == archiveTab)
                {
                    g.FillRectangle(Brushes.LightBlue, e.Bounds);
                }
                

            }

            // Use our own font.
            Font _tabFont = new Font("Arial", (float)10.0, FontStyle.Bold, GraphicsUnit.Pixel);

            // Draw string. Center the text.
            StringFormat _stringFlags = new StringFormat();
            _stringFlags.Alignment = StringAlignment.Center;
            _stringFlags.LineAlignment = StringAlignment.Center;
            g.DrawString(_tabPage.Text, _tabFont, _textBrush, _tabBounds, new StringFormat(_stringFlags));
        }        

        private void newMajorPlantButton_Click_1(object sender, EventArgs e) //Creates new major plant in table. Needs input validation
        {
            da.CreateMajorPlant(newMPplantNo.Text, newMPserialNo.Text, newMPmachMake.Text, newMPmodel.Text, newMPdesc.Text, newMPtireF.Text, Int32.Parse(newMPquanF.Text), newMPtireR.Text, Int32.Parse(newMPquanR.Text));
        }

        private void newSmallPlantButton_Click_1(object sender, EventArgs e) //Creates new small plant in table. Needs input validation
        {
            da.CreateSmallPlant(smallPlantNo.Text, smallPlantSerial.Text, smallPlantMach.Text, smallPlantModel.Text, smallPlantDesc.Text);
        }

        private void addMaterialButton_Click_1(object sender, EventArgs e) //Add a new line to enter details for a material in the Order tab
        {
            Console.WriteLine("Material added");
            Panel newPanel = new Panel();
            newOrderMaterialPanel.Controls.Add(newPanel);

            TextBox txtBox1 = new TextBox();
            TextBox txtBox2 = new TextBox();
            TextBox txtBox3 = new TextBox();
            TextBox txtBox4 = new TextBox();

            Button deleteButton = new Button();
            deleteButton.Text = "Delete";
            
            deleteButton.Click += new EventHandler(deleteButton_Click);     //Creates click event for the new delete button
            txtBox4.Leave += new EventHandler(updateTotal);

            newPanel.Width = newOrderMaterialPanel.Width;
            newPanel.Height = 30;

            newPanel.Controls.Add(deleteButton);
            newPanel.Controls.Add(txtBox1);
            newPanel.Controls.Add(txtBox2);
            newPanel.Controls.Add(txtBox3);
            newPanel.Controls.Add(txtBox4);

            txtBox1.Left = 95;
            txtBox1.Width = 50;
            
            txtBox2.Left = 155;
            txtBox2.Width = 90;
            
            txtBox3.Left = 255;
            txtBox3.Width = 280;
            
            txtBox4.Left = 545;
            txtBox4.Width = 150;     
            
        }

        private void deleteButton_Click(object sender, EventArgs e) //Delete button click event
        {
            int count = 0;
            foreach(Control i in ((sender as Control).Parent).Controls)
            {
                if(count == 4)
                {
                    if(i.Text != "")
                    {        
                        orderTotalLabel.Text = (Double.Parse(orderTotalLabel.Text) - Double.Parse(i.Text)).ToString();
                        orderFormTotalVat.Text = ((Double.Parse(orderTotalLabel.Text)) * vat).ToString();
                    }
                }
                count++;
            }
            newOrderMaterialPanel.Controls.Remove((sender as Button).Parent);
        }

        private void orderSubmitButton_Click(object sender, EventArgs e) //Submit order click event. Needs fixing and input validation
        {
            double total = 0;
            int count = 1;
            foreach (Control i in newOrderMaterialPanel.Controls)
            {
                int quan = 0;
                string partNo = "";
                string desc = "";
                double amount = 0;
                foreach (Control j in i.Controls)
                {
                    switch(count)
                    {
                        case 1:
                            if(j.Text == null)
                            {
                                quan = 0;
                                break;
                            }
                            //quan = Int32.Parse(j.Text);
                            break;
                        case 2:
                            if(j.Text == null)
                            {
                                partNo = "";
                                break;
                            }
                            partNo = j.Text;
                            break;
                        case 3:
                            if(j.Text == null)
                            {
                                desc = "";
                                break;
                            }
                            desc = j.Text;
                            break;
                        case 4:
                            if(j.Text == null)
                            {
                                amount = 0.0;
                                break;
                            }
                            //amount = Double.Parse(j.Text);
                            break;
                    }
                    count++;
                }
                total += amount;
                count = 1;
                //Insert this line into orderdetails
                da.CreateNewOrderDetails(da.GetLastOrderID(), partNo, desc, amount, quan);
            }
            //Insert into order table
            da.CreateNewOrder(da.GetLastOrderID(), companyNameComboBox.ValueMember, orderFormPlantNoText.Text, orderFormDatePicker.Value, orderFormRequestedText.Text, orderFormDescText.Text, Int32.Parse(orderFormMachTimeText.Text), orderFormTypeComboBox.ValueMember, total, orderFormCompletedText.Text);

            newOrderMaterialPanel.Controls.Clear();
            //Clear all the textfields in order
            orderFormDescText.Text = String.Empty;
            orderFormRequestedText.Text = String.Empty;
            orderFormPlantNoText.Text = String.Empty;
            orderFormTypeComboBox.SelectedIndex = 0;
            orderFormMachTimeText.Text = String.Empty;
            orderFormCompletedText.Text = String.Empty;

            int orderid = Int32.Parse(orderFormOrderNoText.Text);
            orderid++;
            orderFormOrderNoText.Text = orderid.ToString();

            label34.Text = (newOrderMaterialPanel.Controls.Count).ToString();
        }

        private void updateTotal(object sender, EventArgs e) //Update the total on the order form
        {
            orderTotalLabel.Text = (double.Parse(orderTotalLabel.Text) + Double.Parse((sender as Control).Text)).ToString();
            orderFormTotalVat.Text = ((Double.Parse(orderTotalLabel.Text))*vat).ToString();
        }

        private void searchMajorPlantsButton_Click(object sender, EventArgs e) //Search the major reports table based on contents of textbox and combobox
        {
            majorPlantsDataView.DataSource = da.SearchMajorPlantsTable("MajorPlantsSearchSelect", reportMPText.Text, reportMPCombo.SelectedIndex);
        }

        private void reportMPClearButton_Click(object sender, EventArgs e)
        {
            majorPlantsDataView.DataSource = da.GetDataTable("majorPlantsSelect");
            reportMPText.Clear();
        }

        private void reportSPClearButton_Click(object sender, EventArgs e)
        {
            smallPlantsDataView.DataSource = da.GetDataTable("SmallPlantsSelect");
            reportSPText.Clear();
        }

        private void searchSmallPlantsButton_Click(object sender, EventArgs e)
        {
            smallPlantsDataView.DataSource = da.SearchSmallPlantsTable("SmallPlantsSearchSelect", reportSPText.Text, reportSPCombo.SelectedIndex);
        }

        private void newOilEntryButton_Click(object sender, EventArgs e)
        {
            da.CreateOilEntry(oilTypeText.Text, oilDatePicker.Value, Double.Parse(oilLitersText.Text), oilOrderNoText.Text, oilArtisanText.Text);
        }

        private void reportLabourSearchButton_Click(object sender, EventArgs e)
        {
            labourDataView.DataSource = da.SearchJobCardsTable("JobCardsSearchSelect", reportLaboutText.Text, reportLabourCombo.SelectedIndex);
        }

        private void reportLabourResetButton_Click(object sender, EventArgs e)
        {
            labourDataView.DataSource = da.GetDataTable("JobCardsSelect");
            reportLaboutText.Clear();
        }

        private void reportOilSearchButton_Click(object sender, EventArgs e)
        {
            oilDataView.DataSource = da.SearchOilDetailsTable("OilDetailsSearchSelect", reportOilText.Text, reportOilCombo.SelectedIndex);
        }

        private void reportOilClearButton_Click(object sender, EventArgs e)
        {
            oilDataView.DataSource = da.GetDataTable("OilDetailsSelect");
            reportOilText.Clear();
        }
    }
}
