namespace GameChess.Interfaces
{
    public interface IGameLog
    {
        public Guid Id { get; set; }
        public string? Message { get; set; }
    }
}
