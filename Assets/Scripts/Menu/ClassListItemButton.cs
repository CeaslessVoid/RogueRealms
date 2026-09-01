using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RogueRealms
{
    public class ClassListItemButton : MonoBehaviour
    {
        public TMP_Text nameText;
        public Button button;

        ClassDef def;
        Action<ClassDef> onClick;

        public void Setup(ClassDef classDef, Action<ClassDef> callback)
        {
            def = classDef;
            onClick = callback;
            nameText.text = def.displayName;
            button.onClick.AddListener(() => onClick?.Invoke(def));
        }
    }
}
