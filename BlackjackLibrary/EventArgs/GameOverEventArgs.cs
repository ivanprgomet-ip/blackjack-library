namespace BlackjackLibrary.EventArgs
{
    public class GameOverEventArgs
    {
        public string GameOverResultsString;

        public GameOverEventArgs(string gameOverResultsString)
        {
            this.GameOverResultsString = gameOverResultsString;
        }
    }
}
