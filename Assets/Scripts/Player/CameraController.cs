using UnityEngine;

namespace RogueRealms
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        public Transform target;
        public float followSpeed = 8f;
        public float leanAmount = 1.5f;
        public float zoomSpeed = 4f;
        public float minZoom = 3f;
        public float maxZoom = 10f;

        Camera cam;

        void Awake()
        {
            cam = GetComponent<Camera>();
        }

        void Update()
        {
            float scroll = Input.mouseScrollDelta.y;
            if (Mathf.Abs(scroll) > 0.001f)
            {
                float size = cam.orthographicSize - scroll * zoomSpeed;
                cam.orthographicSize = Mathf.Clamp(size, minZoom, maxZoom);
            }
        }

        void LateUpdate()
        {
            if (target == null) return;

            Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 offset = Vector2.ClampMagnitude((Vector2)mouseWorld - (Vector2)target.position, 1f);

            Vector3 desired = target.position + (Vector3)(offset * leanAmount);
            desired.z = transform.position.z;

            transform.position = Vector3.Lerp(transform.position, desired, followSpeed * Time.deltaTime);
        }
    }
}
