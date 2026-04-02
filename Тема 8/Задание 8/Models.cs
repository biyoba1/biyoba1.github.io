using System;

namespace Задание_8
{
    [Serializable]
    public class GameResult
    {
        public string Login { get; set; }
        public DateTime Date { get; set; }
        public bool IsWin { get; set; }
        public int TimeSeconds { get; set; }
        public int Difficulty { get; set; }

        public GameResult(string login, bool isWin, int time, int difficulty)
        {
            Login = login;
            Date = DateTime.Now;
            IsWin = isWin;
            TimeSeconds = time;
            Difficulty = difficulty;
        }
    }

    public static class CurrentUser
    {
        public static string Login { get; set; }
    }
}