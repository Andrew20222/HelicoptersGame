using System.Collections;
using UnityEngine;

namespace Characters
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private GameObject[] allCharacters;
        private Dates.Data _data = new();
        private int _indexCharacter;

        private void Start()
        {
            _data = JsonUtility.FromJson<Dates.Data>(PlayerPrefs.GetString("SaveGame"));
            StartCoroutine(LoadCharacter());
        }

        private IEnumerator LoadCharacter()
        {
            _indexCharacter = 0;
            while (allCharacters[_indexCharacter].name != _data.CurrentCharacter)
            {
                _indexCharacter++;
            }
            
            allCharacters[_indexCharacter].SetActive(true);
            yield return null;
        }
    }
}