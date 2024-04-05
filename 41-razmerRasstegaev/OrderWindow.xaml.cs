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
using System.Windows.Shapes;

namespace _41_razmerRasstegaev
{
    /// <summary>
    /// Логика взаимодействия для OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        List<OrderProduct> selectedOrderProducts = new List<OrderProduct>();
        List<Product> selectedProducts = new List<Product>();
        private Order currentOrder = new Order();
        //private OrderProduct orderOrderProduct = new OrderProduct();
        User currentUser;
        private double Cost = 0;

        private int SetDeliveryDay(List<Product> products)
        {

            bool DeliveryStatus = false;
            foreach (var p in products)
            {
                if (p.inStock <= 3)
                {
                    DeliveryStatus = true;
                }
            }

            if (DeliveryStatus)
                return 6;
            else
                return 3;
        }
        public OrderWindow(List<OrderProduct> selectedOrderProducts, List<Product> selectedProducts, User user)
        {
            InitializeComponent();

            currentUser = user;
            Cost = 0;
            var currentPickups = Rasstegaev41Entities.GetContext().PickupPoint.ToList();
            //selectedOrderProducts = Anvarov41Entities.GetContext().OrderProduct.ToList();
            PickupCombo.ItemsSource = currentPickups.Select(x => $"{x.PostIndex}, {x.City}, {x.Street}, {x.HomeNumber}"); //вывод информации о пункте выдачи
            PickupCombo.SelectedIndex = 0;
            int currentID = selectedOrderProducts.First().OrderID; //определение номера текущего заказа
            currentOrder.OrderID = currentID;

            //Рандомный код получения
            List<Order> allOrderCodes = Rasstegaev41Entities.GetContext().Order.ToList();
            List<int> OrderCodes = new List<int>();
            foreach (var p in allOrderCodes.Select(x => $"{x.OrderReceiveCode}").ToList())
            {
                OrderCodes.Add(Convert.ToInt32(p));
            }
            Random random = new Random();

            while (true)
            {
                int num = random.Next(100, 1000);
                if (!OrderCodes.Contains(num))
                {
                    currentOrder.OrderReceiveCode = num.ToString();
                    break;
                }
            }


            foreach (Product p in selectedProducts)
            {
                p.Quantity = 1;//Quantity - столбец таблицы product которой нет в бд
                foreach (OrderProduct q in selectedOrderProducts)
                {
                    if (p.ProductArticleNumber == q.ProductArticleNumber)
                    {
                        p.Quantity = Convert.ToInt32(q.Amount);
                    }
                }
            }


            this.selectedOrderProducts = selectedOrderProducts;
            this.selectedProducts = selectedProducts;


            //определение общей стоимости заказа
            for (int i = 0; i < selectedProducts.Count; i++)
            {
                Cost += (Convert.ToDouble(selectedProducts[i].ProductCost) - Convert.ToDouble(selectedProducts[i].ProductCost) * Convert.ToDouble(selectedProducts[i].ProductDiscountAmount) / 100) * selectedProducts[i].Quantity;
            }

            TotalCost.Text = Cost.ToString();

            //дата формирования заказа и дата доставки
            OrderDP.Text = DateTime.Now.ToString();
            OrderDD.Text = DateTime.Now.AddDays(SetDeliveryDay(selectedProducts)).ToString();

            TBOrderID.Text = currentID.ToString();
            ProductListView.ItemsSource = selectedProducts;


            //вывод ФИО текущего польлзователя 
            if (user != null)
            {
                currentOrder.OrderClient = user.UserID;
                FIOTB.Text = user.UserSurname + " " + user.UserName + " " + user.UserPatronymic;
            }
            else
            {
                FIOTB.Text = "Гость";
                currentOrder.OrderClient = null;
            }

        }

