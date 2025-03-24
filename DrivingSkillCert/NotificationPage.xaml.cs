using Microsoft.EntityFrameworkCore;
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
using Model.Models;

namespace DrivingSkillCert
{
    /// <summary>
    /// Interaction logic for NotificationPage.xaml
    /// </summary>
    public partial class NotificationPage : Page
    {
        private DrivingSkillCertContext _context = new DrivingSkillCertContext();
        public NotificationPage()
        {
            InitializeComponent();
            LoadNotifications();
        }
        private void LoadNotifications()
        {
            // Lấy thông tin người dùng hiện tại từ Application.Current.Properties
            var currentUserObj = Application.Current.Properties["LoggedInUser"];
            if (currentUserObj == null)
            {
                MessageBox.Show("Bạn chưa đăng nhập. Vui lòng đăng nhập lại.");
                return;
            }
            User currentUser = currentUserObj as User;
            if (currentUser == null)
            {
                MessageBox.Show("Lỗi dữ liệu người dùng.");
                return;
            }

            var notifications = _context.Notifications
                .Where(n => n.UserId == currentUser.UserId && n.IsDelete == false)
                .OrderByDescending(n => n.SentDate)
                .ToList();

            NotificationsDataGrid.ItemsSource = notifications;
        }

        private void NotificationsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.AddedItems)
            {
                if (item is Notification notification)
                {
                    if (notification.IsRead != true)
                    {
                        notification.IsRead = true;
                        _context.SaveChanges();
                        LoadNotifications();
                    }
                }
            }
        }
    }
}
