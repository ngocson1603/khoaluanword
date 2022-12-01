using GameStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.ViewModels
{
    public class CheckOutViewModel
    {
        public Account Account { get; set; }
        public Fund Fund { get; set; }
    }
}
