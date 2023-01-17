using GameChess.Interfaces;
using GameChess.Models.Field;
using GameChess.Models.Games;
using System.Drawing;
using System.Globalization;
using System.Linq;
using static GameChess.Models.Figures.Figure;

namespace GameChess.Models.Figures
{
    public class FigureMoves
    {
        private static ColorFigure _color;
        private static string? _pawnSpecialMove = String.Empty;
        private static ColorFigure? _pawnSpecialMoveColor = null;
        private static List<Figure>? Figures => GameControl.GetFigures;



        public static List<string>? GetRookMove(Rook rook)
        {
            _color = rook.Color;

            if (rook.CurrentCell == null)
            {
                return null;
            }

            List<string> w = MoveForQueenRookElephant(rook.CurrentCell, '-', '0');
            List<string> n = MoveForQueenRookElephant(rook.CurrentCell, '0', '+');
            List<string> e = MoveForQueenRookElephant(rook.CurrentCell, '+', '0');
            List<string> s = MoveForQueenRookElephant(rook.CurrentCell, '0', '-');

            return w.Concat(n).Concat(e).Concat(s).ToList();
        }

        public static List<string>? GetElephantMove(Elephant elephant)
        {
            _color = elephant.Color;

            if (elephant.CurrentCell == null)
            {
                return null;
            }

            List<string> sw = MoveForQueenRookElephant(elephant.CurrentCell, '-', '-');
            List<string> nw = MoveForQueenRookElephant(elephant.CurrentCell, '-', '+');
            List<string> ne = MoveForQueenRookElephant(elephant.CurrentCell, '+', '+');
            List<string> se = MoveForQueenRookElephant(elephant.CurrentCell, '+', '-');

            return sw.Concat(nw).Concat(ne).Concat(se).ToList();
        }

        public static List<string>? GetQueenMove(Queen queen)
        {
            _color = queen.Color;

            if (queen.CurrentCell == null)
            {
                return null;
            }

            List<string> w = MoveForQueenRookElephant(queen.CurrentCell, '-', '0');
            List<string> n = MoveForQueenRookElephant(queen.CurrentCell, '0', '+');
            List<string> e = MoveForQueenRookElephant(queen.CurrentCell, '+', '0');
            List<string> s = MoveForQueenRookElephant(queen.CurrentCell, '0', '-');
            List<string> sw = MoveForQueenRookElephant(queen.CurrentCell, '-', '-');
            List<string> nw = MoveForQueenRookElephant(queen.CurrentCell, '-', '+');
            List<string> ne = MoveForQueenRookElephant(queen.CurrentCell, '+', '+');
            List<string> se = MoveForQueenRookElephant(queen.CurrentCell, '+', '-');

            return w.Concat(n).Concat(e).Concat(s).Concat(sw).Concat(nw).Concat(ne).Concat(se).ToList();
        }

        public static (int, int) Cell(string currentCellName)
        {
            string[] cell = currentCellName.Split("-");
            int x = Array.IndexOf(Field.Field.X, cell[0]);
            int y = Convert.ToInt32(cell[1]);
            return (x, y);
        }

        public static string Cell((int, int) cell)
        {
            if (cell.Item1 < 0 || cell.Item1 > 7 || cell.Item2 < 1 || cell.Item2 > 8)
            {
                return String.Empty;
            }

            return $"{Field.Field.X[cell.Item1]}-{cell.Item2}";
        }

        public static void CheckCell(List<string> cells, string cell, bool attack = false)
        {
            if (cell != String.Empty)
            {
                Figure? figure = CheckFigureInCell(cell);

                if ((!attack && figure == null) || (attack && figure != null))
                {
                    cells.Add(cell);
                }
            }
        }

        public static Figure? CheckFigureInCell(string cell)
        {
            Figure? figure = Figures?.FirstOrDefault(e => e.CurrentCell == cell);

            if (figure != null)
            {
                return figure;
            }
            return null;
        }

        public static void RemoveEmptyCell(List<string> cells, string cell)
        {
            if (cell != String.Empty)
            {
                cells.Add(cell);
            }
        }

