using UnityEngine;

namespace RogueRealms
{
    [RequireComponent(typeof(Collider2D))]
    public class CharacterPreviewDisplay : MonoBehaviour
    {
        public HumanoidBodyDrawer drawer;
        public CharacterEditorController editor;

        public void ApplyClass(ClassDef def)
        {
            if (def == null) return;
            foreach (var clothing in def.defaultClothing)
                drawer.SetClothing(clothing);
        }

        void OnMouseDown()
        {
            editor.Open();
        }
    }
}
