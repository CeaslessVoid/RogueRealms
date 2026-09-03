using UnityEngine;

namespace RogueRealms
{
    public class HairColorSelectorUI : MonoBehaviour
    {
        public Transform content;
        public ColorSwatchButton itemPrefab;
        public HumanoidBodyDrawer drawer;

        void Start()
        {
            foreach (var def in DefDatabase<HairColorDef>.All())
            {
                var item = Instantiate(itemPrefab, content);
                item.Setup(def.color, () => { drawer.SetHairColor(def); CharacterProfile.hairColor = def; });
            }
        }
    }
}
