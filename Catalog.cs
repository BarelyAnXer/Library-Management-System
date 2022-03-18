using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalDisposable
{
    public partial class Catalog : Form
    {
        string path;
        BackgroundWorker worker;
        private delegate void DELEGATE();

        public Catalog()
        {
            InitializeComponent();
            worker = new BackgroundWorker();
        }

        private void Catalog_Load_1(object sender, EventArgs e)
        {
            path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\testing";

            worker.DoWork += worker_DoWork;
            worker.RunWorkerAsync();
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Delegate del = new DELEGATE(createLayout);
            this.Invoke(del);
        }

        public void createLayout()
        {
            string[] books = Directory.GetFiles(path).Select(paths => Path.GetFileName(paths)).ToArray();

            foreach (string b in books)
            {
                Console.WriteLine(b);
                Console.WriteLine("asdasd");
            }


            foreach (string book in books)
            {
                bookPanel bookpanel = new bookPanel();

                bookpanel.Title = book.Substring(0, book.Length - 4);
                bookpanel.Link = path + @"\" + book;
                bookpanel.Image = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                    + @"\covers\" + book.Substring(0, book.Length - 4) + ".jpg";

                bookpanel.Height = 240;
                bookpanel.Width = 144;
                //147
                //144

                flowLayoutPanel1.Controls.Add(bookpanel);


            }
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    Console.WriteLine(button1.Width);
        //    path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\testing";

        //    worker.DoWork += worker_DoWork;
        //    worker.RunWorkerAsync();
        //}
    }
}
