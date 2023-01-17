
using GameChess.Interfaces;
using GameChess.Models.Games;

namespace GameChess.Models.Figures
{
    public class Horse : Figure
    {
        public override string? Id { get; set; }
        public override string Name { get; set; } = "horse";
        public override byte Weight { get; set; } = 3;
        public override ColorFigure Color { get; set; }
        public override string? CurrentCell { get; set; }


        public Horse() : base() { }
        public Horse(ColorFigure color, string currentCell) : base(color, currentCell) { }

        public override List<string>? AllMoves()
        {
            if (CurrentCell != null)
            {
                List<string> possibleMove = new List<string>();
                (int, int) cell = FigureMoves.Cell(CurrentCell);

                FigureMoves.RemoveEmptyCell(possibleMove, FigureMoves.Cell((cell.Item1 - 1, cell.Item2 + 2)));
                FigureMoves.RemoveEmptyCell(possibleMove, FigureMoves.Cell((cell.Item1 + 1, cell.Item2 + 2)));
                FigureMoves.RemoveEmptyCell(possibleMove, FigureMoves.Cell((cell.Item1 - 1, cell.Item2 - 2)));
                FigureMoves.RemoveEmptyCell(possibleMove, FigureMoves.Cell((cell.Item1 + 1, cell.Item2 - 2)));
                FigureMoves.RemoveEmptyCell(possibleMove, FigureMoves.Cell((cell.Item1 - 2, cell.Item2 + 1)));
                FigureMoves.RemoveEmptyCell(possibleMove, FigureMoves.Cell((cell.Item1 + 2, cell.Item2 + 1)));
                FigureMoves.RemoveEmptyCell(possibleMove, FigureMoves.Cell((cell.Item1 - 2, cell.Item2 - 1)));
                FigureMoves.RemoveEmptyCell(possibleMove, FigureMoves.Cell((cell.Item1 + 2, cell.Item2 - 1)));

                return possibleMove;
            }
            return null;
        }
    }
}
