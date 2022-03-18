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
    public partial class Borrower_Details : Form
    {
        string connectionString = @"Data Source=localhost;Initial Catalog=Library;Integrated Security=True;Pooling=False";

        Borrow_Books borrow_books;
        public Borrower_Details(Borrow_Books borrow_books)
        {
            InitializeComponent();
            this.borrow_books = borrow_books;
        }

        private void Borrower_Details_Load(object sender, EventArgs e)
        {
            populateOrRefreshTable();
            styleDataGridView();
            dataGridView1.Columns["filename"].Visible = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            borrow_books.txtStudentId.Text = id;
            borrow_books.txtFirstName.Text = firstname;
            borrow_books.txtMiddleName.Text = middlename;
            borrow_books.txtLastName.Text = lastname;
            borrow_books.txtCourse.Text = course;
            this.Close();
        }

        private void populateOrRefreshTable()
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDA = new SqlDataAdapter("SElECT * FROM Students", sqlCon);
                DataTable dtbl = new DataTable();

                sqlDA.Fill(dtbl);

                dataGridView1.DataSource = dtbl;
            }
        }
        string id;
        string firstname;
        string middlename;
        string lastname;
        string course;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                id = row.Cells[0].Value.ToString();
                firstname = row.Cells[2].Value.ToString();
                middlename = row.Cells[3].Value.ToString();
                lastname = row.Cells[4].Value.ToString();
                course = row.Cells[7].Value.ToString();
            }
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


    }
}
