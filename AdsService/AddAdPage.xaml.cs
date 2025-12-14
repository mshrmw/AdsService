using Microsoft.Build.Framework.XamlTypes;
using Microsoft.IdentityModel.Tokens;
using Microsoft.SqlServer.Dac.Model;
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

namespace AdsService
{
    /// <summary>
    /// Логика взаимодействия для AddAdPage.xaml
    /// </summary>
    public partial class AddAdPage : Page
    {
        private ad currentAd;
        private user currentUser;
        private bool isEditMode = false;
        public AddAdPage(user user, ad Ad = null)
        {
            InitializeComponent();
            currentUser = user;
            if (Ad != null)
            {
                currentAd = Ad;
                isEditMode = true;
                DataContext = currentAd;
            }
            else
            {
                currentAd = new ad();
                currentAd.id_user = currentUser.id;
                currentAd.ad_post_date = DateTime.Now;
                DataContext = currentAd;
            }
            LoadComboBoxData();
            statusComboBox.SelectionChanged += StatusComboBox_SelectionChanged;
            completedPriceTextBlock.Visibility = Visibility.Collapsed;
            completedPriceTextBox.Visibility = Visibility.Collapsed;

        }
        private void LoadComboBoxData()
        {
            var context = PrivateAdsServiceEntities.GetContext();

            cityComboBox.ItemsSource = context.city.OrderBy(c => c.city_name).ToList();
            categoryComboBox.ItemsSource = context.category.OrderBy(c => c.category_name).ToList();
            typeComboBox.ItemsSource = context.type.OrderBy(t => t.type_name).ToList();
            statusComboBox.ItemsSource = context.status.OrderBy(s => s.status_name).ToList();

            if (!isEditMode)
            {
                var activeStatus = statusComboBox.ItemsSource.Cast<status>().FirstOrDefault(s => s.status_name == "Активно");
                if (activeStatus != null)
                {
                    statusComboBox.SelectedItem = activeStatus;
                    completedPriceTextBlock.Visibility = Visibility.Collapsed;
                    completedPriceTextBox.Visibility = Visibility.Collapsed;
                }
                else if (currentAd != null && currentAd.status != null && currentAd.status.status_name == "Завершено")
                {
                    completedPriceTextBlock.Visibility = Visibility.Visible;
                    completedPriceTextBox.Visibility = Visibility.Visible;
                }
            }
        }

        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (statusComboBox.SelectedValue != null)
            {
                int selectedStatusId = (int)statusComboBox.SelectedValue;
                var statusList = statusComboBox.ItemsSource as List<status>;
                if (statusList != null)
                {
                    var selectedStatus = statusList.FirstOrDefault(s => s.id == selectedStatusId);
                    if (selectedStatus != null)
                    {
                        if (selectedStatus.status_name == "Завершено")
                        {
                            completedPriceTextBlock.Visibility = Visibility.Visible;
                            completedPriceTextBox.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            completedPriceTextBlock.Visibility = Visibility.Collapsed;
                            completedPriceTextBox.Visibility = Visibility.Collapsed;
                            completedPriceTextBox.Text = string.Empty;
                        }
                    }
                }
            }
        }
        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(currentAd.ad_title))
                errors.AppendLine("Укажите название объявления!");

            if (string.IsNullOrWhiteSpace(currentAd.ad_description))
                errors.AppendLine("Укажите описание объявления!");

            if (currentAd.id_city == 0)
                errors.AppendLine("Укажите город!");

            if (currentAd.id_category == 0)
                errors.AppendLine("Укажите категорию объявления!");

            if (currentAd.id_type == 0)
                errors.AppendLine("Укажите тип объявления!");

            if (currentAd.id_status == 0)
                errors.AppendLine("Укажите статус объявления!");

            if (currentAd.ad_price <= 0)
                errors.AppendLine("Укажите корректную цену объявления!");

            if (statusComboBox.SelectedItem is status selectedStatus && selectedStatus.status_name == "Завершено")
            {
                if (string.IsNullOrWhiteSpace(completedPriceTextBox.Text) || !decimal.TryParse(completedPriceTextBox.Text, out decimal completedPrice) || completedPrice <= 0)
                {
                    errors.AppendLine("Для завершенного объявления укажите фактическую цену!");
                }
                else
                {
                    currentAd.ad_price = Convert.ToInt32(completedPrice);
                }
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var context = PrivateAdsServiceEntities.GetContext();

                if (currentAd.id == 0) 
                {
                    context.ad.Add(currentAd);
                }
                else
                {
                    context.Entry(currentAd).State = System.Data.Entity.EntityState.Modified;
                }

                context.SaveChanges();

                MessageBox.Show(isEditMode ? "Объявление успешно обновлено!" : "Объявление успешно добавлено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                NavigationService.Navigate(new UsersPage(currentUser));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}\n{ex.InnerException?.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtClear_Click(object sender, RoutedEventArgs e)
        {
            if (!isEditMode)
            {
                currentAd = new ad();
                currentAd.id_user = currentUser.id;
                currentAd.ad_post_date = DateTime.Now;
                DataContext = currentAd;

                var activeStatus = statusComboBox.ItemsSource.Cast<status>().FirstOrDefault(s => s.status_name == "Активно");
                if (activeStatus != null)
                    statusComboBox.SelectedItem = activeStatus;
            }
        }
    }
}