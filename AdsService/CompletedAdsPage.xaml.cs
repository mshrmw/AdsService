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
using System.Data.Entity;


namespace AdsService
{
    public partial class CompletedAdsPage : Page
    {
        private user currentUser;

        public CompletedAdsPage(user user)
        {
            InitializeComponent();
            currentUser = user;
            var completedAds = PrivateAdsServiceEntities.GetContext().ad.Include(a => a.user).Include(a => a.city).Include(a => a.category).Include(a => a.type).Include(a => a.status).Where(a => a.id_user == currentUser.id && a.status.status_name == "Завершено").OrderByDescending(a => a.ad_post_date).ToList();
            ListCompletedAds.ItemsSource = completedAds;
            
        }
    }
}