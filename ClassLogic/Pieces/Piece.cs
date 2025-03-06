using ChessLogic;

namespace ChessLogic
{
    public abstract class Piece
    {
        public abstract PieceType Type { get; }
        public abstract Player Color  { get; }

        public bool HasMoved { get; set; } = false;

        public abstract Piece Copy();

        public abstract IEnumerable<Move> GetMoves(Position from, Board board);

        protected IEnumerable<Position> MovePositionsInDir(Position from, Board board, Direction dir)
        {
            for (Position pos = from + dir; Board.IsInside(pos); pos += dir)
            {
                if (board.IsEmaty(pos))
                {
                    yield return pos;
                    continue;
                }

                Piece piece = board[pos];

                if (piece.Color != Color)
                {
                    yield return pos;
                }

                yield break;
            }
        }
        protected IEnumerable<Position> MovePositionsInDirs(Position from, Board board, Direction[] dirs)
        {
            return dirs.SelectMany(dir => MovePositionsInDir(from, board, dir));
        }

        public virtual bool CanCaptureOpponentKing(Position from, Board board)
        {
            return GetMoves(from, board).Any(move =>
            {
                Piece piece = board[move.ToPos];
                return piece != null && piece.Type == PieceType.king;
            });
        }

        public virtual string ToFenSymbol()
        {
            // Type و Color افترض أنك معرفهم في Piece
            // مثال:
            char symbol = Type switch
            {
                PieceType.pawn => 'p',
                PieceType.knight => 'n',
                PieceType.bishop => 'b',
                PieceType.rook => 'r',
                PieceType.queen => 'q',
                PieceType.king => 'k',
                _ => '?'
            };

            // لو القطعة بيضاء، خلّي الحرف UpperCase
            return Color == Player.White ? symbol.ToString().ToUpper() : symbol.ToString();
        }
    }
}
