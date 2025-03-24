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
            txtOldPassword.Text = currentUser.Password;

        }
                User currentUser =
                (Model.Models.User)Application.Current.Properties["LoggedInUser"];
            
        private void btnSaveChange_Click(object sender, RoutedEventArgs e)
        {
               String newPassword = txtNewPassword.Text;
               UserDAO userDAO = new UserDAO();
               bool isAdded = UserDAO.upDatePasswordUser(currentUser.UserId, newPassword);
            if (isAdded) {
                MessageBox.Show("Cập nhật mật khẩu thành công");
                currentUser.Password = newPassword;

                // Cập nhật lại trên giao diện
                txtOldPassword.Text = newPassword;

                // Cập nhật trong Application.Current.Properties
                Application.Current.Properties["LoggedInUser"] = currentUser;
            }
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        { 
               MainWindow mainWindow = new MainWindow();
               mainWindow.Show();
               this.Close();
        }
    }
}
