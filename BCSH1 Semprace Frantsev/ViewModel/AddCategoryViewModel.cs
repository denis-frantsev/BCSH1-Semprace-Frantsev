using BCSH1_Semprace_Frantsev.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BCSH1_Semprace_Frantsev.ViewModel
{
    public class AddCategoryViewModel : ObservableObject
    {
        private readonly DB _db;
        private RelayCommand? _addCategoryCommand;
        private string? _errMessage;

        public string? Name { get; set; }
        public string? Description { get; set; }

        public string? ErrMessage { get => _errMessage; private set => SetProperty(ref _errMessage, value); }

        public AddCategoryViewModel()
        {
            _db = new();
        }

        public RelayCommand? AddCategoryCommand
        {
            get => _addCategoryCommand ??= new RelayCommand(AddBrand);
            set => _addCategoryCommand = value;
        }

        private void AddBrand()
        {
            if (string.IsNullOrEmpty(Name))
                ErrMessage = "Name is required";
            else
            {
                ErrMessage = string.Empty;
                var category = new Category() { Name = Name, Description = Description };
                _db.AddCategory(category);
                var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
                window?.Close();
            }

        }
    }
}
