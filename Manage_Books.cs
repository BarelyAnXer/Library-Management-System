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
    public partial class Manage_Books : Form
    {
        string connectionString = @"Data Source=localhost;Initial Catalog=Library;Integrated Security=True;Pooling=False";

        public Manage_Books()
        {
            InitializeComponent();
        }

        private void Manage_Books_Load(object sender, EventArgs e)
        {
            populateOrRefreshTable();
            populateComboBox();
            styleDataGridView();
            dataGridView1.Columns["filename"].Visible = false;
        }

        private void populateOrRefreshTable()
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                using (SqlDataAdapter sqlDA = new SqlDataAdapter("SElECT ID, Title, Author, Category, Quantity, Publisher, Date_Published, filename FROM Books", sqlCon))
                {
                    DataTable dtbl = new DataTable();

                    sqlDA.Fill(dtbl);

                    dataGridView1.DataSource = dtbl;
                }
            }
        }

        private void populateComboBox()
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("SElECT * FROM Categories", sqlCon);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string Category = reader.GetString(1);
                    cmbCategory.Items.Add(Category);
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
                path = row.Cells[7].Value.ToString();

                txtTitle.Text = row.Cells[1].Value.ToString();
                txtAuthor.Text = row.Cells[2].Value.ToString();
                cmbCategory.Text = row.Cells[3].Value.ToString();
                txtQuantity.Text = row.Cells[4].Value.ToString();
                txtPublisher.Text = row.Cells[5].Value.ToString();
                dateTimePicker1.Value = DateTime.Parse(row.Cells[6].Value.ToString());


                pictureBox1.Image = null;
                if (row.Cells[7].Value.ToString() != "")
                {
                    pictureBox1.Image = Image.FromFile(row.Cells[7].Value.ToString());
                }
            }
        }

         

        private void btnInsert_Click(object sender, EventArgs e)
        {
            Console.WriteLine(dateTimePicker1.Value + "asdas");
            if (txtTitle.Text == "" || txtAuthor.Text == "" || cmbCategory.Text == "" ||
                txtPublisher.Text == "" || txtQuantity.Text == "")
            {
                MessageBox.Show("Please fill up the missing field", "Warning");
                return;
            }


            if (!int.TryParse(txtQuantity.Text, out _))
            {
                MessageBox.Show("Change quantity to a number", "Warning");
                return;
            }
            
            insertBook();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            deleteBook();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if(id == 0)
            {
                return;
            }
            updateBook();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            browsePicture();
        }

        private void insertBook()
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Books(Title, Author, Quantity, Category, Date_Published, Publisher, filename) " +
                "VALUES(@Title, @Author, @Quantity, @Category, @Date_Published, @Publisher, @filename)", sqlCon);
            cmd.Parameters.AddWithValue("@Title", txtTitle.Text);
            cmd.Parameters.AddWithValue("@Author", txtAuthor.Text);
            cmd.Parameters.AddWithValue("@Quantity", Int32.Parse(txtQuantity.Text));
            cmd.Parameters.AddWithValue("@Category", cmbCategory.Text.ToString());
            cmd.Parameters.AddWithValue("@Date_Published", dateTimePicker1.Value.ToString("MM-dd-yyyy"));
            cmd.Parameters.AddWithValue("@Publisher", txtPublisher.Text);
            cmd.Parameters.AddWithValue("@filename", path);
            cmd.ExecuteNonQuery();

            sqlCon.Close();

            //MessageBox.Show("successfully inserted");
            populateOrRefreshTable();
        }

        private void updateBook()
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("UPDATE Books SET Title=@Title, Author=@Author, Quantity=@Quantity, " +
                "Category=@Category, Date_Published=@Date_Published, Publisher=@Publisher, filename=@filename WHERE Id=@Id", sqlCon);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Title", txtTitle.Text);
            cmd.Parameters.AddWithValue("@Author", txtAuthor.Text);

            cmd.Parameters.AddWithValue("@Quantity", Int32.Parse(txtQuantity.Text));

            cmd.Parameters.AddWithValue("@Category", cmbCategory.Text.ToString());
            cmd.Parameters.AddWithValue("@Date_Published", dateTimePicker1.Value.ToString("MM-dd-yyyy"));
            cmd.Parameters.AddWithValue("@Publisher", txtPublisher.Text);
            cmd.Parameters.AddWithValue("@filename", path);

            cmd.ExecuteNonQuery();

            sqlCon.Close();
            //MessageBox.Show("successfully updated");
            populateOrRefreshTable();
        }

        private void deleteBook()
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM Books WHERE Id=@Id", sqlCon);
            cmd.Parameters.AddWithValue("@Id", id);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception error)
            {
                MessageBox.Show("you cant delete this book someone still borrowed this", "Warning");
            }


            sqlCon.Close();

            //MessageBox.Show("successfully deleted");
            populateOrRefreshTable();
        }

        //private void searchBook()
        //{
        //    SqlConnection sqlCon = new SqlConnection(connectionString);
        //    sqlCon.Open();
        //    SqlCommand cmd = new SqlCommand("SELECT * FROM Books WHERE Id=@Id", sqlCon);
        //    cmd.Parameters.AddWithValue("@Id", int.Parse(textBox5.Text));
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    DataTable dt = new DataTable();
        //    da.Fill(dt);
        //    dataGridView1.DataSource = dt;
        //    cmd.ExecuteNonQuery();
        //    sqlCon.Close();

        //}

        private void clear()
        {
            txtAuthor.Text = String.Empty;
            txtPublisher.Text = String.Empty;
            txtQuantity.Text = String.Empty;
            txtTitle.Text = String.Empty;

            cmbCategory.SelectedIndex = -1;
            cmbCategory.Text = String.Empty;

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
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.DeepSkyBlue;// this one
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
