using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace BattleBall
{
    public class Field : ICollisionActor
    {
        public const int thickness = 5;

        public IShapeF Bounds { get; set; }

        private Color color;

        public Field(RectangleF format, Color color)
        {
            Bounds = format;
            this.color = color;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle((RectangleF)Bounds, color, 5);
        }

        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            // do nothing
        }
    }
}