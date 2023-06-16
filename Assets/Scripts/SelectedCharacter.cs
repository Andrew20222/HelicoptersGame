using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectedCharacter : MonoBehaviour
{
    private int _indexCharacter;
    private int _indexCurrentCharacter;

    [SerializeField] 
    private GameObject[] allCharacters;

    [SerializeField]
    private GameObject arrowToLeft;
    [SerializeField]
    private GameObject arrowToRight;

    [SerializeField] 
    private GameObject buttonSelectCharacter;
    [SerializeField] 
    private GameObject textSelectCharacter;
    
    private void Start()
    {
        if (PlayerPrefs.HasKey("CurrentCharacter"))
        {
            _indexCharacter = PlayerPrefs.GetInt("CurrentCharacter");
            _indexCurrentCharacter = PlayerPrefs.GetInt("CurrentCharacter");
        }
        else
        {
            _indexCharacter = 0;
            PlayerPrefs.SetInt("CurrentCharacter", _indexCharacter);
        }
        
        SetActiveCharacter();

        buttonSelectCharacter.SetActive(false);
        textSelectCharacter.SetActive(true);

        UpdateArrowVisibility();
    }

    private void SetActiveCharacter()
    {
        for (int i = 0; i < allCharacters.Length; i++)
        {
            if (i == _indexCharacter)
            {
                allCharacters[i].SetActive(true);
            }
            else
            {
                allCharacters[i].SetActive(false);
            }
        }
    }

    private void UpdateArrowVisibility()
    {
        arrowToLeft.SetActive(_indexCharacter > 0);
        arrowToRight.SetActive(_indexCharacter < allCharacters.Length - 1);
    }

    public void ArrowRight()
    {
        if (_indexCharacter < allCharacters.Length - 1)
        {
            allCharacters[_indexCharacter].SetActive(false);
            _indexCharacter++;
            allCharacters[_indexCharacter].SetActive(true);
            UpdateArrowVisibility();

            if (_indexCurrentCharacter == _indexCharacter)
            {
                buttonSelectCharacter.SetActive(false);
                textSelectCharacter.SetActive(true);
            }
            else
            {
                buttonSelectCharacter.SetActive(true);
                textSelectCharacter.SetActive(false);
            }
        }
    }
    
    public void ArrowLeft()
    {
        if (_indexCharacter > 0)
        {
            allCharacters[_indexCharacter].SetActive(false);
            _indexCharacter--;
            allCharacters[_indexCharacter].SetActive(true);
            UpdateArrowVisibility();

            if (_indexCurrentCharacter == _indexCharacter)
            {
                buttonSelectCharacter.SetActive(false);
                textSelectCharacter.SetActive(true);
            }
            else
            {
                buttonSelectCharacter.SetActive(true);
                textSelectCharacter.SetActive(false);
            }
        }
    }

    public void SelectCharacter()
    {
        PlayerPrefs.SetInt("CurrentCharacter", _indexCharacter);
        _indexCurrentCharacter = _indexCharacter;
        buttonSelectCharacter.SetActive(false);
        textSelectCharacter.SetActive(true);
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
