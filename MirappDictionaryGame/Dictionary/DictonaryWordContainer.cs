namespace MirappDictionaryGame
{
    public class DictonaryWordContainer
    {

        private MyDictonaryWord _currentWord;

        public MyDictonaryWord CurrentWord
        {
            get
            {
                return _currentWord;
            }
            set
            {
                LastWord = _currentWord;
                _currentWord = value;

            }
        }

        public long PassedMillis { get; set; }

        public MyDictonaryWord LastWord;

        public DictonaryWordContainer()
        {
            _currentWord = new MyDictonaryWord();
            LastWord = new MyDictonaryWord();
        }
    }
}