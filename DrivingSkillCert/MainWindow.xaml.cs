using System.Text;
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
using DrivingSkillCert.Models;

namespace DrivingSkillCert
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GoToCourse_Click(object sender, RoutedEventArgs e)
        {
            btnGoToCourse.Visibility = Visibility.Collapsed;
            btnGoToExam.Visibility = Visibility.Collapsed;

            MainFrame.Navigate(new ManageCourses());
        }
        private void GoToExam_Click(object sender, RoutedEventArgs e)
        {
            btnGoToCourse.Visibility = Visibility.Collapsed;
            btnGoToExam.Visibility = Visibility.Collapsed;

            MainFrame.Navigate(new ManageExam());
        }
    }
}