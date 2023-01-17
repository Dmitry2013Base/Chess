using GameChess.Interfaces;
using static GameChess.Models.Figures.Figure;

namespace GameChess.Models.Figures
{
    public class Creater : ICreater, IMirror
    {
        public List<Figure> GetFigures()
        {
            List<Figure> figures = new List<Figure>()
            {
                new Pawn(ColorFigure.White, "a-2"),
                new Pawn(ColorFigure.White, "b-2"),
                new Pawn(ColorFigure.White, "c-2"),
                new Pawn(ColorFigure.White, "d-2"),
                new Pawn(ColorFigure.White, "e-2"),
                new Pawn(ColorFigure.White, "f-2"),
                new Pawn(ColorFigure.White, "g-2"),
                new Pawn(ColorFigure.White, "h-2"),
                new Rook(ColorFigure.White, "a-1"),
                new Horse(ColorFigure.White, "b-1"),
                new Elephant(ColorFigure.White, "c-1"),
                new Queen(ColorFigure.White, "d-1"),
                new King(ColorFigure.White, "e-1"),
                new Elephant(ColorFigure.White, "f-1"),
                new Horse(ColorFigure.White, "g-1"),
                new Rook(ColorFigure.White, "h-1"),
            };

            figures.Concat(Mirror(figures));
            return figures;
        }

        public List<Figure> Mirror(List<Figure> figures)
        {
            int count = figures.Count;

            for (int i = 0; i < count * 2 - i; i++)
            {
                var figureClone = Clone(figures[i]);

                if (figureClone != null)
                {
                    figures.Add(figureClone);
                }
            }

            return figures;
        }

        public Figure? Clone(Figure figure)
        {
            List<ColorFigure> colors = Enum.GetValues(typeof(ColorFigure)).Cast<ColorFigure>().Where(e => e != figure.Color).ToList();
            Figure? fig = null;
            if (figure.CurrentCell != null)
            {
                fig = Activator.CreateInstance(figure.GetType(), colors.FirstOrDefault(e => Math.Abs((int)e) == Math.Abs((int)figure.Color)), GetMirrorCell(figure.CurrentCell)) as Figure;
            }

            int enumCount = Enum.GetNames(typeof(ColorFigure)).Length;
            int num = (int)figure.Color;
            string currentCell = String.Empty;

            if (fig != null)
            {
                fig.Weight = figure.Weight;
            }

            return fig;
        }

        private string GetMirrorCell(string currentCell, bool mirrorX = false, bool mirrorY = true)
        {
            string[] part = currentCell.Split("-");
            string xPos = part[0];
            byte yPos = Convert.ToByte(part[1]);

            if (mirrorY)
            {
                byte y = Convert.ToByte(part[1]);
                yPos = (byte)((byte)Field.Field.Y.Length + 1 - y);
            }

            if (mirrorX)
            {
                string[] array = Field.Field.X;
                byte x = (byte)Array.IndexOf(array, xPos);
                xPos = array[array.Length - 1 - x];
            }

            return $"{xPos}-{yPos}";
        }
    }
}
