using UnityEngine;

namespace RogueRealms
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class WeaponHolder : MonoBehaviour
    {
        public WeaponInventory inventory;
        public Transform player;
        public float holdDistance = 0.5f;

        SpriteRenderer sr;
        Camera cam;

        void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
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
        }

        void Update()
        {
            var weapon = inventory != null ? inventory.CurrentWeapon : null;
            if (weapon == null || player == null) return;

            if (cam == null)
            {
                cam = Camera.main;
                if (cam == null) return;
            }

            Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = (Vector2)(mouseWorld - player.position);
            if (dir.sqrMagnitude < 0.0001f) return;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            transform.position = player.position + (Vector3)(dir.normalized * holdDistance);
            transform.rotation = Quaternion.Euler(0f, 0f, angle - weapon.spriteAngleOffset);
            sr.flipY = dir.x < 0f;
        }
    }
}
