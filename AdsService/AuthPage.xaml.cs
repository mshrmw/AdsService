using Microsoft.IdentityModel.Tokens;
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
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void BtAuth_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(loginTextBox.Text) || string.IsNullOrEmpty(passwordPassBox.Password))
            {
                MessageBox.Show("Введите логин или пароль.");
                return;
            }
            using (var db = new PrivateAdsServiceEntities()) 
            {
                var user = db.user.AsNoTracking().FirstOrDefault(u => u.user_login == loginTextBox.Text && u.user_password == passwordPassBox.Password);
                if (user == null)
                {
                    MessageBox.Show("Пользователь с такими данными не найден!");
                    return;
                }
                else
                {
                    MessageBox.Show("Пользователь успешно найден!");
                    NavigationService?.Navigate(new UsersPage(user));
                }
            }
        }

        private void BtViewProduct_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new PageViewAdsWithoutAuth());
        }
    }
}
