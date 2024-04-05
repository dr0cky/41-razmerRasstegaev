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
        User currentUser;
        int newOrderId;

        List<OrderProduct> selectedOrderProducts = new List<OrderProduct>();
        List<Product> selectedProducts = new List<Product>();



        public BotinokPage(User user)
        {
            InitializeComponent();

            if (selectedProducts.Count == 0)
            {
                BtnOrder.Visibility = Visibility.Hidden;
            }
            currentUser = user;
            if (user != null)
            {
                FIOTB.Text = user.UserSurname + " " + user.UserName + " " + user.UserPatronymic;
                switch (user.UserRole)
                {
                    case 1:
                        RoleTB.Text = "Администратор"; break;
                    case 2:
                        RoleTB.Text = "Клиент"; break;
                    case 3:
                        RoleTB.Text = "Менеджер"; break;
                }
                URole.Visibility = Visibility.Visible;
                RoleTB.Visibility = Visibility.Visible;
            }
            else
            {
                FIOTB.Text = "Гость";
                URole.Visibility = Visibility.Hidden;
                RoleTB.Visibility = Visibility.Hidden;
            }


            List<Product> currentProducts = Rasstegaev41Entities.GetContext().Product.ToList();
            ProductListView.ItemsSource = currentProducts;
            List<Order> allOrder = Rasstegaev41Entities.GetContext().Order.ToList();
            List<int> allOrderId = new List<int>();
            foreach (var p in allOrder.Select(x => $"{x.OrderID}").ToList())
            {
                allOrderId.Add(Convert.ToInt32(p));
            }

            newOrderId = allOrderId.Max() + 1;

            MCount.Text = Rasstegaev41Entities.GetContext().Product.ToList().Count.ToString();
            Filter.SelectedIndex = 0;

            Update();

        }

        private void Update()
        {
            var currentProducts = Rasstegaev41Entities.GetContext().Product.ToList();

            if (selectedProducts.Count > 0)
            {
                BtnOrder.Visibility = Visibility.Visible;
            }

            if (Filter.SelectedIndex == 0)
            {
                currentProducts = currentProducts.Where(p => p.ProductDiscountAmount >= 0 && p.ProductDiscountAmount <= 100).ToList();
            }

            if (Filter.SelectedIndex == 1)
            {
                currentProducts = currentProducts.Where(p => p.ProductDiscountAmount >= 0 && p.ProductDiscountAmount < 10).ToList();
            }

            if (Filter.SelectedIndex == 2)
            {
                currentProducts = currentProducts.Where(p => p.ProductDiscountAmount >= 10 && p.ProductDiscountAmount < 15).ToList();
            }

            if (Filter.SelectedIndex == 3)
            {
                currentProducts = currentProducts.Where(p => p.ProductDiscountAmount >= 15 && p.ProductDiscountAmount <= 100).ToList();
            }

            if (RButtonUp.IsChecked.Value)
            {
                currentProducts = currentProducts.OrderBy(p => p.ProductCost).ToList();
            }

            if (RButtonDown.IsChecked.Value)
            {
                currentProducts = currentProducts.OrderByDescending(p => p.ProductCost).ToList();
            }

            currentProducts = currentProducts.Where(p => p.ProductName.ToLower().Contains(Search.Text.ToLower())).ToList();
            CurAmount.Text = currentProducts.Count.ToString();

            ProductListView.ItemsSource = currentProducts;
            if (selectedProducts.Count == 0)
            {
                BtnOrder.Visibility = Visibility.Hidden;
            }



        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        private void RButtonUp_Checked(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void RButtonDown_Checked(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void AddInOrder_Click(object sender, RoutedEventArgs e)
        {
            if (ProductListView.SelectedIndex >= 0)
            {
                List<Order> allOrder = Rasstegaev41Entities.GetContext().Order.ToList();
                List<int> allOrderId = new List<int>();
                foreach (var p in allOrder.Select(x => $"{x.OrderID}").ToList())
                {
                    allOrderId.Add(Convert.ToInt32(p));
                }

                newOrderId = allOrderId.Max() + 1;
                var prod = ProductListView.SelectedItem as Product;

                //int newOrderID = selectedOrderProducts.Last().Order.OrderID;
                var newOrderProd = new OrderProduct();
                newOrderProd.OrderID = newOrderId;

                newOrderProd.ProductArticleNumber = prod.ProductArticleNumber;
                newOrderProd.Amount = 1;
                var selOP = selectedOrderProducts.Where(p => Equals(p.ProductArticleNumber, prod.ProductArticleNumber));

                if (selOP.Count() == 0)
                {
                    selectedOrderProducts.Add(newOrderProd);
                    selectedProducts.Add(prod);
                }
                else
                {
                    foreach (OrderProduct p in selectedOrderProducts)
                    {
                        if (p.ProductArticleNumber == prod.ProductArticleNumber)
                            p.Amount++;
                    }
                }

                BtnOrder.Visibility = Visibility.Visible;
                ProductListView.SelectedIndex = -1;

                Update();

            }
        }

        private void BtnOrder_Click(object sender, RoutedEventArgs e)
        {
            OrderWindow window = new OrderWindow(selectedOrderProducts, selectedProducts, currentUser);
            window.ShowDialog();
            Update();
        }
    }
}