﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
using Auction.Server;
using Auction.ViewModel;

namespace Auction
{
    /// <summary>
    /// Логика взаимодействия для MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        public MainMenu()
        {
            InitializeComponent();
            this.DataContext = new MainMenuViewModel(this);
        }

        private void AuctionListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (DataContext as MainMenuViewModel).OpenAuctionWindowCommand.Execute((sender as ListView).SelectedItem);
        }
    }

}
