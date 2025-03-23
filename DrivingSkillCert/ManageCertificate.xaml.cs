using DrivingSkillCert.DAO;
using ConsoleApp1.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace DrivingSkillCert
{
    /// <summary>
    /// Interaction logic for ManageCertificate.xaml
    /// </summary>
    public partial class ManageCertificate : Page
    {
        private CertificateDAO certificateDAO = new CertificateDAO();
        private int? editingCertificateId = null;

        public ManageCertificate()
        {
            InitializeComponent();
            LoadCertificates();
        }

        // Load danh sách Certificate lên DataGrid
        void LoadCertificates()
        {
            dgCertificates.ItemsSource = certificateDAO.GetCertificates();
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
            dpIssuedDate.SelectedDate = null;
            dpExpirationDate.SelectedDate = null;
            txtCertificateCode.Text = string.Empty;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            editingCertificateId = null;
            ClearForm();
            detailPanel.Visibility = Visibility.Visible;
            dgCertificates.SelectedItem = null;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadCertificates();
        }

        private void dgCertificates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            detailPanel.Visibility = Visibility.Collapsed;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgCertificates.SelectedItem is Certificate selectedCertificate)
            {
                editingCertificateId = selectedCertificate.CertificateId;
                txtUserID.Text = selectedCertificate.UserId.ToString();
                dpIssuedDate.SelectedDate = new DateTime(selectedCertificate.IssuedDate.Year, selectedCertificate.IssuedDate.Month, selectedCertificate.IssuedDate.Day);
                dpExpirationDate.SelectedDate =new DateTime( selectedCertificate.ExpirationDate.Year, selectedCertificate.ExpirationDate.Month, selectedCertificate.ExpirationDate.Day);
                txtCertificateCode.Text = selectedCertificate.CertificateCode;
                detailPanel.Visibility = Visibility.Visible;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgCertificates.SelectedItem is Certificate selectedCertificate)
            {
                if (MessageBox.Show($"Are you sure you want to delete certificate '{selectedCertificate.CertificateCode}'?",
                    "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        certificateDAO.DeleteCertificate(selectedCertificate.CertificateId);
                        LoadCertificates();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting certificate: {ex.Message}");
                    }
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Kiểm tra dữ liệu bắt buộc
                if (string.IsNullOrWhiteSpace(txtUserID.Text) ||
                    dpIssuedDate.SelectedDate == null ||
                    dpExpirationDate.SelectedDate == null ||
                    string.IsNullOrWhiteSpace(txtCertificateCode.Text))
                {
                    MessageBox.Show("Please fill in all required fields");
                    return;
                }

                // Parse UserID
                if (!int.TryParse(txtUserID.Text, out int userId))
                {
                    MessageBox.Show("UserID must be a valid number");
                    return;
                }

                var certificate = new Certificate
                {
                    UserId = userId,
                    IssuedDate = new DateOnly( dpIssuedDate.SelectedDate.Value.Year, dpIssuedDate.SelectedDate.Value.Month, dpIssuedDate.SelectedDate.Value.Day),
                    ExpirationDate =new DateOnly( dpExpirationDate.SelectedDate.Value.Year, dpExpirationDate.SelectedDate.Value.Month, dpExpirationDate.SelectedDate.Value.Day),
                    CertificateCode = txtCertificateCode.Text
                };

                if (editingCertificateId.HasValue)
                {
                    certificate.CertificateId = editingCertificateId.Value;
                    certificateDAO.UpdateCertificate(certificate);
                }
                else
                {
                    certificateDAO.AddCertificate(certificate);
                }

                LoadCertificates();
                detailPanel.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving certificate: {ex.Message}");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            detailPanel.Visibility = Visibility.Collapsed;
            ClearForm();
        }
    }
}
