using CheckPoints.LookAtTarget.Controller;
using UnityEngine;

namespace CheckPoints.Controller
{
    public class CheckpointController : MonoBehaviour
    {
        [SerializeField] private Checkpoint[] CheckpointsList;
        [SerializeField] private LookAtTargetController Arrow;

        private Checkpoint CurrentCheckpoint;
        private int CheckpointId;

        void Start()
        {
            if (CheckpointsList.Length == 0) return;

            for (int i = 0; i < CheckpointsList.Length; i++)
                CheckpointsList[i].gameObject.SetActive(false);

            CheckpointId = 0;
            SetCurrentCheckpoint(CheckpointsList[CheckpointId]);
        }

        private void SetCurrentCheckpoint(Checkpoint checkpoint)
        {
            if (CurrentCheckpoint != null)
            {
                CurrentCheckpoint.gameObject.SetActive(false);
                CurrentCheckpoint.CheckpointActivated -= CheckpointActivated;
            }

            CurrentCheckpoint = checkpoint;
            CurrentCheckpoint.CheckpointActivated += CheckpointActivated;
            Arrow.Target = CurrentCheckpoint.transform;
            CurrentCheckpoint.gameObject.SetActive(true);
        }

        private void CheckpointActivated()
        {
            CheckpointId++;
            if (CheckpointId >= CheckpointsList.Length)
            {
                CurrentCheckpoint.gameObject.SetActive(false);
                CurrentCheckpoint.CheckpointActivated -= CheckpointActivated;
                Arrow.gameObject.SetActive(false);
                return;
            }

            SetCurrentCheckpoint(CheckpointsList[CheckpointId]);
        }
    }
}