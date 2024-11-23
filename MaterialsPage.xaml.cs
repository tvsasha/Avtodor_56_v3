using Avtodor_56;
using iText.Kernel.Pdf;
using Microsoft.Win32;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace Avtodor_56_v3
{
    public partial class MaterialsPage : Page
    {
        private Avtodor_56Entities _dbContext;
        private Materials _currentMaterial;
        private string _selectedImagePath;

        public MaterialsPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            _dbContext = new Avtodor_56Entities();
            _dbContext.Materials.Load();
            MaterialsDataGrid.ItemsSource = _dbContext.Materials.Local;
            ClearForm();
        }

        private void ClearForm()
        {
            _currentMaterial = null;
            NameTextBox.Text = string.Empty;
            UnitTextBox.Text = string.Empty;
            UnitPriceTextBox.Text = string.Empty;
            ImagePathTextBox.Text = string.Empty;
            _selectedImagePath = null;
        }

        private void NewMaterial_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
            _currentMaterial = new Materials();
        }

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg",
                Title = "Выберите изображение"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _selectedImagePath = openFileDialog.FileName;
                ImagePathTextBox.Text = _selectedImagePath;
            }
        }

        private void SaveMaterial_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_currentMaterial == null)
                {
                    _currentMaterial = new Materials();
                    _dbContext.Materials.Add(_currentMaterial);
                }

                _currentMaterial.Name = NameTextBox.Text;
                _currentMaterial.Unit = UnitTextBox.Text;
                _currentMaterial.UnitPrice = decimal.Parse(UnitPriceTextBox.Text);

                if (!string.IsNullOrEmpty(_selectedImagePath))
                {
                    string fileName = Path.GetFileName(_selectedImagePath);
                    string destinationPath = Path.Combine(Environment.CurrentDirectory, "Images", fileName);

                    Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
                    File.Copy(_selectedImagePath, destinationPath, true);

                    _currentMaterial.ImagePath = destinationPath;
                }

                _dbContext.SaveChanges();
                LoadData();
                MessageBox.Show("Материал успешно сохранён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditMaterial_Click(object sender, RoutedEventArgs e)
        {
            if (MaterialsDataGrid.SelectedItem is Materials selectedMaterial)
            {
                _currentMaterial = selectedMaterial;

                NameTextBox.Text = _currentMaterial.Name;
                UnitTextBox.Text = _currentMaterial.Unit;
                UnitPriceTextBox.Text = _currentMaterial.UnitPrice.ToString();
                ImagePathTextBox.Text = _currentMaterial.ImagePath;
            }
            else
            {
                MessageBox.Show("Выберите материал для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteMaterial_Click(object sender, RoutedEventArgs e)
        {
            if (MaterialsDataGrid.SelectedItem is Materials selectedMaterial)
            {
                var result = MessageBox.Show("Вы действительно хотите удалить этот материал?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _dbContext.Materials.Remove(selectedMaterial);
                    _dbContext.SaveChanges();
                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Выберите материал для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void MaterialsDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (MaterialsDataGrid.SelectedItem is Materials selectedMaterial && !string.IsNullOrEmpty(selectedMaterial.ImagePath))
            {
                var fullImageWindow = new FullImageWindow(selectedMaterial.ImagePath);
                fullImageWindow.Show();
            }
        }


        private void ExportToPdf_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var saveFileDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "PDF Files|*.pdf",
                    FileName = "MaterialsList.pdf"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (var document = new PdfSharp.Pdf.PdfDocument())
                    {
                        var page = document.AddPage();
                        var gfx = XGraphics.FromPdfPage(page);
                        var font = new XFont("Arial", 12);

                        gfx.DrawString("Список материалов", font, XBrushes.Black, new XRect(0, 0, page.Width, 50), XStringFormats.TopCenter);

                        int yOffset = 100;
                        foreach (var material in _dbContext.Materials.ToList())
                        {
                            gfx.DrawString($"{material.MaterialID}: {material.Name} ({material.Unit}) - {material.UnitPrice:F2}", font, XBrushes.Black, new XRect(40, yOffset, page.Width - 80, 20), XStringFormats.TopLeft);
                            yOffset += 20;
                        }

                        document.Save(saveFileDialog.FileName);
                    }

                    MessageBox.Show("PDF успешно сохранён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при экспорте: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
