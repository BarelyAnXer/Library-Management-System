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
    public partial class Pay_Penalty : Form
    {
        string connectionString = @"Data Source=localhost;Initial Catalog=Library;Integrated Security=True;Pooling=False";

        public Pay_Penalty()
        {
            InitializeComponent();
        }

        private void Pay_Penalty_Load(object sender, EventArgs e)
        {
            populateOrRefreshTable();
            styleDataGridView();
            dataGridView1.Columns["filename"].Visible = false;
        }

        private void populateOrRefreshTable()
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                using (SqlDataAdapter sqlDA = new SqlDataAdapter("SElECT * FROM Students", sqlCon))
                {
                    DataTable dtbl = new DataTable();

                    sqlDA.Fill(dtbl);

                    dataGridView1.DataSource = dtbl;
                }
            }
        }

        int id;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                id = Int32.Parse(row.Cells[0].Value.ToString());

                textBox1.Text = row.Cells[2].Value.ToString();
                textBox2.Text = row.Cells[3].Value.ToString();
                textBox3.Text = row.Cells[4].Value.ToString();
                textBox4.Text = row.Cells[9].Value.ToString();


                pictureBox1.Image = null;
                if (row.Cells[1].Value.ToString() != "")
                {
                    pictureBox1.Image = Image.FromFile(row.Cells[1].Value.ToString());
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            


            try
            {
                if (Int32.Parse(textBox5.Text) < 0)
                {
                    MessageBox.Show("number cant be a negative", "Message");
                    return;
                }
                updateFee();
            }
            catch (Exception error)
            {
                MessageBox.Show("Please input a number", "Message");
            }
            
            
        }

        private void updateFee()
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("UPDATE Students SET Fee=@Fee WHERE Id=@Id", sqlCon);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Fee", getFee() - Int32.Parse(textBox5.Text));
            cmd.ExecuteNonQuery();

            sqlCon.Close();
            populateOrRefreshTable();
        }

        private int getFee()
        {
            int fee = 0;
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("Select Fee FROM Students WHERE Id=@Id", sqlCon);

            cmd.Parameters.AddWithValue("@Id", id);
            SqlDataReader sqldr = cmd.ExecuteReader();

            while (sqldr.Read())
            {
                fee = Int32.Parse(sqldr["Fee"].ToString());
            }

            sqlCon.Close();
            sqldr.Close();
            return fee;
        }

        private void styleDataGridView()
        {
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.BackgroundColor = Color.FromArgb(0, 0, 0);
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.DeepSkyBlue;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.Blue;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;

            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;//optional

            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("MS Reference Sans Serif", 10);
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 102, 204);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
        }


    }
}
