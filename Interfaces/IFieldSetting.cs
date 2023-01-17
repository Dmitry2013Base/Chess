namespace GameChess.Interfaces
{
    public interface IFieldSetting
    {
        public List<ICell>? GetCells(bool mirrorX = false);
    }
}
