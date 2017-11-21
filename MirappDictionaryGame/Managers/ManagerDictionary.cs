using Android.App;
using Android.Content.Res;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Android.Util;
using Java.Lang;
using System;
using Android.Widget;

namespace MirappDictionaryGame
{
    public static class ManagerDictionary
    {
        private static List<MyDictonaryWord> _wordList = new List<MyDictonaryWord>();
        private static RepositoryMyDictonaryWord<MyDictonaryWord> _repository = new RepositoryMyDictonaryWord<MyDictonaryWord>();
        private static bool _dictionaryUpdated = false;

        public static void DictonaryUpdated()
        {
            _dictionaryUpdated = true;
        }

        public static List<MyDictonaryWord> WordList
        {
            get
            {
                if (_wordList == null || _wordList.Count == 0 || _dictionaryUpdated == true)
                {
                    _repository.Open();
                    _repository.CreateTable();
                    _wordList = _repository.GetRecords();
                    _dictionaryUpdated = false;
                }

                return _wordList;
            }
        }

        public static List<MyDictonaryWord> GetRandomWordList(List<MyDictonaryWord> lst, int RecordCount)
        {
            List<MyDictonaryWord> result = new List<MyDictonaryWord>();
            Random _random = new Random();
            List<int> randList = new List<int>();
            for (int i = 0; i < RecordCount; i++)
            {
                result.Add(lst[_random.Next(lst.Count)]);
                //var rnd = GetRandomValue(RecordCount, randList);
                //randList.Add(rnd);
                //result.Add(lst[rnd]);
            }
            return result;
        }

        public static int GetRandomValue(int max,List<int> randList)
        {
            Random _random = new Random();

            var rnd =_random.Next(max);
            if (randList.Contains(rnd))
            {
                rnd = GetRandomValue(max, randList);
            }

            return rnd;
        }

        public static List<MyDictonaryWord> GetGameWordList(MyDictonaryWord words)
        {
            var randomWordList = GetRandomWordList(WordList, ManagerGamePlay.GetCurrentLevel().GameRecordCount);
            return GenerateWordList(words, randomWordList);
        }
        public static List<MyDictonaryWord> GetGameWordList(MyDictonaryWord words, bool playWithFavorites)
        {
            if (playWithFavorites)
            {
                var lstword = ManagerRepository.Instance.FavoriteWord.ToDictonaryWordList();
                var randomWordList = GetRandomWordList(lstword, lstword.Count);
                return GenerateWordList(words, randomWordList);
            }
            else
            {
                var randomWordList = GetRandomWordList(WordList, ManagerGamePlay.GetCurrentLevel().GameRecordCount);
                return GenerateWordList(words, randomWordList);
            }
        }

        public static List<MyDictonaryWord> GetNewLevelGameWordList(MyDictonaryWord words)
        {
            var randomWordList = GetRandomWordList(WordList, ManagerGamePlay.LevelFactor);
            return GenerateWordList(words, randomWordList);
        }

        private static List<MyDictonaryWord> GenerateWordList(MyDictonaryWord words, List<MyDictonaryWord> randomWordList)
        {
            List<MyDictonaryWord> lst = new List<MyDictonaryWord>();
            int i = 0;
            foreach (MyDictonaryWord item in randomWordList)
            {
                i++;
                if (item.Language != words.Language)
                {
                    lst.Add(
                            new MyDictonaryWord()
                            {
                                Id = item.Id,
                                Word = item.TranslatedWord.ToUpper(new System.Globalization.CultureInfo("tr-TR", false)),
                                TranslatedWord = item.Word.ToUpper(new System.Globalization.CultureInfo("en-US", false)),
                                Language = words.Language,
                                SnonymWord =item.SnonymWord,
                                OrderId=i
                            }
                        );
                }
                else
                {
                    lst.Add(new MyDictonaryWord()
                    {
                        Id = item.Id,
                        Word = item.Word.ToUpper(new System.Globalization.CultureInfo("en-US", false)).Replace("Ý","I"),
                        TranslatedWord = item.TranslatedWord.ToUpper(new System.Globalization.CultureInfo("tr-TR", false)),
                        Language = item.Language,
                        SnonymWord = item.SnonymWord,
                        OrderId = i
                    });
                }
            }

            return lst;
        }

        public static List<MyDictonaryWord> PrepareWordList(MyDictonaryWord words)
        {
            return GenerateWordList(words, WordList);
            //foreach (MyDictonaryWord item in WordList)
            //{
            //    if (item.Language != words.Language)
            //    {
            //        lst.Add(
            //                new MyDictonaryWord()
            //                {
            //                    Id = item.Id, Word = item.TranslatedWord, TranslatedWord = item.Word, Language = words.Language
            //                }
            //            );
            //    }
            //    else
            //    {
            //        lst.Add(new MyDictonaryWord()
            //        {
            //            Id = item.Id,
            //            Word = item.Word,
            //            TranslatedWord = item.TranslatedWord,
            //            Language = item.Language
            //        });
            //    }
            //}
        }

