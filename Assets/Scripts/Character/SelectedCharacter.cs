using System.Collections;
using Character;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Characters.GameUI.CharacterSelection
{
    public class SelectedCharacter : MonoBehaviour
    {
        [SerializeField] private GameObject[] allCharacters;
        [SerializeField] private Button arrowToLeft;
        [SerializeField] private Button arrowToRight;
        [SerializeField] private Button buttonSelectCharacter;
        [SerializeField] private Button buttonBuyCharacter;
        [SerializeField] private TMP_Text textSelectCharacter;
        [SerializeField] private TMP_Text textPrice;
        [SerializeField] private Button playButton;
        [SerializeField] private Slider speedSlider;
        [SerializeField] private Slider weightSlider;
        
        private string _statusCheck;
        private int _indexCharacter;
        private int _check;

        private void Update()
        {
            speedSlider.value = allCharacters[_indexCharacter].GetComponent<Item>().Speed;
            weightSlider.value = allCharacters[_indexCharacter].GetComponent<Item>().Weight;
        }

        private void Start()
        {
            DataController.Data = new Dates.Data
            {
                Money = 500
            };

            if (PlayerPrefs.HasKey("SaveGame"))
            {
                DataController.Data = JsonUtility.FromJson<Dates.Data>(PlayerPrefs.GetString("SaveGame"));
            }
            else
            {
                PlayerPrefs.SetString("SaveGame", JsonUtility.ToJson(DataController.Data));
            }
                
            SetActiveCharacter();

            if (DataController.Data.CurrentCharacter == allCharacters[_indexCharacter].name)
            {
                buttonBuyCharacter.gameObject.SetActive(false);
                buttonSelectCharacter.gameObject.SetActive(false);
                textSelectCharacter.gameObject.SetActive(true);
            }
            else if (DataController.Data.CurrentCharacter != allCharacters[_indexCharacter].name)
            {
                StartCoroutine(CheckHaveCharacters());
            }

            
            UpdateArrowVisibility();

            playButton.onClick.AddListener(ChangeScene);
            arrowToLeft.onClick.AddListener(ArrowLeft);
            arrowToRight.onClick.AddListener(ArrowRight);
            buttonSelectCharacter.onClick.AddListener(SelectCharacter);
            buttonBuyCharacter.onClick.AddListener(BuyCharacter);
        }

        private IEnumerator CheckHaveCharacters()
        {
            while (_statusCheck != "Check")
            {
                if (DataController.Data.HaveCharacters.Count != _check)
                {
                    if (allCharacters[_indexCharacter].name != DataController.Data.HaveCharacters[_check])
                    {
                        _check++;
                    }
                    else if (allCharacters[_indexCharacter].name == DataController.Data.HaveCharacters[_check])
                    {
                        textSelectCharacter.gameObject.SetActive(false);
                        buttonBuyCharacter.gameObject.SetActive(false);
                        buttonSelectCharacter.gameObject.SetActive(true);
                        _check = 0;
                        _statusCheck = "Check";
                    }
                }
                else if (DataController.Data.HaveCharacters.Count == _check)
                {
                    textSelectCharacter.gameObject.SetActive(false);
                    buttonBuyCharacter.gameObject.SetActive(true);
                    textPrice.text = allCharacters[_indexCharacter].GetComponent<Item>().PriceCharacter.ToString();
                    _check = 0;
                    _statusCheck = "Check";
                }
            }

            _statusCheck = " ";
            yield return null;
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

                if (DataController.Data.CurrentCharacter == allCharacters[_indexCharacter].name)
                {
                    buttonBuyCharacter.gameObject.SetActive(false);
                    buttonSelectCharacter.gameObject.SetActive(false);
                    textSelectCharacter.gameObject.SetActive(true);
                }
                else if (DataController.Data.CurrentCharacter != allCharacters[_indexCharacter].name)
                {
                    StartCoroutine(CheckHaveCharacters());
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

                if (DataController.Data.CurrentCharacter == allCharacters[_indexCharacter].name)
                {
                    buttonBuyCharacter.gameObject.SetActive(false);
                    buttonSelectCharacter.gameObject.SetActive(false);
                    textSelectCharacter.gameObject.SetActive(true);
                }
                else if (DataController.Data.CurrentCharacter != allCharacters[_indexCharacter].name)
                {
                    StartCoroutine(CheckHaveCharacters());
                }
            }
        }

        private void SelectCharacter()
        {
            DataController.Data = JsonUtility.FromJson<Dates.Data>(PlayerPrefs.GetString("SaveGame"));
            DataController.Data.CurrentCharacter = allCharacters[_indexCharacter].name;
            PlayerPrefs.SetString("SaveGame", JsonUtility.ToJson(DataController.Data));
            buttonSelectCharacter.gameObject.SetActive(false);
            textSelectCharacter.gameObject.SetActive(true);
        }
        private void BuyCharacter()
        {
            if (DataController.Data.Money >= allCharacters[_indexCharacter].GetComponent<Item>().PriceCharacter)
            {
                DataController.Data = JsonUtility.FromJson<Dates.Data>(PlayerPrefs.GetString("SaveGame"));
                DataController.Data.Money -= allCharacters[_indexCharacter].GetComponent<Item>().PriceCharacter;
                DataController.Data.HaveCharacters.Add(allCharacters[_indexCharacter].name);
                PlayerPrefs.SetString("SaveGame", JsonUtility.ToJson(DataController.Data));
                
                buttonBuyCharacter.gameObject.SetActive(false);
                buttonSelectCharacter.gameObject.SetActive(true);
            }
        }

        private void ChangeScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        private void OnDisable()
        {
            playButton.onClick.RemoveListener(ChangeScene);
            arrowToLeft.onClick.RemoveListener(ArrowLeft);
            arrowToRight.onClick.RemoveListener(ArrowRight);
            buttonSelectCharacter.onClick.RemoveListener(SelectCharacter);
            buttonBuyCharacter.onClick.RemoveListener(BuyCharacter);
        }
    }
}