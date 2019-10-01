namespace BlackjackLibrary.Enums
{
    public class Decision
    {
        public BlackjackDecision PlayerDecision { get; set; }
        public string PlayerId { get; set; }
        public Decision(string playerid, BlackjackDecision decision)
        {
            PlayerId = playerid;
            PlayerDecision = decision;
        }
    }
}