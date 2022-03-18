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
    public partial class Login : Form
    {
        string connectionString = @"Data Source=localhost;Initial Catalog=Library;Integrated Security=True;Pooling=False";


        public Login()
        {
            InitializeComponent();
            
        }

        private void Login_Load(object sender, EventArgs e)
        {
            //temp_insert();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if(textBox1.Text == "Username")
            {
                textBox1.Text = "";
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Username";
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "Password")
            {
                textBox2.Text = "";
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = "Password";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i = 0;
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();

            SqlCommand cmd = sqlCon.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from Accounts where username ='" + textBox1.Text + "' and password='"
            + textBox2.Text + "'";

            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            i = Convert.ToInt32(dt.Rows.Count.ToString());

            if (i == 0)
            {
                MessageBox.Show("Invalid Password or Username", "Message");
            }
            else
            {
                this.Hide();
                Form1 form1 = new Form1();
                form1.Show();
            }


        }

        private void temp_insert()
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Accounts(username, password) VALUES(@username, @password)", sqlCon);
            cmd.Parameters.AddWithValue("@username", "admin");
            cmd.Parameters.AddWithValue("@password", "test");

            
            cmd.ExecuteNonQuery();
            Console.Write("asdsa");
            sqlCon.Close();

        }

        
    }
}
