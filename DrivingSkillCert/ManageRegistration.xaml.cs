using DrivingSkillCert.DAO;
using DrivingSkillCert.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace DrivingSkillCert
{
    /// <summary>
    /// Interaction logic for ManageRegistration.xaml
    /// </summary>
    public partial class ManageRegistration : Page
    {
        private RegistrationDAO registrationDAO = new RegistrationDAO();
        private int? editingRegistrationId = null;

        public ManageRegistration()
        {
            InitializeComponent();
            LoadRegistrations();
        }

        // Load danh sách Registration vào DataGrid
        void LoadRegistrations()
        {
            dgRegistrations.ItemsSource = registrationDAO.GetRegistrations();
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
            txtUserID.Text = string.Empty;
            txtCourseID.Text = string.Empty;
            cmbStatus.SelectedIndex = -1; // Xóa lựa chọn trong ComboBox
            txtComments.Text = string.Empty;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            editingRegistrationId = null;
            ClearForm();
            detailPanel.Visibility = Visibility.Visible;
            dgRegistrations.SelectedItem = null;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadRegistrations();
        }

        private void dgRegistrations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            detailPanel.Visibility = Visibility.Collapsed;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgRegistrations.SelectedItem is Registration selectedRegistration)
            {
                editingRegistrationId = selectedRegistration.RegistrationId;
                txtUserID.Text = selectedRegistration.UserId.ToString();
                txtCourseID.Text = selectedRegistration.CourseId.ToString();

                // Đặt giá trị cho ComboBox dựa vào Status
                foreach (var item in cmbStatus.Items)
                {
                    if (item is ComboBoxItem comboItem && comboItem.Content.ToString() == selectedRegistration.Status)
                    {
                        cmbStatus.SelectedItem = comboItem;
                        break;
                    }
                }
                txtComments.Text = selectedRegistration.Comments;
                detailPanel.Visibility = Visibility.Visible;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgRegistrations.SelectedItem is Registration selectedRegistration)
            {
                if (MessageBox.Show($"Are you sure you want to delete registration (ID: {selectedRegistration.RegistrationId})?",
                    "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        registrationDAO.DeleteRegistration(selectedRegistration.RegistrationId);
                        LoadRegistrations();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting registration: {ex.Message}");
                    }
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtUserID.Text) ||
                    string.IsNullOrWhiteSpace(txtCourseID.Text) ||
                    cmbStatus.SelectedItem == null)
                {
                    MessageBox.Show("Please fill in all required fields");
                    return;
                }

                // Kiểm tra và parse UserID, CourseID
                if (!int.TryParse(txtUserID.Text, out int userId))
                {
                    MessageBox.Show("User ID must be a valid number");
                    return;
                }
                if (!int.TryParse(txtCourseID.Text, out int courseId))
                {
                    MessageBox.Show("Course ID must be a valid number");
                    return;
                }

                // Lấy giá trị Status từ ComboBox
                string status = (cmbStatus.SelectedItem as ComboBoxItem)?.Content.ToString();

                var registration = new Registration
                {
                    UserId = userId,
                    CourseId = courseId,
                    Status = status,
                    Comments = txtComments.Text
                };

                if (editingRegistrationId.HasValue)
                {
                    registration.RegistrationId = editingRegistrationId.Value;
                    registrationDAO.UpdateRegistration(registration);
                }
                else
                {
                    registrationDAO.AddRegistration(registration);
                }

                LoadRegistrations();
                detailPanel.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving registration: {ex.Message}");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            detailPanel.Visibility = Visibility.Collapsed;
            ClearForm();
        }
    }
}
