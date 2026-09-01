using UnityEngine;

namespace RogueRealms
{
    public class HumanoidBodyDrawer : MonoBehaviour, IBodyDrawer
    {
        [SerializeField] SpriteRenderer bodyRenderer;
        [SerializeField] SpriteRenderer bodyClothingRenderer;
        [SerializeField] SpriteRenderer headRenderer;
        [SerializeField] SpriteRenderer hairRenderer;
        [SerializeField] SpriteRenderer headClothingRenderer;

        DirectionalSprites bodySprites;
        DirectionalSprites headSprites;
        DirectionalSprites hairSprites;
        DirectionalSprites bodyClothingSprites;
        DirectionalSprites headClothingSprites;

        Direction currentFacing = Direction.South;

        void Awake()
        {
            if (bodySprites == null) SetBody(DefDatabase<BodyTypeDef>.Random());
            if (headSprites == null) SetHead(DefDatabase<HeadTypeDef>.Random());
        }

        public void SetBody(BodyTypeDef def)
        {
            bodySprites = def != null ? def.sprites : null;
            Apply(bodyRenderer, bodySprites);
        }

        public void SetHead(HeadTypeDef def)
        {
            headSprites = def != null ? def.sprites : null;
            Apply(headRenderer, headSprites);
        }

        public void SetHair(HairDef def)
        {
            hairSprites = def != null ? def.sprites : null;
            hairRenderer.enabled = def != null;
            Apply(hairRenderer, hairSprites);
        }

        public void SetClothing(ClothingDef def)
        {
            if (def == null) return;

            if (def.slot == ClothingSlot.Body)
            {
                bodyClothingSprites = def.sprites;
                bodyClothingRenderer.enabled = true;
                Apply(bodyClothingRenderer, bodyClothingSprites);
            }
            else
            {
                headClothingSprites = def.sprites;
                headClothingRenderer.enabled = true;
                Apply(headClothingRenderer, headClothingSprites);
            }
        }

        public void ClearClothing(ClothingSlot slot)
        {
            if (slot == ClothingSlot.Body)
            {
                bodyClothingSprites = null;
                bodyClothingRenderer.enabled = false;
            }
            else
            {
                headClothingSprites = null;
                headClothingRenderer.enabled = false;
            }
        }

        public void SetFacing(Direction dir)
        {
            currentFacing = dir;
            Apply(bodyRenderer, bodySprites);
            Apply(headRenderer, headSprites);
            Apply(hairRenderer, hairSprites);
            Apply(bodyClothingRenderer, bodyClothingSprites);
            Apply(headClothingRenderer, headClothingSprites);
        }

        void Apply(SpriteRenderer sr, DirectionalSprites sprites)
        {
            if (sr == null || sprites == null) return;
            sr.sprite = sprites.GetSprite(currentFacing, out bool flip);
            sr.flipX = flip;
        }
    }
}
