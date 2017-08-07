using System.Windows;
using Skype2BlinkGui.ViewModels;


namespace Skype2BlinkGui.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new SkypeStateViewModel();
        }
    }
}
