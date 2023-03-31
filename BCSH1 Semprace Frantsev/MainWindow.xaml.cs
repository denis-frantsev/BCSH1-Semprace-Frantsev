using BCSH1_Semprace_Frantsev.Model;
using BCSH1_Semprace_Frantsev.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BCSH1_Semprace_Frantsev
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            MainViewModel productsViewModel = new();
            DataContext = productsViewModel;
            InitializeComponent();
        }

        private void ProductButton_Click(object sender, RoutedEventArgs e)
        {
            // Hide all other DataGrid controls
            CategoriesPanel.Visibility = Visibility.Collapsed;
            BrandsPanel.Visibility = Visibility.Collapsed;
            DiscountsPanel.Visibility = Visibility.Collapsed;

            // Show the Products DataGrid control
            ProductsPanel.Visibility = Visibility.Visible;
        }

        private void BrandButton_Click(object sender, RoutedEventArgs e)
        {
            // Hide all other DataGrid controls
            CategoriesPanel.Visibility = Visibility.Collapsed;
            ProductsPanel.Visibility = Visibility.Collapsed;
            DiscountsPanel.Visibility = Visibility.Collapsed;

            // Show the Brands DataGrid control
            BrandsPanel.Visibility = Visibility.Visible;
        }


        private void CategoryButton_Click(object sender, RoutedEventArgs e)
        {
            // Hide all other DataGrid controls
            BrandsPanel.Visibility = Visibility.Collapsed;
            ProductsPanel.Visibility = Visibility.Collapsed;
            DiscountsPanel.Visibility = Visibility.Collapsed;

            // Show the Category DataGrid control
            CategoriesPanel.Visibility = Visibility.Visible;
        }

        private void DiscountButton_Click(object sender, RoutedEventArgs e)
        {
            // Hide all other DataGrid controls
            BrandsPanel.Visibility = Visibility.Collapsed;
            ProductsPanel.Visibility = Visibility.Collapsed;
            CategoriesPanel.Visibility = Visibility.Collapsed;

            // Show the Category DataGrid control
            DiscountsPanel.Visibility = Visibility.Visible;
        }

        private void ProductsGrid_LostFocus(object sender, RoutedEventArgs e)
        {

        }
    }
}
