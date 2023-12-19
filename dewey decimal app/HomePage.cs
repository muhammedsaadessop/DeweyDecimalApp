using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace dewey_decimal_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class HomePage : Window
    {
        public HomePage()
        {
            InitializeComponent();
            
        }


        private const int windowbuttons = -16;
        private const int system = 0x80000;
        [DllImport("user32.dll", SetLastError  = true)]
        private static extern int GetWindowLong(IntPtr header, int newpostion);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr header, int newposition, int close);

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            IntPtr header = new WindowInteropHelper(this).Handle;
            SetWindowLong(header, windowbuttons, GetWindowLong(header, windowbuttons) & system);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
         
                var bookordering = new BookOrdering();
                bookordering.Show();
                this.Close();
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var bookid = new bookidentyfying();
         bookid.Show();
            this.Close();

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var boonumber = new findingbooknumbers();
            boonumber.Show();
            this.Close();
        }

      

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();

        }
    }
}
