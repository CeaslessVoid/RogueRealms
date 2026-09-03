using System;
using UnityEngine;
using UnityEngine.UI;

namespace RogueRealms
{
    public class ColorSwatchButton : MonoBehaviour
    {
        public Image swatch;
        public Button button;

        public void Setup(Color color, Action onClick)
        {
            swatch.color = color;
            button.onClick.AddListener(() => onClick?.Invoke());
        }
    }
}
