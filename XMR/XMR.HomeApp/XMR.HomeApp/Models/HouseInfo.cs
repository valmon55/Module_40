using HomeApi.Contracts.Models.Home;
using System;
using System.Collections.Generic;
using System.Text;

namespace XMR.HomeApp.Models
{
    public class HouseInfo
    {
        public int FloorAmount { get; set; }
        public string Telephone { get; set; }
        public string Heating { get; set; }
        public int CurrentVolts { get; set; }
        public bool GasConnected { get; set; }
        public int Area { get; set; }
        public string Material { get; set; }
        public Address Address { get; set; }
    }
}
