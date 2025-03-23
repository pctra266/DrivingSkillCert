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
    /// Interaction logic for PersonalInfoWindow.xaml
    /// </summary>
    public partial class PersonalInfoWindow : Window
    {
        public PersonalInfoWindow()
        {
            InitializeComponent();
            txtFullName.Text = currentUser.FullName;
            txtEmail.Text = currentUser.Email;
            txtPhone.Text = currentUser.Phone;
            txtClass.Text = currentUser.Class;
            txtSchool.Text = currentUser.School;
        }
        User currentUser =
              (ConsoleApp1.Models.User)Application.Current.Properties["LoggedInUser"];
        private void btnback_Click(object sender, RoutedEventArgs e)
        {

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {   
            UserDAO userDAO = new UserDAO();
            if (userDAO.isEmailExistnotCurrentUser(txtEmail.Text, currentUser.UserId)) {
                MessageBox.Show("Email đã tồn tại trong hệ thống");
                return;
            }
            if (userDAO.isPhoneExistnotCurrentUser(txtPhone.Text, currentUser.UserId))
            {
                MessageBox.Show("Phone đã tồn tại trong hệ thống");
                return;
            }

            User updateUser = new User()
            {   
                UserId = currentUser.UserId,
                FullName = txtFullName.Text,
                Email = txtEmail.Text,
                Phone = txtPhone.Text,
                Class = txtClass.Text,
                School = txtSchool.Text,
            };
            bool isUpdated = UserDAO.updateProfile(updateUser);
            if (isUpdated) {
                MessageBox.Show("Cập nhật thành công");
            }
        }
    }
}
