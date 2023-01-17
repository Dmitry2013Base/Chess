using GameChess.Interfaces;
using GameChess.Models.Figures;
using System.Linq;
using static GameChess.Models.Figures.Figure;

namespace GameChess.Models.Games
{
    public class Game
    {
        private List<ColorFigure>? _colors;
        private ColorFigure _tempColor;
        private ISetting? _setting;
        private byte? _playerCount;

        public string? Id { get; set; } = Guid.NewGuid().ToString();
        public ISetting? Setting
        { 
            get 
            { 
                return _setting; 
            }
            set
            {
                _setting = value;
                _playerCount = _setting?.PlayerCount;
                _colors = Enum.GetValues(typeof(ColorFigure)).Cast<ColorFigure>().OrderBy(e => Math.Abs((int)e)).ToList();
            }
        }
        public Stack<IGameLog>? GameLogs { get; set; }
        public GameControl? Control { get; set; }


        public Game(ISetting setting)
        {
            Setting = setting;
            GameLogs = new Stack<IGameLog>();
            Control = new GameControl(Setting);
        }

        public ColorFigure? Join(ColorFigure color = ColorFigure.White)
        {
            if (_playerCount != 0)
            {
                bool? check = _colors?.Contains(color);

                if (check.HasValue && check.Value)
                {
                    _tempColor = color;
                }
                else
                {
                    if (_colors != null)
                    {
                        color = _colors.FirstOrDefault(e => Math.Abs((int)e) == Math.Abs((int)_tempColor));
                    }
                }

                _colors?.Remove(color);
                _playerCount--;
                return color;
            }
            return null;
        }

        public List<string>? Move(string oldCell, string newCell, out string figureId)
        {
            figureId = String.Empty;
            List<string>? result = Control?.Move(oldCell, newCell, out figureId);

            if (result?.Count != 0)
            {
                GameLogs?.Push(new GameLog($"{oldCell} --> {newCell}"));
            }

            return result;
        }

    }
}
