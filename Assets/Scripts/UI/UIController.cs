using AirPlaneSystems;
using Data.Stats;
using TMPro;
using UnityEngine;

namespace GameUI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private AirPlaneController planeController;
        [SerializeField] private Stats stats;
        [SerializeField] private TMP_Text speedText;

        private void OnEnable()
        {
            if (stats != null)
            {
                stats.UpdateScore += ShowScore;
                stats.UpdateLevel += ShowLevel;
            }
        }

        private void Update()
        {
            ShowSpeed();
            ShowScore(stats.Score);
        }

        public void Play()
        {
        }

        public void Setting()
        {
        }

        public void Quit()
        {
            Application.Quit();
        }

        private void OnDisable()
        {
            if (stats != null)
            {
                stats.UpdateScore -= ShowScore;
                stats.UpdateLevel -= ShowLevel;
            }
        }

        private void ShowScore(float score)
        {

        }

        private void ShowLevel(float level)
        {

        }

        private void ShowSpeed()
        {
            speedText.text = planeController.CurrentSpeed().ToString();
        }

    }
}