using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectedCharacter : MonoBehaviour
{
    [SerializeField] private GameObject[] allCharacters;
    [SerializeField] private Button arrowToLeft;
    [SerializeField] private Button arrowToRight;
    [SerializeField] private Button buttonSelectCharacter;
    [SerializeField] private TMP_Text textSelectCharacter;
    [SerializeField] private Button playButton;
    
    private int _indexCharacter;
    private int _indexCurrentCharacter;
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

        buttonSelectCharacter.gameObject.SetActive(false);
        textSelectCharacter.gameObject.SetActive(true);

        UpdateArrowVisibility();
    }


    private void Update()
    {
        playButton.onClick.AddListener(ChangeScene);
        arrowToLeft.onClick.AddListener(ArrowLeft);
        arrowToRight.onClick.AddListener(ArrowRight);
        buttonSelectCharacter.onClick.AddListener(SelectCharacter);
        
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
        arrowToLeft.gameObject.SetActive(_indexCharacter > 0);
        arrowToRight.gameObject.SetActive(_indexCharacter < allCharacters.Length - 1);
    }

    private void ArrowRight()
    {
        if (_indexCharacter < allCharacters.Length - 1)
        {
            allCharacters[_indexCharacter].SetActive(false);
            _indexCharacter++;
            allCharacters[_indexCharacter].SetActive(true);
            UpdateArrowVisibility();

            if (_indexCurrentCharacter == _indexCharacter)
            {
                buttonSelectCharacter.gameObject.SetActive(false);
                textSelectCharacter.gameObject.SetActive(true);
            }
            else
            {
                buttonSelectCharacter.gameObject.SetActive(true);
                textSelectCharacter.gameObject.SetActive(false);
            }
        }
    }

    private void ArrowLeft()
    {
        if (_indexCharacter > 0)
        {
            allCharacters[_indexCharacter].SetActive(false);
            _indexCharacter--;
            allCharacters[_indexCharacter].SetActive(true);
            UpdateArrowVisibility();

            if (_indexCurrentCharacter == _indexCharacter)
            {
                buttonSelectCharacter.gameObject.SetActive(false);
                textSelectCharacter.gameObject.SetActive(true);
            }
            else
            {
                buttonSelectCharacter.gameObject.SetActive(true);
                textSelectCharacter.gameObject.SetActive(false);
            }
        }
    }

    private void SelectCharacter()
    {
        PlayerPrefs.SetInt("CurrentCharacter", _indexCharacter);
        _indexCurrentCharacter = _indexCharacter;
        buttonSelectCharacter.gameObject.SetActive(false);
        textSelectCharacter.gameObject.SetActive(true);
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}