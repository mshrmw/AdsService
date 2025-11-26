using Microsoft.SqlServer.Dac.Model;
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
    /// Логика взаимодействия для PageViewAdsWithoutAuth.xaml
    /// </summary>
    public partial class PageViewAdsWithoutAuth : Page
    {
        public PageViewAdsWithoutAuth()
        {
            InitializeComponent();
            var currentAd = PrivateAdsServiceEntities.GetContext().ad.Include(a => a.user).Include(a => a.city).Include(a => a.category).Include(a => a.type).Include(a => a.status).ToList();
            ListAds.ItemsSource = currentAd;
            cityFilter.ItemsSource = PrivateAdsServiceEntities.GetContext().city.ToList();
            cityFilter.DisplayMemberPath = "city_name";
            categoryFilter.ItemsSource = PrivateAdsServiceEntities.GetContext().category.ToList();
            categoryFilter.DisplayMemberPath = "category_name";
            typeFilter.ItemsSource = PrivateAdsServiceEntities.GetContext().type.ToList();
            typeFilter.DisplayMemberPath = "type_name";
            statusFilter.ItemsSource = PrivateAdsServiceEntities.GetContext().status.ToList();
            statusFilter.DisplayMemberPath = "status_name";
        }
        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           UpdateAds();
        }
        private void cityFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAds();
        }
        private void categoryFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAds();
        }
        private void typeFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAds();
        }
        private void statusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAds();
        }
        private void UpdateAds()
        {
            if (!IsInitialized)
            {
                return;
            }
            try
            {
                List <ad> currentAd = PrivateAdsServiceEntities.GetContext().ad.Include(a => a.user).Include(a => a.city).Include(a => a.category).Include(a => a.type).Include(a => a.status).ToList();
                if (!string.IsNullOrWhiteSpace(searchTextBox.Text))
                {
                    currentAd = currentAd.Where(x => 
                    (x.ad_title.ToLower().Contains(searchTextBox.Text.ToLower())) ||
                    (x.ad_description.ToLower().Contains(searchTextBox.Text.ToLower())) ||
                    (x.user.user_login.ToLower().Contains(searchTextBox.Text.ToLower())) ||
                    (x.city.city_name.ToLower().Contains(searchTextBox.Text.ToLower())) ||
                    (x.category.category_name.ToLower().Contains(searchTextBox.Text.ToLower())) ||
                    (x.type.type_name.ToLower().Contains(searchTextBox.Text.ToLower())) ||
                    (x.status.status_name.ToLower().Contains(searchTextBox.Text.ToLower())) ||
                    (x.ad_price.ToString().Contains(searchTextBox.Text.ToLower()))).ToList();
                }
                if (cityFilter.SelectedItem is city selectedCity)
                {
                    currentAd = currentAd.Where(x => x.city != null && x.city.city_name == selectedCity.city_name).ToList();
                }
                if (categoryFilter.SelectedItem is category selectedCategory)
                {
                    currentAd = currentAd.Where(x => x.category != null && x.category.category_name == selectedCategory.category_name).ToList();
                }
                if (typeFilter.SelectedItem is type selectedType)
                {
                    currentAd = currentAd.Where(x => x.type != null && x.type.type_name == selectedType.type_name).ToList();
                }
                if (statusFilter.SelectedItem is status selectedStatus)
                {
                    currentAd = currentAd.Where(x => x.status != null && x.status.status_name == selectedStatus.status_name).ToList();
                }
                ListAds.ItemsSource = currentAd;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке объявлений: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearFilters_Click(object sender, RoutedEventArgs e)
        {
            searchTextBox.Text = string.Empty;
            cityFilter.SelectedValue = 0;
            typeFilter.SelectedValue = 0;
            categoryFilter.SelectedValue= 0;
            statusFilter.SelectedValue = 0;
        }

        private void BtAuth_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AuthPage());
        }
    }
}
