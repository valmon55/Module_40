﻿using AutoMapper;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XMR.HomeApp.Clients;
using XMR.HomeApp.Data.Tables;
using XMR.HomeApp.Pages;

namespace XMR.HomeApp
{
    public partial class App : Application
    {
        // Инициализация репозитория
        public static HomeDeviceRepository HomeDevices = new HomeDeviceRepository(
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                $"homedevices.db")
            );
        /// <summary>
        /// Инициализация Api-клиента для использования во всех частях приложения
        /// </summary>
        public static HomeApiClient ApiClient = new HomeApiClient("http://10.0.2.2:5001");
        public static IMapper Mapper { get; set; }
        public App()
        {
            Mapper = CreateMapper();
            InitializeComponent();

            //MainPage = new DeviceListPage();                        
            MainPage = new NavigationPage(new LoginPage());
        }
        /// <summary>
        /// Создание Автомаппера для преобразования сущностей
        /// </summary>
        public static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Data.Tables.HomeDevice, Models.HomeDevice>();
                cfg.CreateMap<Models.HomeDevice, Data.Tables.HomeDevice>();
                // Маппинг внешнего контракта API во внутреннюю модель
                cfg.CreateMap<HomeApi.Contracts_net_standart2.Models.Home.AddressInfo, Models.Address>();
                cfg.CreateMap<HomeApi.Contracts_net_standart2.Models.Home.InfoResponse, Models.HouseInfo>();
            });

            return config.CreateMapper();
        }
        protected async override void OnStart()
        {
            await HomeDevices.InitDatabase();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
