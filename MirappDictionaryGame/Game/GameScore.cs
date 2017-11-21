using SQLite;
using System;

namespace MirappDictionaryGame
{
    public class GameScore
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public long Score { get;  set; }

        public DateTime ScoreDate { get; set; }
    }
}