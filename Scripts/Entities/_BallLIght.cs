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
    public class _BallLIght : ICollisionActor, IUpdateDrawable
    {
        public IShapeF Bounds { get; set; }
        public float radius;
        public Color color;

        public List<IDeath> players = new();
        InstantiateBallLight instantiateBallLight;

        public _BallLIght(CircleF circle, Color color, List<IDeath> players, InstantiateBallLight instantiateBallLight)
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
                if (ball.color == Color.Red)
                {
                    if (--players[0].Lives <= 0)
                    {
                        players[0].Death();
                    }
                }

                if (ball.color == Color.Blue)
                {
                    if (--players[1].Lives <= 0)
                    {
                        players[1].Death();
                    }
                }

                instantiateBallLight.DestroyBallLight();
            }
        }
    }
}