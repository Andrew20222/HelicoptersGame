using AirPlaneSystems;
using Data.Stats;
using GameLogics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameUI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private AirPlaneController planeController;
        [SerializeField] private EndGameView endGameView;
        [SerializeField] private Stats stats;
        [SerializeField] private TMP_Text speedText;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button winButton;
       


        private void OnEnable()
        {
            if (stats != null)
            {
                stats.UpdateLevel += ShowLevel;
                endGameView.Win += ShowLevel;
            }
            
            restartButton.onClick.AddListener(RestartLevel);
            winButton.onClick.AddListener(WinLevel);
        }

        private void Update()
        {
            ShowSpeed();
        }
        private void RestartLevel()
        {
            SceneManager.LoadScene(2);
        }

        private void WinLevel()
        {
            SceneManager.LoadScene(1);
        }
        
        private void OnDisable()
        {
            if (stats != null)
            {
                stats.UpdateLevel -= ShowLevel;
                endGameView.Win -= ShowLevel;
            }
            
            restartButton.onClick.RemoveListener(RestartLevel);
            winButton.onClick.RemoveListener(WinLevel);
        }

        private void ShowLevel(float level)
        {
            levelText.text = $"Level: {level}";
        }
        private void ShowLevel()
        {
            levelText.text = $"Level: {stats.StatsData.Level}";
        }

        private void ShowSpeed()
        {
            speedText.text = planeController.CurrentSpeed().ToString("0") + "Km/h";
        }

    }
}