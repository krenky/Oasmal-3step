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
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.IO;
using System.Text.Json;

namespace Oasmal_3step
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataOrders.ItemsSource = Orders;
            DataProducts.ItemsSource = products;
        }
        Shop shop = new Shop(10);
        ObservableCollection<Order> Orders = new ObservableCollection<Order>();
        ObservableCollection<Product> products = new ObservableCollection<Product>();

        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            Order order = new Order(TextBoxName.Text);
            shop.AddOrder(order);
            Orders.Add(order);
        }

        private void DelOrder_Click(object sender, RoutedEventArgs e)
        {
            Order order = DataOrders.SelectedItem as Order;
            Orders.Remove(order);
        }

        private void DataOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Order order = shop.FindOrder(DataOrders.SelectedItem as Order);
            DataProducts.ItemsSource = order?.GetProduct();
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            Order order = shop.FindOrder(DataOrders.SelectedItem as Order);
            order.AddProduct(TextBoxProduct.Text, Convert.ToInt32(TextBoxPriceProduct.Text));
            DataProducts.ItemsSource = order?.GetProduct();
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            Product product = DataProducts.SelectedItem as Product;
            Order order = shop.FindOrder(DataOrders.SelectedItem as Order);
            order.DeleteProduct(product.Name);
            DataProducts.ItemsSource = order?.GetProduct();
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                if ((bool)saveFileDialog.ShowDialog())
                    using (FileStream fs = (FileStream)saveFileDialog.OpenFile())
                    {
                        JsonSerializer.Serialize<ObservableCollection<Order>>(new Utf8JsonWriter(fs), Orders);
                    }
            }
        }

        private async void LoadFile_ClickAsync(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if ((bool)openFileDialog.ShowDialog())
                using (FileStream fs = (FileStream)openFileDialog.OpenFile())
                {
                    //_Enterprises
                    ObservableCollection<Order> orders = await JsonSerializer.DeserializeAsync<ObservableCollection<Order>>(fs);
                    shop = new Shop(10);
                    foreach (var i in orders)
                    {
                        Order order = new Order(i.LastName);
                        shop.AddOrder(order);
                        Orders.Add(order);
                        Product product = i.Head.Next;
                        while(product != null)
                        {
                            order.AddProduct(product.Name, product.Price);
                            product = product.Next;
                        }
                    }
                }

        }
    }
}
