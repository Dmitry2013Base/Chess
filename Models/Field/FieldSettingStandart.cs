using GameChess.Interfaces;

namespace GameChess.Models.Field
{
    public class FieldSettingStandart : IFieldSetting
    {
        public List<ICell>? GetCells(bool mirrorX = false)
        {
            string[] X = new string[] { "a", "b", "c", "d", "e", "f", "g", "h" };
            int[] Y = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            List<ICell> fields = new List<ICell>();
            ICell.CellColor cellColor;
            bool switchColor = true;

            for (int i = 0; i < X.Length; i++)
            {
                for (int j = 0; j < Y.Length; j++)
                {
                    if (switchColor)
                    {
                        if (j % 2 == 0)
                        {
                            cellColor = ICell.CellColor.White;
                        }
                        else
                        {
                            cellColor = ICell.CellColor.Black;
                        }
                    }
                    else
                    {
                        if (j % 2 == 0)
                        {
                            cellColor = ICell.CellColor.Black;
                        }
                        else
                        {
                            cellColor = ICell.CellColor.White;
                        }
                    }

                    string cellName = $"{X[i]}-{Y[j]}";
                    fields.Add(new Cell(cellName, cellColor, true));
                }

                switchColor = !switchColor;
            }

            if (mirrorX)
            {
                return fields.OrderBy(e => e.Name?.Split("-")[1]).ToList();
            }
            else
            {
                return fields.OrderByDescending(e => e.Name?.Split("-")[1]).ToList();
            }
        }
    }
}
