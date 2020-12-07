using Auction.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Windows.Threading;
using MongoDB.Bson;
using Auction.ViewModel;

namespace Auction
{
    /// <summary>
    /// Логика взаимодействия для PlayAuction.xaml
    /// </summary>
    public partial class AuctionWindow : Window
    {

        public AuctionWindow(ObjectId auction)
        {
            InitializeComponent();
            this.DataContext = new AuctionViewModel(this, auction);

        }

        private void PLayersView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (DataContext as AuctionViewModel).OpenProfileWindowCommand.Execute((sender as ListView).SelectedItem);
        }
    }
}
