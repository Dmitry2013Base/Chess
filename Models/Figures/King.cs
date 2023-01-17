using GameChess.Interfaces;
using GameChess.Models.Games;
using System.Net.NetworkInformation;

namespace GameChess.Models.Figures
{
    public class King : Figure, IDynamicSpecialMove
    {
        public override string? Id { get; set; }
        public override string Name { get; set; } = "king";
        public override byte Weight { get; set; } = byte.MaxValue;
        public override ColorFigure Color { get; set; }
        public override string? CurrentCell { get; set; }
        public override DirectionFigure Direction { get => base.Direction; set => base.Direction = value; }
        public string? StartCell { get; }
        public bool CheckMove { get; set; }


        public King() : base() { }
        public King(ColorFigure color, string currentCell) : base(color, currentCell) 
        {
            StartCell = CurrentCell;
        }

        public override List<string>? AllMoves()
        {
            List<string> possibleMove = new List<string>();
            if (CurrentCell != null)
            {
                (int, int) cell = FigureMoves.Cell(CurrentCell);

                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (i == 0 && j == 0)
                        {
                            continue;
                        }

                        string nextCell = FigureMoves.Cell((cell.Item1 + i, cell.Item2 + j));
                        FigureMoves.RemoveEmptyCell(possibleMove, nextCell);
                    }
                }
            }

            return possibleMove;
        }

        public List<string>? AllDynamicSpecialMoves() => FigureMoves.Сastling(this);

        public List<string>? SpecialAction(string newCell, out string figureId) => FigureMoves.KingSpecialMove(this, newCell, out figureId);
    }
}
