using DrivingSkillCert.DAO;
using ConsoleApp1.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace DrivingSkillCert
{
    /// <summary>
    /// Interaction logic for ManageResult.xaml
    /// </summary>
    public partial class ManageResult : Page
    {
        private ResultDAO resultDAO = new ResultDAO();
        private int? editingResultId = null;

        public ManageResult()
        {
            InitializeComponent();
            LoadResults();
        }

        // Load danh sách Result lên DataGrid
        void LoadResults()
        {
            dgResults.ItemsSource = resultDAO.GetResults();
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
            txtExamID.Text = string.Empty;
            txtUserID.Text = string.Empty;
            txtScore.Text = string.Empty;
            chkPassStatus.IsChecked = false;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            editingResultId = null;
            ClearForm();
            detailPanel.Visibility = Visibility.Visible;
            dgResults.SelectedItem = null;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadResults();
        }

        private void dgResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            detailPanel.Visibility = Visibility.Collapsed;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgResults.SelectedItem is Result selectedResult)
            {
                editingResultId = selectedResult.ResultId;
                txtExamID.Text = selectedResult.ExamId.ToString();
                txtUserID.Text = selectedResult.UserId.ToString();
                txtScore.Text = selectedResult.Score.ToString();
                chkPassStatus.IsChecked = selectedResult.PassStatus;
                detailPanel.Visibility = Visibility.Visible;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgResults.SelectedItem is Result selectedResult)
            {
                if (MessageBox.Show($"Are you sure you want to delete this result (ID: {selectedResult.ResultId})?",
                    "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        resultDAO.DeleteResult(selectedResult.ResultId);
                        LoadResults();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting result: {ex.Message}");
                    }
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Kiểm tra dữ liệu bắt buộc
                if (string.IsNullOrWhiteSpace(txtExamID.Text) ||
                    string.IsNullOrWhiteSpace(txtUserID.Text) ||
                    string.IsNullOrWhiteSpace(txtScore.Text))
                {
                    MessageBox.Show("Please fill in all required fields");
                    return;
                }

                // Parse ExamID và UserID
                if (!int.TryParse(txtExamID.Text, out int examId))
                {
                    MessageBox.Show("Exam ID must be a valid number");
                    return;
                }
                if (!int.TryParse(txtUserID.Text, out int userId))
                {
                    MessageBox.Show("User ID must be a valid number");
                    return;
                }
                // Parse Score
                if (!decimal.TryParse(txtScore.Text, out decimal score))
                {
                    MessageBox.Show("Score must be a valid number");
                    return;
                }

                var result = new Result
                {
                    ExamId = examId,
                    UserId = userId,
                    Score = score,
                    PassStatus = chkPassStatus.IsChecked ?? false
                };

                if (editingResultId.HasValue)
                {
                    result.ResultId = editingResultId.Value;
                    resultDAO.UpdateResult(result);
                }
                else
                {
                    resultDAO.AddResult(result);
                }

                LoadResults();
                detailPanel.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving result: {ex.Message}");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            detailPanel.Visibility = Visibility.Collapsed;
            ClearForm();
        }
    }
}
