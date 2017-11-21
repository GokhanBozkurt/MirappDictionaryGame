using SQLite;

namespace MirappDictionaryGame
{
    public class GameSetting
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public bool SoundActive { get; set; }


    }
}