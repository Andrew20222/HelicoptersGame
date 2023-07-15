using Character;
using Characters.GameUI.CharacterSelection;
using TMPro;
using UnityEngine;

namespace MoneySystem.Controlls
{
    public class MoneyController : MonoBehaviour
    {
        public TMP_Text MoneyText;

        private void Start()
        {
            Load();
            UpdateMoneyLabel();
        }

        private void Save()
        {
            PlayerPrefs.SetString("SaveGame", JsonUtility.ToJson(DataController.Data));
        }

        private void Load()
        {
            if (PlayerPrefs.HasKey("SaveGame"))
            {
                DataController.Data = JsonUtility.FromJson<Dates.Data>(PlayerPrefs.GetString("SaveGame"));
            }
        }

        public void AddMoney(int amount)
        {
            Load();
            DataController.Data.Money += amount;
            UpdateMoneyLabel();
            Save();
        }
        public bool HasEnoughMoney(int amount)
        {
            return DataController.Data.Money >= amount;
        }

        public void SubtractMoney(int amount)
        {
            Load();
            DataController.Data.Money -= amount;
            UpdateMoneyLabel();
            Save();
        }

        private void UpdateMoneyLabel()
        {
            MoneyText.text = "$" + DataController.Data.Money;
        }
    }
}