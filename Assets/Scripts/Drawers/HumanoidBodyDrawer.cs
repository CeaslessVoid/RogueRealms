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

        Vector3 headBasePos;
        Vector3 hairBasePos;
        Vector3 headClothingBasePos;

        const float HeadSideOffset = 0.05f;

        void Awake()
        {
            headBasePos = headRenderer.transform.localPosition;
            hairBasePos = hairRenderer.transform.localPosition;
            headClothingBasePos = headClothingRenderer.transform.localPosition;

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
            ApplyHeadOffset(dir);
        }

        void ApplyHeadOffset(Direction dir)
        {
            float x = dir == Direction.East ? HeadSideOffset : dir == Direction.West ? -HeadSideOffset : 0f;
            headRenderer.transform.localPosition = headBasePos + new Vector3(x, 0f, 0f);
            hairRenderer.transform.localPosition = hairBasePos + new Vector3(x, 0f, 0f);
            headClothingRenderer.transform.localPosition = headClothingBasePos + new Vector3(x, 0f, 0f);
        }

        void Apply(SpriteRenderer sr, DirectionalSprites sprites)
        {
            if (sr == null || sprites == null) return;
            sr.sprite = sprites.GetSprite(currentFacing, out bool flip);
            sr.flipX = flip;
        }
    }
}
