using GameChess.Interfaces;
using System.Diagnostics.Metrics;

namespace GameChess.Models.Games
{
    public class GameCollecton
    {
        public static readonly List<Game> Games = new List<Game>();
        public static readonly List<IPlayer> players = new List<IPlayer>();
    }
}
