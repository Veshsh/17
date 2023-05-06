using _5NET.View;
using _5NET.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace _5NET
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext =new MainViewModel();
        }
    }
}
