using ChessLogic;
using ChessLogic.Pieces;
using System.Linq;

namespace ChessLogic
{
    public class Board
    {
        private readonly Piece[,] Pieces = new Piece[8, 8];

        private readonly Dictionary<Player, Position> pawnSkipPositions = new Dictionary<Player, Position>
        {
            { Player.White, null },
            { Player.Black, null }
        };  

        public Piece this[int row, int col]
        {
            get { return Pieces[row, col]; }
            set { Pieces[row, col] = value; }
        }

        public Piece this[Position pos]
        {
            get { return Pieces[pos.Row, pos.Column]; }
            set { Pieces[pos.Row, pos.Column] = value; }
        }

        public static Board InitialBoard()
        {
            Board board = new Board();
            board.AddStartPieces();
            return board;
        }

        public Position GetPawnSkipPosition(Player player)
        {
            return pawnSkipPositions[player];
        }

        public void SetPawnSkipPosition(Player player, Position pos)
        {
            pawnSkipPositions[player] = pos;
        }

        private void AddStartPieces()
        {
            this[7, 0] = new Rook(Player.White);
            this[7, 1] = new Knight(Player.White);
            this[7, 2] = new Boshop(Player.White);
            this[7, 3] = new Queen(Player.White);
            this[7, 4] = new King(Player.White);
            this[7, 5] = new Boshop(Player.White);
            this[7, 6] = new Knight(Player.White);
            this[7, 7] = new Rook(Player.White);

            for (int i = 0; i < 8; i++)
            {
                this[6, i] = new Pawn(Player.White);
                this[1, i] = new Pawn(Player.Black);
            }

            this[0, 0] = new Rook(Player.Black);
            this[0, 1] = new Knight(Player.Black);
            this[0, 2] = new Boshop(Player.Black);
            this[0, 3] = new Queen(Player.Black);
            this[0, 4] = new King(Player.Black);
            this[0, 5] = new Boshop(Player.Black);
            this[0, 6] = new Knight(Player.Black);
            this[0, 7] = new Rook(Player.Black);

            
        }

        public static bool IsInside(Position pos)
        {
            return pos.Row >= 0 && pos.Row < 8 && pos.Column >= 0 && pos.Column < 8;
        }

        public bool IsEmaty(Position pos) 
        {
            return this[pos] == null;
        }

        public IEnumerable<Position> PiecePositions()
        {
            for (int i = 0; i < 8; i++) 
            { 
                for (int j = 0; j < 8; j++)
                {
                    Position pos = new Position(i, j);
                    if (!IsEmaty(pos))
                        yield return pos;
                }
            }
        }

        public IEnumerable<Position> PiecePositionsFor(Player player)
        {
            return PiecePositions().Where(pos => this[pos].Color == player);
        }

        public bool IsInCheck(Player player)
        {
            return PiecePositionsFor(player.Opponent()).Any(pos => 
            {
                Piece piece = this[pos];
                return piece.CanCaptureOpponentKing(pos, this);
            });
        }

        public Board Copy()
        {
            Board copy = new Board();

            foreach (Position pos in PiecePositions())
            {
                copy[pos] = this[pos].Copy();
            }

            return copy;
        }
    }
}
