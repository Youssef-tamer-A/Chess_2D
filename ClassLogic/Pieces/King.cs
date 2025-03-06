using ChessLogic;

namespace ChessLogic.Pieces
{
    public class King : Piece
    {
        public override PieceType Type => PieceType.king;
        public override Player Color { get; }

        private static readonly Direction[] directions = new Direction[]
        {   Direction.North, 
            Direction.South, 
            Direction.East, 
            Direction.West, 
            Direction.NorthEast, 
            Direction.NorthWest, 
            Direction.SouthEast, 
            Direction.SouthWest 
        };

        public King(Player color)
        {
            Color = color;
        }

        private static bool IsUnmovedRook(Position pos, Board board)
        {
            if (board.IsEmaty(pos))
            {
                return false;
            }

            Piece piece = board[pos];
            return piece.Type == PieceType.rook && !piece.HasMoved;
        }

        private static bool AllEmpty(IEnumerable<Position> position, Board board)
        {
            return position.All(position => board.IsEmaty(position)); 
        }

        private bool CanCastleKingSide(Position from, Board board)
        {
            if (HasMoved)
            {
                return false;
            }

            Position rookPos = new Position(from.Row, 7);
            Position[]  betweenPositions = new Position[] { new Position(from.Row, 5), new Position(from.Row, 6) };

            return IsUnmovedRook(rookPos, board) && AllEmpty(betweenPositions, board);
        }

        private bool CanCastleQueenSide(Position from, Board board)
        {
            if (HasMoved)
            {
                return false;
            }

            Position rookPos = new Position(from.Row, 0);
            Position[] betweenPositions = new Position[] { new Position(from.Row, 1), new Position(from.Row, 2), new Position(from.Row, 3) };

            return IsUnmovedRook(rookPos, board) && AllEmpty(betweenPositions, board);
        }

        public override Piece Copy()
        {
            King copy = new King(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        private IEnumerable<Position> MovePositions(Position from, Board board)
        {
            foreach (Direction dir in directions)
            {
                Position to = from + dir;

                if (!Board.IsInside(to))
                {
                    continue;
                }

                if (board.IsEmaty(to) || board[to].Color != Color)
                {
                    yield return to;
                }
            }
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            foreach (Position to in MovePositions(from, board))
            {
                yield return new NormalMove(from, to);
            }

            if (CanCastleKingSide(from, board))
            {
                yield return new Castle(MoveType.CastleKS, from);
            }

            if (CanCastleQueenSide(from, board))
            {
                yield return new Castle(MoveType.CastleQS, from);
            }
        }

        public override bool CanCaptureOpponentKing(Position from, Board board)
        {
            return MovePositions(from, board).Any(to =>
            {
                Piece piece = board[to];
                return piece != null && piece.Type == PieceType.king;
            });
        }
    }
}
