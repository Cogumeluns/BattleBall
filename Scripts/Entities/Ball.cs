using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleBall.Scripts.Constants;
using BattleBall.Scripts.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace BattleBall.Scripts.Entities
{
    public class Ball : ICollisionActor, IUpdateDrawable
    {
        const float DEFAULT_DECRESS_VELOCITY_BALL = 0.4f;
        const float RADIUS = 3;
        // ICollisionActor
        public IShapeF Bounds { get; set; }
        // IUpdateDrawable -> IBaseDisposable
        public bool isDisposed { get; private set; } = false;
        public Color color;
        public Vector2 velocity = Vector2.Zero;
        public Vector2 velocityField = Vector2.Zero;
        public bool isVisible { get; set; } = true;

        public Ball(CircleF circle, Color color)
        {
            Bounds = circle;
            this.color = color;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawCircle((CircleF)Bounds, Physics.SIDES, color, RADIUS);
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            UpdateMovement(deltaTime);
            velocity = DecreaseVelocity(deltaTime, velocity, false);
            velocityField = DecreaseVelocity(deltaTime, velocityField, true);
        }

        void UpdateMovement(float deltaTime)
        {
            Vector2 totalVelocity = velocity + velocityField;
            UpdatePosition(deltaTime, totalVelocity);
        }

        void UpdatePosition(float deltaTime, Vector2 totalVelocity)
        {
            Bounds.Position += totalVelocity * deltaTime;
        }

        Vector2 DecreaseVelocity(float deltaTime, Vector2 velocity, bool isfield)
        {
            if (velocity == Vector2.Zero)
                return Vector2.Zero;

            velocity = CalculateDecreasedVelocity(velocity, deltaTime);

            if (velocity.Length() < 20f)
            {
                ResetVelocity();
            }
            return velocity;
        }

        Vector2 CalculateDecreasedVelocity(Vector2 velocity, float deltaTime)
        {
            float decreaseFactor = DEFAULT_DECRESS_VELOCITY_BALL * deltaTime;

            float newX = Math.Max(0, Math.Abs(velocity.X) - Math.Abs(velocity.X) * decreaseFactor);
            float newY = Math.Max(0, Math.Abs(velocity.Y) - Math.Abs(velocity.Y) * decreaseFactor);

            Vector2 normalizedDirection = GetNormalizedDirection(velocity);

            return new Vector2(newX * normalizedDirection.X, newY * normalizedDirection.Y);
        }

        Vector2 GetNormalizedDirection(Vector2 vector)
        {
            return new Vector2(Math.Sign(vector.X), Math.Sign(vector.Y));
        }

        void ResetVelocity()
        {
            velocity = Vector2.Zero;
            velocityField = Vector2.Zero;
            color = Color.White;
        }


        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            if (collisionInfo.Other is Player player)
            {
                velocity = ApplyPushBack(Bounds.Position, player, Physics.DEFAULT_PUSH_BACK_INTENSITY_BALL);
                color = player.color;
            }
        }

        public static Vector2 ApplyPushBack(Vector2 currentCollider, Player playerCollider, float force)
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

        public void Dispose()
        {
            isDisposed = true;
        }
    }
}