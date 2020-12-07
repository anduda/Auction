using Auction.Model;
using Auction.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Auction.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public string Login { get; set; }
        private Window window { get; set; }

        public LoginViewModel(Window window)
        {
            this.window = window;
        }

        private RelayCommand loginCommand;
        public RelayCommand LoginCommand
        {
            get
            {
                return loginCommand ?? (loginCommand = new RelayCommand(obj =>
                {
                    var psw = obj as PasswordBox;
                    bool isLogin = false;
                    try
                    {
                        isLogin = Session.GetClient().Logon(Login, psw.Password);
                    }
                    /*catch(Exception ex)
                    {
                        ;
                        throw;
                    }*/
                    catch(FaultException<Server.IServerException> ex)
                    {
                        MessageBox.Show(ex.Detail.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    catch (FaultException<Server.UserIsBlockedException> ex)
                    {
                        MessageBox.Show(ex.Detail.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (isLogin)
                    {
                        (new MainMenu()).Show();
                        window.Close();
                    }
                    else
                    {
                        MessageBox.Show("err", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }));
            }
        }
        
        public RelayCommand OpenRegistrationWindowCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {

                    (new RegistrationWindow()).Show();
                    window.Close();
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
