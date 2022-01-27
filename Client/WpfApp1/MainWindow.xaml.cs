using System.Windows;
using WpfApp1.View;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Join_Button(object sender, RoutedEventArgs e)
        {
            Window1 win2 = new Window1(namePlayer.Text,(bool)Debug.IsChecked);
            win2.Show();
            this.Close();
        }
    }
}
