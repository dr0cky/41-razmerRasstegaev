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
    /// Логика взаимодействия для BotinokPage.xaml
    /// </summary>
    public partial class BotinokPage : Page
    {
        List<Product> CurrentPageList = new List<Product>();
        List<Product> TableList;
        public BotinokPage()
        {
            InitializeComponent();

            List<Product> currentProducts = Rasstegaev41Entities.GetContext().Product.ToList();
            ProductListView.ItemsSource = currentProducts;

            ProdAll.Text = Convert.ToString(currentProducts.Count);

            CostComboBox.SelectedIndex = 0;
            DiscntComboBox.SelectedIndex = 0;

            UpdateProduct();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void ProdSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProduct();
        }

        private void CostComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProduct();
        }

        private void DiscntComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProduct();
        }

        private void UpdateProduct()
        {
            var currentProduct = Rasstegaev41Entities.GetContext().Product.ToList();

            currentProduct = currentProduct.Where(p => p.ProductName.ToLower().Contains(ProdSearch.Text.ToLower())).ToList();

            if (CostComboBox.SelectedIndex == 0)
            {

            }
            else if (CostComboBox.SelectedIndex == 1)
            {
                currentProduct = currentProduct.OrderBy(p => p.ProductCost).ToList();
            }
            else if (CostComboBox.SelectedIndex == 2)
            {
                currentProduct = currentProduct.OrderByDescending(p => p.ProductCost).ToList();
            }

            if (DiscntComboBox.SelectedIndex == 0)
            {

            }
            else if (DiscntComboBox.SelectedIndex == 1)
            {
                currentProduct = currentProduct.Where(p => (p.ProductDiscountAmount >= 0 && p.ProductDiscountAmount < 10)).ToList();
            }
            else if (DiscntComboBox.SelectedIndex == 2)
            {
                currentProduct = currentProduct.Where(p => (p.ProductDiscountAmount >= 10 && p.ProductDiscountAmount < 15)).ToList();
            }
            else if (DiscntComboBox.SelectedIndex == 3)
            {
                currentProduct = currentProduct.Where(p => (p.ProductDiscountAmount >= 15)).ToList();
            }

            ProdAtTheMoment.Text = Convert.ToString(currentProduct.Count);

            ProductListView.ItemsSource = currentProduct;

            TableList = currentProduct;

            //ChangePage(0, 0);
        }

    }
}

