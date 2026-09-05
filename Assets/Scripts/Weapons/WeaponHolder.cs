using System.Collections.Generic;
using UnityEngine;

namespace RogueRealms
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(PolygonCollider2D))]
    public class WeaponHolder : MonoBehaviour
    {
        public WeaponInventory inventory;
        public Transform wielder;
        public float holdDistance = 0.5f;

        public float slashArcDegrees = 150f;
        public float impactArcDegrees = 45f;
        public float impactReachBonus = 0.4f;
        public float punctureReachBonus = 0.6f;
        public float swingDuration = 0.25f;

        SpriteRenderer sr;
        PolygonCollider2D hitCollider;
        Camera cam;

        bool isSwinging;
        float swingTimer;
        bool swingFromRight = true;
        float swingBaseAngle;
        WeaponDef swingWeapon;

        static readonly List<Vector2> ShapeBuffer = new List<Vector2>();

        void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
            hitCollider = GetComponent<PolygonCollider2D>();
            hitCollider.isTrigger = true;
            hitCollider.enabled = false;
            cam = Camera.main;
        }

        void Start()
        {
            if (inventory != null) inventory.OnChanged += Refresh;
            Refresh();
        }

        void OnDestroy()
        {
            if (inventory != null) inventory.OnChanged -= Refresh;
        }

        void Refresh()
        {
            var weapon = inventory != null ? inventory.CurrentWeapon : null;
            sr.sprite = weapon != null ? weapon.sprite : null;
            sr.enabled = weapon != null;

            float size = weapon != null ? weapon.defaultSize : 1f;
            var playerEntity = wielder != null ? wielder.GetComponent<PlayerEntity>() : null;
            if (playerEntity != null) size *= playerEntity.stats.weaponSizeMultiplier;
            transform.localScale = Vector3.one * size;

            RebuildHitCollider();

            isSwinging = false;
            hitCollider.enabled = false;
        }

        void RebuildHitCollider()
        {
            hitCollider.pathCount = 0;
            if (sr.sprite == null) return;

            int shapeCount = sr.sprite.GetPhysicsShapeCount();
            if (shapeCount == 0) return;

            hitCollider.pathCount = shapeCount;
            for (int i = 0; i < shapeCount; i++)
            {
                ShapeBuffer.Clear();
                sr.sprite.GetPhysicsShape(i, ShapeBuffer);
                hitCollider.SetPath(i, ShapeBuffer);
            }
        }

        public bool TriggerAttack(Vector2 aimDirection)
        {
            var weapon = inventory != null ? inventory.CurrentWeapon : null;
            if (weapon == null || weapon.type != WeaponType.Melee) return false;
            if (isSwinging) return false;
            if (aimDirection.sqrMagnitude < 0.0001f) return false;

            swingWeapon = weapon;
            swingBaseAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            isSwinging = true;
            swingTimer = 0f;
            hitCollider.enabled = true;
            return true;
        }

        void Update()
        {
            var weapon = inventory != null ? inventory.CurrentWeapon : null;
            if (weapon == null || wielder == null) return;

            if (isSwinging)
            {
                UpdateSwing();
                return;
            }

            UpdateAim(weapon);
        }

        void UpdateAim(WeaponDef weapon)
        {
            if (cam == null)
            {
                cam = Camera.main;
                if (cam == null) return;
            }

            Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = (Vector2)(mouseWorld - wielder.position);
            if (dir.sqrMagnitude < 0.0001f) return;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            transform.position = wielder.position + (Vector3)(dir.normalized * holdDistance);
            transform.rotation = Quaternion.Euler(0f, 0f, angle - weapon.spriteAngleOffset);
            sr.flipY = dir.x < 0f;
        }

        void UpdateSwing()
        {
            swingTimer += Time.deltaTime;
            float t = Mathf.Clamp01(swingTimer / swingDuration);

            float angleOffset = 0f;
            float distance = holdDistance;
            float side = swingFromRight ? 1f : -1f;

            switch (swingWeapon.swingType)
            {
                case MeleeSwingType.Slashing:
                    angleOffset = Mathf.Lerp(side * slashArcDegrees * 0.5f, -side * slashArcDegrees * 0.5f, t);
                    break;

                case MeleeSwingType.Impact:
                    angleOffset = Mathf.Lerp(side * impactArcDegrees * 0.5f, -side * impactArcDegrees * 0.5f, t);
                    distance = holdDistance + impactReachBonus * Mathf.Sin(t * Mathf.PI);
                    break;

                case MeleeSwingType.Puncture:
                    distance = holdDistance + punctureReachBonus * Mathf.Sin(t * Mathf.PI);
                    break;
            }

            float angle = swingBaseAngle + angleOffset;
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            transform.position = wielder.position + (Vector3)(dir * distance);
            transform.rotation = Quaternion.Euler(0f, 0f, angle - swingWeapon.spriteAngleOffset);
            sr.flipY = dir.x < 0f;

            if (t >= 1f)
            {
                isSwinging = false;
                hitCollider.enabled = false;

                if (swingWeapon.swingType != MeleeSwingType.Puncture)
                    swingFromRight = !swingFromRight;
            }
        }
    }
}
