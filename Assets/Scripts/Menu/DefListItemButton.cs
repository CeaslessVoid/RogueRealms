using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RogueRealms
{
    public class DefListItemButton : MonoBehaviour
    {
        public Image northImage;
        public Image eastImage;
        public Image southImage;
        public TMP_Text nameText;
        public Button button;

        public void Setup(string displayName, Sprite north, Sprite east, Sprite south, Action onClick)
        {
            nameText.text = displayName;
            northImage.sprite = north;
            eastImage.sprite = east;
            southImage.sprite = south;
            button.onClick.AddListener(() => onClick?.Invoke());
        }
    }
}
