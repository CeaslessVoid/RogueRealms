using System;
using UnityEngine;

namespace RogueRealms
{
    public class WeaponInventory : MonoBehaviour
    {
        public const int SlotCount = 5;

        public WeaponDef[] slots = new WeaponDef[SlotCount];
        public int CurrentIndex { get; private set; }

        public event Action OnChanged;

        void Start()
        {
            if (CharacterProfile.selectedClass != null)
            {
                var starting = CharacterProfile.selectedClass.startingWeapons;
                for (int i = 0; i < starting.Count && i < SlotCount; i++)
                    slots[i] = starting[i];
            }

            OnChanged?.Invoke();
        }

        public WeaponDef CurrentWeapon => slots[CurrentIndex];

        public void SelectSlot(int index)
        {
            if (index < 0 || index >= SlotCount) return;
            if (CurrentIndex == index) return;
            CurrentIndex = index;
            OnChanged?.Invoke();
        }

        public void NextSlot()
        {
            SelectSlot((CurrentIndex + 1) % SlotCount);
        }

        public void PreviousSlot()
        {
            SelectSlot((CurrentIndex - 1 + SlotCount) % SlotCount);
        }
    }
}
