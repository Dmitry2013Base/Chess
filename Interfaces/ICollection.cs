using GameChess.Models.Games;

namespace GameChess.Interfaces
{
    public interface ICollection
    {
        public List<Game> GetGames();
        public void AddGame(Game game);
        public void RemoveGame(Game game);
    }
}
