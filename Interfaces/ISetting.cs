using GameChess.Models;
using GameChess.Models.Field;
using GameChess.Models.Figures;

namespace GameChess.Interfaces
{
    public interface ISetting
    {
        public byte PlayerCount { get; set; }
        public TimeSpan Time { get; set; }
        public TimeSpan AddTime { get; set; }
        public Field? Field { get; set; }
        public List<Figure>? Figures { get; set; }
    }
}
