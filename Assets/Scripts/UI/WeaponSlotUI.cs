using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RogueRealms
{
    public class WeaponSlotUI : MonoBehaviour
    {
        public Image icon;
        public TMP_Text nameText;

        public float inactiveScale = 1f;
        public float activeScale = 1.3f;

        public void Show(WeaponDef def, bool isActive)
        {
            gameObject.SetActive(def != null);
            if (def == null) return;

            icon.sprite = def.sprite;
            nameText.text = def.displayName;
            transform.localScale = Vector3.one * (isActive ? activeScale : inactiveScale);
        }
    }
}
