using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Avtodor_56_v3
{
    public partial class OrdersPage : Page
    {
        private Avtodor_56Entities _dbContext;

        public OrdersPage()
        {
            InitializeComponent();
            _dbContext = new Avtodor_56Entities();
            LoadData();
        }

        private void LoadData()
        {
            OrdersDataGrid.ItemsSource = _dbContext.Orders.Select(o => new
            {
                o.OrderID,
                o.OrderDate,
                EmployeeName = o.Employee.FullName,
                ClientName = o.Client.FullName,
                o.Status
            }).ToList();
            EmployeeComboBox.ItemsSource = _dbContext.Employee.ToList();
            ClientComboBox.ItemsSource = _dbContext.Client.ToList();
        }

        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newOrder = new Orders
                {
                    OrderDate = OrderDatePicker.SelectedDate ?? System.DateTime.Now,
                    EmployeeID = (int)EmployeeComboBox.SelectedValue,
                    ClientID = (int)ClientComboBox.SelectedValue,
                    Status = ((ComboBoxItem)StatusComboBox.SelectedItem).Content.ToString()
                };

                _dbContext.Orders.Add(newOrder);
                _dbContext.SaveChanges();
                LoadData();
            }
            catch
            {
                MessageBox.Show("Ошибка при добавлении заказа. Проверьте корректность введённых данных.");
            }
        }

        private void EditOrder_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem is null)
            {
                MessageBox.Show("Выберите заказ для редактирования.");
                return;
            }

            var selectedOrderId = (int)((dynamic)OrdersDataGrid.SelectedItem).OrderID;
            var order = _dbContext.Orders.FirstOrDefault(o => o.OrderID == selectedOrderId);

            if (order != null)
            {
                try
                {
                    order.OrderDate = OrderDatePicker.SelectedDate ?? System.DateTime.Now;
                    order.EmployeeID = (int)EmployeeComboBox.SelectedValue;
                    order.ClientID = (int)ClientComboBox.SelectedValue;
                    order.Status = ((ComboBoxItem)StatusComboBox.SelectedItem).Content.ToString();

                    _dbContext.SaveChanges();
                    LoadData();
                }
                catch
                {
                    MessageBox.Show("Ошибка при редактировании заказа. Проверьте данные.");
                }
            }
        }

        private void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem is null)
            {
                MessageBox.Show("Выберите заказ для удаления.");
                return;
            }

            var selectedOrderId = (int)((dynamic)OrdersDataGrid.SelectedItem).OrderID;
            var order = _dbContext.Orders.FirstOrDefault(o => o.OrderID == selectedOrderId);

            if (order != null)
            {
                _dbContext.Orders.Remove(order);
                _dbContext.SaveChanges();
                LoadData();
            }
        }

        private void ApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            var selectedStatus = ((ComboBoxItem)FilterStatusComboBox.SelectedItem).Content.ToString();
            var filteredOrders = _dbContext.Orders.Select(o => new
            {
                o.OrderID,
                o.OrderDate,
                EmployeeName = o.Employee.FullName,
                ClientName = o.Client.FullName,
                o.Status
            });
            if (selectedStatus != "Все")
            {
                filteredOrders = filteredOrders.Where(o => o.Status == selectedStatus);
            }
            OrdersDataGrid.ItemsSource = filteredOrders.ToList();
        }

    }
}
