using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCSH1_Semprace_Frantsev.Model
{
    public class Category : ObservableObject
    {
        private string name;
        private string? description;

        public int Id { get; set; }
        public string Name { get => name; set { name = value; OnPropertyChanged(nameof(Name)); } }
        public string? Description { get => description; set { description = value; OnPropertyChanged(nameof(Description)); } }

        public static List<Category> ChangedCategories { get; } = new List<Category>();

        public Category()
        {
            PropertyChanged += Category_PropertyChanged;
        }

        private void Category_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!ChangedCategories.Contains(this))
                ChangedCategories.Add(this);
        }

        public override string? ToString() => Name ?? "A category without name";
    }
}
