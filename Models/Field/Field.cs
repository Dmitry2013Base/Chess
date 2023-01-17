using GameChess.Interfaces;

namespace GameChess.Models.Field
{
    public class Field
    {
        public static readonly string[] X = new string[] { "a", "b", "c", "d", "e", "f", "g", "h" };
        public static readonly byte[] Y = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

        private readonly IFieldSetting? _fieldSetting;

        public Guid Id { get; set; }
        public List<ICell>? Cells
        {
            get => _fieldSetting?.GetCells();
        }

        public Field() { }

        public Field(IFieldSetting fieldSetting)
        {
            _fieldSetting = fieldSetting;
        }
    }
}
