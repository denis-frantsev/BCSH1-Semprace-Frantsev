using BCSH1_Semprace_Frantsev.Model;
using BCSH1_Semprace_Frantsev.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BCSH1_Semprace_Frantsev.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        #region fields
        private readonly DB _db;

        private RelayCommand? _deleteProductCommand;
        private RelayCommand? _addProductCommand;
        private RelayCommand? _deleteBrandCommand;
        private RelayCommand? _addBrandCommand;
        private RelayCommand? _deleteCategoryCommand;
        private RelayCommand? _addCategoryCommand;
        private RelayCommand? _deleteDiscountCommand;
        private RelayCommand? _addDiscountCommand;
        private RelayCommand? _saveProductsCommand;
        private RelayCommand? _saveBrandsCommand;
        private RelayCommand? _saveCategoriesCommand;
        private RelayCommand? _saveDiscountsCommand;


        private Product? _selectedProduct;
        private Brand? _selectedBrand;
        private Category? _selectedCategory;
        private Discount? _selectedDiscount;

        private ObservableCollection<Product> _products;
        private ObservableCollection<Brand> _brands;
        private ObservableCollection<Category> _categories;
        private ObservableCollection<Discount> _discounts;

        private bool _isDirtyProductsGrid = false;
        private bool _isDirtyBrandsGrid = false;
        private bool _isDirtyCategoriesGrid = false;
        private bool _isDirtyDiscountsGrid = false;


        #endregion
        #region properties
        public ObservableCollection<Product> Products
        {
            get => _products;
            set
            {
                SetProperty(ref _products, value);
                foreach (var product in _products)
                {
                    product.PropertyChanged += (s, a) => IsDirtyProductsGrid = true;
                }
            }
        }

        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set
            {
                SetProperty(ref _categories, value);
                foreach (var category in _categories)
                {
                    category.PropertyChanged += (s, a) => IsDirtyCategoriesGrid = true;
                }
            }
        }
        public ObservableCollection<Brand> Brands
        {
            get => _brands;
            set
            {
                SetProperty(ref _brands, value);
                foreach (var brand in _brands)
                {
                    brand.PropertyChanged += (s, a) => IsDirtyBrandsGrid = true;
                }
            }
        }
        public ObservableCollection<Discount> Discounts
        {
            get => _discounts;
            set
            {
                SetProperty(ref _discounts, value);
                foreach (var discount in _discounts)
                {
                    discount.PropertyChanged += (s, a) => IsDirtyDiscountsGrid = true;
                }
            }
        }
        public int ProductCount => Products.Count;
        public int CategoryCount => Categories.Count;
        public int BrandCount => Brands.Count;
        public int DiscountCount => Discounts.Count;
        public bool IsProductDeleteEnabled => SelectedProduct != null;
        public bool IsBrandDeleteEnabled => SelectedBrand != null;
        public bool IsCategoryDeleteEnabled => SelectedCategory != null;
        public bool IsDiscountDeleteEnabled => SelectedDiscount != null;

        public bool IsDirtyProductsGrid
        {
            get => _isDirtyProductsGrid;
            set => SetProperty(ref _isDirtyProductsGrid, value);
        }
        public bool IsDirtyBrandsGrid
        {
            get => _isDirtyBrandsGrid;
            set => SetProperty(ref _isDirtyBrandsGrid, value);
        }
        public bool IsDirtyCategoriesGrid
        {
            get => _isDirtyCategoriesGrid;
            set => SetProperty(ref _isDirtyCategoriesGrid, value);
        }
        public bool IsDirtyDiscountsGrid
        {
            get => _isDirtyDiscountsGrid;
            set => SetProperty(ref _isDirtyDiscountsGrid, value);
        }
        #endregion
        #region commands
        public RelayCommand DeleteProductCommand
        {
            get => _deleteProductCommand ??= new RelayCommand(DeleteProduct);
            set => _deleteProductCommand = value;
        }

        public RelayCommand? AddProductCommand
        {
            get => _addProductCommand ??= new RelayCommand(AddProduct);
            set => _addProductCommand = value;
        }

        public RelayCommand DeleteBrandCommand
        {
            get => _deleteBrandCommand ??= new RelayCommand(DeleteBrand);
            set => _deleteBrandCommand = value;
        }

        public RelayCommand? AddBrandCommand
        {
            get => _addBrandCommand ??= new RelayCommand(AddBrand);
            set => _addBrandCommand = value;
        }

        public RelayCommand DeleteCategoryCommand
        {
            get => _deleteCategoryCommand ??= new RelayCommand(DeleteCategory);
            set => _deleteCategoryCommand = value;
        }

        public RelayCommand? AddCategoryCommand
        {
            get => _addCategoryCommand ??= new RelayCommand(AddCategory);
            set => _addCategoryCommand = value;
        }

        public RelayCommand DeleteDiscountCommand
        {
            get => _deleteDiscountCommand ??= new RelayCommand(DeleteDiscount);
            set => _deleteDiscountCommand = value;
        }

        public RelayCommand? AddDiscountCommand
        {
            get => _addDiscountCommand ??= new RelayCommand(AddDiscount);
            set => _addDiscountCommand = value;
        }

        public RelayCommand? SaveProductsCommand
        {
            get => _saveProductsCommand ??= new RelayCommand(SaveProducts);
            set => _saveProductsCommand = value;
        }

        public RelayCommand? SaveBrandsCommand
        {
            get => _saveBrandsCommand ??= new RelayCommand(SaveBrands);
            set => _saveBrandsCommand = value;
        }

        public RelayCommand? SaveCategoriesCommand
        {
            get => _saveCategoriesCommand ??= new RelayCommand(SaveCategories);
            set => _saveCategoriesCommand = value;
        }

        public RelayCommand? SaveDiscountsCommand
        {
            get => _saveDiscountsCommand ??= new RelayCommand(SaveDiscounts);
            set => _saveDiscountsCommand = value;
        }
        #endregion

        public MainViewModel()
        {

            _products = new();
            _brands = new();
            _categories = new();
            _discounts = new();
            _db = new();
            FetchData();
        }


        public Product? SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                SetProperty(ref _selectedProduct, value);
                OnPropertyChanged(nameof(IsProductDeleteEnabled));
            }
        }
        public Brand? SelectedBrand
        {
            get => _selectedBrand;
            set
            {
                SetProperty(ref _selectedBrand, value);
                OnPropertyChanged(nameof(IsBrandDeleteEnabled));
            }
        }
        public Category? SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                SetProperty(ref _selectedCategory, value);
                OnPropertyChanged(nameof(IsCategoryDeleteEnabled));
            }
        }
        public Discount? SelectedDiscount
        {
            get => _selectedDiscount;
            set
            {
                SetProperty(ref _selectedDiscount, value);
                OnPropertyChanged(nameof(IsDiscountDeleteEnabled));
            }
        }

        private void AddProduct()
        {
            var view = new AddProductDialog();
            var viewModel = new AddProductViewModel();
            view.DataContext = viewModel;
            view.ShowDialog();
            Products = _db.FetchProducts(Brands, Categories);
        }

        private void DeleteProduct()
        {
            if (SelectedProduct != null)
            {
                _db.DeleteProduct(SelectedProduct.Id);
                Products.Remove(SelectedProduct);
                SelectedProduct = null;
            }
        }

        private void AddBrand()
        {
            var view = new AddBrandDialog();
            var viewModel = new AddBrandViewModel();
            view.DataContext = viewModel;
            view.ShowDialog();
            Brands = _db.FetchBrands();
        }

        private void DeleteBrand()
        {
            if (SelectedBrand != null)
            {
                _db.DeleteBrand(SelectedBrand.Id);
                Brands.Remove(SelectedBrand);
                SelectedBrand = null;
            }
        }
        private void AddCategory()
        {
            var view = new AddCategoryDialog();
            var viewModel = new AddCategoryViewModel();
            view.DataContext = viewModel;
            view.ShowDialog();
            Categories = _db.FetchCategories();
        }

        private void DeleteCategory()
        {
            if (SelectedCategory != null)
            {
                _db.DeleteCategory(SelectedCategory.Id);
                Categories.Remove(SelectedCategory);
                SelectedCategory = null;
            }
        }

        private void AddDiscount()
        {
            var view = new AddDiscountDialog();
            var viewModel = new AddDiscountViewModel();
            view.DataContext = viewModel;
            view.ShowDialog();
            Discounts = _db.FetchDiscounts(Products);
        }

        private void DeleteDiscount()
        {
            if (SelectedDiscount != null)
            {
                _db.DeleteDiscount(SelectedDiscount.Id);
                Discounts.Remove(SelectedDiscount);
                SelectedDiscount = null;
            }
        }

        private void FetchData()
        {
            Brands = _db.FetchBrands();
            Categories = _db.FetchCategories();
            Products = _db.FetchProducts(Brands, Categories);
            Discounts = _db.FetchDiscounts(Products);
        }

        private void SaveProducts()
        {
            _db.UpdateProducts(Product.ChangedProducts);
            IsDirtyProductsGrid = false;
            Product.ChangedProducts.Clear();
        }
        private void SaveBrands()
        {
            _db.UpdateBrands(Brand.ChangedBrands);
            IsDirtyBrandsGrid = false;
            Brand.ChangedBrands.Clear();
        }
        private void SaveCategories()
        {
            _db.UpdateCategories(Category.ChangedCategories);
            IsDirtyCategoriesGrid = false;
            Category.ChangedCategories.Clear();
        }
        private void SaveDiscounts()
        {
            _db.UpdateDiscounts(Discount.ChangedDiscounts);
            IsDirtyDiscountsGrid = false;
            Discount.ChangedDiscounts.Clear();
        }
    }
}
