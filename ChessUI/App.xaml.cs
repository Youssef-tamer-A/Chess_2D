using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace ChessUI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnExit(ExitEventArgs e)
    {
        (MainWindow as MainWindow)?.DisposeStockfish();
        base.OnExit(e);
    }
}
public partial class MainWindow : Window, IAnimatable, ISupportInitialize, IFrameworkInputElement, IInputElement, IQueryAmbient, IAddChild, IComponentConnector
{
    // Other members...

    public StockfishManager Stockfish
    {
        get { return stockfish; }
    }

    public void DisposeStockfish()
    {
        stockfish?.Dispose();
    }

    // Other members...
}

