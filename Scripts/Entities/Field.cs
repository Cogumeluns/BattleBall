
using System;
using System.Collections.Generic;
using System.Linq;
using BattleBall.Scripts.Constants;
using BattleBall.Scripts.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace BattleBall.Scripts.Entities
{
    public class Field : ICollisionActor, IUpdateDrawable
    {
        private const int DISTANCE_TO_BOUND = 5;
        // ICollisionActor
        public IShapeF Bounds { get; set; }
        // IUpdateDrawable -> IBaseDisposable
        public bool isDisposed { get; private set; } = false;
        public Color color;
        public float thickness;

        public Field(RectangleF rectangle, Color color)
        {
            Bounds = rectangle;
            this.color = color;
            thickness = 5;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle((RectangleF)Bounds, color, 5);
        }

        public void Update(GameTime gameTime)
        {
        }

        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            if (collisionInfo.Other is Player player)
            {
                HandlePlayerCollision(player);
            }
            else if (collisionInfo.Other is Ball ball)
            {
                HandleBallCollision(ball);
            }
        }

        void HandlePlayerCollision(Player player)
        {
            float[] distanceToBorder = GetDistanceToBorder(player.Bounds.BoundingRectangle);
            player.isColliderBorderField = IsCollidingWithBorder(distanceToBorder);

            if (IsCollided(player.isColliderBorderField))
            {
                if (player.timePushBackDuration != 0)
                {
                    player.pushBackIntensity = Vector2.Zero;
                    player.timePushBackDuration = 0;
                }

                player.Bounds.Position = AdjustPosition(player.Bounds.Position, player.isColliderBorderField, ((CircleF)player.Bounds).Radius);
                player.velocity = InversePhysics(player.velocity, player.isColliderBorderField);
                player.velocitydash = InversePhysics(player.velocitydash, player.isColliderBorderField);

                UpdatePushBackDuration(player);
            }
        }

        void HandleBallCollision(Ball ball)
        {
            float[] distanceToBorder = GetDistanceToBorder(ball.Bounds.BoundingRectangle);
            bool[] colliderBorder = IsCollidingWithBorder(distanceToBorder);

            if (IsCollided(colliderBorder))
            {
                ball.velocity = InversePhysicsBall(ball.velocity, colliderBorder);
                ball.velocityField = UpdateBallVelocityField(ball.velocity, ball.velocityField, colliderBorder, 300);
                ball.Bounds.Position = AdjustPosition(ball.Bounds.Position, colliderBorder, ((CircleF)ball.Bounds).Radius);
            }
        }

        void UpdatePushBackDuration(Player player)
        {
            if (player.timeDash != 0 || player.timePushBackDuration != 0)
            {
                player.timePushBackDuration = Physics.DEFAULT_TIME_PUSH_BACK_DURATION;
            }
        }

        bool IsCollided(bool[] isCollidingWithBorder)
        {
            return isCollidingWithBorder.Any(collision => collision);
        }

        bool[] IsCollidingWithBorder(float[] distances)
        {
            return new[]
            {
                distances[0] > 0,
                distances[1] < 0,
                distances[2] > 0,
                distances[3] < 0
            };
        }

        float[] GetDistanceToBorder(RectangleF collider)
        {
            RectangleF rectangleField = Bounds.BoundingRectangle;

            return new[]
            {
                rectangleField.Top + DISTANCE_TO_BOUND - collider.Top,
                rectangleField.Bottom - DISTANCE_TO_BOUND - collider.Bottom,
                rectangleField.Left + DISTANCE_TO_BOUND - collider.Left,
                rectangleField.Right - DISTANCE_TO_BOUND - collider.Right
            };
        }

        Vector2 AdjustPosition(Vector2 position, bool[] bordersColliding, float radius)
        {
            RectangleF rectangle = Bounds.BoundingRectangle;
            float distanceBase = radius + thickness + DISTANCE_TO_BOUND;

            if (bordersColliding[0]) position.Y = rectangle.Top + distanceBase;
            else if (bordersColliding[1]) position.Y = rectangle.Bottom - distanceBase;

            if (bordersColliding[2]) position.X = rectangle.Left + distanceBase;
            else if (bordersColliding[3]) position.X = rectangle.Right - distanceBase;

            return position;
        }

        Vector2 InversePhysics(Vector2 velocity, bool[] bordersColliding)
        {
            if (bordersColliding[0] && velocity.Y < 0) velocity.Y = Physics.DEFAULT_PUSH_BACK_INTENSITY_BALL;
            else if (bordersColliding[1] && velocity.Y > 0) velocity.Y = -Physics.DEFAULT_PUSH_BACK_INTENSITY_BALL;

            if (bordersColliding[2] && velocity.X < 0) velocity.X = Physics.DEFAULT_PUSH_BACK_INTENSITY_BALL;
            else if (bordersColliding[3] && velocity.X > 0) velocity.X = -Physics.DEFAULT_PUSH_BACK_INTENSITY_BALL;

            return velocity;
        }

        Vector2 InversePhysicsBall(Vector2 velocity, bool[] bordersColliding)
        {
            if (bordersColliding[0] && velocity.Y < 0) velocity.Y = -velocity.Y;
            else if (bordersColliding[1] && velocity.Y > 0) velocity.Y = -velocity.Y;

            if (bordersColliding[2] && velocity.X < 0) velocity.X = -velocity.X;
            else if (bordersColliding[3] && velocity.X > 0) velocity.X = -velocity.X;

            return velocity;
        }

        Vector2 UpdateBallVelocityField(Vector2 velocity, Vector2 velocityField, bool[] bordersColliding, float value)
        {
            if (bordersColliding[0] && velocity.Y > 0) velocityField.Y = value;
            else if (bordersColliding[1] && velocity.Y < 0) velocityField.Y = -value;

            if (bordersColliding[2] && velocity.X > 0) velocityField.X = value;
            else if (bordersColliding[3] && velocity.X < 0) velocityField.X = -value;

            return velocityField;
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

