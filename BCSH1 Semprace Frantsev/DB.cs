using BCSH1_Semprace_Frantsev.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Data.Entity.Infrastructure.Design.Executor;
using Xceed.Wpf.AvalonDock.Themes;

namespace BCSH1_Semprace_Frantsev
{

    class DB
    {
        private static string connectionString = "Data Source=D:\\Projects\\BCSH1 Semprace Frantsev\\bcsh1_semprace.db;Version=3;";
        private SQLiteConnection connection = new(connectionString);

        //Methods for fetching data from database
        public ObservableCollection<Product> FetchProducts(ObservableCollection<Brand> brands, ObservableCollection<Category> categories)
        {
            var products = new ObservableCollection<Product>();
            try
            {
                ;
                SQLiteCommand command = new("SELECT * FROM Product", connection);
                connection.Open();

                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    string? description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader["Description"].ToString();
                    double price = reader.GetDouble(3);
                    int brandId = reader.GetInt32(4);
                    int categoryId = reader.GetInt32(5);

                    Product product = new(id, name, description, price, categoryId, brandId);
                    product.Brand = brands.First(brand => brand.Id == product.BrandId);
                    product.Category = categories.First(category => category.Id == product.CategoryId);

                    products.Add(product);
                }
                connection.Close();
                return products;
            }
            catch
            {
                connection.Close();
                return products;
            }

        }

