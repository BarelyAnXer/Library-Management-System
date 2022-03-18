using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalDisposable
{
    class bookPanel : FlowLayoutPanel
    {
        public bookPanel()
        {
            pBImage.Width = 140;
            pBImage.Height = 190;
            pBImage.SizeMode = PictureBoxSizeMode.Zoom;
            pBImage.Click += new EventHandler(imageClicked);


            lblTitle.Width = 140;
            lblTitle.Font = new Font("Arial", 8, FontStyle.Bold);

            create();

        }

        private string title;
        private string image;
        private string link;

        Label lblTitle = new Label();
        PictureBox pBImage = new PictureBox();

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                if (title != null)
                {
                    lblTitle.Text = title;
                }
            }
        }

        public string Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
                if (image != null)
                {
                    pBImage.Load(image);
                }

            }
        }

        public string Link
        {
            get
            {
                return link;
            }
            set
            {
                link = value;

            }
        }

        private void imageClicked(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(link);
            //gumagana rin website dito 
            //pati iba pang app lagay mo lang yung app nung location  ng link nila 
        }

        private void create()
        {
            this.FlowDirection = FlowDirection.TopDown;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(lblTitle);
            this.Controls.Add(pBImage);
        }
    }
}
