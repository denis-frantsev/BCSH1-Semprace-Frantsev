using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BCSH1_Semprace_Frantsev.Model
{
    public class Discount : ObservableObject
    {
        private double discountPercentage = 0;
        private DateTime startDate = DateTime.Now;
        private DateTime endDate;
        private int productId;

        public int Id { get; set; }
        public double DiscountPercentage { get => discountPercentage; set { discountPercentage = value; OnPropertyChanged(nameof(DiscountPercentage)); } }
        public DateTime StartDate
        {
            get => startDate;
            set
            {
                if (value > EndDate)
                    endDate = value.AddDays(1);

                startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }
        public DateTime EndDate
        {
            get => endDate;
            set
            {
                if (value < StartDate)
                    startDate = value.AddDays(-1);

                endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }
        public int ProductId { get => productId; set { productId = value; OnPropertyChanged(nameof(ProductId)); } }
        public Product? Product { get; set; }

        public static List<Discount> ChangedDiscounts { get; } = new List<Discount>(); 

        public Discount(double discountPercentage, DateTime startDate, DateTime endDate, int productId)
        {
            DiscountPercentage = discountPercentage;
            StartDate = startDate;
            EndDate = endDate;
            ProductId = productId;
            PropertyChanged += Discount_PropertyChanged;
        }

        public Discount()
        {
            PropertyChanged += Discount_PropertyChanged;
        }

        private void Discount_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(!ChangedDiscounts.Contains(this))
                ChangedDiscounts.Add(this);
        }
    }
}
