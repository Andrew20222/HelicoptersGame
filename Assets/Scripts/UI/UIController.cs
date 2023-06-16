using HeheGames.Simple_Airplane_Controller;
using TMPro;
using UnityEngine;
using Zenject;

public class UIController : MonoBehaviour
{
    private IStats _stats;

    [SerializeField] 
    private TMP_Text scoreText;

    [SerializeField] 
    private TMP_Text levelText;

    [SerializeField] 
    private AirPlaneController planeController;

    [SerializeField] 
    private TMP_Text speedText;

    [SerializeField] 
    private GameObject playerPanel;
    
    [SerializeField] 
    private GameObject settingPanel;

    [Inject]
    private void Construct(IStats stats)
    {
        _stats = stats;
    }

    private void Update()
    {
        ShowScoreAndLevel();
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

    private void ShowScoreAndLevel()
    {
        scoreText.text = $"Score: {_stats.Score}";
        levelText.text = $"Level: {_stats.Level}";
    }

    private void ShowSpeed()
    {
        speedText.text = $"Speed: {planeController.CurrentSpeed()}";
    }
}
