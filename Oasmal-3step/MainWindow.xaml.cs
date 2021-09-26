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
            DataOrders.ItemsSource = Orders;         //Подключение коллекций данных к DataGrid
            DataProducts.ItemsSource = products;     //Подключение коллекций данных к DataGrid
        }
        Shop shop = new Shop(10);
        ObservableCollection<Order> Orders = new ObservableCollection<Order>();         //Создание коллекции ObservableCollection
        ObservableCollection<Product> products = new ObservableCollection<Product>();   //Создание коллекции ObservableCollection

        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            Order order = new Order(TextBoxName.Text); //Создание объекта класса Заказ
            shop.AddOrder(order); //Добавление объекта в коллекцию
            Orders.Add(order);    //Добавление объекта в коллекцию
        }

        private void DelOrder_Click(object sender, RoutedEventArgs e)
        {
            Order order = DataOrders.SelectedItem as Order;//получение объекта из DataGrid
            Orders.Remove(order); //удаление выбранного объекта
        }

        private void DataOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataOrders.SelectedItem != null)
            {
                Order order = shop.FindOrder(DataOrders.SelectedItem as Order);//поиск выбранного объекта в коллекции
                DataProducts.ItemsSource = order?.GetProduct(); //Подключение коллекций данных к DataGrid
            }
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            Order order = shop.FindOrder(DataOrders.SelectedItem as Order);//поиск выбранного объекта в коллекции
            order.AddProduct(TextBoxProduct.Text, Convert.ToInt32(TextBoxPriceProduct.Text));//добавление продукта в заказ
            DataProducts.ItemsSource = order?.GetProduct();//Подключение коллекций данных к DataGrid
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            Product product = DataProducts.SelectedItem as Product;//получение объекта из DataGrid
            Order order = shop.FindOrder(DataOrders.SelectedItem as Order);//поиск выбранного объекта в коллекции
            order.DeleteProduct(product.Name);//удаление продукта из выбранного заказа
            DataProducts.ItemsSource = order?.GetProduct();//Подключение коллекций данных к DataGrid
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)//Метод сохранения
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

        private async void LoadFile_ClickAsync(object sender, RoutedEventArgs e)//метод загрузки
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

        private void TextBoxName_PreviewTextInput(object sender, TextCompositionEventArgs e)  //метод для ограничения вводимой информации         
        {                                                                                              
            e.Handled = !(Char.IsLetter(e.Text, 0));                                                   
        }                                                                                              
                                                                                                       
        private void TextBoxName_PreviewKeyDown(object sender, KeyEventArgs e)   //метод для ограничения вводимой информации                      
        {                                                                                              
            if (e.Key == Key.Space)                                                                    
                e.Handled = true;                                                                      
        }                                                                                              
                                                                                                       
        private void TextBoxProduct_PreviewTextInput(object sender, TextCompositionEventArgs e)  //метод для ограничения вводимой информации      
        {                                                                                              
            e.Handled = !(Char.IsLetter(e.Text, 0));                                                   
        }                                                                                              
                                                                                                       
        private void TextBoxProduct_PreviewKeyDown(object sender, KeyEventArgs e)   //метод для ограничения вводимой информации                   
        {                                                                                              
            if (e.Key == Key.Space)                                                                    
                e.Handled = true;                                                                      
        }                                                                                              
                                                                                                       
        private void TextBoxPriceProduct_PreviewKeyDown(object sender, KeyEventArgs e)  //метод для ограничения вводимой информации               
        {                                                                                              
            if (e.Key == Key.Space)                                                                    
                e.Handled = true;                                                                      
        }                                                                                              
                                                                                                       
        private void TextBoxPriceProduct_PreviewTextInput(object sender, TextCompositionEventArgs e) //метод для ограничения вводимой информации
        {                                                                                              
            e.Handled = !(Char.IsDigit(e.Text, 0));                                                    
        }                                                                                              
    }
}
