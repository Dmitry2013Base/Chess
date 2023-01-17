using GameChess.Interfaces;
using GameChess.Models.Field;
using GameChess.Models.Figures;
using GameChess.Models.Games;
using GameChess.Models.GameSettings;
using GameChess.Models.Players;
using GameChess.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace GameChess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IHubContext<GameChat> _hub;
        public GameController(IHubContext<GameChat> hub)
        {
            _hub = hub;
        }

        [HttpGet]
        [Route("CreateGame/{time}/{addTime}")]
        public IActionResult CreateGame(string time, string addTime)
        {
            ICreater creater = new Creater();
            IPlayer player = new Player();
            GameCollecton.players.Add(player);

            StandartGame standart = new StandartGame()
            {
                PlayerCount = 2,
                Time = TimeSpan.FromMinutes(Convert.ToDouble(time)),
                AddTime = TimeSpan.FromSeconds(Convert.ToDouble(addTime)),
                Field = new Field(new FieldSettingStandart()),
                Figures = creater.GetFigures(),
            };

            Random random = new Random();
            int num = random.Next(0, 2);

            List<Figure.ColorFigure>? color = standart?.Figures?.Select(e => e.Color).ToHashSet().ToList();
            if (color != null)
            {
                player.CreateGame(standart, color[num]);
            } 
            return RedirectToAction("Game", "Home", new { userId = player.Id });
        }

        [HttpGet]
        [Route("Join/{gameId}")]
        public IActionResult Join(string gameId)
        {
            IPlayer player = new Player();
            GameCollecton.players.Add(player);
            Game game = GameCollecton.Games.First(e => e.Id == gameId);
            player.JoinGame(gameId, Figure.ColorFigure.White);
            CheckLost(player?.Id);

            if (player?.Front?.Color == Figure.ColorFigure.Black)
            {
                return RedirectToAction("Game", "Home", new { mirrorX = true, userId = player.Id });
            }
            return RedirectToAction("Game", "Home", new { userId = player?.Id });
        }

        [HttpGet]
        [Route("GetPossibleMove/{userId}/{currentCell}")]
        public List<string>? GetPossibleMove(string userId, string currentCell)
        {
            Player? player = GameCollecton.players.FirstOrDefault(e => e.Id == userId) as Player;
            return player?.GetMoves(currentCell);
        }

        [HttpGet]
        [Route("GetUserName/{gameId}/{userId}")]
        public List<string>? GetUserName(string gameId, string userId)
        {
            List<IPlayer>? players = GameCollecton.players.FindAll(e => e.Game?.Id == gameId);
            IPlayer player = players.First(e => e.Id == userId);
            return players.FindAll(e => e != player).Select(e => e.Name).ToList();
        }

        [HttpGet]
        [Route("GetLogs/{gameId}")]
        public Stack<IGameLog>? GetLogs(string gameId)
        {
            Stack<IGameLog>? logs = GameCollecton.Games.FirstOrDefault(e => e.Id == gameId)?.GameLogs;
            return logs;
        }

        [HttpGet]
        [Route("SwitchPawn/{gameId}/{newCell}/{figureName}")]
        public void SwitchPawn(string gameId, string newCell, string figureName)
        {
            Game? game = GameCollecton.Games.FirstOrDefault(e => e.Id == gameId);
            string? result = game?.Control?.Transform(newCell, figureName);

            if (result != String.Empty)
            {
                _hub.Clients.All.SendAsync("Transform", gameId, result, figureName);
            }
        }

        [HttpGet]
        [Route("Move/{userId}/{oldCell}/{newCell}")]
        public void Move(string? userId, string oldCell, string newCell)
        {
            IPlayer? player = GameCollecton.players.FirstOrDefault(e => e.Id == userId);
            IPlayer? player2 = GameCollecton.players.FirstOrDefault(e => e.Game == player?.Game && e != player);

            if (player2 == null)
            {
                _hub.Clients.All.SendAsync("Move", player?.Game?.Id, null);
                return;
            }

            string figureId = String.Empty;
            List<string>? moves = player?.Move(oldCell, newCell, out figureId);
            int? logCount = player?.Game?.GameLogs?.Count;

            if (logCount != 1 && logCount != 2 && logCount % 2 == 0)
            {
                player?.Front?.Timer?.Stop();
                player2?.Front?.Timer?.Resume();
            }
            else if (logCount != 1 && logCount != 2 && logCount % 2 != 0)
            {
                player?.Front?.Timer?.Resume();
                player2?.Front?.Timer?.Stop();
            }

            if (logCount == 1)
            {
                _hub.Clients.All.SendAsync("Start", player?.Game?.Id);
                player?.Front?.Timer?.Stop();
                player?.Front?.Timer?.Start();

            }

            if (logCount == 2)
            {
                player2?.Front?.Timer?.Start();
                player?.Front?.Timer?.Stop();
            }

                _hub.Clients.All.SendAsync("Move", player?.Game?.Id, moves);

            if (figureId != String.Empty)
            {
                _hub.Clients.All.SendAsync("Remove", player?.Game?.Id, figureId);
            }

            bool? result = player?.Game?.Control?.CheckStalemate();

            if (result == null)
            {
                LeaveGame(player?.Game?.Id, userId);
            }
            else
            {
                if (result.Value)
                {
                    LeaveGame(player?.Game?.Id, null);
                }
            }
        }

        [HttpGet]
        [Route("GetNamesAndTimes/{gameId}")]
        public void GetNamesAndTimes(string gameId)
        {
            List<IPlayer> players = GameCollecton.players.FindAll(e => e.Game?.Id == gameId);
            _hub.Clients.All.SendAsync("NamesAndTimes", gameId, players.Select(e => e.Name).ToList(), players.Select(e => e?.Front?.Timer?.Time), players[0].Front?.Timer?.AddTime);
        }

        [HttpGet]
        [Route("LeaveGame/{gameId}/{userId}")]
        public void LeaveGame(string? gameId, string? userId)
        {
            Game? game = GameCollecton.Games.FirstOrDefault(e => e.Id == gameId);

            if (userId == null)
            {
                _hub.Clients.All.SendAsync("Gameover", gameId, null);
            }

            List<IPlayer> players = GameCollecton.players.FindAll(e => e.Game == game);
            string? color = String.Empty;

            players.ForEach(e =>
            {
                Player player = (Player)e;

                if (e.Id == userId)
                {
                    color = player?.Front?.Color.ToString();
                }

                player?.LeaveGame();
            });

            _hub.Clients.All.SendAsync("Gameover", gameId, color);
        }

        private async void CheckLost(string? userId)
        {
            IPlayer? player = GameCollecton.players.FirstOrDefault(e => e.Id == userId);
            IPlayer? player2 = GameCollecton.players.FirstOrDefault(e => e.Game == player?.Game && e != player);

            await Task.Run(async () =>
            {
                bool? time = player?.Front?.Timer?.Lost;
                bool? time2 = player2?.Front?.Timer?.Lost;
                if (time.HasValue && time2.HasValue)
                {
                    while (!time.Value || !time2.Value)
                    {
                        await Task.Delay(1000);
                    }
                    LeaveGame(player?.Game?.Id, player2?.Id);
                }
            });
        }
    }
}
