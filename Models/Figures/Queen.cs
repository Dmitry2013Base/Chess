
namespace GameChess.Models.Figures
{
    public class Queen : Figure
    {
        public override string? Id { get; set; }
        public override string Name { get; set; } = "queen";
        public override byte Weight { get; set; } = 3;
        public override ColorFigure Color { get; set; }
        public override string? CurrentCell { get; set; }


        public Queen() : base() { }
        public Queen(ColorFigure color, string currentCell) : base(color, currentCell) { }


        public override List<string>? AllMoves() => FigureMoves.GetQueenMove(this);
    }
}
