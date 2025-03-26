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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DrivingSkillCert.DAO;
using Model.Models;

namespace DrivingSkillCert
{
    /// <summary>
    /// Interaction logic for ManageExam.xaml
    /// </summary>
    public partial class ManageExam : Page
    {
        private readonly ExamDAO examDAO;
        private readonly CourseDAO courseDAO;
        private int? editingExamId = null;
        public ManageExam()
        {
            examDAO = new ExamDAO();
            courseDAO = new CourseDAO();
            InitializeComponent();
            LoadCourses();
            LoadExams();
        }

        private void LoadExams()
        {
            var exams = examDAO.GetExams();
            exams.Reverse();
            dgExams.ItemsSource = exams;
        }
        private void LoadCourses()
        {
            cmbCourse.ItemsSource = courseDAO.GetCourses();
        }
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
            
        {
            txtFilter.Text = "";
            cbTypeSearch.SelectedIndex = -1;
            LoadExams();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            detailPanel.Visibility = Visibility.Visible;
                cmbCourse.SelectedValue = -1;
                dpDate.Value =  DateTime.Now;
                txtRoom.Text = "";
        }

        private void dgExams_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            detailPanel.Visibility = Visibility.Collapsed;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            detailPanel.Visibility= Visibility.Visible;
            if (dgExams.SelectedItem is Exam selectedExam) {
                editingExamId = selectedExam.ExamId;
                cmbCourse.SelectedValue = selectedExam.CourseId;
                cbType.SelectedIndex = selectedExam.Type.Equals("Theory")?0:1;
                dpDate.Value = selectedExam.Date;
                txtRoom.Text = selectedExam.Room;
             }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgExams.SelectedItem is Exam selectedExam)
            {
                if (MessageBox.Show($"Are you sure you want to delete exam '{selectedExam.Course.CourseName}'?",
                    "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        examDAO.DeleteExam(selectedExam.ExamId);
                        LoadExams();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting exam: {ex}");
                    }
                }
            }

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Exam exam = new Exam();

            if (editingExamId != null)
            {
                 exam = examDAO.FindExamById(editingExamId.Value);
            }

            if (dpDate.Value==null || txtRoom.Text.Count()<1 || cbType.SelectionBoxItem.ToString().Count()<1) {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            exam.Date = dpDate.Value.Value;
            exam.Room = txtRoom.Text;
            exam.CourseId = (int)cmbCourse.SelectedValue;
            exam.Type = cbType.SelectionBoxItem.ToString();

            if (editingExamId==null) {
                examDAO.UpdateExam(exam);
                LoadExams();
                return;
            }
            exam.ExamId = editingExamId.Value;

            examDAO.UpdateExam(exam);
            LoadExams() ;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            detailPanel.Visibility= Visibility.Collapsed;
        }

        private void dgExams_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
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

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            dgExams.Items.Filter = FilterMethod;
        }

        private bool FilterMethod(object obj)
        {
            var exam = (Exam)obj;
            return exam.Course.CourseName.Contains(txtFilter.Text, StringComparison.OrdinalIgnoreCase)
                || exam.Course.Teacher.FullName.Contains(txtFilter.Text, StringComparison.OrdinalIgnoreCase)
                || exam.Room.Contains(txtFilter.Text, StringComparison.OrdinalIgnoreCase);
        }

        private void cbTypeSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (examDAO!=null&&dgExams!=null&& cbTypeSearch.SelectedItem is ComboBoxItem selectedItem)
            {
                string searchType = selectedItem.Content.ToString();
                dgExams.ItemsSource = examDAO.GetExams().Where(e => e.Type.Contains(searchType)).ToList();
            }
        }
    }
}
