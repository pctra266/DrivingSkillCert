using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DrivingSkillCert.DAO;
using Model.Models;

namespace DrivingSkillCert
{
    public partial class ManageBankQuestion : Page
    {
        private readonly BankQuestionDAO _bankQuestionDAO;
        private readonly CourseDAO _courseDAO;
        private List<BankQuestion> _BankQuestion;
        private List<Course> _courses;
        private BankQuestion _currentBankQuestion;

        public ManageBankQuestion()
        {
            InitializeComponent();
            _bankQuestionDAO = new BankQuestionDAO();
            _courseDAO = new CourseDAO();
            LoadData();
        }

        private void LoadData()
        {
            _BankQuestion = _bankQuestionDAO.GetBankQuestion();
            _courses = _courseDAO.GetCourses();
            dgBankQuestion.ItemsSource = _BankQuestion;
            cmbCourse.ItemsSource = _courses;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null && NavigationService.CanGoBack)
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

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            _currentBankQuestion = new BankQuestion();
            txtBankName.Text = string.Empty;
            cmbCourse.SelectedIndex = -1;
            detailPanel.Visibility = Visibility.Visible;
            dgBankQuestion.SelectedItem = null;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is BankQuestion bankQuestion)
            {
                _currentBankQuestion = bankQuestion;
                txtBankName.Text = bankQuestion.BankName;
                cmbCourse.SelectedValue = bankQuestion.CourseId;
                detailPanel.Visibility = Visibility.Visible;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is BankQuestion bankQuestion)
            {
                if (MessageBox.Show($"Are you sure you want to delete bank '{bankQuestion.BankName}'?",
                    "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        _bankQuestionDAO.DeleteBankQuestion(bankQuestion.BankId);
                        _BankQuestion.Remove(bankQuestion);
                        dgBankQuestion.ItemsSource = null;
                        dgBankQuestion.ItemsSource = _BankQuestion;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting bank question: {ex.Message}");
                    }
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBankName.Text) || cmbCourse.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            try
            {
                Course course = cmbCourse.SelectedItem as Course;
                _currentBankQuestion.BankName = txtBankName.Text;
                _currentBankQuestion.CourseId = course.CourseId;

                if (_currentBankQuestion.BankId > 0)
                {
                    _bankQuestionDAO.UpdateBankQuestion(_currentBankQuestion);
                }
                else
                {
                    _bankQuestionDAO.CreateBankQuestion(_currentBankQuestion);
                    _BankQuestion.Add(_currentBankQuestion);
                }

                detailPanel.Visibility = Visibility.Collapsed;
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving bank question: {ex.Message} {cmbCourse.SelectedValuePath} {cmbCourse.SelectedItem}");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            detailPanel.Visibility = Visibility.Collapsed;
            _currentBankQuestion = null;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void dgBankQuestion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            detailPanel.Visibility = Visibility.Collapsed;
        }

        private void btnManageQuestions_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is BankQuestion bankQuestion)
            {
                NavigationService?.Navigate(new ManageQuestionsAndAnswers(bankQuestion));
            }
        }
    }
}