using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace AdsService
{
    /// <summary>
    /// Логика взаимодействия для UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page
    {
        private user currentUser;
        public UsersPage(user user)
        {
            InitializeComponent();
            currentUser = user;
            var currentAd = PrivateAdsServiceEntities.GetContext().ad.Include(a => a.user).Include(a => a.city).Include(a => a.category).Include(a => a.type).Include(a => a.status).Where(a => a.id_user == currentUser.id).ToList();
            ListAds.ItemsSource = currentAd;

        }

        private void editAdBt_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addAdBt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AddAdPage());
        }

        private void deleteAdBt_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
