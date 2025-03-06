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

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for StartMenu.xaml
    /// </summary>
    public partial class StartMenu : UserControl
    {
        public event Action PlayerVsPlayerSelected;
        public event Action PlayerVsComputerSelected;

        public StartMenu()
        {
            InitializeComponent();
        }

        private void PlayerVsPlayer_Click(object sender, RoutedEventArgs e)
        {
            PlayerVsPlayerSelected?.Invoke();
        }

        private void PlayerVsComputer_Click(object sender, RoutedEventArgs e)
        {
            PlayerVsComputerSelected?.Invoke();
        }
    }
}
