using System.Collections.Generic;
using System.Linq;
using SQLite;

namespace MirappDictionaryGame
{
    public class RepositoryGameSetting<T> : Repository<T>
    {
        public List<GameSetting> GetRecords()
        {
            try
            {
                var db = new SQLiteConnection(Path);
                // this counts all records in the database, it can be slow depending on the size of the database
                var records = db.Table<GameSetting>();

                return records.ToList<GameSetting>();
                // for a non-parameterless query
                // var count = db.ExecuteScalar<int>("SELECT Count(*) FROM DictonaryWords WHERE Word="Amy");

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (SQLiteException ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return null;
            }
        }

        public GameSetting GetRecord(GameSetting gameSetting)
        {
            try
            {
                var db = new SQLiteConnection(Path);
                var records = db.Table<GameSetting>().Where(a => a.Id == gameSetting.Id);
                return records.First();

            }
            catch (SQLiteException)
            {
                return null;
            }
        }
    }
}