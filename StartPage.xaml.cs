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

namespace Avtodor_56_v3
{
    /// <summary>
    /// Логика взаимодействия для StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        private Avtodor_56Entities _dbContext;
        public StartPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            _dbContext = new Avtodor_56Entities();
            var activeOrdersCount = _dbContext.Orders.Count(o => o.Status == "В работе");
            ActiveOrdersCount.Text = activeOrdersCount.ToString();
            var employeesCount = _dbContext.Employee.Count();
            EmployeesCount.Text = employeesCount.ToString();
            var materials = (from material in _dbContext.Materials
                             join history in _dbContext.OrderHistory
                             on material.MaterialID equals history.MaterialID
                             select new
                             {
                                 material.Name,             
                                 material.UnitPrice,        
                                 history.StockRemaining     
                             }).ToList();
            MaterialsDataGrid.ItemsSource = materials;

        }
    }
}
