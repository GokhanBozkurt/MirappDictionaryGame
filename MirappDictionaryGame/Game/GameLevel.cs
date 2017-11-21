using SQLite;

namespace MirappDictionaryGame
{
    public class GameLevel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int LevelNumber { get; internal set; }
        public int GameRecordCount
        {
            get
            {
                return LevelNumber * ManagerGamePlay.LevelFactor;
            }
        }


        public override string ToString()
        {
           return $" Level " + LevelNumber.ToString();
        }
    }
}