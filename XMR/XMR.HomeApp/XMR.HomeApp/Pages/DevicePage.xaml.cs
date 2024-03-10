using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XMR.HomeApp.Models;

namespace XMR.HomeApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DevicePage : ContentPage
    {
        public static string PageName { get; set; }
        public static bool CreateNew { get; set; }

        // Ссылка на модель
        public HomeDevice HomeDevice { get; set; }
        public DevicePage(string pageName, HomeDevice homeDevice = null)
        {
            PageName = pageName;

            if (homeDevice == null)
            {
                HomeDevice = new HomeDevice(); 
                CreateNew= true;
            }
            else
            {
                HomeDevice = homeDevice;
                CreateNew = false;
            }

            InitializeComponent();
            OpenEditor();
        }
        public void OpenEditor()
        {
            // Создание однострочного текстового поля для названия
            var newDeviceName = new Entry
            {
                BackgroundColor = Color.AliceBlue,
                Margin = new Thickness(30, 10),
                Placeholder = "Название",
                Text = HomeDevice.Name,
                Style = (Style)App.Current.Resources["ValidInputStyle"],
            };
            newDeviceName.TextChanged += (sender, e) => InputTextChanged(sender, e, newDeviceName);
            stackLayout.Children.Add(newDeviceName);

            // Создание многострочного поля для описания
            var newDeviceDescription = new Editor
            {
                HeightRequest = 200,
                BackgroundColor = Color.AliceBlue,
                Margin = new Thickness(30, 10),
                Placeholder = "Описание",
                Text = HomeDevice.Description,
                Style = (Style)App.Current.Resources["ValidInputStyle"]
            };
            newDeviceDescription.TextChanged += (sender, e) => InputTextChanged(sender, e, newDeviceDescription);
            stackLayout.Children.Add(newDeviceDescription);

            // Создаем заголовок для переключателя
            var switchHeader = new Label { Text = "Не использует газ", HorizontalOptions = LayoutOptions.Center, Margin = new Thickness(0, 5, 0, 0) };
            stackLayout.Children.Add(switchHeader);

            // Создаем переключатель
            Switch switchControl = new Switch
            {
                IsToggled = false,
                HorizontalOptions = LayoutOptions.Center,
                ThumbColor = Color.DodgerBlue,
                OnColor = Color.LightSteelBlue,
            };
            stackLayout.Children.Add(switchControl);

            var roomPicker = new Picker()
            {
                Margin = new Thickness(30, 0)
            };
            roomPicker.Items.Add("Кухня");
            roomPicker.Items.Add("Ванная");
            roomPicker.Items.Add("Гостиная");
            
            roomPicker.SelectedItem = roomPicker.Items.FirstOrDefault(i => i == HomeDevice.Room);
            
            roomPicker.SelectedIndexChanged += (sender, eventArgs) => RoomPicker_SelectedIndexChanged(sender, eventArgs, roomPicker);
            stackLayout.Children.Add(roomPicker);
            // Регистрируем обработчик события переключения
            switchControl.Toggled += (sender, e) => SwitchHandler(sender, e, switchHeader);

            // Кнопка вызова страницы для вывода инструкции
            var userManualButton = new Button
            {
                Text = "Инструкция по эксплуатации",
                Margin = new Thickness(30, 10),
                BackgroundColor = Color.Silver,
            };
            userManualButton.Clicked += (sender, eventArgs) => ManualButtonClicked(sender, eventArgs);
            // Кнопка сохранения с обработчиками
            var addButton = new Button
            {
                Text = "Сохранить",
                Margin = new Thickness(30, 10),
                BackgroundColor = Color.Silver,
            };
            addButton.Clicked += (sender, eventArgs) => SaveButtonClicked(sender, eventArgs, new View[] { newDeviceName, newDeviceDescription, switchControl });

            stackLayout.Children.Add(userManualButton);
            stackLayout.Children.Add(addButton);
        }
        /// <summary>
        /// Обновляем комнату в модели
        /// </summary>
        private void RoomPicker_SelectedIndexChanged(object sender, EventArgs e, Picker picker)
        {
            HomeDevice.Room = picker.Items[picker.SelectedIndex];
        }
        private async void ManualButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DeviceManualPage(HomeDevice.Name, HomeDevice.Id));
        }
        /// <summary>
        /// Кнопка сохранения деактивирует все контролы
        /// </summary>
        private async void SaveButtonClicked(object sender, EventArgs e, View[] views)
        {
            if(String.IsNullOrEmpty(HomeDevice.Room))
            {
                await DisplayAlert("Выберите комнату", $"Комната подключения не выбрана!", "ОК");
                return;
            }
            foreach (var view in views)
                view.IsEnabled = false;

            if (CreateNew)
            {
                // Если нужно создать новое - то сначала выполним проверку, не существует ли ещё такое.
                var existingDevices = await App.HomeDevices.GetHomeDevices();
                if (existingDevices.Any(d => d.Name == HomeDevice.Name))
                {
                    await DisplayAlert("Ошибка", $"Устройство {HomeDevice.Name} уже подключено.{Environment.NewLine}Выберите другое имя.", "ОК");
                }
                else
                {
                    var newDeviceDto = App.Mapper.Map<Data.Tables.HomeDevice>(HomeDevice);
                    await App.HomeDevices.AddHomeDevice(newDeviceDto);

                    // Пример другого способа навигации - с помощью удаления предыдущей страницы из стека и "вставки" (дано для демонстрации возможностей)
                    Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                    Navigation.InsertPageBefore(new DeviceListPage(), this);
                    await Navigation.PopAsync();
                }
                return;
            }

            var updatedDevice = App.Mapper.Map<Data.Tables.HomeDevice>(HomeDevice);
            await App.HomeDevices.UpdateHomeDevice(updatedDevice);
            await Navigation.PopAsync();
        }
        /// <summary>
        /// Обработка переключателя
        /// </summary>
        public void SwitchHandler(object sender, ToggledEventArgs e, Label header)
        {
            if (!e.Value)
            {
                header.Text = "Не использует газ";
                return;
            }

            header.Text = "Использует газ";
        }
        ///
        private void InputTextChanged(object sender, TextChangedEventArgs e, InputView view)
        {
            if (view is Entry)
            {
                HomeDevice.Name = view.Text;
            }
            else
            {
                HomeDevice.Description = view.Text;
            }
        }
    }
}