using GameChess.Models.Figures;

namespace GameChess.Interfaces
{
    public interface IStaticSpecialMove
    {
        public List<string>? SpecialMoves { get; }
        public List<string>? AllStaticSpecialMoves();
        public string SpecialAction(Figure figure, string figureName);
    }
}
