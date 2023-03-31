using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCSH1_Semprace_Frantsev.Model
{
    public class Brand : ObservableObject
    {
        private string name;

        public int Id { get; set; }
        public string Name { get => name; set { name = value; OnPropertyChanged(nameof(Name)); } }

        public static List<Brand> ChangedBrands { get; } = new List<Brand>();
        public Brand()
        {
            PropertyChanged += Brand_PropertyChanged;
        }

        private void Brand_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(!ChangedBrands.Contains(this))
                ChangedBrands.Add(this);
        }

        public override string? ToString() => Name ?? "Brand has no name";


    }
}
