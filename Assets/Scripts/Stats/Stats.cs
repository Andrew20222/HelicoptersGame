using System;
using MoneySystem.Controlls;
using UnityEngine;

namespace Data.Stats
{
    public class Stats : MonoBehaviour
    {
        private const int MAX_SCORE_LEVEL = 10;
        
        [SerializeField] private MoneyController moneyController;
        public StatsData StatsData => _statsData;
        private StatsData _statsData = new();
        private IStorageService _storageService = new JsonToFileStorageService();

        public Action<float> UpdateScore;
        public Action<float> UpdateLevel;
        public void ScoreUpdate(float checkPointScore)
        {
            _statsData = _storageService.Load<StatsData>("key");
            _statsData.Score += checkPointScore;
            
            CheckScore();
            
            moneyController.AddMoney((int)checkPointScore);
            UpdateScore?.Invoke(_statsData.Score);
            _storageService.Save("Key", _statsData);
        }

        public void CheckScore()
        {
            if (_statsData.Score > MAX_SCORE_LEVEL)
            {
                LevelUpdate();
            }
        }

        private void LevelUpdate()
        {
            _statsData.Level++;
            UpdateLevel?.Invoke(_statsData.Level);
            _storageService.Save("Key", _statsData);
        }
    }
}