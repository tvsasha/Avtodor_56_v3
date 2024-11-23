using Avtodor_56_v3;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Avtodor_56_v3
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new StartPage());
        }       
        public void Order_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new OrdersPage());
        }

        public void Employee_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new EmployeePage());
        }

        public void Material_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new MaterialsPage());
        }

        public void Home_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new StartPage());
        }

        public void Logaut_Click(object sender, RoutedEventArgs e)
        {
            Authorization authorization = new Authorization();
            authorization.Show();
            this.Close();
        }

        private void MainFrame_ContentRendered(object sender, EventArgs e)
        {

        }
    }
}
