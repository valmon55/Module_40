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
    public partial class LoginPage : ContentPage
    {
        public const string BUTTON_TEXT = "Войти";
        public static int loginCounter = 0;
        // Создаем объект, возвращающий свойства устройства
        IDeviceDetector detector = DependencyService.Get<IDeviceDetector>();
        public LoginPage()
        {
            InitializeComponent();
            //if(Device.RuntimePlatform == Device.UWP)
            //{
            //    loginButton.CornerRadius = 0;
            //}
            if(Device.Idiom == TargetIdiom.Desktop)
            {
                loginButton.CornerRadius = 0;
            }
            // Передаем информацию о платформе на экран
            //runningDevice.Text = detector.GetDevice();

            // Устанавливаем динамический ресурс с помощью специально метода
            infoMessage.SetDynamicResource(Label.TextColorProperty, "errorColor");
        }
        /// <summary>
        /// По клику обрабатываем счётчик и выводим разные сообщения
        /// </summary>
        private async void login_Click(object sender, EventArgs e)
        {
            loginButton.Text = $"Выполняется вход..";
            await Task.Delay(150);
            await Navigation.PushAsync(new DeviceListPage());
            loginButton.Text = BUTTON_TEXT;
        }
    }
}