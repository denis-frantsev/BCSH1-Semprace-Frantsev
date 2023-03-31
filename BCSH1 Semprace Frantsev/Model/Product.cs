using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCSH1_Semprace_Frantsev.Model
{
    public class Product : ObservableObject
    {
        private string name;
        private double price;
        private string? description;
        private int categoryId;
        private int brandId;

        public int Id { get; set; }
        public string Name { get => name; set { name = value; OnPropertyChanged(nameof(Name)); } }
        public string? Description { get => description; set { description = value; OnPropertyChanged(nameof(Description)); } }
        public double Price { get => price; set { price = value; OnPropertyChanged(nameof(Price)); } }
        public int CategoryId { get => categoryId; set { categoryId = value; OnPropertyChanged(nameof(CategoryId)); } }
        public int BrandId { get => brandId; set { brandId = value; OnPropertyChanged(nameof(BrandId)); } }
        public Category? Category { get; set; }
        public Brand? Brand { get; set; }

        public static List<Product> ChangedProducts { get; } = new List<Product>();

        public Product(int id, string name, string? description, double price, int categoryID, int brandID)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            CategoryId = categoryID;
            BrandId = brandID;
            PropertyChanged += Product_PropertyChanged; ;
        }

        private void Product_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!ChangedProducts.Contains(this))
                ChangedProducts.Add(this); 
        }

        public Product(string name, string? description, double price, int categoryID, int brandID)
        {
            Name = name;
            Description = description;
            Price = price;
            CategoryId = categoryID;
            BrandId = brandID;
        }
    }
}
