using UnityEngine;

namespace RogueRealms
{
    public class SkinToneSelectorUI : MonoBehaviour
    {
        public Transform content;
        public ColorSwatchButton itemPrefab;
        public HumanoidBodyDrawer drawer;

        void Start()
        {
            foreach (var def in DefDatabase<SkinToneDef>.All())
            {
                var item = Instantiate(itemPrefab, content);
                item.Setup(def.color, () => { drawer.SetSkinTone(def); CharacterProfile.skinTone = def; });
            }
        }
    }
}
