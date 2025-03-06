using ChessLogic;

namespace ChessLogic.Pieces
{
    public class Queen : Piece
    {
        public override PieceType Type => PieceType.queen;
        public override Player Color { get; }

        private static readonly Direction[] directions = new Direction[]
        {
            Direction.North,
            Direction.East,
            Direction.South,
            Direction.West,
            Direction.NorthEast,
            Direction.SouthEast,
            Direction.NorthWest,
            Direction.SouthWest
        };

        public Queen(Player color)
        {
            Color = color;
        }

        public override Piece Copy()
        {
            Queen copy = new Queen(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return MovePositionsInDirs(from, board, directions).Select(to => new NormalMove(from, to));
        }
    }
}
