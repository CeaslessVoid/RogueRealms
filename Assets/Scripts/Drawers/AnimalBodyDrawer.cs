using UnityEngine;

namespace RogueRealms
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class AnimalBodyDrawer : MonoBehaviour, IBodyDrawer
    {
        [SerializeField] DirectionalSprites sprites;

        SpriteRenderer sr;

        void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        public void SetFacing(Direction dir)
        {
            sr.sprite = sprites.GetSprite(dir, out bool flip);
            sr.flipX = flip;
        }

        public void SetSprites(DirectionalSprites newSprites)
        {
            sprites = newSprites;
        }
    }
}
