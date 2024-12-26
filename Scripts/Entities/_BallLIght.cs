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
    public class _BallLIght : ICollisionActor, IUpdateDrawable
    {
        public IShapeF Bounds { get; set; }
        public bool isDisposed { get; private set; }
        public float radius;
        public Color color;
        public List<_Player> players = new();
        InstantiateBallLight instantiateBallLight;
        int damage = 1;

        public _BallLIght(CircleF circle, Color color, List<_Player> players, InstantiateBallLight instantiateBallLight)
        {
            Bounds = circle;
            radius = circle.Radius;
            this.color = color;
            this.players = players;
            this.instantiateBallLight = instantiateBallLight;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawCircle((CircleF)Bounds, Physics.SIDES, color, 3);
        }
        public void Update(GameTime gameTime)
        {
        }

        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            if (collisionInfo.Other is _Ball ball)
            {
                instantiateBallLight.DestroyBallLight();
                if (ball.color == Color.Blue && damage == 1)
                {
                    damage = 0;
                    if (--players[1].Lives <= 0)
                    {
                        ((IBaseDisposable)players[1]).Dispose();
                    }
                }
                else if (ball.color == Color.Red && damage == 1)
                {
                    damage = 0;
                    if (--players[0].Lives <= 0)
                    {
                        ((IBaseDisposable)players[0]).Dispose();
                    }
                }
            }
        }

        public void Dispose()
        {
            isDisposed = true;
        }
    }
}