using GameChess.Interfaces;

namespace GameChess.Models.Games
{

    public class GameLog : IGameLog
    {
        public Guid Id { get; set; }
        public string? Message { get; set; }

        public GameLog(string? message)
        {
            Id = Guid.NewGuid();
            Message = message;
        }
    }
}
