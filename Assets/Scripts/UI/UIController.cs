using HeheGames.Simple_Airplane_Controller;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Stats stats;
    [SerializeField] private Image imageScore;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private AirPlaneController planeController;

    [SerializeField] private TMP_Text speedText;

    [SerializeField] private GameObject playerPanel;

    [SerializeField] private GameObject settingPanel;
    
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
        imageScore.fillAmount = score;
    }

    private void ShowLevel(float level)
    {
        levelText.text = $"Level: {level}";
    }

    private void ShowSpeed()
    {
        speedText.text = planeController.CurrentSpeed().ToString();
    }

}