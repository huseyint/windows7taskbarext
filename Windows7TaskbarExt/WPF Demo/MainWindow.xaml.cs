namespace Huseyint.Windows7.WPF.Demo
{
    using System.Windows;

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new MainWindowViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var w = new MainWindow();

            w.Show();
        }
    }
}