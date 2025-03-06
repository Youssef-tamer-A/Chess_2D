using ChessLogic;

namespace ChessLogic.Pieces
{
    public class Knight : Piece
    {
        public override PieceType Type => PieceType.knight;
        public override Player Color { get; }

        (int, int)[] knightMoves = new (int, int)[]
        {
                ( 2,  1),
                ( 2, -1),
                (-2,  1),
                (-2, -1),
                ( 1,  2),
                ( 1, -2),
                (-1,  2),
                (-1, -2)
        };

        public Knight(Player color)
        {
            Color = color;
        }

        public override Piece Copy()
        {
            Knight copy = new Knight(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        private static IEnumerable<Position> PotentialDestinations(Position from)
        {
            // مصفوفة الإزاحات لحركة الحصان
            var knightMoves = new (int, int)[]
            {
                    ( 2,  1),
                    ( 2, -1),
                    (-2,  1),
                    (-2, -1),
                    ( 1,  2),
                    ( 1, -2),
                    (-1,  2),
                    (-1, -2)
            };

            foreach (var (dx, dy) in knightMoves)
            {
                // تكوين الموضع الجديد
                Position newPos = new Position(from.Row + dx, from.Column + dy);

                // عادةً تتحقّق هل الموضع الجديد صالح (داخل حدود الرقعة) قبل الإرجاع
                if (Board.IsInside(newPos))
                {
                    yield return newPos;
                }
            }
        }


        private IEnumerable<Position> MovePositions(Position from, Board board)
        {
            return PotentialDestinations(from).Where(pos => Board.IsInside(pos)
                   && (board.IsEmaty(pos) || board[pos].Color != Color));
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return MovePositions(from, board).Select(to => new NormalMove(from, to));
        }
    }
}
