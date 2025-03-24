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
using Microsoft.EntityFrameworkCore;
using Model.Models;

namespace DrivingSkillCert
{
    /// <summary>
    /// Interaction logic for ScoringPractical.xaml
    /// </summary>
    public partial class ScoringPractical : Window
    {
        public ScoringPractical()
        {
            InitializeComponent();
            loadDatagrid();
        }
        User currentUser =
              (Model.Models.User)Application.Current.Properties["LoggedInUser"];
        private void loadDatagrid()
        {
            string searchText = txtSearch.Text.Trim().ToLower();
            DrivingSkillCertContext context = new DrivingSkillCertContext();
            var results = context.Results
            .Include(r => r.User) // Load thông tin học sinh
            .Include(r => r.Exam)
                .ThenInclude(e => e.Course) // Load thông tin khóa học
            .Where(r => r.Exam.Type.Equals("Practice") && r.Exam.Course.TeacherId == currentUser.UserId)
            .Where(r => r.User.FullName.ToLower().Contains(searchText) && searchText!= null)
            .ToList();

            this.dgResults.ItemsSource = results; // Gán vào DataGrid
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void dgResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgResults.SelectedItem is Result selectedResult)
            {
                txtExamID.Text = selectedResult.ExamId.ToString();
                txtUserID.Text = selectedResult.User.FullName;
                txtScore.Text = selectedResult.Score.ToString();
                chkPassStatus.IsChecked = selectedResult.PassStatus;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (dgResults.SelectedItem is Result selectedResult)
            {
                int score;
                if (!int.TryParse(txtScore.Text, out score))
                {
                    MessageBox.Show("Invalid score!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                bool passStatus = chkPassStatus.IsChecked ?? false;
                bool isUpdated = ResultDAO.UpdateResult(selectedResult.ResultId, score, passStatus);

                if (isUpdated)
                {
                    MessageBox.Show("Update successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    loadDatagrid(); // Refresh DataGrid
                }
                else
                {
                    MessageBox.Show("Update failed!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
           
        
    }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            txtExamID.Text = "";
            txtUserID.Text = "";
            txtScore.Text = "";
            chkPassStatus.IsChecked = false ;
            txtSearch.Text = "";
            loadDatagrid();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            loadDatagrid();
        }
    }
}
