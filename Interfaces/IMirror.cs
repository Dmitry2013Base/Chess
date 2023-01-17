using GameChess.Models.Figures;

namespace GameChess.Interfaces
{
    public interface IMirror
    {
        public List<Figure> Mirror(List<Figure> figures);
        public Figure? Clone(Figure figure);
    }
}
