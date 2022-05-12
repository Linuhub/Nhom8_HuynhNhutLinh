using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Limilabs.Mail;
using Limilabs.Client.IMAP;
using System.Net.Mail;

namespace smtp
{
    public partial class MailBox : Form
    {

        private Imap imap;
        private IMail imail;
        private DataTable table;
        public MailBox()
        {
            InitializeComponent();
        }

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            imap = new Imap();
            imap.ConnectSSL("imap.gmail.com", 993);

            try
            {
                imap.Login(txtFrom.Text, txtPass.Text);

                table = new DataTable();
                table.Columns.Add("IDmail", typeof(string));
                table.Columns.Add("Subject", typeof(string));
                MessageBox.Show("Success!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                if (e.ColumnIndex == 0)
                {
                    string id = dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    string eml = Encoding.ASCII.GetString(imap.GetMessageByUID(long.Parse(id)));
                    imail = new MailBuilder().CreateFromEml(eml);
                    richTextBox1.Text = imail.Text;
                }
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void btnOpen_Click(object sender, EventArgs e)
        {

            imap.SelectInbox();
            List<long> uids = imap.SearchFlag(Flag.All);
            uids.Reverse();
            int i = int.Parse(txtNumOfMail.Text);
            int j = 0;
            foreach (long uid in uids)
            {
                if (j < i)
                {
                    byte[] eml = imap.GetHeadersByUID(uid);
                    imail = new MailBuilder().CreateFromEml(Encoding.ASCII.GetString(eml));
                    DataRow row = table.NewRow();
                    row["IDMail"] = uid.ToString();
                    row["Subject"] = imail.Subject;
                    table.Rows.Add(row);
                    table.AcceptChanges();
                    j++;
                }
                else break;
            }
            dataGridView2.DataSource = table;
        }
    
    }
}
