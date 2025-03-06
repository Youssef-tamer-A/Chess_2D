using ChessLogic;
using System.Windows.Controls;
using System.Windows.Input;

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for PromotionMenu.xaml
    /// </summary>
    public partial class PromotionMenu : UserControl
    {
        public event Action<PieceType> PieceSelected;

        public PromotionMenu(Player  player)
        {
            InitializeComponent();

            QueenImg.Source = Images.GetImage(player, PieceType.queen);
            RookImg.Source = Images.GetImage(player, PieceType.rook);
            KnightImg.Source = Images.GetImage(player, PieceType.knight);
            BishopImg.Source = Images.GetImage(player, PieceType.bishop);
        }

        private void BishopImg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PieceSelected.Invoke(PieceType.bishop);
        }

        private void KnightImg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PieceSelected.Invoke(PieceType.knight);
        }

        private void RookImg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PieceSelected.Invoke(PieceType.rook);
        }

        private void QueenImg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PieceSelected.Invoke(PieceType.queen);
        }
    }
}
