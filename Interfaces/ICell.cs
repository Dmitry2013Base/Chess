
namespace GameChess.Interfaces
{
    public interface ICell
    {
        public string? Name { get; set; }
        public CellColor Color { get; set; }
        public bool Active { get; set; }

        public enum CellColor
        {
            White,
            Black,
        }
    }
}
