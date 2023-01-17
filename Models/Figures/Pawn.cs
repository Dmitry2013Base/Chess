using GameChess.Interfaces;
using GameChess.Models.Field;
using GameChess.Models.Games;
using System.Globalization;
using System.Linq;

namespace GameChess.Models.Figures
{
    public class Pawn : Figure, IPawn, IDynamicSpecialMove, IStaticSpecialMove
    {
        private List<string>? _specialMoves;
        private int _xCoefficient;


        public override string? Id { get; set; }
        public override string Name { get; set; } = "pawn";
        public override byte Weight { get; set; } = 1;
        public override ColorFigure Color { get; set; }
        public override string? CurrentCell { get; set; }
        public string? StartCell { get; }
        public override DirectionFigure Direction { get => base.Direction; set => base.Direction = value; }
        public List<string>? SpecialMoves
        {
            get
            {
                _specialMoves = SpecialMove();
                return _specialMoves;
            }
        }
        public string? TempSpecialMoves { get; set; }
        private double ZCoefficient
        {
            get
            {
                _xCoefficient = (int)Color / Math.Abs((int)Color);
                return (double)Math.Abs((double)Color);
            }
        }
        public double XCoefficient
        {
            get
            {
                return (ZCoefficient - 1) / ZCoefficient * ZCoefficient;
            }
        }
        public double YCoefficient
        {
            get
            {
                return (2 - ZCoefficient) / ZCoefficient;
            }
        }


        public Pawn() : base() { }
        public Pawn(ColorFigure color, string currentCell) : base(color, currentCell) 
        { 
            StartCell = CurrentCell;
        }

        private List<string> Move(bool flag = false)
        {
            List<string> cells = new List<string>();

            if (CurrentCell != null)
            {
                (int, int) cell = FigureMoves.Cell(CurrentCell);

                FigureMoves.CheckCell(cells, FigureMoves.Cell(((int)(cell.Item1 + (XCoefficient * _xCoefficient)), (int)(cell.Item2 + (YCoefficient * _xCoefficient)))));

                if (StartCell == CurrentCell && cells.Count != 0)
                {
                    TempSpecialMoves = FigureMoves.Cell(((int)(cell.Item1 + (2 * XCoefficient * _xCoefficient)), (int)(cell.Item2 + (2 * YCoefficient * _xCoefficient))));
                    FigureMoves.CheckCell(cells, TempSpecialMoves);
                }
                cells.AddRange(Attack(true));
            }

            return cells;
        }
        public List<string> Attack(bool flag = true)
        {
            List<string> cells = new List<string>();
            if (CurrentCell != null)
            {
                (int, int) cell = FigureMoves.Cell(CurrentCell);
                CheckCell(cells, FigureMoves.Cell(((int)(cell.Item1 + (XCoefficient * _xCoefficient) + YCoefficient), (int)(cell.Item2 + (YCoefficient * _xCoefficient) + XCoefficient))), flag);
                CheckCell(cells, FigureMoves.Cell(((int)(cell.Item1 + (XCoefficient * _xCoefficient) - YCoefficient), (int)(cell.Item2 + (YCoefficient * _xCoefficient) - XCoefficient))), flag);
            }
            return cells;
        }
        private void CheckCell(List<string> cells, string cell, bool attack = false)
        {
            if (cell != String.Empty)
            {
                Figure? figure = FigureMoves.CheckFigureInCell(cell);
                Figure? figureEnemy = FigureMoves.CheckFigureInCell(CurrentCell);

                if (figure != null && figure.Color != figureEnemy.Color)
                {
                    cells.Add(cell);
                }
            }
        }
        private List<string> SpecialMove()
        {
            int _direction = (int)Direction;
            string[] field = Field.Field.X;
            List<string> specialMoves = new List<string>();

            if (StartCell != null)
            {
                int y = int.Parse(StartCell.Split('-')[1]) - _direction + (7 * _direction);

                for (int i = 0; i < field.Length; i++)
                {
                    specialMoves.Add($"{field[i]}-{Math.Abs(y)}");
                }
            }

            return specialMoves;
        }
        public override List<string> AllMoves() => Move(true);
        public override List<string> AllAttacks() => Attack();
        public List<string> AllDynamicSpecialMoves() => FigureMoves.PawnUltraAttack(this);
        public List<string> AllStaticSpecialMoves() => SpecialMove();
        List<string>? IDynamicSpecialMove.SpecialAction(string newCell, out string figureId) => FigureMoves.PawnSpecialMove(this, newCell, out figureId);
        string IStaticSpecialMove.SpecialAction(Figure figure, string figureName) => FigureMoves.PawnSpecialMoveTransform(figure, figureName);
    }
}
