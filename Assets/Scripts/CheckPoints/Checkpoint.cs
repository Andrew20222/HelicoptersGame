using System;
using Data.Stats;
using UnityEngine;

namespace CheckPoints
{
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] private float checkPointScore;

        public Action CheckpointActivated;
        public Stats score;

        private void OnTriggerEnter(Collider other)
        {
            if (CheckpointActivated != null) CheckpointActivated();
            score.ScoreUpdate(checkPointScore);
            score.CheckScore();
        }
    }
}