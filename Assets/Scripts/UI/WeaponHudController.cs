using UnityEngine;

namespace RogueRealms
{
    public class WeaponHudController : MonoBehaviour
    {
        public WeaponInventory inventory;
        public WeaponSlotUI[] slotUI = new WeaponSlotUI[WeaponInventory.SlotCount];

        void Start()
        {
            inventory.OnChanged += Refresh;
            Refresh();
        }

        void OnDestroy()
        {
            if (inventory != null) inventory.OnChanged -= Refresh;
        }

        void Refresh()
        {
            for (int i = 0; i < slotUI.Length; i++)
                slotUI[i].Show(inventory.slots[i], i == inventory.CurrentIndex);
        }
    }
}
