using Equipments;
using Equipments.Controller;
using MoneySystem.Controlls;
using UnityEngine;

namespace Mechanics.Shoping
{
    public class ShopService : MonoBehaviour
    {
        public EquipmentController equipmentController;
        public MoneyController moneyController;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                BuyEquipment(equipmentController.currentEquipment);
            }
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                SellEquipment(equipmentController.currentEquipment);
            }
        }

        private void BuyEquipment(Equipment equipment)
        {
            if (moneyController.HasEnoughMoney(equipment.Price))
            {
                moneyController.SubtractMoney(equipment.Price);
                equipmentController.AddEquipment(equipment);
            }
            else
            {
                Debug.Log("Not enough money to buy " + equipment.Name);
            }
        }

        public void SellEquipment(Equipment equipment)
        {
            moneyController.AddMoney(equipment.Price);
            equipmentController.RemoveEquipment(equipment);
        }
    }
}