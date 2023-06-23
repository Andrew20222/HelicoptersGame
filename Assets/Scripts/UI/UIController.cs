using HeheGames.Simple_Airplane_Controller;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private Stats _stats;

    [SerializeField] private TMP_Text scoreText;

    [SerializeField] private TMP_Text levelText;

    [SerializeField] private AirPlaneController planeController;

    [SerializeField] private TMP_Text speedText;

    [SerializeField] private GameObject playerPanel;

    [SerializeField] private GameObject settingPanel;
    
    private void OnEnable()
    {
        if (_stats != null)
        {
            _stats.UpdateScore += ShowScore;
            _stats.UpdateLevel += ShowLevel;
        }
    }

    private void Update()
    {
        ShowSpeed();
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
        if (_stats != null)
        {
            _stats.UpdateScore -= ShowScore;
            _stats.UpdateLevel -= ShowLevel;
        }
    }

    private void ShowScore(float score)
    {
        scoreText.text = $"Score: {score}";
    }

    private void ShowLevel(float level)
    {
        levelText.text = $"Level: {level}";
    }

    private void ShowSpeed()
    {
        speedText.text = $"Speed: {planeController.CurrentSpeed()}";
    }

}