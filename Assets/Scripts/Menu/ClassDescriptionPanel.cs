using TMPro;
using UnityEngine;

namespace RogueRealms
{
    public class ClassDescriptionPanel : MonoBehaviour
    {
        public TMP_Text nameText;
        public TMP_Text descriptionText;

        public void Show(ClassDef def)
        {
            nameText.text = def.displayName;
            descriptionText.text = def.description;
        }
    }
}
