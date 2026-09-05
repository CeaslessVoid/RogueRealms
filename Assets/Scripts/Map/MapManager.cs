using UnityEngine;

namespace RogueRealms
{
    [RequireComponent(typeof(LineRenderer))]
    public class MapManager : MonoBehaviour
    {
        public static MapManager Instance { get; private set; }

        public MapDef mapDef;
        public Vector2 origin = Vector2.zero;
        public Color borderColor = Color.red;
        public float lineWidth = 0.15f;

        LineRenderer line;

        public Vector2 Size => mapDef != null ? new Vector2(mapDef.width * mapDef.tileSize, mapDef.height * mapDef.tileSize) : Vector2.zero;

        public Bounds GetBounds() => new Bounds(origin, Size);

        public bool Contains(Vector2 worldPos) => GetBounds().Contains(worldPos);

        public static Vector2 Clamp(Vector2 pos)
        {
            if (Instance == null) return pos;

            Bounds b = Instance.GetBounds();
            pos.x = Mathf.Clamp(pos.x, b.min.x, b.max.x);
            pos.y = Mathf.Clamp(pos.y, b.min.y, b.max.y);
            return pos;
        }

        void Awake()
        {
            Instance = this;
            line = GetComponent<LineRenderer>();
            DrawBorder();
        }

        void DrawBorder()
        {
            if (mapDef == null) return;

            float halfW = Size.x * 0.5f;
            float halfH = Size.y * 0.5f;

            Vector3[] corners =
            {
                new Vector3(origin.x - halfW, origin.y - halfH, 0f),
                new Vector3(origin.x - halfW, origin.y + halfH, 0f),
                new Vector3(origin.x + halfW, origin.y + halfH, 0f),
                new Vector3(origin.x + halfW, origin.y - halfH, 0f)
            };

            line.useWorldSpace = true;
            line.loop = true;
            line.positionCount = corners.Length;
            line.SetPositions(corners);
            line.startWidth = lineWidth;
            line.endWidth = lineWidth;
            line.startColor = borderColor;
            line.endColor = borderColor;
            line.material = new Material(Shader.Find("Sprites/Default"));
        }

        void OnDrawGizmos()
        {
            if (mapDef == null) return;
            Gizmos.color = borderColor;
            Gizmos.DrawWireCube(origin, Size);
        }
    }
}
