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
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
        }

        public void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (!IsValidPassword(txtPassword.Password))
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 8 ký tự, bao gồm chữ hoa, chữ thường, số và ký tự đặc biệt.");
                return;
            }
            UserDAO userDAO = new UserDAO();
            string name = txtFullName.Text;
            string email = txtEmail.Text;
            string password = BCrypt.Net.BCrypt.HashPassword(txtPassword.Password);
            string phone = txtPhone.Text;
            string userclass = txtClass.Text;
            string school = txtSchool.Text;
            string role = "Student";
            if (userDAO.isEmailExist(email))
            {
                MessageBox.Show("Email đã tồn tại ở hệ thống");
                return;
            }
            if (userDAO.isPhoneExist(phone))
            {
                MessageBox.Show("Phone đã tồn tại ở hệ thống");
                return;
            }
            User user = new User()
            {   UserId = 0, 
                FullName = name,
                Email = email,
                Password = password,
                Role = role,
                Phone = phone,
                Class = userclass,
                School = school
            };
            userDAO.AddUser(user);
            MessageBox.Show("Đăng kí thành công");
        }

        public void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Login loginWindow = new Login();
            loginWindow.Show();
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
