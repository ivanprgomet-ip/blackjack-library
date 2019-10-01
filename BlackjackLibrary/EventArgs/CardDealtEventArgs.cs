using BlackjackLibrary.Enums;
using BlackjackLibrary.Models;

namespace BlackjackLibrary.EventArgs
{
    public class CardDealtEventArgs {
        public string playerid;
        public Card card;
        public int cardIndex;

        public CardDealtEventArgs(string playerid, Card card, int cardIndex)
        {
            this.playerid = playerid;
            this.card = card;
            this.cardIndex = cardIndex;
        }
    }
}
