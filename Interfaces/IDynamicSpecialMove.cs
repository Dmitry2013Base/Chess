using GameChess.Models.Figures;

namespace GameChess.Interfaces
{
    public interface IDynamicSpecialMove
    {
        public List<string>? AllDynamicSpecialMoves();
        public List<string>? SpecialAction(string newCell, out string figureId);
    }
}
