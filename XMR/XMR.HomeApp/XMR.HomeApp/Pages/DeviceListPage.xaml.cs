using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XMR.HomeApp.Models;

namespace XMR.HomeApp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DeviceListPage : ContentPage
	{
        public List<HomeDevice> Devices { get; set; } = new List<HomeDevice>();
        public DeviceListPage ()
		{
			InitializeComponent ();

            // Заполняем список устройств
            Devices.Add(new HomeDevice("Чайник", description: "LG, объем 2л.", image: "Chainik.png"));
            Devices.Add(new HomeDevice("Стиральная машина", description: "BOSCH", image: "StiralnayaMashina.png"));
            Devices.Add(new HomeDevice("Посудомоечная машина", description: "Gorenje", image: "PosudomoechnayaMashina.png"));
            Devices.Add(new HomeDevice("Мультиварка", description: "Philips", image: "Multivarka.png"));

            BindingContext = this;
        }
        /// <summary>
        /// Обработчик нажатия
        /// </summary>
        private void deviceList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            // распаковка модели из объекта
            var tappedDevice = (HomeDevice)e.Item;
            // уведомление
            DisplayAlert("Нажатие", $"Вы нажали на элемент {tappedDevice.Name}", "OK");
        }

        /// <summary>
        /// Обработчик выбора
        /// </summary>
        private void deviceList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // распаковка модели из объекта
            var selectedDevice = (HomeDevice)e.SelectedItem;
            // уведомление
            DisplayAlert("Выбор", $"Вы выбрали {selectedDevice.Name},{selectedDevice.Description}", "OK");
        }
    }
}