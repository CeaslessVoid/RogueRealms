using UnityEngine;

namespace RogueRealms
{
    [RequireComponent(typeof(WeaponInventory))]
    public class WeaponInputController : MonoBehaviour
    {
        WeaponInventory inventory;

        void Awake()
        {
            inventory = GetComponent<WeaponInventory>();
        }

        void Update()
        {
            for (int i = 0; i < WeaponInventory.SlotCount; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    inventory.SelectSlot(i);
                    break;
                }
            }
        }
    }
}
