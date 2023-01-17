using GameChess.Interfaces;

namespace GameChess.Models.Field
{
    public class Cell : ICell
    {
        public string? Name { get; set; }
        public ICell.CellColor Color { get; set; }
        public bool Active { get; set; }


        public Cell(string? name, ICell.CellColor color, bool active)
        {
            Name = name;
            Color = color;
            Active = active;
        }
    }
}
