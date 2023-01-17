namespace GameChess.Interfaces
{
    public interface IPerson
    {
        public List<string>? GetMoves(string currentCell);
        public void LeaveGame();
    }
}
