using Auction.ViewModel;
using System.Windows;

namespace Auction
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
            this.DataContext = new RegistrationViewModel(this);
        }
    }
}
