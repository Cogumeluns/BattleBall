
using System;
using System.Collections.Generic;
using BattleBall.Scripts.Constantes;
using BattleBall.Scripts.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace BattleBall.Scripts.Entities
{
    public class _Field : ICollisionActor, IUpdateDrawable
    {
        private int distance = 5;
        public IShapeF Bounds { get; set; }
        public bool isDisposed { get; private set; } = false;
        public Color color;
        public float thickness;
        bool isColliderPlayer;

        public _Field(RectangleF rectangle, Color color)
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
            if (collisionInfo.Other is _Player player)
            {
                float distanceToTop = Bounds.BoundingRectangle.Top - player.Bounds.BoundingRectangle.Top;
                float distanceToBottom = Bounds.BoundingRectangle.Bottom - player.Bounds.BoundingRectangle.Bottom;
                float distanceToLeft = Bounds.BoundingRectangle.Left - player.Bounds.BoundingRectangle.Left;
                float distanceToRight = Bounds.BoundingRectangle.Right - player.Bounds.BoundingRectangle.Right;

                player.isborder[0] = distanceToTop > 0;
                player.isborder[1] = distanceToBottom < 0;
                player.isborder[2] = distanceToLeft > 0;
                player.isborder[3] = distanceToRight < 0;

                isColliderPlayer = player.isborder[0] || player.isborder[1] || player.isborder[2] || player.isborder[3];

                if (isColliderPlayer)
                {
                    AdjustePosition(player);
                    InversePhysics(player);
                }
            }

            if (collisionInfo.Other is _Ball ball)
            {
                float distanceToTop = Bounds.BoundingRectangle.Top - ball.Bounds.BoundingRectangle.Top;
                float distanceToBottom = Bounds.BoundingRectangle.Bottom - ball.Bounds.BoundingRectangle.Bottom;
                float distanceToLeft = Bounds.BoundingRectangle.Left - ball.Bounds.BoundingRectangle.Left;
                float distanceToRight = Bounds.BoundingRectangle.Right - ball.Bounds.BoundingRectangle.Right;

                bool isTop = distanceToTop > 0;
                bool isBottom = distanceToBottom < 0;
                bool isLeft = distanceToLeft > 0;
                bool isRight = distanceToRight < 0;



                if (isTop && ball.velocity.Y < 0)
                {
                    ball.velocity.Y = -ball.velocity.Y;
                    ball.velocityField.Y = Physics.DEFAULT_VELOCITY;
                }
                else if (isBottom && ball.velocity.Y > 0)
                {
                    ball.velocity.Y = -ball.velocity.Y;
                    ball.velocityField.Y = -Physics.DEFAULT_VELOCITY;
                }

                if (isLeft && ball.velocity.X < 0)
                {
                    ball.velocity.X = -ball.velocity.X;
                    ball.velocityField.X = Physics.DEFAULT_VELOCITY;
                }
                else if (isRight && ball.velocity.X > 0)
                {
                    ball.velocity.X = -ball.velocity.X;
                    ball.velocityField.X = -Physics.DEFAULT_VELOCITY;
                }

                if (isTop)
                {
                    ball.Bounds.Position = new Vector2(ball.Bounds.Position.X, Bounds.BoundingRectangle.Top + ball.radius + thickness + distance);
                }
                else if (isBottom)
                {
                    ball.Bounds.Position = new Vector2(ball.Bounds.Position.X, Bounds.BoundingRectangle.Bottom - ball.radius - thickness - distance);
                }

                if (isLeft)
                {
                    ball.Bounds.Position = new Vector2(Bounds.BoundingRectangle.Left + ball.radius + thickness + distance, ball.Bounds.Position.Y);
                }
                else if (isRight)
                {
                    ball.Bounds.Position = new Vector2(Bounds.BoundingRectangle.Right - ball.radius - thickness - distance, ball.Bounds.Position.Y);
                }
            }
        }

        void AdjustePosition(_Player player)
        {
            if (player.isborder[0])
            {
                player.Bounds.Position = new Vector2(player.Bounds.Position.X, Bounds.BoundingRectangle.Top + player.radius + thickness);
            }
            else if (player.isborder[1])
            {
                player.Bounds.Position = new Vector2(player.Bounds.Position.X, Bounds.BoundingRectangle.Bottom - player.radius - thickness);
            }

            if (player.isborder[2])
            {
                player.Bounds.Position = new Vector2(Bounds.BoundingRectangle.Left + player.radius + thickness, player.Bounds.Position.Y);
            }
            else if (player.isborder[3])
            {
                player.Bounds.Position = new Vector2(Bounds.BoundingRectangle.Right - player.radius - thickness, player.Bounds.Position.Y);
            }
        }

        void InversePhysics(_Player player)
        {
            if (player.isborder[0])
            {
                if (player.velocitydash.Y < 0)
                {
                    player.velocitydash.Y = Physics.DEFAULT_PUSH_BACK_INTENSITY;
                    player.timeDash = Physics.DEFAULT_TIME_PUSH_BACK_DURATION;
                }
                if (player.pushBackIntensity.Y < 0)
                {
                    player.pushBackIntensity.Y = Physics.DEFAULT_PUSH_BACK_INTENSITY;
                    player.timePushBackDuration = Physics.DEFAULT_TIME_PUSH_BACK_DURATION;
                }
            }
            else if (player.isborder[1])
            {
                if (player.velocitydash.Y > 0)
                {
                    player.velocitydash.Y = -Physics.DEFAULT_PUSH_BACK_INTENSITY;
                    player.timeDash = Physics.DEFAULT_TIME_PUSH_BACK_DURATION;
                }
                if (player.pushBackIntensity.Y > 0)
                {
                    player.pushBackIntensity.Y = -Physics.DEFAULT_PUSH_BACK_INTENSITY;
                    player.timePushBackDuration = Physics.DEFAULT_TIME_PUSH_BACK_DURATION;
                }
            }
            if (player.isborder[2])
            {
                if (player.velocitydash.X < 0)
                {
                    player.velocitydash.X = Physics.DEFAULT_PUSH_BACK_INTENSITY;
                    player.timeDash = Physics.DEFAULT_TIME_PUSH_BACK_DURATION;
                }
                if (player.pushBackIntensity.X < 0)
                {
                    player.pushBackIntensity.X = Physics.DEFAULT_PUSH_BACK_INTENSITY;
                    player.timePushBackDuration = Physics.DEFAULT_TIME_PUSH_BACK_DURATION;
                }
            }
            else if (player.isborder[3])
            {
                if (player.velocitydash.X > 0)
                {
                    player.velocitydash.X = -Physics.DEFAULT_PUSH_BACK_INTENSITY;
                    player.timeDash = Physics.DEFAULT_TIME_PUSH_BACK_DURATION;
                }
                if (player.pushBackIntensity.X > 0)
                {
                    player.pushBackIntensity.X = -Physics.DEFAULT_PUSH_BACK_INTENSITY;
                    player.timePushBackDuration = Physics.DEFAULT_TIME_PUSH_BACK_DURATION;
                }
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

