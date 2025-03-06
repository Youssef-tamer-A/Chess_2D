using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChessLogic;

namespace ChessUI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly Image[,] piecesImages = new Image[8, 8];
    private readonly Rectangle[,] highlights = new Rectangle[8, 8];
    private readonly Dictionary<Position, Move> legalMoves = new Dictionary<Position, Move>();

    private GameState gameState;
    private Position selectedPosition = null;

    private StartMenu _startMenu;
    private Board _currentBoard;

    private StockfishManager stockfish;
    private bool vsComputer;

    public MainWindow()
    {
        InitializeComponent();
        InitializeBoard();

        gameState = new GameState(Board.InitialBoard(), Player.White);
        DrawBoard(gameState.Board);
        SetCursor(gameState.CurrentPlayer);

        ShowStartMenu();
    }
    private void InitializeBoard()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Image image = new Image();
                piecesImages[i, j] = image;
                PieceGrid.Children.Add(image);

                Rectangle highlight = new Rectangle();
                highlights[i, j] = highlight;
                HighlightGrid.Children.Add(highlight);
            }
        }
    }

    private void DrawBoard(Board board)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Piece piece = board[i, j];
                piecesImages[i, j].Source = Images.GetImage(piece);
            }
        }
    }

    private void BoardGriad_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if(IsMenuOnScreen())
        {
            return;
        }

        Point point = e.GetPosition(PieceGrid);
        Position pos = ToSquarPosition(point);

        if (selectedPosition == null)
        {
            OnFromPositionSelected(pos);
        }
        else
        {
            OnToPositionSelected(pos);
        }
    }

    private Position ToSquarPosition(Point point)
    {
        double squareSize = BoardGrid.ActualWidth / 8;
        int row = (int)(point.Y / squareSize);
        int col = (int)(point.X / squareSize);
        return new Position(row, col);
    }

    private void OnFromPositionSelected(Position pos)
    {
        IEnumerable<Move> moves = gameState.LegalMovesForPiece(pos);

        if (moves.Any())
        {
            selectedPosition = pos;
            CashMove(moves);
            ShowHighlights();
        }
    }

    private void OnToPositionSelected(Position pos)
    {
        selectedPosition = null;
        HideHighlights();

        if (legalMoves.TryGetValue(pos, out Move move))
        {
            if (move.Type ==  MoveType.PawnPromotion)
                HandlePromotion(move.FromPos, move.ToPos);
            else
                HandleMove(move);
        }
    }

    private void HandlePromotion(Position from, Position to)
    {
        piecesImages[to.Row, to.Column].Source = Images.GetImage(gameState.CurrentPlayer, PieceType.pawn);
        piecesImages[from.Row, from.Column].Source = null;

        PromotionMenu promMenu = new PromotionMenu(gameState.CurrentPlayer);
        MenuContainer.Content = promMenu;

        promMenu.PieceSelected += type =>
        {
            MenuContainer.Content = null;
            Move promMove = new PawnPromotion(from, to, type);
            HandleMove(promMove);
        };
    }

    private void HandleMove(Move move)
    {
        gameState.MakeMove(move);
        DrawBoard(gameState.Board);
        SetCursor(gameState.CurrentPlayer);

        if (gameState.IsGameOver())
        {
            ShowGameOver();
        }
    }


    private void CashMove(IEnumerable<Move> Moves)
    {
        legalMoves.Clear();

        foreach (Move move in Moves)
        {
            legalMoves[move.ToPos] = move;
        }
    }

    private void ShowHighlights()
    {
        Color color = Color.FromArgb(150, 125, 255, 125);

        foreach (Position to in legalMoves.Keys)
        {
            highlights[to.Row, to.Column].Fill = new SolidColorBrush(color);
        }
    }

    private void HideHighlights()
    {
        foreach (Position to in legalMoves.Keys)
        {
            highlights[to.Row, to.Column].Fill = Brushes.Transparent;
        }
    }

    private void SetCursor(Player player)
    {
        if (player == Player.White)
            Cursor = ChessCursors.WhiteCursor;
        else
            Cursor = ChessCursors.BlackCursor;
    }

    private bool IsMenuOnScreen()
    {
        return MenuContainer.Content != null;
    }

    private void ShowGameOver()
    {
        GameOverMenu gameovermenu = new GameOverMenu(gameState);
        MenuContainer.Content = gameovermenu;

        gameovermenu.OptionSelected += option =>
        {
            if (option == Option.Restart)
            {
                MenuContainer.Content = null;
                RestartGame();
            }
            else
            {
                Application.Current.Shutdown();
            }
        };
    }

    private void RestartGame()
    {
        HideHighlights();
        legalMoves.Clear();
        gameState = new GameState(Board.InitialBoard(), Player.White);
        DrawBoard(gameState.Board);
        SetCursor(gameState.CurrentPlayer);
    }

    private void ShowStartMenu()
    {

        // Initialize start menu
        _startMenu = new StartMenu();
        _startMenu.PlayerVsPlayerSelected += StartPlayerVsPlayer;
        _startMenu.PlayerVsComputerSelected += StartPlayerVsComputer;
        MenuContainer.Content = _startMenu;
    }

    private void StartGame()
    {

        MenuContainer.Content = null;

        // Initialize your existing board logic here
        _currentBoard = new Board();
        InitializePieces();  // Your existing piece initialization
    }

    private void StartPlayerVsPlayer()
    {
        StartGame();
        // Your existing 1v1 logic
    }

    private void StartPlayerVsComputer()
    {
        try
        {
            stockfish = new StockfishManager();
            stockfish.Initialize();
            vsComputer = true;
            StartGame();
        }
        catch (FileNotFoundException)
        {
            MessageBox.Show("Stockfish engine not found! Computer mode unavailable.");
            vsComputer = false;
            ShowStartMenu();
        }
    }

    private async void HandlePlayerMove(Move move)
    {
        if (!vsComputer) return;

        _currentBoard.MakeMove(move);
        UpdateBoardDisplay();

        if (_currentBoard.CurrentPlayer == Player.Black)
        {
            var fen = _currentBoard.ToFenString();
            var bestMove = await Task.Run(() => stockfish.GetBestMove(fen));

            if (bestMove != null)
            {
                var computerMove = ParseMove(bestMove); // Implement move parsing
                _currentBoard.MakeMove(computerMove);
                UpdateBoardDisplay();
            }
        }
    }
    // Keep your existing board logic below
    private void InitializePieces() { /* ... */ }
    private void BoardGriad_MouseDownStart(object sender, MouseButtonEventArgs e) { /* ... */ }
    private void UpdateBoardDisplay()
    {
        DrawBoard(gameState.Board);
        SetCursor(gameState.CurrentPlayer);
    }
    private Move ParseMove(string bestMove)
    {
        // Assuming the bestMove is in UCI format (e.g., "e2e4", "e7e8q")
        Position from = new Position(bestMove[1] - '1', bestMove[0] - 'a');
        Position to = new Position(bestMove[3] - '1', bestMove[2] - 'a');
        PieceType? promotion = null;

        if (bestMove.Length == 5)
        {
            promotion = bestMove[4] switch
            {
                'q' => PieceType.queen,
                'r' => PieceType.rook,
                'b' => PieceType.bishop,
                'n' => PieceType.knight,
                _ => throw new ArgumentException("Invalid promotion piece type")
            };
        }

        // Assuming Move is an abstract class, we need to create an instance of a derived class
        if (promotion.HasValue)
        {
            return new PawnPromotion(from, to, promotion.Value);
        }
        else
        {
            return new NormalMove(from, to);
        }
    }
}


