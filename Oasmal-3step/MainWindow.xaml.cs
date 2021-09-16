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
        }
        Shop shop = new Shop(10);
        ObservableCollection<Order> Orders = new ObservableCollection<Order>();
        
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
    }
}
