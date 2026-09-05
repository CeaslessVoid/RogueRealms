using UnityEngine;

namespace RogueRealms
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class Entity : MonoBehaviour
    {
        public abstract EntityStats BaseStats { get; }

        public Direction FacingDirection { get; private set; } = Direction.South;

        protected Rigidbody2D rb;
        protected IBodyDrawer bodyDrawer;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            bodyDrawer = GetComponentInChildren<IBodyDrawer>();
            BaseStats.InitializeDefaults();
        }

        protected virtual void Start()
        {
            bodyDrawer?.SetFacing(FacingDirection);
        }

        public virtual void Move(Vector2 dir)
        {
            if (dir.sqrMagnitude > 1f) dir.Normalize();
            Vector2 target = rb.position + dir * BaseStats.CurrentMoveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(MapManager.Clamp(target));
        }

        public virtual void SetFacing(Direction dir)
        {
            if (FacingDirection == dir) return;
            FacingDirection = dir;
            bodyDrawer?.SetFacing(dir);
        }
    }
}
