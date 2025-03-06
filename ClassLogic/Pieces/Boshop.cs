using ChessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class Boshop : Piece
    {
        public override PieceType Type => PieceType.bishop;
        public override Player Color { get; }

        private static readonly Direction[] directions = new Direction[]
        {
            Direction.NorthEast,
            Direction.SouthEast,
            Direction.NorthWest,
            Direction.SouthWest
        };

        public Boshop(Player color)
        {
            Color = color;
        }

        public override Piece Copy()
        {
            Boshop copy = new Boshop(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return MovePositionsInDirs(from, board, directions).Select(to => new NormalMove(from, to));
        }
    }
}
