using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
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
        public IShapeF Bounds { get; set; }
        public Color color;

        bool isColliderPlayer;

        public _Field(RectangleF rectangle, Color color)
        {
            Bounds = rectangle;
            this.color = color;
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
            }
        }
    }
}