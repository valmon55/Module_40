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
        public ProfilePage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Вызывается до того, как элемент становится видимым
        /// </summary>
        protected async override void OnAppearing()
        {
            // Проверяем доступность сетевого соединения
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                // При наличии - запрашиваем данные с сервера
                await GetHouseInfo();
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
            // Сохраним значения ползунков в настройки.
            Preferences.Set("gasState", gasSwitch.On);
            Preferences.Set("climateState", climateSwitch.On);
            Preferences.Set("electroState", electroSwitch.On);

            // Возврат на предыдущую страницу
            await Navigation.PopAsync();
        }

        /// <summary>
        /// Загружает информацию о строении
        /// </summary>
        private async Task GetHouseInfo()
        {
            // Получим информацию с помощью клиента
            var inforResponse = await App.ApiClient.GetInfo();
            // Маппинг внешней модели данных во внутреннюю   
            var houseInfo = App.Mapper.Map<HouseInfo>(inforResponse);
            houseInfo.Address = App.Mapper.Map<Address>(inforResponse.AddressInfo);

            // Проставляем нужные значения, полученные с сервера
            addressEntry.Text = $" {houseInfo.Address.Street} {houseInfo.Address.House}/{houseInfo.Address.Building}";
            telephoneEntry.Text = $" {houseInfo.Telephone}";
            areaEntry.Text = $" {houseInfo.Area} кв. м.";
            voltageEntry.Text = $" {houseInfo.CurrentVolts} в";
            heatingEntry.Text = $" {houseInfo.Heating}";
        }
    }
}