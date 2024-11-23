using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Avtodor_56_v3
{
    public partial class Authorization : Window
    {
        Avtodor_56Entities db;
        private int failedAttempts = 0;
        private string captchaText = string.Empty;

        public Authorization()
        {
            InitializeComponent();
        }

        private void ShowCaptcha()
        {
            var captchaImage = CaptchaGeneratorWPF.GenerateCaptchaImage(out captchaText);
            var captchaWindow = new Window
            {
                Title = "Введите капчу",
                Height = 200,
                Width = 300,
            };

            StackPanel panel = new StackPanel();
            panel.Children.Add(new Image { Source = captchaImage });
            TextBox captchaTextBox = new TextBox();
            Button submitButton = new Button { Content = "Подтвердить" };

            submitButton.Click += (s, e) =>
            {
                if (captchaTextBox.Text == captchaText)
                {
                    captchaWindow.Close();
                    failedAttempts = 0; // сброс попыток после прохождения капчи
                }
                else
                {
                    MessageBox.Show("Неверная капча! Попробуйте снова.");
                }
            };

            panel.Children.Add(captchaTextBox);
            panel.Children.Add(submitButton);
            captchaWindow.Content = panel;
            captchaWindow.ShowDialog();
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            string pass = CreateSHA256(tbP.Text);
            db = new Avtodor_56Entities();

            try
            {
                var user = db.User.Where(d => (d.Login == tbL.Text && d.Password == pass)).FirstOrDefault();
                if (user != null)
                {
                    MainWindow main = new MainWindow();
                    main.Show();
                    this.Close();
                }
                else
                {
                    failedAttempts++;
                    MessageBox.Show("Данные неверны");

                    if (failedAttempts >= 3)
                    {
                        ShowCaptcha();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Registration registration = new Registration();
            registration.Show();
            this.Close();
        }
    }
}
