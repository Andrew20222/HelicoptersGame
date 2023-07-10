using TMPro;
using UnityEngine;

namespace Equipments.View
{
    public class EquipmentView : MonoBehaviour
    {
        public TMP_Text NameText;
        public TMP_Text PriceText;

        public void DisplayEquipment(Equipment equipment)
        {
            NameText.text = equipment.Name;
            PriceText.text = "$" + equipment.Price.ToString();
        }
    }
}