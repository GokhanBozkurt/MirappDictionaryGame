using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace MirappDictionaryGame
{
    public class FavoriteWord
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Word { get; set; }

        public string TranslatedWord { get; set; }

        public string Language { get; set; }

        public string SnonymWord { get; set; }
        public override string ToString()
        {
            return string.Format("{0}=>{1} ", Word, TranslatedWord);
        }
    }
}