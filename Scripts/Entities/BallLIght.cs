using System.Collections.Generic;
using BattleBall.Scripts.Constants;
using BattleBall.Scripts.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace BattleBall.Scripts.Entities
{
    public class BallLight : ICollisionActor, IUpdateDrawable
    {
        public IShapeF Bounds { get; set; }
        public bool isDisposed { get; private set; }
        public float radius;
        public Color color;
        public List<Player> players = new();
        ControllerBallLight controllerBallLight;
        int damage = 1;

        public BallLight(CircleF circle, Color color, List<Player> players, ControllerBallLight instantiateBallLight)
        {
            Bounds = circle;
            radius = circle.Radius;
            this.color = color;
            this.players = players;
            this.controllerBallLight = instantiateBallLight;
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
            if (collisionInfo.Other is Ball ball)
            {
                HandleBallCollision(ball);
            }

            if (collisionInfo.Other is Player player)
            {
                AdjustPosition(player);
            }
        }

        void AdjustPosition(Player player)
        {
            Vector2 overlapDirection = player.Bounds.Position - Bounds.Position;

            if (overlapDirection != Vector2.Zero)
            {
                overlapDirection.Normalize();
            }

            float overlapDistance = CalculateOverlapDistance((CircleF)Bounds, (CircleF)player.Bounds);

            player.Bounds.Position += overlapDirection * overlapDistance;
        }

        float CalculateOverlapDistance(CircleF bounds1, CircleF bounds2)
        {
            float distanceBetweenCenters = Vector2.Distance(bounds1.Position, bounds2.Position);

            float combinedRadius = bounds1.Radius + bounds2.Radius;

            return combinedRadius - distanceBetweenCenters;
        }

        private void HandleBallCollision(Ball ball)
        {
            controllerBallLight.DestroyBallLight();

            if (damage == 1)
            {
                damage = 0; // Previne múltiplas reduções de vida

                if (ball.color == Color.Blue)
                {
                    HandlePlayerDamage(players[1]);
                }
                else if (ball.color == Color.Red)
                {
                    HandlePlayerDamage(players[0]);
                }
            }
        }

        private void HandlePlayerDamage(Player player)
        {
            if (player == null) return;

            player.Damage(1);
        }

        public void Dispose()
        {
            if (isDisposed) return;
            isDisposed = true;
        }
    }
}