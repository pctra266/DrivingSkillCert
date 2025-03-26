using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DrivingSkillCert.DAO;
using Model.Models;

namespace DrivingSkillCert
{
    /// <summary>
    /// Interaction logic for ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        public ChangePasswordWindow()
        {
            InitializeComponent();
          

        }
                User currentUser =
                (Model.Models.User)Application.Current.Properties["LoggedInUser"];
            
        private void btnSaveChange_Click(object sender, RoutedEventArgs e)
        {
            string oldPasswordInput = txtOldPassword.Text;
            string newPassword = txtNewPassword.Text;
            UserDAO userDAO = new UserDAO();

            // Kiểm tra mật khẩu cũ có đúng không
            if (!BCrypt.Net.BCrypt.Verify(oldPasswordInput, currentUser.Password))
            {
                MessageBox.Show("Mật khẩu cũ không chính xác.");
                return;
            }

            // Kiểm tra điều kiện mật khẩu mới (nếu cần)
            if (!IsValidPassword(newPassword))
            {
                MessageBox.Show("Mật khẩu mới phải có ít nhất 8 ký tự, gồm chữ hoa, chữ thường, số và ký tự đặc biệt.");
                return;
            }

            // Mã hóa mật khẩu mới trước khi lưu
            string hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
            bool isUpdated = UserDAO.upDatePasswordUser(currentUser.UserId, hashedNewPassword);

            if (isUpdated)
            {
                MessageBox.Show("Cập nhật mật khẩu thành công");
                currentUser.Password = hashedNewPassword;
                Application.Current.Properties["LoggedInUser"] = currentUser;
            }
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        { 
               MainWindow mainWindow = new MainWindow();
               mainWindow.Show();
               this.Close();
        }
        private bool IsValidPassword(string password)
        {
            return password.Length >= 8 &&
                   Regex.IsMatch(password, ".*[A-Z].*") &&
                   Regex.IsMatch(password, ".*[a-z].*") &&
                   Regex.IsMatch(password, ".*[0-9].*") &&
                   Regex.IsMatch(password, ".*[^a-zA-Z0-9].*");
        }
    }
}
