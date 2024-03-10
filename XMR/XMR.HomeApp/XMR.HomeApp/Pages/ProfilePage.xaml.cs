using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using XMR.HomeApp.Models;

namespace XMR.HomeApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        /// <summary>
        /// Модель пользовательских данных
        /// </summary>
        public UserInfo UserInfo { get; set; }
        public ProfilePage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Вызывается до того, как элемент становится видимым
        /// </summary>
        protected override void OnAppearing()
        {
            // Проверяем, есть ли в словаре значение
            if (App.Current.Properties.TryGetValue("CurrentUser", out object user))
            {
                UserInfo = user as UserInfo;
                loginEntry.Text = UserInfo.Name;
                emailEntry.Text = UserInfo.Email;
            }
            else
            {
                // Добавляем, если нет
                UserInfo = new UserInfo();
                App.Current.Properties.Add("CurrentUser", UserInfo);
            }
            // Получим значения ползунков из Preferences.
            // Если значений нет - установим значения по умолчанию (false)
            gasSwitch.On = Preferences.Get("gasState", false);
            climateSwitch.On = Preferences.Get("climateState", false);
            electroSwitch.On = Preferences.Get("electroState", false);

            base.OnAppearing();
        }
        /// <summary>
        ///  Сохраним информацию о пользователе
        /// </summary>
        private async void saveButton_Clicked(object sender, System.EventArgs e)
        {
            UserInfo.Name = loginEntry.Text;
            UserInfo.Email = emailEntry.Text;
            // Сохраним значения ползунков в настройки.
            Preferences.Set("gasState", gasSwitch.On);
            Preferences.Set("climateState", climateSwitch.On);
            Preferences.Set("electroState", electroSwitch.On);
            await Navigation.PopAsync();
        }
        /// <summary>
        /// Покажем уведомление с предупреждением, если пользователю выдаются сразу все доступы.
        /// </summary>
        private void NotifyUser(object sender, ToggledEventArgs e)
        {
            if (gasSwitch.On && climateSwitch.On && electroSwitch.On)
                DisplayAlert("Внимание!", "Пользователь получит полный доступ ко всем системам", "OK");
        }
    }
}