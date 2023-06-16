using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    private int _indexCharacter;
    [SerializeField] 
    private GameObject[] allCharacters;

    private void Start()
    {
        _indexCharacter = PlayerPrefs.GetInt("CurrentCharacter");
        allCharacters[_indexCharacter].SetActive(true);
    }
}
