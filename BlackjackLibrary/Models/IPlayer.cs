using BlackjackLibrary.Enums;
using BlackjackLibrary.Models;
using System.Collections.Generic;

namespace BlackjackLibrary.Interfaces
{
    public interface IPlayer
    {
        string Id { get; set; }
        string Name { get; set; }
        decimal Balance { get; set; }
        decimal BetAmount { get; set; }
        List<Card> Hand { get; set; }
        HandStatus Status { get; set; }
    }
}
