using System.Collections.Generic;
using System.Linq;
using SQLite;

namespace MirappDictionaryGame
{
    public class RepositoryGameScore<T> : Repository<T>
    {
        public List<GameScore> GetRecords()
        {
            try
            {
                var db = new SQLiteConnection(Path);
                // this counts all records in the database, it can be slow depending on the size of the database
                var records = db.Table<GameScore>();

                return records.ToList<GameScore>();
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
        

        public GameScore GetRecord(GameScore gameScore)
        {
            try
            {
                var db = new SQLiteConnection(Path);
                var records = db.Table<GameScore>().Where(a => a.Id == gameScore.Id);
                return records.First();

            }
            catch (SQLiteException)
            {
                return null;
            }
        }

        public long MaxScore
        {
            get
            {
                var records = GetRecords();
                if (records.Count == 0)
                {
                    return 0;
                }
                return records.OrderByDescending(a => a.ScoreDate).OrderByDescending(s => s.Score).First().Score;
            }
        }

    }
}