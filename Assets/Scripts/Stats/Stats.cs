using System;
using MoneySystem.Controlls;
using UnityEngine;


namespace Data.Stats
{
    public class Stats : MonoBehaviour, IStats
    {
        private const int ZERO = 0;
        private const int MAX_SCORE_LEVEL = 10;
        
        [SerializeField] private float score;
        public float Score => score;
        [SerializeField] private float level;

        public float Level => level;
        [SerializeField] private MoneyController moneyController;

        public Action<float> UpdateScore;
        public Action<float> UpdateLevel;

        public void ScoreUpdate(float checkPointScore)
        {
            score += checkPointScore;
            moneyController.AddMoney((int)score);
            UpdateScore?.Invoke(score);
            CheckScore();
        }

        public void CheckScore()
        {
            if (score > MAX_SCORE_LEVEL)
            {
                LevelUpdate();
            }
        }

        private void LevelUpdate()
        {
            level++;
            UpdateLevel?.Invoke(level);
        }
    }
}