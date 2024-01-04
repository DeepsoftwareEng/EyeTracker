using EyeTracker.MVVM.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EyeTracker.MVVM.View
{
    /// <summary>
    /// Interaction logic for Recovery.xaml
    /// </summary>
    public partial class Recovery : Page
    {
        private DataConnection dc = new DataConnection();
        private SqlCommand cmd;
        public Recovery()
        {
            InitializeComponent();
        }

        private void BackBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            string query = "Select MatKhau from TaiKhoan where TenTaiKhoan = @tk and Email = @email";
            string usserpassword = string.Empty;
            if (dc.GetConnection().State == System.Data.ConnectionState.Closed)
                dc.GetConnection().Open();
            cmd = new SqlCommand(query, dc.GetConnection());
            try
            {
                cmd.Parameters.AddWithValue("@tk", UsernameTxb.Text);
                cmd.Parameters.AddWithValue("@email", EmailTxb.Text);
                usserpassword = cmd.ExecuteScalar().ToString();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            if (usserpassword != string.Empty)
            {
                string to = EmailTxb.Text;
                string from = "imhunggg02@gmail.com";
                string subject = "Recovery password";
                string body = @"Your password is: " + usserpassword;
                string password = "dpkw xanj ydkd ingd";
                MailMessage message = new MailMessage();
                message.To.Add(to);
                message.From = new MailAddress(from);
                message.IsBodyHtml = true;
                message.Body = body;
                message.Subject = subject;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.EnableSsl = true;
                smtp.Port = 587;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(from, password);
                try
                {
                    smtp.Send(message);
                    MessageBox.Show("Email sent");
                }catch(Exception ex )
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Incorrect information, please fill in again");
            }
            dc.GetConnection().Close();
        }
    }
}
