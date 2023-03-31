using BCSH1_Semprace_Frantsev.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BCSH1_Semprace_Frantsev.ViewModel
{
    public class AddBrandViewModel : ObservableObject
    {

        private readonly DB _db;
        private RelayCommand? _addBrandCommand;
        private string? _errMessage;

        public string? Name { get; set; }
        public string? ErrMessage { get => _errMessage; private set => SetProperty(ref _errMessage, value); }

        public AddBrandViewModel()
        {
            _db = new();
        }

        public RelayCommand? AddBrandCommand
        {
            get => _addBrandCommand ??= new RelayCommand(AddBrand) ;
            set => _addBrandCommand = value;
        }

        private void AddBrand()
        {
            if (string.IsNullOrEmpty(Name))
                ErrMessage = "Name is required";
            else
            {
                ErrMessage = string.Empty;
                var brand = new Brand() {Name = Name};
                _db.AddBrand(brand);
                var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
                window?.Close();
            }

        }
    }
}
