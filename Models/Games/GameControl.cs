using GameChess.Interfaces;
using GameChess.Models.Field;
using GameChess.Models.Figures;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using static GameChess.Models.Figures.Figure;

namespace GameChess.Models.Games
{
    public class GameControl
    {
        private static List<Figure>? _figures;
        private readonly List<ColorFigure> _colors;
        private readonly List<Figure>? _kings;
        private static int _currentColorMove;


        public static List<Figure>? GetFigures { get => _figures; }


        public GameControl(ISetting? setting)
        {
            _figures = setting?.Figures;
            _kings = _figures?.FindAll(e => e is King);
            _colors = Enum.GetValues(typeof(ColorFigure)).Cast<ColorFigure>().ToList();
            _colors.RemoveAll(e => Math.Abs((int)e) > _kings?.Count / 2);
            _currentColorMove = (int)_colors.FirstOrDefault(e => e == ColorFigure.White);
        }

        public List<string>? GetMoves(string currentCell) => FigureMoves.ExcludeBadMoves(currentCell);

        public bool? CheckStalemate() => FigureMoves.Stalemate();

        public List<string>? Move(string oldCell, string newCell, out string figureId)
        {
            Figure? figure = _figures?.FirstOrDefault(e => e.CurrentCell == oldCell);

            if (figure != null && (int)figure.Color == _currentColorMove)
            {
                var moves = FigureMoves.Move(oldCell, newCell, out figureId);

                if (moves?.Count != 0)
                {
                    _currentColorMove *= -1;
                }
                return moves;
            }
            figureId = String.Empty;
            return null;
        } 

        public string Transform(string newCell, string figureName)
        {
            Figure? figure = _figures?.FirstOrDefault(e => e.CurrentCell == newCell);

            if (figure is IStaticSpecialMove move)
            {
                if (move.SpecialMoves != null && move.SpecialMoves.Contains(newCell))
                {
                    return move.SpecialAction(figure, figureName);
                }
            }
            return String.Empty;
        }
    }
}
