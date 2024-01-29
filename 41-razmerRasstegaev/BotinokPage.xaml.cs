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
        int CountRecords;
        int CountPage;
        int CurrentPage;
        private int UserRole1;
        List<Product> CurrentPageList = new List<Product>();
        List<Product> TableList;
        public Visibility EditBtnVisibility { get; set; }

        public Visibility EditButtonVisibility { get { return EditBtnVisibility; } set { EditBtnVisibility = value; OnPropertyChanged(Name(EditButtonVisibility)); } }

        private new DependencyPropertyChangedEventArgs Name(Visibility editButtonVisibility)
        {
            throw new NotImplementedException();
        }
        public BotinokPage(int UserRole)
        {
            InitializeComponent();
            var _currentProduct = Rasstegaev41ramzerEntities.GetContext().Product.ToList();
            BotinokListView.ItemsSource = _currentProduct;
            UserRole1 = UserRole;
            switch (UserRole1)
            {
                case 0:
                    EditBtnVisibility = Visibility.Hidden;
                    break;
                case 1:
                    EditBtnVisibility = Visibility.Hidden;
                    break;
            }
        }

        private void TBSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            UpdateServices();
        }

        private void SortCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateServices();
        }

        private void Edit_Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddBotinokPage((sender as Button).DataContext as Product));
        }

        private void TypeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }
        private void UpdateServices()
        {
            var currentServices = Rasstegaev41ramzerEntities.GetContext().Product.ToList();

            if (TypeCombo.SelectedIndex == 0)
            {
                currentServices = currentServices.Where(p => (p.ProductDiscountAmount >= 0 && p.ProductDiscountAmount <= 100)).ToList();
            }
            if (TypeCombo.SelectedIndex == 1)
            {
                currentServices = currentServices.Where(p => (p.ProductDiscountAmount >= 0 && p.ProductDiscountAmount < 9.99)).ToList();
            }
            if (TypeCombo.SelectedIndex == 2)
            {
                currentServices = currentServices.Where(p => (p.ProductDiscountAmount >= 10 && p.ProductDiscountAmount < 14.99)).ToList();
            }
            if (TypeCombo.SelectedIndex == 3)
            {
                currentServices = currentServices.Where(p => (p.ProductDiscountAmount >= 15)).ToList();
            }
            if (TypeCombo.SelectedIndex == 4)
            {
                currentServices = currentServices.Where(p => (p.ProductDiscountAmount >= 15 && p.ProductDiscountAmount < 100)).ToList();
            }
            if (SortCombo.SelectedIndex == 0)
            {
                currentServices = currentServices.OrderBy(p => p.ProductCost).ToList();
            }
            if (SortCombo.SelectedIndex == 1)
            {
                currentServices = currentServices.OrderByDescending(p => p.ProductCost).ToList();
            }
            currentServices = currentServices.Where(p => p.ProductName.ToLower().Contains(TBSearch.Text.ToLower())).ToList();

            BotinokListView.ItemsSource = currentServices;
            TableList = currentServices;
            ChangePage(0, 0);
        }

        private void ChangePage(int direction, int? selectedPage)
        {
            CurrentPageList.Clear();
            CountRecords = TableList.Count;
            if (CountRecords % 10 > 0)
            {
                CountPage = CountRecords / 10 + 1;
            }
            else
            {
                CountPage = CountRecords / 10;
            }
            Boolean Ifupdate = true;
            int min;
            if (selectedPage.HasValue)
            {
                if (selectedPage >= 0 && selectedPage <= CountPage)
                {
                    CurrentPage = (int)selectedPage;
                    min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                    for (int i = CurrentPage * 10; i < min; i++)
                    {
                        CurrentPageList.Add(TableList[i]);
                    }
                }
            }
            else
            {
                switch (direction)
                {
                    case 1:
                        if (CurrentPage > 0)
                        {
                            CurrentPage--;
                            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords; ;
                            for (int i = CurrentPage * 10; i < min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;
                    case 2:
                        if (CurrentPage < CountPage - 1)
                        {
                            CurrentPage++;
                            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                            for (int i = CurrentPage * 10; i < min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;
                }
            }
            if (Ifupdate)
            {
                PageListBox.Items.Clear();
                for (int i = 1; i <= CountPage; i++)
                {
                    PageListBox.Items.Add(i);
                }
                PageListBox.SelectedIndex = CurrentPage;
                min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CurrentPage;
                TBCount.Text = min.ToString();
                TBAllRecords.Text = " из " + CountRecords.ToString();
                BotinokListView.ItemsSource = CurrentPageList;
                BotinokListView.Items.Refresh();
            }

        }
    }
}