        public ObservableCollection<Product> FetchProducts()
        {
            var products = new ObservableCollection<Product>();
            try
            {
                ;
                SQLiteCommand command = new("SELECT * FROM Product", connection);
                connection.Open();

                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    string? description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader["Description"].ToString();
                    double price = reader.GetDouble(3);
                    int brandId = reader.GetInt32(4);
                    int categoryId = reader.GetInt32(5);

                    Product product = new(id, name, description, price, categoryId, brandId);
                    products.Add(product);
                }
                connection.Close();
                return products;
            }
            catch
            {
                connection.Close();
                return products;
            }
        }

        public ObservableCollection<Brand> FetchBrands()
        {
            var brands = new ObservableCollection<Brand>();
            try
            {

                SQLiteCommand command = new("SELECT * FROM Brand", connection);
                connection.Open();

                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);

                    Brand brand = new() { Id = id, Name = name };
                    brands.Add(brand);
                }

                connection.Close();
                return brands;
            }
            catch
            {
                connection.Close();
                return brands;
            }
        }

        public ObservableCollection<Category> FetchCategories()
        {
            var categories = new ObservableCollection<Category>();
            try
            {
                SQLiteCommand command = new("SELECT * FROM Category", connection);
                connection.Open();

                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    string? description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader["Description"].ToString();

                    Category category = new() { Id = id, Name = name, Description = description };
                    categories.Add(category);
                }
                connection.Close();

                return categories;
            }
            catch
            {
                connection.Close();
                return categories;
            }

        }


        public ObservableCollection<Discount> FetchDiscounts(ObservableCollection<Product> products)
        {
            var discounts = new ObservableCollection<Discount>();
            try
            {
                SQLiteCommand command = new("SELECT * FROM Discount", connection);
                connection.Open();

                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    double percentage = reader.GetDouble(1);
                    string startDate = reader.GetString(2);
                    string endDate = reader.GetString(3);
                    int productId = reader.GetInt32(4);

                    DateTimeOffset startDateTimeOffset = DateTimeOffset.ParseExact(startDate, "yyyy-MM-ddTHH:mm:ss.fffffffzzz", CultureInfo.InvariantCulture);
                    DateTime startDateTime = startDateTimeOffset.DateTime;
                    DateTimeOffset endDateTimeOffset = DateTimeOffset.ParseExact(endDate, "yyyy-MM-ddTHH:mm:ss.fffffffzzz", CultureInfo.InvariantCulture);
                    DateTime endDateTime = endDateTimeOffset.DateTime;

                    Discount discount = new() { Id = id, DiscountPercentage = percentage, StartDate = startDateTime, EndDate = endDateTime, ProductId = productId };
                    discount.Product = products.First(s => s.Id == discount.ProductId);
                    discounts.Add(discount);
                }

                connection.Close();
                return discounts;
            }
            catch
            {
                connection.Close();
                return discounts;
            }
        }

        public async void AddProduct(Product product)
        {
            try
            {
                SQLiteCommand command = new($"INSERT INTO Product (Id, Name, Description, Price, Category, Brand) " +
                    $"VALUES (null,'{product.Name}','{product.Description}',{product.Price}, {product.CategoryId},{product.BrandId});", connection);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                await connection.CloseAsync();
            }
            catch (Exception)
            {
                await connection.CloseAsync();
                return;
            }
        }

        public async void AddBrand(Brand brand)
        {
            try
            {
                SQLiteCommand command = new($"INSERT INTO Brand (Id, Name) " +
                    $"VALUES (null,'{brand.Name}');", connection);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                await connection.CloseAsync();
            }
            catch (Exception)
            {
                await connection.CloseAsync();
                return;
            }
        }

        public async void AddCategory(Category category)
        {
            try
            {
                SQLiteCommand command = new($"INSERT INTO Category (Id, Name, Description) " +
                    $"VALUES (null,'{category.Name}', '{category.Description}');", connection);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                await connection.CloseAsync();
            }
            catch (Exception)
            {
                await connection.CloseAsync();
                return;
            }
        }

        public async void AddDiscount(Discount discount)
        {
            try
            {
                SQLiteCommand command = new($"INSERT INTO Discount ( Id, Percentage, Start_date, End_date, ProductId ) " +
                    $"VALUES ( null,  {discount.DiscountPercentage}, '{discount.StartDate:O}', '{discount.EndDate:O}', {discount.ProductId} );", connection);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                await connection.CloseAsync();
            }
            catch (Exception)
            {
                await connection.CloseAsync();
                return;
            }
        }

        public async void DeleteProduct(int productId)
        {
            try
            {
                SQLiteCommand command = new($"DELETE FROM Product WHERE Id = {productId}", connection);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                await connection.CloseAsync();
            }
            catch
            {
                await connection.CloseAsync();
                return;
            }

        }

        public async void DeleteBrand(int brandId)
        {
            try
            {
                SQLiteCommand command = new($"DELETE FROM Brand WHERE Id = {brandId}", connection);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                await connection.CloseAsync();
            }
            catch (Exception)
            {
                await connection.CloseAsync();
                return;
            }
        }

        public async void DeleteCategory(int categoryId)
        {
            try
            {
                SQLiteCommand command = new($"DELETE FROM Category WHERE Id = {categoryId}", connection);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                await connection.CloseAsync();
            }
            catch (Exception)
            {
                await connection.CloseAsync();
                return;
            }
        }

        public async void DeleteDiscount(int discountId)
        {
            try
            {
                SQLiteCommand command = new($"DELETE FROM Discount WHERE Id = {discountId}", connection);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                await connection.CloseAsync();
            }
            catch (Exception)
            {
                await connection.CloseAsync();
                return;
            }
        }

        public async void UpdateProducts(IEnumerable<Product> products)
        {
            try
            {
                await connection.OpenAsync();
                foreach (var product in products)
                {
                    SQLiteCommand command = new($"UPDATE Product SET Name = '{product.Name}', Description = '{product.Description}', " +
                        $"Price = {product.Price} , Category = {product.CategoryId}, Brand = {product.BrandId} WHERE Id = {product.Id};", connection);
                    await command.ExecuteNonQueryAsync();
                };
                await connection.CloseAsync();
            }
            catch (Exception)
            {
                await connection.CloseAsync();
                return;
            }
        }
        public async void UpdateBrands(IEnumerable<Brand> brands)
        {
            try
            {
                await connection.OpenAsync();
                foreach (var brand in brands)
                {
                    SQLiteCommand command = new($"UPDATE Brand SET Name = '{brand.Name}' WHERE Id = {brand.Id};", connection);
                    await command.ExecuteNonQueryAsync();
                };
                await connection.CloseAsync();
            }
            catch (Exception)
            {
                await connection.CloseAsync();
                return;
            }
        }
        public async void UpdateCategories(IEnumerable<Category> categories)
        {
            try
            {
                await connection.OpenAsync();
                foreach (var category in categories)
                {
                    SQLiteCommand command = new($"UPDATE Category SET Name = '{category.Name}'," +
                        $" Description = '{category.Description}' WHERE Id = {category.Id};", connection);
                    await command.ExecuteNonQueryAsync();
                };
                await connection.CloseAsync();
            }
            catch (Exception)
            {
                await connection.CloseAsync();
                return;
            }
        }
        public async void UpdateDiscounts(IEnumerable<Discount> discounts)
        {
            try
            {
                await connection.OpenAsync();
                foreach (var discount in discounts)
                {
                    SQLiteCommand command = new($"UPDATE Discount SET Percentage = '{discount.DiscountPercentage}', " +
                        $"Start_date = '{discount.StartDate:O}', End_date = '{discount.EndDate:O}', ProductId = '{discount.ProductId}' WHERE Id = '{discount.Id}';", connection);
                    await command.ExecuteNonQueryAsync();
                };
                await connection.CloseAsync();
            }
            catch (Exception)
            {
                await connection.CloseAsync();
                return;
            }
        }
    }
}
