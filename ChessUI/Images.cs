using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChessLogic;

namespace ChessUI
{
    public static class Images
    {
        private static readonly Dictionary<PieceType, ImageSource> blackSources = new()
        {
            { PieceType.pawn, LoadImage("Assets/PawnB.png") },
            { PieceType.rook, LoadImage("Assets/RookB.png") },
            { PieceType.knight, LoadImage("Assets/KnightB.png") },
            { PieceType.bishop, LoadImage("Assets/BishopB.png") },
            { PieceType.queen, LoadImage("Assets/QueenB.png") },
            { PieceType.king, LoadImage("Assets/KingB.png") },
        };


        private static readonly Dictionary<PieceType, ImageSource> whiteSources = new() 
        {
            { PieceType.pawn, LoadImage("Assets/PawnW.png") },
            { PieceType.rook, LoadImage("Assets/RookW.png") },
            { PieceType.knight, LoadImage("Assets/KnightW.png") },
            { PieceType.bishop, LoadImage("Assets/BishopW.png") },
            { PieceType.queen,  LoadImage("Assets/QueenW.png") },
            { PieceType.king, LoadImage("Assets/KingW.png") },
        };

        
        private static ImageSource LoadImage(string filePath)
        {
            return new BitmapImage(new Uri(filePath, UriKind.Relative));
        }

        public static ImageSource GetImage(Player color, PieceType pieceType)
        {
            return color switch
            {
                Player.Black => blackSources[pieceType],
                Player.White => whiteSources[pieceType],
                _ => null
            };
        }

        public static ImageSource GetImage(Piece piece)
        {
            if (piece == null)
            {
                return null;
            }

            return GetImage(piece.Color, piece.Type);
        }
    }
}
