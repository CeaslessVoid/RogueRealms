using System.Collections.Generic;
using UnityEngine;

namespace RogueRealms
{
    public class ClassSelectorUI : MonoBehaviour
    {
        public Transform listContent;
        public ClassListItemButton itemPrefab;
        public ClassDescriptionPanel descriptionPanel;
        public CharacterPreviewDisplay preview;

        List<ClassDef> classes;
        int selectedIndex;

        void Start()
        {
            CharacterSaveService.EnsureProfileLoaded();

            classes = new List<ClassDef>(DefDatabase<ClassDef>.All());

            foreach (var def in classes)
            {
                var item = Instantiate(itemPrefab, listContent);
                item.Setup(def, OnClassPicked);
            }

            if (classes.Count == 0) return;

            int startIndex = CharacterProfile.selectedClass != null ? classes.IndexOf(CharacterProfile.selectedClass) : 0;
            if (startIndex < 0) startIndex = 0;
            Select(startIndex);
        }

        void OnClassPicked(ClassDef def)
        {
            Select(classes.IndexOf(def));
        }

        public void Next()
        {
            if (classes.Count == 0) return;
            Select((selectedIndex + 1) % classes.Count);
        }

        public void Previous()
        {
            if (classes.Count == 0) return;
            Select((selectedIndex - 1 + classes.Count) % classes.Count);
        }

        void Select(int index)
        {
            selectedIndex = index;
            var def = classes[index];
            CharacterProfile.selectedClass = def;
            descriptionPanel.Show(def);
            preview.ApplyClass(def);
        }
    }
}
