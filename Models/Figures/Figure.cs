
namespace GameChess.Models.Figures
{
    public abstract class Figure
    {
        private DirectionFigure _directionFigure;

        public abstract string? Id { get; set; }
        public abstract string Name { get; set; }
        public abstract byte Weight { get; set; }
        public abstract ColorFigure Color { get; set; }
        public abstract string? CurrentCell { get; set; }
        public virtual DirectionFigure Direction 
        {
            get
            {
                _directionFigure = (DirectionFigure)(int)Color;
                return _directionFigure;
            }
            set
            {
                _directionFigure = value;
            }
        }



        public enum ColorFigure
        {
            White = 1,
            Black = -1,
            Red = 2,
            Green = -2,
        }
        public enum DirectionFigure
        {
            North = 1,
            South = -1,
            West = 2,
            East = -2,
        }



        public Figure()
        {
            Id = Guid.NewGuid().ToString();
        }

        public Figure(ColorFigure color, string currentCell)
        {
            Id = Guid.NewGuid().ToString();
            Color = color;
            CurrentCell = currentCell;
        }

        public abstract List<string>? AllMoves();

        public virtual List<string>? AllAttacks() => AllMoves();

    }
}
