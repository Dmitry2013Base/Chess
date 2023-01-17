using GameChess.Interfaces;
using GameChess.Models.Games;
using static GameChess.Models.Figures.Figure;

namespace GameChess.Models.Players
{
    public class PlayerFront
    {
        private DirectionFigure? _directionFigure;
        private readonly List<ICell>? _field;

        public ColorFigure? Color { get; }
        public DirectionFigure? Direction
        {
            get
            {
                if (Color != null)
                {
                    _directionFigure = (DirectionFigure)(int)Color;
                }

                return _directionFigure;
            }
        }
        public List<ICell>? Field
        {
            get
            {
                switch (Direction)
                {
                    case DirectionFigure.North:
                        {
                            return _field?.OrderByDescending(e => e.Name?.Split("-")[1]).ToList();
                        }
                    case DirectionFigure.South:
                        {
                            return _field?.OrderBy(e => e.Name?.Split("-")[1]).ToList();
                        }
                    default:
                        {
                            return null;
                        }
                }
            }
        }
        public ChessTimer? Timer { get; set; }


        public PlayerFront(ColorFigure? color, ISetting? setting)
        {
            Color = color;
            if (setting != null)
            {
                Timer = new ChessTimer(setting.Time, setting.AddTime);
            }
            _field = setting?.Field?.Cells;
        }
    }
}
