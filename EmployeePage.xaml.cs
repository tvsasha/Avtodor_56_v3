using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using Avtodor_56_v3;
namespace Avtodor_56_v3
{
    /// <summary>
    /// Логика взаимодействия для EmployeePage.xaml
    /// </summary>
    public partial class EmployeePage : Page
    {
        private Avtodor_56Entities _dbContext;

        public EmployeePage()
        {
            InitializeComponent();
            _dbContext = new Avtodor_56Entities();
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            _dbContext.Employee.Load();
            EmployeeDataGrid.ItemsSource = _dbContext.Employee.Local.ToBindingList();
        }

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            var newEmployee = new Employee
            {
                FullName = "Новый сотрудник",
                Position = "Должность",
                Phone = "Телефон"
            };

            _dbContext.Employee.Add(newEmployee);
            _dbContext.SaveChanges();
            LoadEmployees();
        }

        private void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            var selectedEmployee = EmployeeDataGrid.SelectedItem as Employee;
            if (selectedEmployee != null)
            {
                _dbContext.Employee.Remove(selectedEmployee);
                _dbContext.SaveChanges();
                LoadEmployees();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите сотрудника для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void EditEmployee_Click(object sender, RoutedEventArgs e)
        {
            var selectedEmployee = EmployeeDataGrid.SelectedItem as Employee;
            if (selectedEmployee != null)
            {
                LoadEmployees();
                _dbContext.SaveChanges();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите сотрудника для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        
    }
}
