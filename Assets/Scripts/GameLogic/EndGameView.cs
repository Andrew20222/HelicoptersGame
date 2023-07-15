using System;
using UnityEngine;

namespace GameLogics
{
    public class EndGameView : MonoBehaviour
    {
        [SerializeField] private GameObject winPanel;
        [SerializeField] private GameObject losePanel;

        public Action Win;
        public Action Lose;


        public void ShowWinPanel()
        {
            winPanel.SetActive(true);
            Win?.Invoke();
        }

        public void ShowLosePanel()
        {
            losePanel.SetActive(true);
            Lose?.Invoke();
        }
    }
}