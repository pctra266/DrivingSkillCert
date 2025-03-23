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
using DrivingSkillCert.DAO;
using ConsoleApp1.Models;

namespace DrivingSkillCert
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        public void btnLogin_Click(object sender, RoutedEventArgs e)
        {   
            string username = txtUsername.Text;
            string password = txtPassword.Password;
            if (txtUsername == null || txtPassword == null)
            {
                MessageBox.Show("tên đăng nhập hoặc mật khẩu hông được để trống");
            }
            UserDAO userDAO = new UserDAO();
            User account = userDAO.getAccountByEmailAndPassword(username, password);
            if(account.IsDelete == true)
            {
                MessageBox.Show("tài khoản đã bị ban");
            }
            if (account != null && account.IsDelete == false)
            {
                Application.Current.Properties["LoggedInUser"] = account;
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
                
            }
            else
            {
                MessageBox.Show("tên đăng nhập hoặc mật khẩu không đúng");
            }
        }
        public void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            Register registerWindow = new Register();
            registerWindow.Show();
            this.Close();
        }

    }
}
