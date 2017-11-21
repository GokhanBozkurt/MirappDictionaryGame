using SQLite;

namespace MirappDictionaryGame
{
    public class MyDictonaryWord
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Word { get; set; }

        public string TranslatedWord { get; set; }

        public string Language { get; set; }

        public bool MyWord { get; set; }
        public string SnonymWord { get; set; }

        public int OrderId;
        public override string ToString()
        {
            return string.Format("{0}=>{1} ", Word, TranslatedWord);
        }
    }

}