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
        }        

        private void Settings_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Satistyc_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Game_Click(object sender, RoutedEventArgs e)
        {
            Main_menu.Visibility = Visibility.Hidden;
            Game.Visibility = Visibility.Visible;
        }

        private void B2_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
