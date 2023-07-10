using Equipments.View;
using UnityEngine;

namespace Equipments.Controller
{
    public class EquipmentController : MonoBehaviour
    {
        [SerializeField] private EquipmentView equipmentView;

        public Equipment currentEquipment;

        private void Start()
        {
            currentEquipment = new Equipment("Helicopter", 100);
            UpdateEquipmentView();
        }

        public void AddEquipment(Equipment equipment)
        {
            currentEquipment = equipment;
            UpdateEquipmentView();
        }

        public void RemoveEquipment(Equipment equipment)
        {
            if (currentEquipment == equipment)
            {
                currentEquipment = null;
                UpdateEquipmentView();
            }
        }

        private void UpdateEquipmentView()
        {
            equipmentView.DisplayEquipment(currentEquipment);
        }
    }
}