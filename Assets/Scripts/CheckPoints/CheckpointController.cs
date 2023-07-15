using System.Collections.Generic;
using System.Linq;
using CheckPoints.LookAtTarget.Controller;
using UnityEngine;
using UnityEngine.Serialization;

namespace CheckPoints.Controller
{
    public class CheckpointController : MonoBehaviour
    {
        [SerializeField] private List<Checkpoint> checkpointsList;
        [SerializeField] private LookAtTargetController arrow;

        private Checkpoint _currentCheckpoint;
        private int _checkpointId;

        private void Start()
        {
            if (checkpointsList.Count == 0) return;

            for (int i = 0; i < checkpointsList.Count; i++)
                checkpointsList[i].gameObject.SetActive(false);

            _checkpointId = 0;
            SetCurrentCheckpoint(checkpointsList[_checkpointId]);
        }

        public bool CheckPointsIsZero()
        {
            if (checkpointsList.Count == 0)
            {
                return true;
            }

            return false;
        }

        private void SetCurrentCheckpoint(Checkpoint checkpoint)
        {
            if (_currentCheckpoint != null)
            {
                _currentCheckpoint.gameObject.SetActive(false);
                _currentCheckpoint.CheckpointActivated -= CheckpointActivated;
            }

            _currentCheckpoint = checkpoint;
            _currentCheckpoint.CheckpointActivated += CheckpointActivated;
            arrow.Target = _currentCheckpoint.transform;
            _currentCheckpoint.gameObject.SetActive(true);
        }

        private void CheckpointActivated()
        {
            checkpointsList.Remove(_currentCheckpoint);

            if (_checkpointId >= checkpointsList.Count)
            {
                _currentCheckpoint.gameObject.SetActive(false);
                _currentCheckpoint.CheckpointActivated -= CheckpointActivated;
                arrow.gameObject.SetActive(false);
                return;
            }

            SetCurrentCheckpoint(checkpointsList[_checkpointId]);
        }


    }
}