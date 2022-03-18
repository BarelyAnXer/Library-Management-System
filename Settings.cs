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

namespace FinalDisposable
{
    
    public partial class Settings : Form
    {
        string connectionString = @"Data Source=localhost;Initial Catalog=Library;Integrated Security=True;Pooling=False";

        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            populateOrRefreshTable();
            styleDataGridView();
        }

        private void populateOrRefreshTable()
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDA = new SqlDataAdapter("SElECT * FROM Categories", sqlCon);
                DataTable dtbl = new DataTable();

                sqlDA.Fill(dtbl);

                dataGridView1.DataSource = dtbl;

            }
        }
        
        int id;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                id = Int32.Parse(row.Cells[0].Value.ToString());
                txtCategory.Text = row.Cells[1].Value.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(txtCategory.Text == "")
            {
                MessageBox.Show("Put something on the field", "Message");
                return;
            }
            insertCategory();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            updateCategory();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            deleteCategory();
        }
        
        private void insertCategory()
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Categories(category) VALUES(@category)", sqlCon);
            cmd.Parameters.AddWithValue("@category", txtCategory.Text);
            cmd.ExecuteNonQuery();

            sqlCon.Close();
            //MessageBox.Show("successfully inserted");
            populateOrRefreshTable();
        }

        private void updateCategory()
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("UPDATE Categories SET category=@category WHERE Id=@Id", sqlCon);
            Console.WriteLine("asdas");
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@category", txtCategory.Text);


            cmd.ExecuteNonQuery();
            sqlCon.Close();
            populateOrRefreshTable();
        }

        private void deleteCategory()
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM Categories WHERE Id=@Id", sqlCon);
            Console.WriteLine("aasdasd");
            cmd.Parameters.AddWithValue("@Id", id);

            cmd.ExecuteNonQuery();
            sqlCon.Close();


            populateOrRefreshTable();
        }

        private void styleDataGridView()
        {
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.BackgroundColor = Color.FromArgb(0, 0, 0);
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.Aqua;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.DeepSkyBlue;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;

            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;//optional

            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("MS Reference Sans Serif", 10);
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 102, 204);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (txtCurrent.Text == "" || txtNew.Text == "" || txtRetype.Text == "")
            {
                MessageBox.Show("input something on the missing field", "Warning");
            }
            else
            {
                if (getCurrentPassword() == txtCurrent.Text)
                {
                    if (txtNew.Text == txtRetype.Text)
                    {
                        updatePassword();
                        MessageBox.Show("Password successfully changed", "Info");
                    }
                    else
                    {
                        MessageBox.Show("New and Retype passwor does not match", "Warning");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Current password does not match", "Warning");
                    return;
                }
            }
        }

        private void updatePassword()
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("UPDATE Accounts SET password=@password WHERE username=@username", sqlCon);
            cmd.Parameters.AddWithValue("@username", "admin");
            cmd.Parameters.AddWithValue("@password", txtNew.Text);


            cmd.ExecuteNonQuery();
            sqlCon.Close();
            
        } 

        private string getCurrentPassword()
        {
            string password = "";
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();

            SqlCommand cmd = new SqlCommand("SELECT password FROM Accounts where username=@username", sqlCon);
            cmd.Parameters.AddWithValue("@username", "admin");
            cmd.CommandType = CommandType.Text;

            using (SqlDataReader rdr = cmd.ExecuteReader())
            {

                if (rdr.HasRows)
                {
                    rdr.Read(); // get the first row
                    password = rdr.GetString(0);
                    Console.WriteLine(rdr.GetString(0));

                }
            }

            return password;
        }


        private void button5_Click(object sender, EventArgs e)
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("Select * From Books", sqlCon);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds, "Books");

            CrystalReport1 rpt = new CrystalReport1();
            rpt.SetDataSource(ds);

            Report_Viewer frm = new Report_Viewer();
            frm.crystalReportViewer1.ReportSource = rpt;
            frm.ShowDialog();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("Select * From Students", sqlCon);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds, "Students");

            CrystalReport2 rpt = new CrystalReport2();
            rpt.SetDataSource(ds);

            Report_Viewer2 frm = new Report_Viewer2();
            frm.crystalReportViewer1.ReportSource = rpt;
            frm.ShowDialog();

        }
    }
}
