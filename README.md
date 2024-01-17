# Introduction

C# dotnet 8 chess library with SAN and FEN parsing/generation, moves validation, PGN filter, import an save games in PGN format, read any Polyglot openings book and build your Polyglot openings book and much more!

# Getting started
## Prerequisites
dotnet 8.0

## Installation
You can install it with the Package Manager in your IDE or alternatively using the command line:

```bash
dotnet add package ChessDotNetCore
```
## Usage

```csharp
string initialBoard = "8/8/3k4/8/8/4K3/8/Q6R w - - 0 1";
ChessGame game = new ChessGame(initialBoard);
game.MakeMove(new Move("f8", "a3", Player.Black), true);
string San = game.AllMoves[0].SAN;
string Fen = game.GetFen();
var GameResult = game.GameResult
```