        public static List<string> Сastling(King king)
        {
            List<string> moves = new List<string>();
            if (king.CurrentCell != null)
            {
                var kingCell = Cell(king.CurrentCell);

                if (king.StartCell == king.CurrentCell)
                {
                    if (!king.CheckMove)
                    {
                        List<Figure>? rooks = Figures?.FindAll(e => e is Rook && e.Color == king.Color);

                        for (int i = 0; i < rooks?.Count; i++)
                        {
                            Rook? rook = rooks[i] as Rook;

                            if (rook != null && !rook.CheckMove)
                            {
                                if (rook.StartCell == rook.CurrentCell)
                                {
                                    List<string> temp1 = new List<string>();
                                    List<string> temp2 = new List<string>();
                                    List<string> enemyAttack = GetEnemyMove(king);

                                    if (rook.CurrentCell != null)
                                    {
                                        var rookСell = Cell(rook.CurrentCell);

                                        if (kingCell.Item2 == rookСell.Item2)
                                        {
                                            int x = kingCell.Item1 - rookСell.Item1;

                                            if (x > 0)
                                            {
                                                CheckCell(temp2, Cell((kingCell.Item1 - 1, kingCell.Item2)));

                                                if (temp2.Count != 0)
                                                {
                                                    CheckCell(temp2, Cell((kingCell.Item1 - 2, kingCell.Item2)));
                                                }
                                            }
                                            else
                                            {
                                                CheckCell(temp1, Cell((kingCell.Item1 + 1, kingCell.Item2)));

                                                if (temp1.Count != 0)
                                                {
                                                    CheckCell(temp1, Cell((kingCell.Item1 + 2, kingCell.Item2)));
                                                }
                                            }
                                        }

                                        if (kingCell.Item1 == rookСell.Item1)
                                        {
                                            int y = kingCell.Item2 - rookСell.Item2;
                                            if (y > 0)
                                            {
                                                CheckCell(temp2, Cell((kingCell.Item1, kingCell.Item2 - 1)));

                                                if (temp2.Count != 0)
                                                {
                                                    CheckCell(temp2, Cell((kingCell.Item1, kingCell.Item2 - 2)));
                                                }
                                            }
                                            else
                                            {
                                                CheckCell(temp1, Cell((kingCell.Item1, kingCell.Item2 + 1)));

                                                if (temp1.Count != 0)
                                                {
                                                    CheckCell(temp1, Cell((kingCell.Item1, kingCell.Item2 + 2)));
                                                }
                                            }
                                        }


                                        for (int j = 0; j < temp1.Count; j++)
                                        {
                                            if (enemyAttack.Contains(temp1[j]))
                                            {
                                                temp1.Clear();
                                                break;
                                            }
                                        }

                                        for (int j = 0; j < temp2.Count; j++)
                                        {
                                            if (enemyAttack.Contains(temp2[j]))
                                            {
                                                temp2.Clear();
                                                break;
                                            }
                                        }

                                        List<string>? kingMoves = king.AllMoves();

                                        if (kingMoves != null)
                                        {


                                            for (int j = 0; j < temp1.Count; j++)
                                            {
                                                if (kingMoves.Contains(temp1[j]))
                                                {
                                                    temp1.Remove(temp1[j]);
                                                    if (j != 0)
                                                    {
                                                        j--;
                                                    }
                                                }
                                            }

                                            for (int j = 0; j < temp2.Count; j++)
                                            {
                                                if (kingMoves.Contains(temp2[j]))
                                                {
                                                    temp2.Remove(temp2[j]);
                                                    if (j != 0)
                                                    {
                                                        j--;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    moves.AddRange(temp1);
                                    moves.AddRange(temp2);
                                }
                            }
                        }
                    }
                }
            }
            return moves;
        }

        public static bool? Stalemate()
        {
            List<Figure>? myFigures = Figures?.FindAll(e => e.Color == _color);
            List<Figure>? enemyFigures = Figures?.FindAll(e => e.Color != _color);

            List<string> myMoves = new List<string>();
            List<string> enemyMoves = new List<string>();

            for (int i = 0; i < myFigures?.Count; i++)
            {
                string? currentCell = myFigures[i].CurrentCell;

                if (currentCell != null)
                {
                    List<string>? moves = ExcludeBadMoves(currentCell);

                    if (moves != null)
                    {
                        myMoves.AddRange(moves);
                    }
                }
            }

            for (int i = 0; i < enemyFigures?.Count; i++)
            {
                string? currentCell = enemyFigures[i].CurrentCell;

                if (currentCell != null)
                {
                    List<string>? moves = ExcludeBadMoves(currentCell);

                    if (moves != null)
                    {
                        enemyMoves.AddRange(moves);
                    }
                }
            }

            if (myMoves.Count == 0 && enemyMoves.Count == 0)
            {
                return true;
            }

            if (myMoves.Count == 0 || enemyMoves.Count == 0)
            {
                return null;
            }

            return false;
        }

        public static List<string>? Move(string oldCell, string newCell, out string figureId)
        {
            Figure? figure = Figures?.FirstOrDefault(e => e.CurrentCell == oldCell);
            figureId = String.Empty;
            List<string>? moves = null;

            if (figure != null)
            {
                List<string>? allMoves = figure.AllMoves();

                if (allMoves != null)
                {
                    if (allMoves.Contains(newCell) || figure is IDynamicSpecialMove)
                    {
                        List<string>? attacks = MyAttacks(figure);
                        if (attacks != null)
                        {
                            if (attacks.Contains(newCell))
                            {
                                Figure? fig = Figures?.FirstOrDefault(e => e.CurrentCell == newCell);
                                if (fig != null && fig.Id != null)
                                {
                                    figureId = fig.Id;
                                    Figures?.Remove(fig);
                                }
                            }
                        }

                        if (figure is IDynamicSpecialMove dynamic)
                        {
                            List<string>? dynamicMoves = dynamic.AllDynamicSpecialMoves();

                            if (dynamicMoves != null)
                            {
                                if (dynamicMoves.Contains(newCell))
                                {
                                    moves = dynamic.SpecialAction(newCell, out figureId);
                                }
                            }
                        }

                        _pawnSpecialMoveColor = null;
                        if (figure is Pawn pawn)
                        {
                            if (newCell == pawn?.TempSpecialMoves)
                            {
                                _pawnSpecialMove = pawn.TempSpecialMoves;
                                _pawnSpecialMoveColor = pawn.Color;
                                pawn.TempSpecialMoves = String.Empty;
                            }
                        }

                        figure.CurrentCell = newCell;

                        if (moves != null)
                        {
                            return moves;
                        }

                        return new List<string>() { oldCell, newCell };
                    }
                }
            }

            return null;
        }

        public static List<string>? PawnSpecialMove(Figure figure, string newCell, out string figureId)
        {
            var temp = Cell(newCell);
            string cell = Cell((temp.Item1, temp.Item2 + (int)figure.Color * -1));
            figureId = String.Empty;


            Figure? fig = Figures?.FirstOrDefault(e => e.CurrentCell == cell);
            if (fig != null && fig.Id != null)
            {
                figureId = fig.Id;
                string str = figureId;
                Figure? fig2 = Figures?.FirstOrDefault(e => e.Id == str);

                if (fig2 != null)
                {
                    Figures?.Remove(fig2);
                }
            }
            return null;
        }

        public static string? PawnSpecialMoveTransform(Figure figure, string figureName)
        {
            if (figureName != null)
            {
                string name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(figureName.ToLower());
                Type? type = Type.GetType($"{typeof(Figure).Namespace}.{name}");

                if (type != null)
                {
                    Figure? fig = Activator.CreateInstance(type) as Figure;
                    if (fig != null)
                    {
                        fig.Color = figure.Color;
                        fig.CurrentCell = figure.CurrentCell;
                        Figures?.Add(fig);
                        Figures?.Remove(figure);
                    }
                    return fig?.Id;
                }
            }
            return String.Empty;
        }
        
        public static List<string>? KingSpecialMove(Figure figure, string newCell, out string figureId)
        {
            figureId = String.Empty;
            var cell1 = Cell(newCell);
            if (figure.CurrentCell != null)
            {
                var cell2 = Cell(figure.CurrentCell);

                int pos = cell1.Item1 - cell2.Item1;
                string pos2 = $"h-{cell1.Item2}";
                int coef = 7;

                if (pos < 0)
                {
                    coef = -1;
                    pos *= -1;
                    pos2 = $"a-{cell1.Item2}";
                }


                string newCell1 = Cell((Math.Abs(coef - pos), cell1.Item2));
                Figure? rook = Figures?.FirstOrDefault(e => e.CurrentCell == pos2);
                if (rook != null)
                {
                    rook.CurrentCell = newCell1;
                }
                King? king = figure as King;

                if (king != null)
                {
                    king.CheckMove = true;
                }

                return new List<string>() { figure.CurrentCell, newCell, pos2, newCell1 };
            }
            return null;
        }

        public static List<string> PawnUltraAttack(Figure figure)
        {
            List<string> moves = new List<string>();

            if (_pawnSpecialMove != String.Empty && _pawnSpecialMoveColor != null && figure.Color != _pawnSpecialMoveColor)
            {
                if (_pawnSpecialMove != null && figure.CurrentCell != null)
                {
                    var cell1 = Cell(_pawnSpecialMove);
                    var cell2 = Cell(figure.CurrentCell);

                    int _xCoefficient = (int)figure.Color / Math.Abs((int)figure.Color);
                    int _yCoefficient = _xCoefficient * -1;

                    if ((cell2.Item1 + _xCoefficient == cell1.Item1 || cell2.Item1 + _yCoefficient == cell1.Item1) && cell2.Item2 == cell1.Item2)
                    {
                        Pawn pawn = (Pawn)figure;
                        moves.Add(Cell((cell1.Item1 + (int)pawn.XCoefficient * _yCoefficient, (int)(cell1.Item2 + pawn.YCoefficient * _xCoefficient))));
                    }
                }
            }
            return moves;
        }

        public static List<string>? ExcludeBadMoves(string currentCell)
        {
            Figure? figure = Figures?.FirstOrDefault(e => e.CurrentCell == currentCell);
            string? temp = figure?.CurrentCell;
            List<string>? moves = GetMoves(figure);

            for (int i = 0; i < moves?.Count; i++)
            {
                Figure? fig = Figures?.FirstOrDefault(e => e.CurrentCell == moves[i]);

                if (fig != null)
                {
                    Figures?.Remove(fig);
                }

                if (figure != null)
                {
                    figure.CurrentCell = moves[i];

                    if (CheckShah(figure))
                    {
                        moves.Remove(moves[i]);
                        i--;
                    }

                    if (fig != null)
                    {
                        Figures?.Add(fig);
                    }
                }
            }
            if (figure != null)
            {
                figure.CurrentCell = temp;
            }
            return moves;
        }

        private static List<string> MoveForQueenRookElephant(string currentCell, char symbolHorizontal, char symbolVertical)
        {
            (int, int) cell = Cell(currentCell);

            List<string> listPossibleMove = new List<string>();
            int numLeft = 1;
            int numRight = 1;

            if (symbolHorizontal == '-')
            {
                numLeft = -1;
            }
            if (symbolVertical == '-')
            {
                numRight = -1;
            }
            if (symbolHorizontal == '0')
            {
                numLeft = 0;
            }
            if (symbolVertical == '0')
            {
                numRight = 0;
            }

            bool check = false;

            for (int i = 1; i < 8; i++)
            {
                string itemCell = Cell((cell.Item1 + (i * numLeft), cell.Item2 + (i * numRight)));
                if (itemCell != String.Empty)
                {
                    var figure = CheckFigureInCell(itemCell);

                    if (figure == null)
                    {
                        if (!check)
                        {
                            listPossibleMove.Add(itemCell);
                            continue;
                        }
                        
                    }
                    else
                    {
                        if (figure.Color != _color && !check)
                        {
                            listPossibleMove.Add(itemCell);
                            check = true;
                            continue;
                        }
                    }
                    break;
                }
                else
                {
                    break;
                }
            }

            return listPossibleMove;
        }

        private static List<string> GetEnemyMove(Figure figure)
        {
            List<string> cells = new List<string>();
            List<Figure>? enemyFigures = Figures?.FindAll(e => e.Color != figure.Color);

            enemyFigures?.ForEach(e =>
            {
                List<string>? moveCells = e.AllMoves();

                if (e is Pawn pawn)
                {
                    cells.AddRange(pawn.Attack());
                }
                else
                {
                    if (moveCells != null)
                    {
                        cells.AddRange(moveCells);
                    }
                }
            });
            return cells;
        }

        private static List<string>? GetMoves(Figure? figure)
        {
            if (figure != null)
            {
                List<string>? moves = ExcludeMyFigures(figure);

                if (figure is Pawn pawn)
                {
                    List<string>? attacks = MyAttacks(pawn);

                    if (attacks != null)
                    {
                        moves?.AddRange(attacks);
                    }

                }

                if (figure is IDynamicSpecialMove dynamic)
                {
                    List<string>? dynamicMoves = dynamic.AllDynamicSpecialMoves();

                    if (dynamicMoves != null)
                    {
                        moves?.AddRange(dynamicMoves);
                    }

                }

                return moves;
            }

            return null;
        }

        private static List<string>? ExcludeMyFigures(Figure figure)
        {
            List<string>? moves = figure?.AllMoves();
            List<Figure>? myFigures = Figures?.FindAll(e => e.Color == figure?.Color && e != figure);

            if (moves != null)
            {
                myFigures?.ForEach(e =>
                {
                    if (e.CurrentCell != null && moves.Contains(e.CurrentCell))
                    {
                        moves.Remove(e.CurrentCell);
                    }
                });
            }

            return moves;
        }

        private static List<string>? MyAttacks(Figure figure)
        {
            List<string>? moves = ExcludeMyFigures(figure);
            List<Figure>? enemyFigures = Figures?.FindAll(e => e.Color != figure?.Color);
            List<string>? temp = new();

            for (int i = 0; i < enemyFigures?.Count; i++)
            {
                for (int j = 0; j < moves?.Count; j++)
                {
                    if (enemyFigures[i].CurrentCell == moves[j])
                    {
                        temp.Add(moves[j]);
                    }
                }
            }
            return temp;
        }

        private static bool CheckShah(Figure figure)
        {
            Figure? king = Figures?.FirstOrDefault(e => e.Color == figure.Color && e is King);
            List<Figure>? enemyFigures = Figures?.FindAll(e => e.Color != figure.Color);

            for (int i = 0; i < enemyFigures?.Count; i++)
            {
                List<string>? enemyAttacks = enemyFigures[i].AllAttacks();
                if (enemyAttacks != null && king?.CurrentCell != null)
                {
                    if (enemyAttacks.Contains(king.CurrentCell))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}
