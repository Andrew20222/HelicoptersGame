using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameUI
{
    public class MenuUI : MonoBehaviour
    {
        [SerializeField] private GameObject playPanel;
        [SerializeField] private GameObject settingPanel;
        [SerializeField] private Button playButton;
        [SerializeField] private Button settingButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private Button undoButton;

        private void OnEnable()
        {
            playButton.onClick.AddListener(Play);
            settingButton.onClick.AddListener(Setting);
            quitButton.onClick.AddListener(Quit);
            undoButton.onClick.AddListener(Undo);
        }

        private void Play()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        private void Setting()
        {
            playPanel.SetActive(false);
            settingPanel.SetActive(true);
        }

        private void Undo()
        {
            playPanel.SetActive(true);
            settingPanel.SetActive(false);
        }

        private void Quit()
        {
            Application.Quit();
        }

        private void OnDisable()
        {
            playButton.onClick.RemoveListener(Play);
            settingButton.onClick.RemoveListener(Setting);
            quitButton.onClick.RemoveListener(Quit);
            undoButton.onClick.RemoveListener(Undo);
        }
    }
}