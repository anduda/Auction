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
using System.Windows.Forms;
using System.Collections.ObjectModel;
using Auction.ViewModel;

namespace Auction
{
    /// <summary>
    /// Логика взаимодействия для CreateAuction.xaml
    /// </summary>
    public partial class AddAuctionWindow : Window
    {
        public AddAuctionWindow()
        {
            InitializeComponent();
            this.DataContext = new AddAuctionViewModel(this);
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as AddAuctionViewModel).AddAuctionCommand.Execute(new Tuple<DateTime, DateTime, double, Server.Lot>(StartTime.Value, EndTime.Value, Convert.ToDouble(StartPrice.Text), LotsView.SelectedItem as Server.Lot));
        }

        private void LotsView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (this.DataContext as AddAuctionViewModel).OpenLotWindowCommand.Execute(LotsView.SelectedItem);
        }
    }
}
