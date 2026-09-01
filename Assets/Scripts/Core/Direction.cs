using UnityEngine;

namespace RogueRealms
{
    public enum Direction
    {
        North,
        East,
        South,
        West
    }

    public static class DirectionUtility
    {
        public static Direction FromVector(Vector2 v, Direction fallback = Direction.South)
        {
            if (v.sqrMagnitude < 0.0001f)
                return fallback;

            float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;

            if (angle > -45f && angle <= 45f) return Direction.East;
            if (angle > 45f && angle <= 135f) return Direction.North;
            if (angle > 135f || angle <= -135f) return Direction.West;
            return Direction.South;
        }
    }
}
