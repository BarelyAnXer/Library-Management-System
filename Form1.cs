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
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            styleMenuButton();
            styleMainButton();
            startTimer();

            label2.Text = DateTime.UtcNow.Date.ToString("MM/dd/yyyy");
        }

        Timer t = null;
        private void startTimer()
        {
            t = new Timer();
            t.Interval = 1000;
            t.Tick += new EventHandler(timer1_Tick);
            t.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }

        private Form activeForm = null;
        private void openChildForm(Form childForm)
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelChildForm.Controls.Add(childForm);
            panelChildForm.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }



        //---------------------------------------------------------------------------------


        //0, 102, 204 top
        //26, 32, 40 right 
        //37, 46, 59 mid


        #region styles
        private void styleMainButton()
        {
            pictureBox1.Image = Image.FromFile(Path.Combine(Path.GetDirectoryName(
             Assembly.GetExecutingAssembly().Location), "images/nameless.png"));
        }


        private void styleMenuButton()
        {
            // middle picturebox
            pictureBox2.Image = Image.FromFile(Path.Combine(Path.GetDirectoryName(
               Assembly.GetExecutingAssembly().Location), "images/nameless.png"));

            //---------------------

            Image MyImage = Image.FromFile(Path.Combine(Path.GetDirectoryName(
               Assembly.GetExecutingAssembly().Location), "images/dashboard.png"));
            btnDashboard.Image = (Image)(new Bitmap(MyImage, new Size(32, 32)));
            btnDashboard.ImageAlign = ContentAlignment.MiddleLeft;

            MyImage = Image.FromFile(Path.Combine(Path.GetDirectoryName(
               Assembly.GetExecutingAssembly().Location), "images/manage_books.png"));
            btnManageBooks.Image = (Image)(new Bitmap(MyImage, new Size(32, 32)));
            btnManageBooks.ImageAlign = ContentAlignment.MiddleLeft;

            MyImage = Image.FromFile(Path.Combine(Path.GetDirectoryName(
              Assembly.GetExecutingAssembly().Location), "images/manage_students.png"));
            btnManageStudents.Image = (Image)(new Bitmap(MyImage, new Size(32, 32)));
            btnManageStudents.ImageAlign = ContentAlignment.MiddleLeft;

            MyImage = Image.FromFile(Path.Combine(Path.GetDirectoryName(
              Assembly.GetExecutingAssembly().Location), "images/borrow_books.png"));
            btnBorrowBooks.Image = (Image)(new Bitmap(MyImage, new Size(32, 32)));
            btnBorrowBooks.ImageAlign = ContentAlignment.MiddleLeft;

            MyImage = Image.FromFile(Path.Combine(Path.GetDirectoryName(
              Assembly.GetExecutingAssembly().Location), "images/return_books.png"));
            btnReturnBooks.Image = (Image)(new Bitmap(MyImage, new Size(32, 32)));
            btnReturnBooks.ImageAlign = ContentAlignment.MiddleLeft;

            MyImage = Image.FromFile(Path.Combine(Path.GetDirectoryName(
             Assembly.GetExecutingAssembly().Location), "images/pay_penalty.png"));
            btnPayPenalty.Image = (Image)(new Bitmap(MyImage, new Size(32, 32)));
            btnPayPenalty.ImageAlign = ContentAlignment.MiddleLeft;

            MyImage = Image.FromFile(Path.Combine(Path.GetDirectoryName(
             Assembly.GetExecutingAssembly().Location), "images/settings.png"));
            btnSettings.Image = (Image)(new Bitmap(MyImage, new Size(32, 32)));
            btnSettings.ImageAlign = ContentAlignment.MiddleLeft;

            MyImage = Image.FromFile(Path.Combine(Path.GetDirectoryName(
             Assembly.GetExecutingAssembly().Location), "images/catalog.png"));
            btnCatalog.Image = (Image)(new Bitmap(MyImage, new Size(32, 32)));
            btnCatalog.ImageAlign = ContentAlignment.MiddleLeft;

            MyImage = Image.FromFile(Path.Combine(Path.GetDirectoryName(
             Assembly.GetExecutingAssembly().Location), "images/menu.png"));
            btnCollapse.Image = (Image)(new Bitmap(MyImage, new Size(32, 32)));
            btnCollapse.ImageAlign = ContentAlignment.MiddleLeft;

            MyImage = Image.FromFile(Path.Combine(Path.GetDirectoryName(
             Assembly.GetExecutingAssembly().Location), "images/logout.png"));
            btnLogout.Image = (Image)(new Bitmap(MyImage, new Size(32, 32)));
            btnLogout.ImageAlign = ContentAlignment.MiddleLeft;



        }

        #endregion


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            openChildForm(new Dashboard());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openChildForm(new Manage_Books());
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            openChildForm(new Manage_Students());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openChildForm(new Borrow_Books());
        }
        private void button4_Click(object sender, EventArgs e)
        {
            openChildForm(new Return_Books());
        }

        private void btnCollapse_Click(object sender, EventArgs e)
        {
            Console.WriteLine(splitContainer1.SplitterDistance);
            if (splitContainer1.SplitterDistance > 50)
            {
                splitContainer1.SplitterDistance = 50;
                btnDashboard.Text = string.Empty;
                btnManageBooks.Text = string.Empty;
                btnManageStudents.Text = string.Empty;
                btnManageStudents.Text = string.Empty;
                btnBorrowBooks.Text = string.Empty;
                btnReturnBooks.Text = string.Empty;
                btnPayPenalty.Text = string.Empty;
                btnSettings.Text = string.Empty;
                btnCatalog.Text = string.Empty;
                btnCollapse.Text = string.Empty;
                btnLogout.Text = string.Empty;
            }
            else
            {
                splitContainer1.SplitterDistance = 240;
                btnDashboard.Text = "DashBoard";
                btnManageBooks.Text = "Manage Books";
                btnManageStudents.Text = "Manage Students";
                btnBorrowBooks.Text = "Borrow Books";
                btnReturnBooks.Text = "Return Books";
                btnPayPenalty.Text = "Pay Penalty";
                btnSettings.Text = "Settings";
                btnCatalog.Text = "Catalog";
                btnCollapse.Text = "Collapse";
                btnLogout.Text = "Logout";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            openChildForm(new Pay_Penalty());
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            openChildForm(new Settings());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            openChildForm(new Catalog());
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to logout", "Warning", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                this.Hide();
                Login login = new Login();
                login.Show();
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }


            
        }
    }
}