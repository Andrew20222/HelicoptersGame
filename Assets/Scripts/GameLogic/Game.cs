using AirPlaneSystems;
using CheckPoints.Controller;
using Inputs;
using Mechanics.Movement;
using UnityEngine;
using Views.Task;

namespace GameLogics
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private EndGameView endGameView;
        [SerializeField] private CheckpointController checkpointController;
        [SerializeField] private Task task;
        [SerializeField] private GameObject taskPanel;
        [SerializeField] private InputProvider inputProvider;
        private IMovable _player;

        private void Start()
        {
            _player = FindObjectOfType<AirPlaneController>();
            inputProvider.Initialize(_player);
            task.ShowTask();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                taskPanel.SetActive(false);
            }
            
            if (_player.PlaneIsDead())
            {
                endGameView.ShowLosePanel();
            }
            else if (checkpointController.CheckPointsIsZero())
            {
                endGameView.ShowWinPanel();
            }
        }
    }
}