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
    public partial class Manage_Students : Form
    {

        string connectionString = @"Data Source=localhost;Initial Catalog=Library;Integrated Security=True;Pooling=False";


        public Manage_Students()
        {
            InitializeComponent();
        }

        private void Manage_Students_Load(object sender, EventArgs e)
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
        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                id = Int32.Parse(row.Cells[0].Value.ToString());
                path = row.Cells[1].Value.ToString();

                txtFirstName.Text = row.Cells[2].Value.ToString();
                txtLastName.Text = row.Cells[3].Value.ToString();
                textMiddleName.Text = row.Cells[4].Value.ToString();
                txtAddress.Text = row.Cells[5].Value.ToString();
                txtContact.Text = row.Cells[6].Value.ToString();
                txtCourse.Text = row.Cells[7].Value.ToString();

                if (row.Cells[8].Value.ToString() == "Male")
                {
                    radioButton1.Checked = true;
                }
                else
                {
                    radioButton2.Checked = true;
                }


                pictureBox1.Image = null;
                if (row.Cells[1].Value.ToString() != "")
                {
                    pictureBox1.Image = Image.FromFile(row.Cells[1].Value.ToString());
                }
            }
        }
        


        string gender;
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            gender = "Male";
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            gender = "Female";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            browsePicture();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (txtFirstName.Text == "" || txtLastName.Text == "" || textMiddleName.Text == "" ||
                txtContact.Text == "" || txtAddress.Text == "" || txtCourse.Text == "" || gender == null)
            {
                MessageBox.Show("Please fill up the missing field", "Warning");
                return;
            }

            if (!int.TryParse(txtContact.Text, out _))
            {
                MessageBox.Show("Change contact to a number", "Warning");
                return;
            }

            insertStudent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            deleteStudent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (id == 0)
            {
                return;
            }
            updateStudent();
        }

        private void insertStudent()
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Students(First_Name, Last_Name, Middle_Name, Address, Contact, Course, Gender, Fee, filename) " +
                "VALUES(@First_Name, @Last_Name, @Middle_Name, @Address, @Contact, @Course, @Gender, @Fee, @filename)", sqlCon);
            cmd.Parameters.AddWithValue("@First_Name", txtFirstName.Text);
            cmd.Parameters.AddWithValue("@Last_Name", txtLastName.Text);
            cmd.Parameters.AddWithValue("@Middle_Name", textMiddleName.Text);
            cmd.Parameters.AddWithValue("@Address", txtAddress.Text.ToString());
            cmd.Parameters.AddWithValue("@Contact", txtContact.Text.ToString());
            cmd.Parameters.AddWithValue("@Course", txtCourse.Text);
            cmd.Parameters.AddWithValue("@Gender", gender);
            cmd.Parameters.AddWithValue("@Fee", 0);
            cmd.Parameters.AddWithValue("@filename", path);
            cmd.ExecuteNonQuery();

            sqlCon.Close();

            //MessageBox.Show("successfully inserted");
            populateOrRefreshTable();
        }

        private void deleteStudent()
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM Students WHERE Id=@Id", sqlCon);
            cmd.Parameters.AddWithValue("@Id", id);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show("you cant delete student with still borrowed book/s", "Warning");

            }


            sqlCon.Close();

            //MessageBox.Show("successfully deleted");
            populateOrRefreshTable();
        }

        private void updateStudent()
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("UPDATE Students SET First_Name=@First_Name, Last_Name=@Last_Name, Middle_Name=@Middle_Name, " +
                "Address=@Address, Contact=@Contact, Course=@Course, Gender=@Gender, filename=@filename WHERE Id=@Id", sqlCon);

            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@First_Name", txtFirstName.Text);
            cmd.Parameters.AddWithValue("@Last_Name", txtLastName.Text);
            cmd.Parameters.AddWithValue("@Middle_Name", textMiddleName.Text);
            cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
            cmd.Parameters.AddWithValue("@Contact", txtContact.Text);
            cmd.Parameters.AddWithValue("@Course", txtCourse.Text);
            cmd.Parameters.AddWithValue("@Gender", gender);
            cmd.Parameters.AddWithValue("@filename", path);

            //cmd.Parameters.AddWithValue("@filename", path);


            cmd.ExecuteNonQuery();

            sqlCon.Close();
            //MessageBox.Show("successfully updated");
            populateOrRefreshTable();
        }

        private void clear()
        {
            txtFirstName.Text = String.Empty;
            txtLastName.Text = String.Empty;
            textMiddleName.Text = String.Empty;
            txtAddress.Text = String.Empty;
            txtContact.Text = String.Empty;
            txtCourse.Text = String.Empty;

            radioButton1.Checked = false;
            radioButton2.Checked = false;

            path = String.Empty;
            pictureBox1.Image = null;
        }

        string path = "";
        private void browsePicture()
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                path = ofd.FileName.ToString();
                Console.WriteLine(path);
                pictureBox1.ImageLocation = path;
            }
        }

        #region styles

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



        #endregion styles

        
    }
}
