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
    /// Interaction logic for ExamStudent.xaml
    /// </summary>
    public partial class ExamStudent : Page
    {
        private readonly ExamDAO _examDAO;
        private readonly RegistrationDAO _registrationDAO;
        private List<Exam> _exams;
        private User _currentUser = (User) Application.Current.Properties["LoggedInUser"];

        public ExamStudent( )
        {
            InitializeComponent();
            _examDAO = new ExamDAO();
            _registrationDAO = new RegistrationDAO();
            LoadData();
        }

        private void LoadData()
        {
            _exams = _examDAO.GetExamsOfStudent(_currentUser.UserId);
            dgExams.ItemsSource = _exams;
            txtStudentName.Text = $"Exams for: {_currentUser.FullName}";
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }

   
}

  
