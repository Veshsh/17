using _5NET.Model;
using _5NET.View;
using _5NET.View.Helpers;
using Prism.Commands;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Xml.Linq;

namespace _5NET.ViewModel
{
    internal class MainViewModel : BindingHelper
    {
        #region 
        public EnterModel Enter { get; set; } = new EnterModel("127.0.0.1:8888", "noname");
        public DelegateCommand NP 
        {
            get 
            {
                return new DelegateCommand(() =>
                {
                    if (Validate.EnterValidate(Enter))
                    {
                        TcpServer.ServerStart(Enter);
                        TcpClient.ClientStart(Enter);
                        Window2 window2 = new Window2();
                        _ = TcpClient.SendMessage(Enter.Name + " CreateChat");
                        window2.ShowDialog();
                    }
                });
            } 
        }
        public DelegateCommand CP
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (Validate.EnterValidate(Enter))
                    {
                        TcpClient.ClientStart(Enter);
                        Window1 window1 = new Window1();
                        _ = TcpClient.SendMessage(Enter.Name + " ConnectChat");
                        window1.ShowDialog();
                        _ = TcpClient.SendMessage("/disconnect");
                    }
                });
            }
        }
        #endregion

        public MainViewModel()
        {

        }
    }
}
