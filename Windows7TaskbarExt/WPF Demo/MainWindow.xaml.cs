namespace Huseyint.Windows7.WPF.Demo
{
    using System.Windows;
    using System.Windows.Controls;

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void iconsList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            taskbarButton.OverlayIcon = (iconsList.SelectedItem as Image).Source;
        }
    }
}