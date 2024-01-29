using Microsoft.Win32;
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

namespace _41_razmerRasstegaev
{
    /// <summary>
    /// Логика взаимодействия для AddBotinokPage.xaml
    /// </summary>
    public partial class AddBotinokPage : Page
    {
        private Product _currentProduct = new Product();
        public AddBotinokPage(Product selectedProduct)
        {
            InitializeComponent();
            if (selectedProduct != null)
            {
                _currentProduct = selectedProduct;
            }
            DataContext = _currentProduct;
        }

        private void ChangePictureBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myOpenFileDialog = new OpenFileDialog();
            if (myOpenFileDialog.ShowDialog() == true)
            {
                _currentProduct.ProductPhoto = myOpenFileDialog.FileName;
                LogoImage.Source = new BitmapImage(new Uri(myOpenFileDialog.FileName));
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentProduct.ProductName))
                errors.AppendLine("Уканите наиненование товара");
            if (string.IsNullOrWhiteSpace(_currentProduct.ProductDescription))
                errors.AppendLine("Уканите описание товара");
            if (string.IsNullOrWhiteSpace(_currentProduct.ProductManufacturer))
                errors.AppendLine("Уканите производителя");
            if (string.IsNullOrWhiteSpace(_currentProduct.ProductCost.ToString()))
                errors.AppendLine("Уканите цену товара");
            if (string.IsNullOrWhiteSpace(_currentProduct.ProductCost.ToString()))
                errors.AppendLine("Уканите цену товара");
            else
            {
                if (_currentProduct.ProductArticleNumber)
                    Rasstegaev41ramzerEntities.GetContext().Product.Add(_currentProduct);
                try
                {
                    Rasstegaev41ramzerEntities.GetContext().SaveChanges();
                    MessageBox.Show("Информация сохранена");
                    Manager.MainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
