using UnityEngine;

namespace Assets.Scripts.Utilities
{
    public static class Physics2DExt
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="direction"></param>
        /// <param name="distance"></param>
        /// <param name="includingOrigin">If true, include the origin when detecting collision</param>
        /// <returns></returns>
        public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance, bool includingOrigin)
        {
            var currentDistance = includingOrigin ? 0 : 1;

            for (; currentDistance <= distance; currentDistance++)
            {
                var newOrigin = origin + currentDistance * direction;
                var hit = Physics2D.Raycast(newOrigin, direction, 0);

                if (hit.collider != null) return hit;
            }

            return new RaycastHit2D();
        }

        /// <summary>
        /// Include the origin when detecting collision
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="direction"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance)
        {
            return Raycast(origin, direction, distance, true);
        }

        /// <summary>
        /// Exclude the origin when detecting collision
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="direction"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static RaycastHit2D RaycastExclusive(Vector2 origin, Vector2 direction, float distance)
        {
            return Raycast(origin, direction, distance, false);
        }
    }
}
