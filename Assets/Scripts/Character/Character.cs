using UnityEngine;

namespace Characters
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private GameObject[] allCharacters;
        private int _indexCharacter;

        private void Start()
        {
            _indexCharacter = PlayerPrefs.GetInt("CurrentCharacter");
            allCharacters[_indexCharacter].SetActive(true);
        }
    }
}