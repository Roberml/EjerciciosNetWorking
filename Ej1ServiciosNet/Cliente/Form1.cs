using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cliente
{
    public partial class Form1 : Form
    {
        public string IP = "127.0.0.1";
        public int port = 31416;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            string userMsg = b.Text;

            IPEndPoint ie = new IPEndPoint(IPAddress.Parse(IP), port);
            using (Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                try
                {
                    s.Connect(ie);
                    using (NetworkStream ns = new NetworkStream(s))
                    using (StreamReader sr = new StreamReader(ns))
                    using (StreamWriter sw = new StreamWriter(ns))
                    {
                        sw.WriteLine(userMsg);
                        sw.Flush();
                        label1.Text = sr.ReadLine();
                    }
                }
                catch (Exception)
                {


                }
            }



            if (b.Text == "APAGAR")
            {
                this.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            DialogResult res = f.ShowDialog();

            switch (res)
            {
                
                case DialogResult.OK:
                    this.IP = f.textBox1.Text;
                    this.port = Convert.ToInt32(f.textBox2.Text);
                    break;
              
              
                default:
                    break;
            }
        }
    }
}
