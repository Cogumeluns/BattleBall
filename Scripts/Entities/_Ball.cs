using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleBall.Scripts.Constantes;
using BattleBall.Scripts.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace BattleBall.Scripts.Entities
{
    public class _Ball : ICollisionActor, IUpdateDrawable
    {
        const float DEFAULT_DECRESS_VELOCITY_BALL = 0.4f;
        public IShapeF Bounds { get; set; }
        public bool isDisposed { get; private set; } = false;
        public float radius;
        public Color color;

        public Vector2 velocity = Vector2.Zero;
        public Vector2 velocityField = Vector2.Zero;

        public _Ball(CircleF circle, Color color)
        {
            Bounds = circle;
            this.color = color;
            radius = circle.Radius;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawCircle((CircleF)Bounds, Physics.SIDES, color, 3);
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 current = Bounds.Position;

            current += (velocity + velocityField) * deltaTime;

            Bounds.Position = current;

            DecressVelocity(deltaTime);
            DecressVelocityField(deltaTime);

        }
        void DecressVelocityField(float deltaTime)
        {
            if (velocityField != Vector2.Zero)
            {
                float x = Math.Max(0, Math.Abs(velocityField.X) - Math.Abs(velocityField.X) * DEFAULT_DECRESS_VELOCITY_BALL * deltaTime);
                float y = Math.Max(0, Math.Abs(velocityField.Y) - Math.Abs(velocityField.Y) * DEFAULT_DECRESS_VELOCITY_BALL * deltaTime);

                Vector2 normalizedDirection = Vector2.Normalize(velocityField);

                normalizedDirection.X = Math.Sign(normalizedDirection.X);
                normalizedDirection.Y = Math.Sign(normalizedDirection.Y);

                velocityField = new Vector2(x * normalizedDirection.X, y * normalizedDirection.Y);

                if (velocityField.Length() < 0.01f) velocityField = Vector2.Zero;
            }
        }

        void DecressVelocity(float deltaTime)
        {
            if (velocity != Vector2.Zero)
            {
                float x = Math.Max(0, Math.Abs(velocity.X) - Math.Abs(velocity.X) * DEFAULT_DECRESS_VELOCITY_BALL * deltaTime);
                float y = Math.Max(0, Math.Abs(velocity.Y) - Math.Abs(velocity.Y) * DEFAULT_DECRESS_VELOCITY_BALL * deltaTime);

                Vector2 normalizedDirection = Vector2.Normalize(velocity);

                normalizedDirection.X = Math.Sign(normalizedDirection.X);
                normalizedDirection.Y = Math.Sign(normalizedDirection.Y);

                velocity = new Vector2(x * normalizedDirection.X, y * normalizedDirection.Y);

                if (velocity.Length() < 0.01f)
                {
                    velocity = Vector2.Zero;
                    color = Color.White;
                }
            }
        }

        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            if (collisionInfo.Other is _Player player)
            {
                Vector2 direction = Bounds.Position - player.Bounds.Position;

                direction.Normalize();

                float someIntensity = 1;

                if (player.timeDash != 0)
                {
                    someIntensity = 1.5f;
                }

                velocity = Physics.DEFAULT_PUSH_BACK_INTENSITY * someIntensity * direction;
                color = player.color;
            }
        }

        public void Dispose()
        {
            isDisposed = true;
        }
    }
}