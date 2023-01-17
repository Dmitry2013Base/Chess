using GameChess.Interfaces;
using GameChess.Models.Games;

namespace GameChess.Models.Players
{
    public class Player : IPlayer, IPerson
    {
        public string? Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "Anonymous";
        public Game? Game { get; set; }
        public PlayerFront? Front { get; set; }


        public Player() { }
        public Player(string name) { Name = name; }

        public List<string>? GetMoves(string currentCell) => Game?.Control?.GetMoves(currentCell);

        public List<string>? Move(string oldCell, string newCell, out string figureId)
        {
            figureId = String.Empty;
            return Game?.Move(oldCell, newCell, out figureId);
        }

        public void LeaveGame()
        {
            if (Game != null)
            {
                GameCollecton.Games.Remove(Game);
            }
            Game = null;
            Front = null;
        }
    }
}
