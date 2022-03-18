using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalDisposable
{

    public partial class Return_Books : Form
    {
        string connectionString = @"Data Source=localhost;Initial Catalog=Library;Integrated Security=True;Pooling=False";

        public Return_Books()
        {
            InitializeComponent();     
        }
        private void Return_Books_Load(object sender, EventArgs e)
        {
            populateOrRefreshTable();
            styleDataGridView();
            dataGridView1.Columns["filename"].Visible = false;
            read_history();
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

        private void populateOrRefreshTable2()
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();

            SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Borrow WHERE student_id=" + student_id, sqlCon);
            
            DataTable dtbl = new DataTable();
            sqlDa.Fill(dtbl);

            dataGridView2.DataSource = dtbl;

            sqlCon.Close();
        }

        int student_id;
        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine(e.RowIndex);
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                student_id = Int32.Parse(row.Cells[0].Value.ToString());
                populateOrRefreshTable2();
            }
        }


        int borrow_id;
        int book_id;
        DateTime date_to_be_returned;
        private void dataGridView2_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = this.dataGridView2.Rows[e.RowIndex];
            borrow_id = Int32.Parse(row.Cells[0].Value.ToString());
            book_id = Int32.Parse(row.Cells[2].Value.ToString());
            date_to_be_returned = DateTime.Parse(row.Cells[4].Value.ToString());
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if(book_id == 0)
            {
                return;
            }

            write_history();
            return_books();
            
        }

        private void return_books()
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM Borrow WHERE Id=@Id", sqlCon);
            cmd.Parameters.AddWithValue("@Id", borrow_id);
            cmd.ExecuteNonQuery();

            sqlCon.Close();


            populateOrRefreshTable2();
            populateOrRefreshTable();

            checkPenalty();

            incrementQuantity();

        }

        private void incrementQuantity()
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("UPDATE Books SET Quantity=@Quantity WHERE ID=@ID", sqlCon);
            cmd.Parameters.AddWithValue("@Id", book_id);
            cmd.Parameters.AddWithValue("@Quantity", getQuantity() + 1);
            cmd.ExecuteNonQuery();
        }

        private int getQuantity()
        {
            int quantity = 0;
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("Select Quantity FROM Books WHERE Id=@Id", sqlCon);

            cmd.Parameters.AddWithValue("@Id", book_id);
            SqlDataReader sqldr = cmd.ExecuteReader();

            while (sqldr.Read())
            {
                //Console.WriteLine(sqldr["Quantity"].ToString());
                quantity = Int32.Parse(sqldr["Quantity"].ToString());
            }

            //cmd.ExecuteNonQuery();
            sqlCon.Close();
            sqldr.Close();
            return quantity;
        }

        private void checkPenalty()
        {
            DateTime date_today = DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy"));

            if (date_today > date_to_be_returned)
            {
                Console.WriteLine("penalty");
                addFee();
            }
            else
            {
                Console.WriteLine("no penalty");
            }
        }

        private void addFee()
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("UPDATE Students SET Fee=@Fee WHERE ID=@ID", sqlCon);
            cmd.Parameters.AddWithValue("@ID", student_id);
            cmd.Parameters.AddWithValue("@Fee", getFee() + 5);
            cmd.ExecuteNonQuery();
            sqlCon.Close();

        }

        private int getFee()
        {
            int fee = 0;
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("SELECT Fee FROM Students WHERE ID=@ID", sqlCon);

            cmd.Parameters.AddWithValue("@ID", student_id);
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


            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.Aqua;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.DeepSkyBlue;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;

            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;//optional

            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("MS Reference Sans Serif", 10);
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 102, 204);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;

            //--------------------------------------------------------------------------------------

            dataGridView2.BorderStyle = BorderStyle.None;
            dataGridView2.BackgroundColor = Color.FromArgb(0, 0, 0);
            dataGridView2.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


            dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.Aqua;
            dataGridView2.DefaultCellStyle.SelectionBackColor = Color.DeepSkyBlue;
            dataGridView2.DefaultCellStyle.SelectionForeColor = Color.Black;

            dataGridView2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;//optional

            dataGridView2.EnableHeadersVisualStyles = false;
            dataGridView2.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView2.ColumnHeadersDefaultCellStyle.Font = new Font("MS Reference Sans Serif", 10);
            dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 102, 204);
            dataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;

        }


        string filePath = Path.Combine(Path.GetDirectoryName(
            Assembly.GetExecutingAssembly().Location), "library_return_history.txt");
        List<string> lines = new List<string>();
        BindingSource bs = new BindingSource();
        private void read_history()
        {
            Console.WriteLine(filePath);
            lines = File.ReadAllLines(filePath).ToList();
            listBox1.DataSource = lines;
        }

        private void write_history()
        {
            lines.Add("[BOOK ID]  " + book_id + "  [DATE RETURNED]  " + DateTime.Now.ToString("M/d/yyyy") + "[RETURN BY]  " + student_id);
            bs.DataSource = lines;
            bs.ResetBindings(false);


            File.WriteAllLines(filePath, lines);
            read_history();
        }

        
    }
}
