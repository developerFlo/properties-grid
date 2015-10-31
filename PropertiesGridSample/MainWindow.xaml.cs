using PropertiesGrid.Classes;
using System;
using System.Collections.Generic;
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

namespace PropertiesGridSample
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ItemTextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;
            ((ItemViewModel)tb.DataContext).StartEdit();
        }

        private void ItemComboBox_LostFocus(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            ((ItemViewModel)cb.DataContext).StopEdit();
        }
    }
}
