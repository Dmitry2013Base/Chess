using GameChess.Interfaces;
using GameChess.Models.Figures;
using GameChess.Models.Field;

namespace GameChess.Models.GameSettings
{
    public class StandartGame : ISetting
    {
        public byte PlayerCount { get; set; }
        public TimeSpan Time { get; set; }
        public TimeSpan AddTime { get; set; }
        public Field.Field? Field { get; set; }
        public List<Figure>? Figures { get; set; }
    }
}
