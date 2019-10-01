using BlackjackLibrary.Enums;

namespace BlackjackLibrary.EventArgs
{
    public class RoundEvaluatedEventArgs
    {
        public PlayerResultsData Results { get; set; }
        public RoundEvaluatedEventArgs(PlayerResultsData results)
        {
            this.Results = results;
        }
    }
}