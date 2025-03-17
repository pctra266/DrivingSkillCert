using DrivingSkillCert.DAO;
using DrivingSkillCert.Models;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace DrivingSkillCert
{
    /// <summary>
    /// Interaction logic for ManageUser.xaml
    /// </summary>
    public partial class ManageUser : Page
    {
        private UserDAO userDAO = new UserDAO();
        private int? editingUserId = null;
        public ManageUser()
        {
            InitializeComponent();
            loadUser();
        }

        void loadUser()
        {
            this.dgUsers.ItemsSource = userDAO.GetUsers();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
            else
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Window.GetWindow(this).Close();
            }
        }
        private void ClearForm()
        {
            
            txtUserName.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtClass.Text = string.Empty;
            cmbRole.SelectedIndex = -1;
            txtSchool.Text = string.Empty;

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            editingUserId = null;
            ClearForm();
            detailPanel.Visibility = Visibility.Visible;
            dgUsers.SelectedItem = null;


        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            loadUser();
        }

        private void dgUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            detailPanel.Visibility = Visibility.Collapsed;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgUsers.SelectedItem is User selectedUser)
            {
                editingUserId = selectedUser.UserId;
                txtUserName.Text = selectedUser.FullName;
                txtPassword.Text = selectedUser.Password ;
                txtEmail.Text = selectedUser.Email;
                txtPhone.Text = selectedUser.Phone ;
                txtClass.Text = selectedUser.Class ;
                cmbRole.SelectedItem = selectedUser.Role ;
                txtSchool.Text = selectedUser.School;
                detailPanel.Visibility = Visibility.Visible;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgUsers.SelectedItem is User selectedUser)
            {
                if (MessageBox.Show($"Are you sure you want to delete user '{selectedUser.FullName}'?",
                    "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        userDAO.DeleteUser(selectedUser.UserId);
                        loadUser();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting user: {ex.Message}");
                    }
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtUserName.Text) ||
                    string.IsNullOrWhiteSpace(txtPassword.Text) ||
                    string.IsNullOrWhiteSpace(txtEmail.Text) ||
                    string.IsNullOrWhiteSpace(cmbRole.SelectedItem.ToString())) 
                {
                    MessageBox.Show("Please fill in all required fields");
                    return;
                }

                var user = new User
                {
                    FullName = txtUserName.Text,
                    Password = txtPassword.Text,
                    Email = txtEmail.Text,
                    Phone = txtPhone.Text,
                    Class = txtClass.Text,
                    Role = cmbRole.SelectedItem.ToString(),
                    School = txtSchool.Text
                };
                

                if (editingUserId.HasValue)
                {
                    var existingUser = userDAO.GetUsers().FirstOrDefault(u => u.Email.Equals(txtEmail.Text, StringComparison.OrdinalIgnoreCase));
                    if (existingUser != null)
                    {
                        MessageBox.Show("Email already exists. Please use another email.");
                        return;
                    }

                    user.UserId = editingUserId.Value;
                    userDAO.UpdateUser(user);
                    loadUser();
                }
                else
                {
                    userDAO.AddUser(user);
                    loadUser();
                }

                loadUser();
                detailPanel.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving user: {ex.Message}");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            detailPanel.Visibility = Visibility.Collapsed;
            ClearForm();
        }
    }
}
