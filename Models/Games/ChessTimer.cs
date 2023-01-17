namespace GameChess.Models.Games
{
    public class ChessTimer
    {
        private TimeSpan _time;
        private readonly TimeSpan _addTime;
        private bool _stop;

        public string? Time { get => _time.ToString("mm':'ss"); }
        public int? AddTime { get => _addTime.Seconds; }
        public bool Lost { get; set; }


        public ChessTimer(TimeSpan time, TimeSpan addTime)
        {
            _time = time;
            _addTime = addTime;
        }

        public async void Start()
        {
            await Task.Run(async () =>
            {
                while (_time != TimeSpan.Zero)
                {
                    if (!_stop)
                    {
                        _time -= new TimeSpan(0, 0, 1);
                    }

                    await Task.Delay(1000);
                }

                Lost = true;
            });
        }

        public void Resume()
        {
            _stop = false;
        }

        public void Stop()
        {
            _stop = true;
            _time += _addTime;
        }
    }
}
