using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net;
using System.IO;


namespace smtp
{
    public partial class SendMail : Form
    {

        public SendMail()
        {
            InitializeComponent();
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            string from = tbFrom.Text;
            string to = tbTo.Text;
            string Subject = tbSubject.Text;
            string password = tbPassword.Text;
            string body = tbBody.Text;
            try
            {
                MailMessage mess = new MailMessage(from, to, Subject, body);
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(from, password);
                if (listBox1.Items.Count > 0)
                {
                    foreach (var filename in listBox1.Items)
                    {

                        if (File.Exists(filename.ToString()))
                        {
                            mess.Attachments.Add(new Attachment(filename.ToString()));
                        }
                    }
                }
                client.Send(mess);
                MessageBox.Show("Gửi thành công");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error");
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (var filename in openFileDialog.FileNames)
                {
                    listBox1.Items.Add(filename.ToString());
                }
            }
        }

    
    }
}
