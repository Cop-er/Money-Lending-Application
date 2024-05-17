using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Globalization;

namespace DebtStat
{
    public partial class Form1 : Form
    {

        private string dataTable = "debtrecords";
        private string idPost = "";
        private string PaymentDatesPost = "";
        private string TotalPayedPost = "";
        private string TotalBalancePost = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string x1 = textBox1.Text;
            string x2 = textBox2.Text;
            string x3 = textBox3.Text;
            string x4 = dateTimePicker1.Value.ToShortDateString();

            DatabaseConnection.access(x1, x2, x3, x4);
            dataGridView1.Refresh();

            MessageBox.Show("Data has been Saved Successfully!", "Data", MessageBoxButtons.OK, MessageBoxIcon.Information);


            clearData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clearData();
        }

        private void clearData()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            loadtabledata();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.debtrecordsTableAdapter.Fill(this.xMoneyDataSet1.debtrecords);
        }

        private void loadtabledata()
        {
            string connectionString = DatabaseConnection.conAccess();
            string query = DatabaseConnection.getAllQuery();
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection);
                DataSet dataSet = new DataSet();
                connection.Open();
                adapter.Fill(dataSet, dataTable);
                dataGridView1.DataSource = dataSet.Tables[dataTable];
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            clearData();
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                Console.WriteLine(e.RowIndex);
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                string id = selectedRow.Cells[0].Value.ToString();
                string Name = selectedRow.Cells[1].Value.ToString();
                string Principal = selectedRow.Cells[2].Value.ToString();
                string Interest = selectedRow.Cells[3].Value.ToString();
                string DateStarted = selectedRow.Cells[4].Value.ToString();
                string TotalPayed = selectedRow.Cells[5].Value.ToString();
                string PaymentDates = selectedRow.Cells[6].Value.ToString();
                string RemainingBalance = selectedRow.Cells[7].Value.ToString();

                idPost = id;
                PaymentDatesPost = PaymentDates;
                TotalPayedPost = TotalPayed;
                textBox6.Text = Name;


                DateTime endDate = DateTime.Now;
                DateTime startDate = DateTime.Parse(DateStarted);
                double newBalance = double.Parse(RemainingBalance);

                    int months = endDate.Month - startDate.Month;
                    int years = endDate.Year - startDate.Year;
                    months += years * 12;
                    if (startDate > endDate.AddMonths(months))
                    {
                        months--;
                        
                    }

                newBalance = double.Parse(Principal) + (months + 1) * double.Parse(Principal) * (double.Parse(Interest) / 100);
                double finalBalance = double.Parse(newBalance.ToString()) - double.Parse(TotalPayedPost);

                if (double.Parse(RemainingBalance) < double.Parse(TotalPayedPost)) {
                 finalBalance = double.Parse(RemainingBalance);
                }

                TotalBalancePost = finalBalance.ToString();
                textBox5.Text = finalBalance.ToString("C", CultureInfo.GetCultureInfo("en-PH"));
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            double TBP = double.Parse(TotalBalancePost);
            if (TBP > 0)
            {
                string connectionString = DatabaseConnection.conAccess();
                string query = @"UPDATE " + dataTable + " SET RemainingBalance = @NewValue1, PaymentDates = @NewValue2, TotalPayed = @NewValue3 WHERE ID = @ID";

                double remainingx = double.Parse(TotalBalancePost) - double.Parse(textBox4.Text);
                double totalPayedx = double.Parse(TotalPayedPost) + double.Parse(textBox4.Text);
                string totaldatesx = PaymentDatesPost.ToString() + " " + dateTimePicker2.Value.ToString();

                int idToUpdate = int.Parse(idPost);
                string newValue1 = remainingx.ToString();
                string newValue2 = totaldatesx;
                string newValue3 = totalPayedx.ToString();

                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        // Add parameters
                        command.Parameters.AddWithValue("@NewValue1", newValue1);
                        command.Parameters.AddWithValue("@NewValue2", newValue2);
                        command.Parameters.AddWithValue("@NewValue3", newValue3);
                        command.Parameters.AddWithValue("@ID", idToUpdate);

                        // Open connection
                        connection.Open();

                        // Execute command
                        int rowsAffected = command.ExecuteNonQuery();

                        MessageBox.Show("Data has been Saved Successfully!", "Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }

            }
            else
            {
                MessageBox.Show("Unable to resume value is invalid!", "Data", MessageBoxButtons.OK, MessageBoxIcon.Hand);

            }
            clearData();

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
