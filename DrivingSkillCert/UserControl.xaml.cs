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
using Model.Models;

namespace DrivingSkillCert
{
    /// <summary>
    /// Interaction logic for UserControl.xaml
    /// </summary>
    public partial class UserControl : Window
    {
        public UserControl()
        {
            InitializeComponent();
            loadDatagrid();     
            
        }
        User currentUser =
            (Model.Models.User)Application.Current.Properties["LoggedInUser"];
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        private void loadDatagrid()
        {
            UserDAO userDAO = new UserDAO();
            this.dgCertificates.ItemsSource = userDAO.getUserCertificate(currentUser.UserId);
        }
    }
}
