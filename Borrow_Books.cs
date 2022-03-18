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
    public partial class Borrow_Books : Form
    {
        string connectionString = @"Data Source=localhost;Initial Catalog=Library;Integrated Security=True;Pooling=False";

        public Borrow_Books()
        {
            InitializeComponent();
        }
        private void Borrow_Books_Load(object sender, EventArgs e)
        {
            read_history();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Book_Details book_details = new Book_Details(this);
            book_details.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Borrower_Details borrower_details = new Borrower_Details(this);
            borrower_details.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            txtBookId.Text = String.Empty;
            txtTitle.Text = String.Empty;
            txtAuthor.Text = String.Empty;
            txtPublisher.Text = String.Empty;
            txtCategory.Text = String.Empty;
            txtDate_Published.Text = String.Empty;

            txtStudentId.Text = String.Empty;
            txtFirstName.Text = String.Empty;
            txtLastName.Text = String.Empty;
            txtMiddleName.Text = String.Empty;
            txtCourse.Text = String.Empty;
        }

        private void btnBorrow_Click(object sender, EventArgs e)
        {
            if (txtQuantity.Text == "0")
            {
                MessageBox.Show("There is no book left", "Warning");
                return;
            }
            if (txtBookId.Text == "")
            {
                MessageBox.Show("Choose a book to borrow", "Warning");
                return;
            }

            if (txtStudentId.Text == "")
            {
                MessageBox.Show("Choose the borrower", "Warning");
                return;
            }


            borrowBook();
            write_history();

            MessageBox.Show("Book successfully borrowed", "Message");

        }

        private void borrowBook()
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Borrow(student_id, books_id, date_borrowed, date_to_return) " +
                "VALUES(@student_id, @books_id, @date_borrowed, @date_to_return)", sqlCon);

            cmd.Parameters.AddWithValue("@student_id", Int32.Parse(txtStudentId.Text.ToString()));
            cmd.Parameters.AddWithValue("@books_id", Int32.Parse(txtBookId.Text.ToString()));
            cmd.Parameters.AddWithValue("@date_borrowed", DateTime.Now.ToString("MM/dd/yyyy"));
            cmd.Parameters.AddWithValue("@date_to_return", monthCalendar1.SelectionRange.Start.ToString("MM/dd/yyyy"));

            Console.WriteLine(DateTime.Now.ToString("MM / dd / yyyy"));
            Console.WriteLine(monthCalendar1.SelectionRange.Start.ToString("MM/dd/yyyy"));

            cmd.ExecuteNonQuery();
            sqlCon.Close();

            decrementQuantity();
        }

        private void decrementQuantity()
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("UPDATE Books SET quantity=@quantity WHERE Id=@Id", sqlCon);
            cmd.Parameters.AddWithValue("@Id", Int32.Parse(txtBookId.Text.ToString()));
            cmd.Parameters.AddWithValue("@quantity", getQuantity() - 1);
            cmd.ExecuteNonQuery();
        }

        private int getQuantity()
        {
            int quantity = 0;
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("Select quantity FROM Books WHERE Id=@Id", sqlCon);

            cmd.Parameters.AddWithValue("@Id", Int32.Parse(txtBookId.Text.ToString()));
            SqlDataReader sqldr = cmd.ExecuteReader();

            while (sqldr.Read())
            {
                Console.WriteLine(sqldr["quantity"].ToString());
                quantity = Int32.Parse(sqldr["quantity"].ToString());
            }

            //cmd.ExecuteNonQuery(); walang ganto pag select potek tagal kong hinahanp bakit di gumagana 
            sqlCon.Close();
            sqldr.Close();
            return quantity;
        }

        string filePath = Path.Combine(Path.GetDirectoryName(
            Assembly.GetExecutingAssembly().Location), "library_borrow_history.txt");
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
            lines.Add("[BOOK TITLE]  " + txtTitle.Text + "  [DATE BORROWED]  " + DateTime.Now.ToString("MM/dd/yyyy") + "  [BORROWED BY]  " + txtFirstName.Text +
                " " + txtLastName.Text + " " + "  [DATE TO BE RETURNED]  " + monthCalendar1.SelectionRange.Start.ToString("MM/dd/yyyy"));

            bs.DataSource = lines;
            bs.ResetBindings(false);


            File.WriteAllLines(filePath, lines);
            read_history();
        }



    }
}
