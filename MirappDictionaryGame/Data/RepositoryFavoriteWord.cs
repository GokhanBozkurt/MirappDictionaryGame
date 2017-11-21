using System.Collections.Generic;
using System.Linq;
using SQLite;
using Java.IO;
using Java.Nio.Channels;
using Android.OS;

namespace MirappDictionaryGame
{
    public class RepositoryFavoriteWord<T> : Repository<T>
    {
        public List<FavoriteWord> GetRecords()
        {
            try
            {
                var db = new SQLiteConnection(Path);
                // this counts all records in the database, it can be slow depending on the size of the database
                var records = db.Table<FavoriteWord>();

                return records.ToList();
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

        public FavoriteWord GetRecord(FavoriteWord favoriteWord)
        {
            try
            {
                var db = new SQLiteConnection(Path);
                var records = db.Table<FavoriteWord>().Where(a => a.Id == favoriteWord.Id);
                return records.First();

            }
            catch (SQLiteException)
            {
                return null;
            }
        }

        public bool MyInsert(FavoriteWord data)
        {
            try
            {
                var db = new SQLiteConnection(Path);
                if (db.Insert(data) != 0)
                    db.Update(data);
                return true;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (SQLiteException ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return false;
            }
        }

        public bool Add(FavoriteWord favoriteWord)
        {
            try
            {
                var db = new SQLiteConnection(Path);
                var count = db.Table<FavoriteWord>().Where(a => a.TranslatedWord == favoriteWord.TranslatedWord && a.Word==favoriteWord.Word ).Count();

                if (count>0)
                {
                    return true;
                }
                return MyInsert(favoriteWord);
            }
            catch (SQLiteException)
            {
                return false;
            }
        }

        internal List<MyDictonaryWord> ToDictonaryWordList()
        {
            var db = new SQLiteConnection(Path);
            var list = new List<MyDictonaryWord>();
            // this counts all records in the database, it can be slow depending on the size of the database
            var records = db.Table<FavoriteWord>();
            foreach (var item in records)
            {
                list.Add(new MyDictonaryWord
                {
                    Id=item.Id,
                    Language=item.Language,
                    MyWord=false,
                    Word=item.Word,
                    TranslatedWord=item.TranslatedWord,
                    SnonymWord =item.SnonymWord
                });
            }

            return list;
        }


        private static void exportDB()
        {
            try
            {
                File sd = Environment.ExternalStorageDirectory;
                File data = Environment.DataDirectory;

                if (sd.CanWrite())
                {
                    string currentDBPath = "//data//MirappDictionaryGame.MirappDictionaryGame//databases//MirappDictonaryGame";
                    string path = System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path, "Mirapp");
                    Java.IO.File imageFile = new Java.IO.File(path, "MirappFavoriteWords.txt");
                    string backupDBPath = "<destination>";
                    File currentDB = new File(data, currentDBPath);
                    File backupDB = new File(sd, backupDBPath);

                    FileChannel src = new FileInputStream(currentDB).Channel;
                    FileChannel dst = new FileOutputStream(backupDB).Channel;
                    dst.TransferFrom(src, 0, src.Size());
                    src.Close();
                    dst.Close();
                    //Toast.MakeText(getApplicationContext(), "Backup Successful!",Toast.LENGTH_SHORT).show();

                }
            }
            catch
            {

                //    Toast.makeText(getApplicationContext(), "Backup Failed!", Toast.LENGTH_SHORT).show();

            }
        }
    }
}