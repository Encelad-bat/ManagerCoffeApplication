using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SellerCoffeApplication.Model
{
    class Coffe : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public int Quanity { get; set; }
        public string Type { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
