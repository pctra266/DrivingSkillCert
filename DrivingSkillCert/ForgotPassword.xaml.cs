using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Net.Mail;
using System.Net;
using DrivingSkillCert.DAO;
using Model.Models;

namespace DrivingSkillCert
{
    /// <summary>
    /// Interaction logic for ForgotPassword.xaml
    /// </summary>
    public partial class ForgotPassword : Window
    {
        public ForgotPassword()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        

        private void btnForgot_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text.Trim();
            UserDAO userDAO = new UserDAO();

            // Kiểm tra email có tồn tại không
            User user = userDAO.getAccountByEmailAndPassword(email);
            if (user == null)
            {
                MessageBox.Show("Email không tồn tại trong hệ thống.");
                return;
            }

            // Tạo mật khẩu mới
            string newPassword = GenerateRandomPassword();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);

            // Cập nhật mật khẩu vào database
            bool isUpdated = UserDAO.upDatePasswordUser(user.UserId,hashedPassword);
            if (!isUpdated)
            {
                MessageBox.Show("Có lỗi xảy ra khi cập nhật mật khẩu.");
                return;
            }

            // Gửi email chứa mật khẩu mới
            bool emailSent = SendEmail(email, newPassword);
            if (emailSent)
            {
                MessageBox.Show("Mật khẩu mới đã được gửi đến email của bạn.");
            }
            else
            {
                MessageBox.Show("Không thể gửi email. Vui lòng thử lại.");
            }
        }
        private string GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private bool SendEmail(string toEmail, string newPassword)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("sondphe182318@fpt.edu.vn");
                mail.To.Add(toEmail);
                mail.Subject = "Khôi phục mật khẩu";
                mail.Body = $"Mật khẩu mới của bạn là: {newPassword}";
                mail.IsBodyHtml = false;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential("sondphe182318@fpt.edu.vn", "xvvb mjad yxwm gtes");
                smtp.EnableSsl = true;
                smtp.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi gửi email: {ex.Message}");
                return false;
            }
        }
    }
}
