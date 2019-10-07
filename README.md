# BlackjackLibrary
[![NuGet](https://img.shields.io/nuget/v/IvanPrgomet.BlackjackLibrary.svg)](https://nuget.org/packages/IvanPrgomet.BlackjackLibrary/)
[![NuGet](https://img.shields.io/nuget/dt/IvanPrgomet.BlackjackLibrary.svg)](https://nuget.org/packages/IvanPrgomet.BlackjackLibrary/)


A blackjack library containing methods and functionality which quickly lets anyone create their  own blackjack game in various .net application model such as windows forms, wpf, console etc. BlackjackLibrary currently supports 1v1 with blackjack rules for hitting, standing, doubling and betting.

## Download
- [Nuget package](https://www.nuget.org/packages/IvanPrgomet.BlackjackLibrary/)

## Features (latest v.1.0.2)
- Enables for quick creation of a blackjack game in any .NET app model
- Encapsulated business logic layer
- Support for Hitting, Standing, Doubling and Betting.
- Support for player vs dealer

# Using the library
To make full use of the blackjakc library, pleae consider using the blackjack game class, which is the single entry point of the library. This is where all the functionality that you will need to create the game can be accessed from. This class will contain methods such as "hit" and "stand", but also events that you can subscribe to for the full experience of the library functionality.

## using statements
After installing the nuget package into your .NET application, start by adding the using statetments to get access to required object types of the library:
```csharp
using BlackjackLibrary.API;
using BlackjackLibrary.EventArgs;
using BlackjackLibrary.Models;
using System;
```

## Instantiating the game class
Instantiate the blackjack game class to get access to all functionality of the library. To instantiate the game class, please provide the constructor with the required arguments:
```csharp
public class Program
{
    private static BlackjackGame game;
    static void Main(string[] args)
    {
        game = new BlackjackGame(
                new AiDealer("dealer", 1000), // AiDealer with name and starting balance 
                new HumanPlayer("player", 50),  // HumanPlayer with name and starting balance
                6); // number of decks that the game will use
    }
}
```

## Game event subscriptions
After instantiating the game class, you can now subscribe to the events of the class. These are the events that will drive the blackjack game forward. Notice you will have to create the event handler methods yourself.

```csharp
public class Program
{
    private static BlackjackGame game;
    static void Main(string[] args)
    {
        game = new BlackjackGame(
                new AiDealer("dealer", 1000), // AiDealer with name and starting balance 
                new HumanPlayer("player", 50),  // HumanPlayer with name and starting balance
                6); // number of decks that the game will use

        game.GameOver += OnGameOver; // runs whenever the game is over
        game.RoundEvaluated += OnRoundEvaluated; // runs whenever the round has been evaluated
        game.DealerInfo.CardDealt += OnCardDealt; // runs whenever a card is dealt to anyone
        game.OnValidationError += OnValidationError; // runs whenever there is a validation error
    }
}
```