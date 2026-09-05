using UnityEngine;

namespace RogueRealms
{
    public class PlayerAttackInput : MonoBehaviour
    {
        public WeaponHolder weaponHolder;
        public Transform wielder;

        Camera cam;

        void Awake()
        {
            cam = Camera.main;
        }

        void Update()
        {
            if (!Input.GetMouseButton(0)) return;
            if (weaponHolder == null || wielder == null) return;

            if (cam == null)
            {
                cam = Camera.main;
                if (cam == null) return;
            }

            Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = (Vector2)(mouseWorld - wielder.position);
            weaponHolder.TriggerAttack(dir);
        }
    }
}
