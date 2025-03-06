using System.Windows.Input;
using System.IO;

namespace ChessUI
{
    public class ChessCursors
    {
        public static readonly Cursor WhiteCursor = LoadCursor("Assets/CursorW.cur");
        public static readonly Cursor BlackCursor = LoadCursor("Assets/CursorB.cur");

        private static Cursor LoadCursor(string filePath)
        {
            Stream stream = App.GetResourceStream(new Uri(filePath, UriKind.Relative)).Stream;
            return new Cursor(stream, true);
        }
    }
}
