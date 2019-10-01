using BlackjackLibrary.EventArgs;
using BlackjackLibrary.Interfaces;
using BlackjackLibrary.Models;
using System;
using System.Collections.Generic;

namespace BlackjackLibrary.Enums
{
    public interface IDealer: IPlayer
    {
        event EventHandler<CardDealtEventArgs> CardDealt;
    }
}