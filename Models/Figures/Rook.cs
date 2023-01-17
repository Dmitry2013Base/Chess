
namespace GameChess.Models.Figures
{
    public class Rook : Figure
    {
        public override string? Id { get; set; }
        public override string Name { get; set; } = "rook";
        public override byte Weight { get; set; } = 5;
        public override ColorFigure Color { get; set; }
        public override string? CurrentCell { get; set; }
        public string? StartCell { get; }
        public bool CheckMove { get; set; }


        public Rook() : base() { }
        public Rook(ColorFigure color, string currentCell) : base(color, currentCell) 
        {
            StartCell = CurrentCell;
        }

        public override List<string>? AllMoves() => FigureMoves.GetRookMove(this);
    }
}
