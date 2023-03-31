using BCSH1_Semprace_Frantsev.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace BCSH1_Semprace_Frantsev.ViewModel
{

    public class AddDiscountViewModel : ObservableObject
    {
        private readonly DB _db;
        private RelayCommand? _addDiscountCommand;
        private string? _errMessage;
        public double Percentage { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(1);
        public int ProductId { get; set; } = 1;
        public string? ErrMessage { get => _errMessage; private set => SetProperty(ref _errMessage, value); }

        public ObservableCollection<Product> Products => _db.FetchProducts();
        public AddDiscountViewModel()
        {
            _db = new();
        }

        public RelayCommand? AddDiscountCommand
        {
            get => _addDiscountCommand ??= new RelayCommand(AddDiscount);
            set => _addDiscountCommand = value;
        }

        private void AddDiscount()
        {
            if (Percentage <= 0)
                ErrMessage = "Discount percentage must be greater than 0";
            else if (StartDate > EndDate)
                ErrMessage = "Discount's start date cant be earlier than the end date";
            else
            {
                ErrMessage = string.Empty;
                var discount = new Discount(Percentage, StartDate,EndDate,ProductId);
                _db.AddDiscount(discount);
                var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
                window?.Close();
            }           
        }
    }
}
