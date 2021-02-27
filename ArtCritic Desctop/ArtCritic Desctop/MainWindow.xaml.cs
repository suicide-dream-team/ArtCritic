using System.Windows;
/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;*/

namespace ArtCritic_Desctop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            QuestionKeeper question = new QuestionKeeper();
            MessageBox.Show(question.getTestText());
            
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            G2.Visibility = Visibility.Hidden;
            G3.Visibility = Visibility.Visible;
        }

        private void B2_Click(object sender, RoutedEventArgs e)
        {
            G2.Visibility = Visibility.Visible;
            G3.Visibility = Visibility.Hidden;
        }
    }
}
