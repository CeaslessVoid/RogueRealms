using UnityEngine;

namespace RogueRealms
{
    [RequireComponent(typeof(PlayerEntity))]
    public class PlayerController : MonoBehaviour
    {
        PlayerEntity entity;
        Camera cam;
        Vector2 moveInput;

        void Awake()
        {
            entity = GetComponent<PlayerEntity>();
            cam = Camera.main;
        }

        void Update()
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
            UpdateFacing();
        }

        void FixedUpdate()
        {
            entity.Move(moveInput);
        }

        void UpdateFacing()
        {
            if (cam == null)
            {
                cam = Camera.main;
                if (cam == null) return;
            }

            Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 toMouse = (Vector2)(mouseWorld - transform.position);
            Direction dir = DirectionUtility.FromVector(toMouse, entity.FacingDirection);
            entity.SetFacing(dir);
        }
    }
}
