
namespace GameChess.Models.Figures
{
    public class Elephant : Figure
    {
        public override string? Id { get; set; }
        public override string Name { get; set; } = "elephant";
        public override byte Weight { get; set; } = 3;
        public override ColorFigure Color { get; set; }
        public override string? CurrentCell { get; set; }

        public Elephant() : base() { }
        public Elephant(ColorFigure color, string currentCell) : base(color, currentCell) { }

        public override List<string>? AllMoves() => FigureMoves.GetElephantMove(this);
    }
}
