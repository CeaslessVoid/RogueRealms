using UnityEngine;

namespace RogueRealms
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerDash : MonoBehaviour
    {
        public PlayerEntity entity;
        public float dashDuration = 0.15f;
        public DashCooldownUI cooldownUI;

        public bool IsDashing { get; private set; }

        Rigidbody2D rb;
        Camera cam;

        float dashTimer;
        Vector2 dashVelocity;
        float cooldownTimer;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            cam = Camera.main;
        }

        void Update()
        {
            if (cooldownTimer > 0f)
                cooldownTimer = Mathf.Max(0f, cooldownTimer - Time.deltaTime);

            float cd = Mathf.Max(0.0001f, entity.stats.dashCooldown);
            cooldownUI?.SetProgress(1f - cooldownTimer / cd);
            cooldownUI?.SetTimer(cooldownTimer);

            if (Input.GetKeyDown(KeyCode.Space) && !IsDashing && cooldownTimer <= 0f)
                StartDash();

            if (IsDashing)
            {
                dashTimer -= Time.deltaTime;
                if (dashTimer <= 0f) IsDashing = false;
            }
        }

        void FixedUpdate()
        {
            if (IsDashing)
                rb.MovePosition(rb.position + dashVelocity * Time.fixedDeltaTime);
        }

        void StartDash()
        {
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (input.sqrMagnitude < 0.01f)
            {
                if (cam == null) cam = Camera.main;
                if (cam != null)
                {
                    Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
                    input = (Vector2)(mouseWorld - transform.position);
                }
            }

            if (input.sqrMagnitude < 0.0001f) return;

            Vector2 direction = input.normalized;
            float speed = entity.stats.dashDistance / dashDuration;
            dashVelocity = direction * speed;

            IsDashing = true;
            dashTimer = dashDuration;
            cooldownTimer = entity.stats.dashCooldown;
        }
    }
}
