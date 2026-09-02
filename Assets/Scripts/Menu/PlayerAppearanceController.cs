using UnityEngine;

namespace RogueRealms
{
    public class PlayerAppearanceController : MonoBehaviour
    {
        HumanoidBodyDrawer drawer;

        void Awake()
        {
            drawer = GetComponentInChildren<HumanoidBodyDrawer>();
        }

        void Start()
        {
            Apply();
        }

        public void Apply()
        {
            CharacterSaveService.EnsureProfileLoaded();
            if (drawer == null) return;

            drawer.SetBody(CharacterProfile.body);
            drawer.SetHead(CharacterProfile.head);
            drawer.SetHair(CharacterProfile.hair);

            if (CharacterProfile.selectedClass != null)
            {
                foreach (var clothing in CharacterProfile.selectedClass.defaultClothing)
                    drawer.SetClothing(clothing);
            }

            drawer.SetFacing(Direction.South);
        }
    }
}
