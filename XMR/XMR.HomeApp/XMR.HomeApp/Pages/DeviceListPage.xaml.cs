using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XMR.HomeApp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DeviceListPage : ContentPage
	{
        public List<string> Devices { get; set; } = new List<string>();
        public DeviceListPage ()
		{
			InitializeComponent ();

            Devices.Add("Чайник");
            Devices.Add("Стиральная машина");
            Devices.Add("Посудомоечная машина");
            Devices.Add("Мультиварка");
            Devices.Add("Водонагреватель");
            Devices.Add("Плита");
            Devices.Add("Микроволновая печь");
            Devices.Add("Духовой шкаф");
            Devices.Add("Холодильник");
            Devices.Add("Увлажнитель воздуха");
            Devices.Add("Телевизор");
            Devices.Add("Пылесос");
            Devices.Add("музыкальный центр");
            Devices.Add("Компьютер");
            Devices.Add("Игровая консоль");

            BindingContext = this;
        }
        /// <summary>
        /// Обработчик нажатия
        /// </summary>
        private void deviceList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            DisplayAlert("Нажатие", $"Вы нажали на элемент {e.Item}", "OK"); ; ;
        }

        /// <summary>
        /// Обработчик выбора
        /// </summary>
        private void deviceList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            DisplayAlert("Выбор", $"Вы выбрали {e.SelectedItem}", "OK"); ; ;
        }
    }
}