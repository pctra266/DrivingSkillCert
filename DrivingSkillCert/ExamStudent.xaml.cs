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
using static DrivingSkillCert.ExamStudent;

namespace DrivingSkillCert
{
    /// <summary>
    /// Interaction logic for ExamStudent.xaml
    /// </summary>
    public partial class ExamStudent : Page
    {
        public bool CameFromTheoryExam { get; set; }
        private readonly ExamDAO _examDAO;
        private readonly RegistrationDAO _registrationDAO;
        private readonly ResultDAO _resultDAO;
        private List<Exam> _exams;
        private User _currentUser = (User) Application.Current.Properties["LoggedInUser"];

        public class ViewExam
        {
            public Exam ExamItem { get; set; }
            public Visibility IsTheory { get; set; }
            public bool IsValid { get; set; }
            public override string ToString()
            {
                return $"{ExamItem.ExamId} {IsTheory} {IsValid}";
            }
        }

        public ExamStudent( )
        {
            InitializeComponent();
            _examDAO = new ExamDAO();
            _registrationDAO = new RegistrationDAO();
            _resultDAO = new ResultDAO();
            LoadData();
        }

        private void LoadData()
        {
            List<ViewExam> exams = new List<ViewExam>();
            _exams = _examDAO.GetExamsOfStudent(_currentUser.UserId);
            foreach (var item in _exams)
            {
                exams.Add(new ViewExam {ExamItem = item, 
                                        IsTheory="Theory".Equals(item.Type)?Visibility.Visible: Visibility.Collapsed,
                                        IsValid=IsValidTime(item)&&!HadResult(_currentUser.UserId,item)});
            }
   
            dgExams.ItemsSource = exams;
            txtStudentName.Text = $"Exams for: {_currentUser.FullName}";
        }

        private bool HadResult(int userId, Exam exam)
        {
            List<Result> results = _resultDAO.GetExamResultOfUser(userId, exam);
            return results.Count > 0;
        }

        private bool IsValidTime(Exam item)
        {
            return true;
            return DateTime.Now >= item.Date && DateTime.Now <= item.Date.AddMinutes(30);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack && !CameFromTheoryExam)
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

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        

        private void btnDoExam_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is ViewExam viewExam)
            {
                NavigationService?.Navigate(new DoTheoryExam(viewExam.ExamItem));
            }
        }

    }

   
}

  
