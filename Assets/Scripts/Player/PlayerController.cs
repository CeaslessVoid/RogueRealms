using UnityEngine;

namespace RogueRealms
{
    [RequireComponent(typeof(PlayerEntity))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        public DashCooldownUI cooldownUI;

        PlayerEntity entity;
        Rigidbody2D rb;
        Camera cam;

        Vector2 moveInput;

        bool isDashing;
        float dashTimer;
        Vector2 dashVelocity;
        float dashCooldownTimer;

        void Awake()
        {
            entity = GetComponent<PlayerEntity>();
            rb = GetComponent<Rigidbody2D>();
            cam = Camera.main;
        }

        void Update()
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
            UpdateFacing();
            UpdateDash();
        }

        void FixedUpdate()
        {
            if (isDashing)
            {
                Vector2 target = rb.position + dashVelocity * Time.fixedDeltaTime;
                rb.MovePosition(MapManager.Clamp(target));
                return;
            }

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

        void UpdateDash()
        {
            if (dashCooldownTimer > 0f)
                dashCooldownTimer = Mathf.Max(0f, dashCooldownTimer - Time.deltaTime);

            float cd = Mathf.Max(0.0001f, entity.stats.dashCooldown);
            cooldownUI?.SetProgress(1f - dashCooldownTimer / cd);
            cooldownUI?.SetTimer(dashCooldownTimer);

            if (Input.GetKeyDown(KeyCode.Space) && !isDashing && dashCooldownTimer <= 0f)
                StartDash();

            if (isDashing)
            {
                dashTimer -= Time.deltaTime;
                if (dashTimer <= 0f) isDashing = false;
            }
        }

        void StartDash()
        {
            Vector2 dir = moveInput;

            if (dir.sqrMagnitude < 0.01f)
            {
                if (cam == null) cam = Camera.main;
                if (cam != null)
                {
                    Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
                    dir = (Vector2)(mouseWorld - transform.position);
                }
            }

            if (dir.sqrMagnitude < 0.0001f) return;

            dashVelocity = dir.normalized * (entity.stats.dashDistance / entity.stats.dashDuration);
            isDashing = true;
            dashTimer = entity.stats.dashDuration;
            dashCooldownTimer = entity.stats.dashCooldown;
        }
    }
}