        private void BtnPlus_Click(object sender, RoutedEventArgs e)
        {
            Cost = 0;
            var prod = (sender as Button).DataContext as Product;
            prod.Quantity++;
            var selectedOP = selectedOrderProducts.FirstOrDefault(p => p.ProductArticleNumber == prod.ProductArticleNumber);
            int index = selectedOrderProducts.IndexOf(selectedOP);
            selectedOrderProducts[index].Amount++;
            //if (prod.ProductQuantityInStock > 0)
            //{
            //    prod.ProductQuantityInStock--;
            //}
            ProductListView.Items.Refresh();
            for (int i = 0; i < selectedProducts.Count; i++)
            {
                Cost += (Convert.ToDouble(selectedProducts[i].ProductCost) - Convert.ToDouble(selectedProducts[i].ProductCost) * Convert.ToDouble(selectedProducts[i].ProductDiscountAmount) / 100) * selectedProducts[i].Quantity;
            }

            TotalCost.Text = Cost.ToString();
            OrderDD.Text = DateTime.Now.AddDays(SetDeliveryDay(selectedProducts)).ToString();


        }

        private void BtnMinus_Click(object sender, RoutedEventArgs e)
        {
            var prod = (sender as Button).DataContext as Product;
            prod.Quantity--;
            Cost = 0;
            var selectedOP = selectedProducts.FirstOrDefault(p => p.ProductArticleNumber == prod.ProductArticleNumber);
            int index = selectedProducts.IndexOf(selectedOP);

            if (prod.Quantity == 0)
            {
                selectedOrderProducts[index].Amount = 0;
                var pr = ProductListView.SelectedItem as Product;
                selectedOrderProducts.RemoveAt(index);
                selectedProducts.RemoveAt(index);
                if (ProductListView.Items.Count == 0)
                {
                    this.Close();
                }
            }
            else
            {
                selectedOrderProducts[index].Amount--;
                //if (this.selectedProducts[index].ProductQuantityInStock > prod.Quantity)
                //    prod.ProductQuantityInStock++;
            }
            for (int i = 0; i < selectedProducts.Count; i++)
            {
                Cost += (Convert.ToDouble(selectedProducts[i].ProductCost) - Convert.ToDouble(selectedProducts[i].ProductCost) * Convert.ToDouble(selectedProducts[i].ProductDiscountAmount) / 100) * selectedProducts[i].Quantity;
            }
            OrderDD.Text = DateTime.Now.AddDays(SetDeliveryDay(selectedProducts)).ToString();
            TotalCost.Text = Cost.ToString();
            ProductListView.Items.Refresh();

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {


            var prod = (sender as Button).DataContext as Product;
            prod.Quantity = 0;
            var selectedOP = selectedProducts.FirstOrDefault(p => p.ProductArticleNumber == prod.ProductArticleNumber);
            int index = selectedProducts.IndexOf(selectedOP);
            selectedOrderProducts[index].Amount = 0;
            var pr = ProductListView.SelectedItem as Product;
            selectedOrderProducts.RemoveAt(index);
            selectedProducts.RemoveAt(index);

            for (int i = 0; i < selectedProducts.Count; i++)
            {
                Cost += (Convert.ToDouble(selectedProducts[i].ProductCost) - Convert.ToDouble(selectedProducts[i].ProductCost) * Convert.ToDouble(selectedProducts[i].ProductDiscountAmount) / 100) * selectedProducts[i].Quantity;
            }
            TotalCost.Text = Cost.ToString();
            ProductListView.Items.Refresh();
            OrderDD.Text = DateTime.Now.AddDays(SetDeliveryDay(selectedProducts)).ToString();
            if (ProductListView.Items.Count == 0)
            {
                this.Close();
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            currentOrder.OrderPickupPoint = PickupCombo.SelectedIndex + 1;
            currentOrder.OrderDate = DateTime.Now;
            currentOrder.OrderDeliveryDate = DateTime.Now.AddDays(SetDeliveryDay(selectedProducts));
            currentOrder.OrderStatus = "Новый";
            for (int i = 0; i < selectedProducts.Count; i++)
            {
                if (selectedProducts[i].ProductQuantityInStock >= selectedOrderProducts[i].Amount)
                    selectedProducts[i].ProductQuantityInStock -= Convert.ToInt32(selectedOrderProducts[i].Amount);
                else
                    selectedProducts[i].ProductQuantityInStock = 0;
            }
            foreach (var p in selectedOrderProducts)
            {
                Rasstegaev41Entities.GetContext().OrderProduct.Add(p);
            }

            Rasstegaev41Entities.GetContext().Order.Add(currentOrder);

            try
            {
                Rasstegaev41Entities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                selectedOrderProducts.Clear();
                selectedProducts.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

            this.Close();
        }
    }

}
