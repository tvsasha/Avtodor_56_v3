using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Avtodor_56_v3
{
   
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        Avtodor_56Entities db;

        public Registration()
        {
            InitializeComponent();
            db = new Avtodor_56Entities();
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {            
            User user = new User();
            user.EmployeeID = Convert.ToInt32(tbE.Text);
            user.Login = tbL.Text;
            user.Password = CreateSHA256(tbP.Text);
            db.User.Add(user);
            db.SaveChanges();   
            Authorization authorization = new Authorization();
            authorization.Show();
            this.Close();
        }

        public static string CreateSHA256(string input)
        {
            string PassWithSalt = input + "Salt";
            var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(PassWithSalt));
            var builder = new StringBuilder();
            foreach (var b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }           
            return builder.ToString();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Authorization authorization = new Authorization();
            authorization.Show();
            this.Close();
        }
    }
}
