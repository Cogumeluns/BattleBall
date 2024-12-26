using BattleBall.Scripts.Constants;
using BattleBall.Scripts.Entities;
using Microsoft.Xna.Framework;

namespace BattleBall.Scripts.Utils
{
    public static class PhysicForce
    {
        public static Vector2 ApplyPushBack(Vector2 currentCollider, _Player playerCollider, float force)
        {
            Vector2 direction = currentCollider - playerCollider.Bounds.Position;

            direction.Normalize();

            float someIntensity = 1;

            if (playerCollider.timeDash != 0)
            {
                someIntensity = 1.5f;
            }

            return force * someIntensity * direction;
        }
    }
}