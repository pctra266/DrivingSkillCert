using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DrivingSkillCert.DAO;
using Model.Models;
using DrivingSkillCert.Commands;

namespace DrivingSkillCert.ViewModel
{
    internal class ManageCourseViewModel
    {
        public ObservableCollection<Course> Courses { get; set; }
        public ObservableCollection<User> Teachers { get; set; }
        public ICommand ShowManageCourseCommand {  get; set; }
        public ManageCourseViewModel()
        {
            CourseDAO courseDAO = new CourseDAO();
            UserDAO userDAO = new UserDAO();
            Courses = new ObservableCollection<Course>(courseDAO.GetCourses());
            Teachers = new ObservableCollection<User>(userDAO.GetUsersByRole("Teacher"));
            ShowManageCourseCommand = new RelayCommand(ShowManageCourse, CanShowManageCourse);
        }

        private bool CanShowManageCourse(object arg)
        {
            return true;
        }

        private void ShowManageCourse(object obj)
        {
            ManageCourses manageCourses = new ManageCourses();
            manageCourses.ShowsNavigationUI = true;
        }
    }
}