        public static List<MyDictonaryWord> PrepareWordList(MyDictonaryWord words, bool favorites, string searchText)
        {
            if (favorites)
            {
                var lstword = ManagerRepository.Instance.FavoriteWord.ToDictonaryWordList();
                var filteredList2 = lstword.Where(a => a.Word.ToLower().StartsWith(searchText.ToLower()));
                return GenerateWordList(words, filteredList2.ToList());
            }
            var filteredList = WordList.Where(a => a.Word.StartsWith(searchText.ToUpper()));
            return GenerateWordList(words, filteredList.ToList());

        }
        public static List<MyDictonaryWord> PrepareWordList(MyDictonaryWord words, string searchText, bool favorites)
        {
            if (favorites)
            {
                var lstword = ManagerRepository.Instance.FavoriteWord.ToDictonaryWordList();
                var filteredList2 = lstword.Where(a => a.Word.ToLower().StartsWith(searchText.ToLower()));
                return GenerateWordList(words, filteredList2.ToList());
            }
            var filteredList = WordList.Where(a => a.Word.StartsWith(searchText.ToUpper()));
            return GenerateWordList(words, filteredList.ToList());
        }

        public static List<MyDictonaryWord> PrepareWordList(MyDictonaryWord words, string searchText, bool favorites,string OrderListBy)
        {
            if (favorites)
            {
                var lstword = ManagerRepository.Instance.FavoriteWord.ToDictonaryWordList();
                var filteredList2 = lstword.Where(a => a.Word.ToLower().StartsWith(searchText.ToLower()));
                return GenerateWordList(words, filteredList2.ToList());
            }
            if (OrderListBy=="DateTime")
            {

            }
            else
            {

            }
            var filteredList = WordList.Where(a => a.Word.StartsWith(searchText.ToUpper()));
            return GenerateWordList(words, filteredList.ToList());
        }
        public static void LoadDictionary(Activity activity, bool delete)
        {
            try
            {
                _repository = new RepositoryMyDictonaryWord<MyDictonaryWord>();
                _repository.Open();
                if (delete)
                {
                    List<MyDictonaryWord> myWordList = new List<MyDictonaryWord>();
                    foreach (var item in WordList.Where(a => a.MyWord == true).Select(a => a))
                    {
                        myWordList.Add(item);
                    }
                    _repository.DropTable();
                    _repository.CreateTable();
                    _repository.DeleteAll();
                    var dictionaryFilesLines = ReadDictionaryFiles(activity);
                    LoadFromCvv(activity, dictionaryFilesLines);

                    foreach (var item in myWordList)
                    {
                        _repository.Insert(item);
                    }
                }
                else
                {
                    var count = WordList.Count(word => word.MyWord == false);
                    Toast.MakeText(activity, $"Count:{count}", ToastLength.Long).Show();
                    if (count> 0)
                    {
                        return;
                    }
                    var dictionaryFilesLines = ReadDictionaryFiles(activity);
                    if (count == 0 || dictionaryFilesLines.Count != count)
                    {
                        LoadFromCvv(activity, dictionaryFilesLines);
                    }
                }
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
            finally
            {
                _repository.Close();
            }



        }

        private static List<MyDictonaryWord> ReadDictionaryFiles(Activity actvity)
        {
            AssetManager assetManager = actvity.Assets;
            List<string> wordFiles = GetWordFiles(assetManager);
            List<MyDictonaryWord> dictonaryWords = new List<MyDictonaryWord>();

            foreach (var item in wordFiles)
            {
                var sFileContents = "";
                using (StreamReader sr = new StreamReader(assetManager.Open(item), System.Text.Encoding.ASCII))
                {
                    sFileContents = sr.ReadToEnd();
                }

                foreach (var line in sFileContents.Split(System.Environment.NewLine.ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries))
                {
                    try
                    {
                        var content = line.Split(";".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
                        if (content.Length > 1 && content[0] != string.Empty && content[1] != string.Empty)
                        {
                            dictonaryWords.Add(new MyDictonaryWord
                            {
                                Word = content[0].ToUpper().Trim(),
                                TranslatedWord = content[1].ToUpper().Trim(),
                                Language = "Türkçe",
                                MyWord = false
                            }
                                                );
                        }

                    }
                    catch (System.Exception exc)
                    {
                        Log.Debug("dd", exc.Message);
                    }
                }
            }

            return dictonaryWords;
        }

        private static List<string> GetWordFiles(AssetManager assetManager)
        {
            
            var sFileContents = "";
            using (StreamReader sr = new StreamReader(assetManager.Open("WordFiles.txt"), System.Text.Encoding.ASCII))
            {
                sFileContents = sr.ReadToEnd();
            }
            return new List<string>(sFileContents.Split(';'));
        }

        private static void LoadFromCvv(Activity actvity, List<MyDictonaryWord> dictionaryFilesLines)
        {
            var records = _repository.GetRecords();

            foreach (var item in dictionaryFilesLines)
            {
                if (!records.Contains(item))
                {
                    _repository.Insert(item);
                    records.Add(item);
                }
            }
            DictonaryUpdated();
        }

        public static MyDictonaryWord GetWord(string translatedWord)
        {
            var count  = WordList.Where(a => a.TranslatedWord == translatedWord).Count();
            if (count>0)
            {
                return WordList.Where(a => a.TranslatedWord == translatedWord).Select(a => a).First();

            }

            count = WordList.Where(a => a.Word== translatedWord).Count();
            if (count > 0)
            {
                return WordList.Where(a => a.Word == translatedWord).Select(a => a).First();

            }

            return null;
        }

        public static void Delete()
        {
            _repository = new RepositoryMyDictonaryWord<MyDictonaryWord>();
            _repository.Open();
            _repository.DeleteAll();
        }

    }
}