using Microsoft.Build.Framework.XamlTypes;
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
            UpdateAds();

        }
        private void UpdateAds()
        {
            try
            {
                var currentAd = PrivateAdsServiceEntities.GetContext().ad.Include(a => a.user).Include(a => a.city).Include(a => a.category).Include(a => a.type).Include(a => a.status).Where(a => a.id_user == currentUser.id).OrderByDescending(a => a.ad_post_date).ToList();
                ListAds.ItemsSource = currentAd;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке объявлений: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void editAdBt_Click(object sender, RoutedEventArgs e)
        {
            if (ListAds.SelectedItem is ad selectedAd)
            {
                NavigationService.Navigate(new AddAdPage(currentUser, selectedAd));
            }
            else
            {
                MessageBox.Show("Выберите объявление для редактирования!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void addAdBt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AddAdPage(currentUser));
        }

        private void deleteAdBt_Click(object sender, RoutedEventArgs e)
        {
            if (ListAds.SelectedItem is ad selectedAd)
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить это объявление?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var context = PrivateAdsServiceEntities.GetContext();
                        var adToDelete = context.ad.FirstOrDefault(a => a.id == selectedAd.id);
                        if (adToDelete != null)
                        {
                            if (adToDelete.id_user != currentUser.id)
                            {
                                MessageBox.Show("Вы можете удалять только свои объявления!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            context.ad.Remove(adToDelete);
                            context.SaveChanges();

                            UpdateAds();

                            MessageBox.Show("Объявление успешно удалено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите объявление для удаления!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void completeAdBt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new CompletedAdsPage(currentUser));
        }
    }
}
