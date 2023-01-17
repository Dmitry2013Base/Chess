using GameChess.Models.Figures;

namespace GameChess.Interfaces
{
    public interface IPawn
    {
        public string? StartCell { get; }
        public string? TempSpecialMoves { get; set; }
    }
}
