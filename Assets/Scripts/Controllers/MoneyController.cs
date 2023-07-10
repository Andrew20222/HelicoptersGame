using TMPro;
using UnityEngine;

namespace MoneySystem.Controlls
{
    public class MoneyController : MonoBehaviour
    {
        public TMP_Text MoneyText;

        private int money;

        private void Start()
        {
            money = 1000;
            UpdateMoneyLabel();
        }

        public void AddMoney(int amount)
        {
            money += amount;
            UpdateMoneyLabel();
        }
        public bool HasEnoughMoney(int amount)
        {
            return money >= amount;
        }

        public void SubtractMoney(int amount)
        {
            money -= amount;
            UpdateMoneyLabel();
        }

        private void UpdateMoneyLabel()
        {
            MoneyText.text = "$" + money;
        }
    }
}