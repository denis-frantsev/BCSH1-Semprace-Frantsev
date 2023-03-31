using BCSH1_Semprace_Frantsev.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BCSH1_Semprace_Frantsev.ViewModel
{
    public  class AddProductViewModel : ObservableObject
    {
        private readonly DB _db;
        private RelayCommand? _addProductCommand;
        private string? _errMessage;
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; } = 1;
        public int BrandId { get; set; } = 1;
        public string? ErrMessage { get => _errMessage; private set => SetProperty(ref _errMessage, value); }
        public ObservableCollection<Brand> Brands => _db.FetchBrands();
        public ObservableCollection<Category> Categories => _db.FetchCategories();

        public AddProductViewModel()
        {
            _db = new();
        }

        public RelayCommand? AddProductCommand
        {
            get => _addProductCommand ??= new RelayCommand(AddProduct);
            set => _addProductCommand = value;
        }

        private void AddProduct()
        {
            if (string.IsNullOrEmpty(Name))
                ErrMessage = "Name is required";
            else if (Price <= 0)
                ErrMessage = "Price must be greater than zero";
            else
            {
                ErrMessage = string.Empty;
                var product = new Product(Name, Description, Price, CategoryId, BrandId);
                _db.AddProduct(product);
                var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
                window?.Close();
            }
        }

    }
}
