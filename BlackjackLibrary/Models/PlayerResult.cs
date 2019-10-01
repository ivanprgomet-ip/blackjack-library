using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackjackLibrary.Enums;

namespace BlackjackLibrary.Enums
{
    public class PlayerResultsData
    {
        public Winninghand CurrentWinner { get; internal set; }
        public decimal Amount { get; internal set; }
        public string InfoText { get; internal set; }
    }
}
