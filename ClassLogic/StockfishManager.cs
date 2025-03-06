using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

public class StockfishManager : IDisposable
{
    private Process engineProcess;
    private bool isInitialized;
    const string EnginePath = @"Assets\Engine\stockfish.exe";


    public void Initialize()
    {
        if (!File.Exists(EnginePath))
            throw new FileNotFoundException("Stockfish engine not found!");

        engineProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = EnginePath,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };

        engineProcess.Start();
        SendCommand("uci");
        WaitForResponse("uciok");
        isInitialized = true;
    }

    public string GetBestMove(string fenPosition, int thinkTimeMs = 1000)
    {
        if (!isInitialized) return null;

        SendCommand($"position fen {fenPosition}");
        SendCommand($"go movetime {thinkTimeMs}");

        var response = WaitForResponse("bestmove");
        var match = Regex.Match(response, @"bestmove (\w+)");
        return match.Success ? match.Groups[1].Value : null;
    }

    private void SendCommand(string command)
    {
        engineProcess.StandardInput.WriteLine(command);
        engineProcess.StandardInput.Flush();
    }

    private string WaitForResponse(string waitFor)
    {
        var output = new StringBuilder();
        while (true)
        {
            var line = engineProcess.StandardOutput.ReadLine();
            if (line == null) break;
            output.AppendLine(line);
            if (line.StartsWith(waitFor)) break;
        }
        return output.ToString();
    }

    public void Dispose()
    {
        if (engineProcess != null)
        {
            SendCommand("quit");
            if (!engineProcess.WaitForExit(1000))
                engineProcess.Kill();
            engineProcess.Dispose();
        }
    }
}