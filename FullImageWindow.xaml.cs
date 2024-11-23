using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Avtodor_56
{
    public partial class FullImageWindow : Window
    {
        public FullImageWindow(string imagePath)
        {
            InitializeComponent();
            if (!string.IsNullOrWhiteSpace(imagePath))
            {
                try
                {
                    FullImage.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке изображения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Путь к изображению не указан", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
