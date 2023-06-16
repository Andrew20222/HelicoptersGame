using UnityEngine;

public class Stats : MonoBehaviour, IStats
{
    private const int ZERO = 0;
    private const int MAX_SCORE_LEVEL = 10;
    [SerializeField]
    private float score;

    public float Score => score;
    [SerializeField] 
    private float level;

    public float Level => level;

    public void ScoreUpdate(float checkPointScore)
    {
        score += checkPointScore;
    }

    public void CheckScore()
    {
        if (score > MAX_SCORE_LEVEL)
        {
            LevelUpdate();
        }
    }

    private void LevelUpdate()
    {
        level++;
    }
}