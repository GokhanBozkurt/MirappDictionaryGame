using System.Collections.Generic;
using SQLite;
using Android.OS;
using Java.IO;

namespace MirappDictionaryGame
{
    public class Repository<T>
    {
        private SQLiteConnection _connection;
        public SQLiteException RepositoryException { get; private set; }
           
        public string Path
        {
            get
            {
                var folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
                var toDatabase = System.IO.Path.Combine(folderPath, "MirappDictonaryGame.db");
                return toDatabase;
            }
        }


        public void Open()
        {
            _connection = new SQLiteConnection(Path);
        }

        public void Close()
        {
            _connection.Close();
        }

        public bool DeleteTable()
        {
            try
            {
                _connection.DeleteAll<T>();
                return true;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (SQLiteException ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return false;
            }
        }

        public bool DropTable()
        {
            try
            {
                _connection.DropTable<T>();
                return true;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (SQLiteException ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return false;
            }
        }

        public string CreateTable()
        {
            try
            {
                _connection.CreateTable<T>();
                return "Database created";
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }

        public bool Insert(T data)
        {
            try
            {
                var db = new SQLiteConnection(Path);
                if (db.Insert(data) != 0)
                    db.Update(data);
                return true;
            }
            catch (SQLiteException ex)
            {
                RepositoryException = ex;
                return false;
            }
        }


        public bool Update(T data)
        {
            try
            {
                var db = new SQLiteConnection(Path);
                db.Update(data);
                return true;
            }
            catch (SQLiteException ex)
            {
                RepositoryException = ex;
                return false;
            }
        }

        public bool Delete(T data)
        {
            try
            {
                var db = new SQLiteConnection(Path);
                if (db.Delete(data) > 0)
                {
                    return true;
                }
                return false;
            }
            catch (SQLiteException ex)
            {
                RepositoryException = ex;
                return false;
            }
        }

        public bool DeleteAll()
        {
            try
            {
                var db = new SQLiteConnection(Path);
                if (db.DeleteAll<T>() > 0)
                {
                    return true;
                }
                return false;
            }
            catch (SQLiteException ex)
            {
                RepositoryException = ex;
                return false;
            }
        }


        private string InsertUpdateAllData(IEnumerable<T> data)
        {
            try
            {
                var db = new SQLiteConnection(Path);
                if (db.InsertAll(data) != 0)
                    db.UpdateAll(data);
                return "List of data inserted or updated";
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }

    }
}