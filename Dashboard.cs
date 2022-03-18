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
    public partial class Dashboard : Form
    {

        string connectionString = @"Data Source=localhost;Initial Catalog=Library;Integrated Security=True;Pooling=False";

        public Dashboard()
        {
            InitializeComponent();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            label5.Text = getStudentCount().ToString();
            label6.Text = getBooksCount().ToString();
            label4.Text = getTotalBorrwedBooksCount().ToString();
            label2.Text = getTotalCategories().ToString();

            picStyles();
        }

        public int getStudentCount()
        {
            string stmt = "SELECT COUNT(*) FROM Students";
            int count = 0;
            Console.WriteLine("aasda");
            using (SqlConnection thisConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmdCount = new SqlCommand(stmt, thisConnection))
                {
                    thisConnection.Open();
                    count = (int)cmdCount.ExecuteScalar();
                }
            }
            return count;
        }

        public int getBooksCount()
        {
            string stmt = "SELECT COUNT(*) FROM Books";
            int count = 0;
            Console.WriteLine("aasda");
            using (SqlConnection thisConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmdCount = new SqlCommand(stmt, thisConnection))
                {
                    thisConnection.Open();
                    count = (int)cmdCount.ExecuteScalar();
                }
            }
            return count;
        }

        public int getTotalBorrwedBooksCount()
        {
            string stmt = "SELECT COUNT(*) FROM Borrow";
            int count = 0;
            Console.WriteLine("aasda");
            using (SqlConnection thisConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmdCount = new SqlCommand(stmt, thisConnection))
                {
                    thisConnection.Open();
                    count = (int)cmdCount.ExecuteScalar();
                }
            }
            return count;
        }

        public int getTotalCategories()
        {
            string stmt = "SELECT COUNT(*) FROM Categories";
            int count = 0;
            Console.WriteLine("aasda");
            using (SqlConnection thisConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmdCount = new SqlCommand(stmt, thisConnection))
                {
                    thisConnection.Open();
                    count = (int)cmdCount.ExecuteScalar();
                }
            }
            return count;
        }
    
        public void picStyles()
        {
            pictureBox1.Image = Image.FromFile(Path.Combine(Path.GetDirectoryName(
               Assembly.GetExecutingAssembly().Location), "images/total_students.jpg"));
            pictureBox2.Image = Image.FromFile(Path.Combine(Path.GetDirectoryName(
               Assembly.GetExecutingAssembly().Location), "images/total_books.png"));
            pictureBox3.Image = Image.FromFile(Path.Combine(Path.GetDirectoryName(
               Assembly.GetExecutingAssembly().Location), "images/total_borrowed_books.jpg"));
            pictureBox4.Image = Image.FromFile(Path.Combine(Path.GetDirectoryName(
               Assembly.GetExecutingAssembly().Location), "images/total_categories.jpg"));
        }

    }
}
