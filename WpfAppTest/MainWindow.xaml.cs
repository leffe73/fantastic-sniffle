using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAppTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //WpfAppTest.ThemeSelector.ThemeSelector.SetCurrentThemeDictionary(this, new Uri("/WPFAppTest;component/Themes/BureauBlack.xaml", UriKind.Relative));
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            Printer.Snapshot(this.MainGrid);
        }
    }
}
