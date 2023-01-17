using GameChess.Models;
using GameChess.Models.Games;
using GameChess.Models.Players;
using static GameChess.Models.Figures.Figure;

namespace GameChess.Interfaces
{
    public interface IPlayer
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public Game? Game { get; set; }
        public PlayerFront? Front { get; set; }


        public List<string>? Move(string oldCell, string newCell, out string figureId);

        public string? CreateGame(ISetting? setting, ColorFigure color = ColorFigure.White)
        {
            Game? game = null;
            if (setting != null)
            {
                game = new Game(setting);
                GameCollecton.Games.Add(game);
                JoinGame(game.Id, color);
            }
            return game?.Id;
        }

        public void JoinGame(string? id, ColorFigure color)
        {
            Game? game = GameCollecton.Games.FirstOrDefault(e => e.Id == id);

            if (game != null)
            {
                ColorFigure? colorFigure = game.Join(color);
                if (colorFigure != null)
                {
                    Game = game;
                    Front = new PlayerFront(colorFigure, Game?.Setting);
                }
            }
        }
    }
}
