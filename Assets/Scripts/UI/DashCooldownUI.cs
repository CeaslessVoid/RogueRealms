using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RogueRealms
{
    public class DashCooldownUI : MonoBehaviour
    {
        public Image grayImage;
        public Image colorImage;
        public TMP_Text timerText;

        void Awake()
        {
            colorImage.type = Image.Type.Filled;
            colorImage.fillMethod = Image.FillMethod.Horizontal;
            colorImage.fillOrigin = (int)Image.OriginHorizontal.Left;
        }

        public void SetProgress(float progress01)
        {
            colorImage.fillAmount = Mathf.Clamp01(progress01);
        }

        public void SetTimer(float secondsRemaining)
        {
            timerText.text = secondsRemaining > 0.05f ? secondsRemaining.ToString("F1") : "";
        }
    }
}
