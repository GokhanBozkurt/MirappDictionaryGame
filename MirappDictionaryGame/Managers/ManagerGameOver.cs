using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MirappDictionaryGame
{
    public class ManagerGameOver
    {
        private static ManagerGameOver _instance;
        private static readonly object LockingObject = new object();
        private readonly List<DictonaryWordContainer> _dictonaryWordContainers = new List<DictonaryWordContainer>();
        public Stopwatch ElapsedStropWatch = new Stopwatch();
        public GameProperty GameProperty;

        public static ManagerGameOver Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (LockingObject)
                    {
                        _instance = new ManagerGameOver();
                    }
                }
                return _instance;
            }
        }

        public int TryCount { get; set; }
        public int SuccessCount { get; set; }

        public decimal SuccessPercentage
        {
            get
            {
                try
                {
                    if (TryCount == 0)
                    {
                        return 0;
                    }
                    return Math.Round(100 * Convert.ToDecimal(SuccessCount) / Convert.ToDecimal(TryCount), 0);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public long MillisInFuture { get; set; }
        public GamePlayLevels GamePlayLevel { get; set; }

        public void ClearWordContainer()
        {
            _dictonaryWordContainers.Clear();
        }
        public void AddWordContainer(DictonaryWordContainer wordContainer)
        {
           //if (!_dictonaryWordContainers.Contains(wordContainer))
            {
                _dictonaryWordContainers.Add(wordContainer);
            }
        }

        internal void InsertScoreToDb(long piScore)
        {
            if (piScore<=0)
            {
                return;
            }
            GameScore score = new GameScore { Score = piScore > 0 ? piScore : 0, ScoreDate = DateTime.Now };
            ManagerRepository.Instance.GameScore.Insert(score);
        }

        public long MaxScore
        {
            get
            {
                var records = ManagerRepository.Instance.GameScore.GetRecords();
                if (records.Count == 0)
                {
                    return 0;
                }
                return records.OrderByDescending(a => a.ScoreDate).OrderByDescending(s=>s.Score).First().Score;
            }
        }

        public void Reset()
        {
            TryCount = 0;
            SuccessCount = 0;
            ElapsedStropWatch.Reset();
        }

        //public override string ToString()
        //{
        //    var sucessWord = _dictonaryWordContainers.Count;
        //    var sucessWordPoint = CalculateSucessWordPoint(sucessWord, 10);
        //    var totalPassedMillis = _dictonaryWordContainers.Sum(a => a.PassedMillis);
        //    var totalMills = sucessWord * GameProperty.MillisInFuture;
        //    var MillsPoint = totalMills - totalPassedMillis;
        //    var LevelPoint = GetGameLevelPoint(GamePlayLevel);
        //    long TotalMillsPoint = MillsPoint / LevelPoint;
        //    var TryCountPoint = TryCount * 1000;
        //    var totalScore = ((TotalMillsPoint * sucessWordPoint) - TryCountPoint) / 100;

        //    return $"sucessWord:{sucessWord} sucessWordPoint:{sucessWordPoint} totalPassedMillis:{totalPassedMillis} totalMills:{totalMills} MillsPoint:{MillsPoint} LevelPoint:{LevelPoint}  TotalMillsPoint:{TotalMillsPoint} TryCountPoint:{TryCountPoint}" ;


        //}
        public override string ToString()
        {
            var sucessWord = _dictonaryWordContainers.Count;
            var sucessWordPoint = CalculateSucessWordPoint(sucessWord, 10);
            var totalPassedMillis = _dictonaryWordContainers.Sum(a => a.PassedMillis);
            //var totalMills = sucessWord * GameProperty.MillisInFuture;
            // var MillsPoint = totalMills - totalPassedMillis;
            var LevelPoint = GetGameLevelPoint(GamePlayLevel);
            // long TotalMillsPoint = MillsPoint / LevelPoint;
            // var TryCountPoint = TryCount * 1000;
            return $"sucessWord:{sucessWord} tryCount :{TryCount} sucessWordPoint:{sucessWordPoint} LevelPoint:{LevelPoint} totalPassedMillis:{totalPassedMillis}";


        }
        public long CalculateTotalScore
        {
            get
            {
                var sucessWord = _dictonaryWordContainers.Count;
                var sucessWordPoint = CalculateSucessWordPoint(sucessWord, 10);
                var totalPassedMillis = _dictonaryWordContainers.Sum(a => a.PassedMillis) / 1000;
                //var totalMills = sucessWord * GameProperty.MillisInFuture;
                // var MillsPoint = totalMills - totalPassedMillis;
                var LevelPoint = GetGameLevelPoint(GamePlayLevel);
                // long TotalMillsPoint = MillsPoint / LevelPoint;
                // var TryCountPoint = TryCount * 1000;
                var totalScore = sucessWord * sucessWordPoint * LevelPoint - totalPassedMillis - (TryCount * 2);
                if (Convert.ToInt32(totalScore) < 0)
                {
                    totalScore = 0;
                }
                return totalScore;
            }
        }

        //public long CalculateTotalScore
        //{
        //    get
        //    {
        //        //Kelime sayýsý (From)
        //        var sucessWord = _dictonaryWordContainers.Count;
        //        var sucessWordPoint = CalculateSucessWordPoint(sucessWord, 10);
        //        var totalPassedMillis = _dictonaryWordContainers.Sum(a => a.PassedMillis);
        //        var totalMills = sucessWord * GameProperty.MillisInFuture;
        //        var MillsPoint = totalMills - totalPassedMillis;
        //        var LevelPoint = GetGameLevelPoint(GamePlayLevel);
        //        long TotalMillsPoint = MillsPoint / LevelPoint;
        //        var TryCountPoint = TryCount * 1000;
        //        var totalScore = ((TotalMillsPoint * sucessWordPoint) - TryCountPoint) / 100;
        //        if (Convert.ToInt32(totalScore) < 0)
        //        {
        //            totalScore = 0;
        //        }
        //        return totalScore;
        //    }
        //}




        private int GetGameLevelPoint(GamePlayLevels gameLevel)
        {
            switch (gameLevel)
            {
                case GamePlayLevels.Easy:
                    return 5;
                case GamePlayLevels.Medium:
                    return 7;
                case GamePlayLevels.Hard:
                    return 10;
                default:
                    return 5;
            }
        }

        private int CalculateSucessWordPoint(int count, int factor)
        {
            var d = count - factor;
            if (d > 0)
            {
                return CalculateSucessWordPoint(count, factor + 10);
            }
            else
            {
                return factor;
            }
        }
    }
}