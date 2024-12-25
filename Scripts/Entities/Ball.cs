using System;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace BattleBall
{
    public class Ball : ICollisionActor
    {
        private const int SIDES = 256;
        private const float REPULSE_DECAY = 0.5f;
        private const int DIRECTIONS = 4;
        private Color color;
        public IShapeF Bounds { get; set; }
        private float radius;

        private Vector2 repulseForce = Vector2.Zero;

        float distance = 0;

        bool isColliderPlayer = false;

        private Field field;
        private IShapeF playerBounds;
        private Vector2 initial;

        public Ball(CircleF format, Color color)
        {
            Bounds = format;
            initial = format.Position;
            radius = format.Radius;
            this.color = color;
        }

        public void SetFieldBounds(Field field)
        {
            this.field = field;
        }
        public void Update(GameTime gameTime)
        {
            if (repulseForce.Length() < 20)
            {
                color = Color.White;
            }

            Vector2 current = Bounds.Position;

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            current += repulseForce * deltaTime;

            if (repulseForce.Length() > 0)
            {
                repulseForce -= repulseForce * REPULSE_DECAY * deltaTime;
                if (repulseForce.Length() < 0.1f) // Força quase zero
                    repulseForce = Vector2.Zero;
            }

            if (isColliderPlayer)
                current = OnValidatePositionWhithPlayer(current, deltaTime);
            current = OnValidatePositionWhithField(current, deltaTime);

            Bounds.Position = current;
        }

        public Vector2 OnValidatePositionWhithField(Vector2 current, float deltaTime)
        {
            bool[] isBorderField = new bool[DIRECTIONS];
            isBorderField[0] = Math.Abs(current.Y - field.Bounds.BoundingRectangle.Top) < radius + distance;     // Topo
            isBorderField[1] = Math.Abs(current.Y - field.Bounds.BoundingRectangle.Bottom) < radius + distance;  // Base
            isBorderField[2] = Math.Abs(current.X - field.Bounds.BoundingRectangle.Left) < radius + distance;    // Esquerda
            isBorderField[3] = Math.Abs(current.X - field.Bounds.BoundingRectangle.Right) < radius + distance;   // Direita


            if (isBorderField[0])
            {
                repulseForce = new Vector2(repulseForce.X, -repulseForce.Y);
                current.Y = field.Bounds.BoundingRectangle.Top + radius + distance;
            }
            else if (isBorderField[1])
            {
                repulseForce = new Vector2(repulseForce.X, -repulseForce.Y);
                current.Y = field.Bounds.BoundingRectangle.Bottom - radius - distance;
            }

            if (isBorderField[2])
            {
                repulseForce = new Vector2(-repulseForce.X, repulseForce.Y);
                current.X = field.Bounds.BoundingRectangle.Left + radius + distance;
            }
            else if (isBorderField[3])
            {
                repulseForce = new Vector2(-repulseForce.X, repulseForce.Y);
                current.X = field.Bounds.BoundingRectangle.Right - radius - distance;
            }


            if (current.X < field.Bounds.BoundingRectangle.Left - radius ||
                current.X > field.Bounds.BoundingRectangle.Right + radius ||
                current.Y < field.Bounds.BoundingRectangle.Top - radius ||
                current.Y > field.Bounds.BoundingRectangle.Bottom + radius)
            {
                current = initial;
                repulseForce = Vector2.Zero;
            }

            return current;
        }

        public Vector2 OnValidatePositionWhithPlayer(Vector2 current, float deltaTime)
        {
            bool[] isBorderField = new bool[4];
            isBorderField[0] = Math.Abs(current.Y - playerBounds.BoundingRectangle.Top) < radius + distance;     // Topo
            isBorderField[1] = Math.Abs(current.Y - playerBounds.BoundingRectangle.Bottom) < radius + distance;  // Base
            isBorderField[2] = Math.Abs(current.X - playerBounds.BoundingRectangle.Left) < radius + distance;    // Esquerda
            isBorderField[3] = Math.Abs(current.X - playerBounds.BoundingRectangle.Right) < radius + distance;   // Direita

            if (isBorderField[0])
            {
                repulseForce = new Vector2(repulseForce.X, -Math.Abs(repulseForce.Y)); // Reflete no eixo Y
                current.Y = playerBounds.BoundingRectangle.Top - radius - distance;   // Posiciona fora do jogador
            }
            else if (isBorderField[1])
            {
                repulseForce = new Vector2(repulseForce.X, Math.Abs(repulseForce.Y)); // Reflete no eixo Y
                current.Y = playerBounds.BoundingRectangle.Bottom + radius + distance; // Posiciona fora do jogador
            }

            if (isBorderField[2])
            {
                repulseForce = new Vector2(-Math.Abs(repulseForce.X), repulseForce.Y); // Reflete no eixo X
                current.X = playerBounds.BoundingRectangle.Left - radius - distance;   // Posiciona fora do jogador
            }
            else if (isBorderField[3])
            {
                repulseForce = new Vector2(Math.Abs(repulseForce.X), repulseForce.Y); // Reflete no eixo X
                current.X = playerBounds.BoundingRectangle.Right + radius + distance; // Posiciona fora do jogador
            }

            isColliderPlayer = false;
            return current;
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.DrawCircle((CircleF)Bounds, SIDES, color, 2);
        }

        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            if (collisionInfo.Other is Player player && !isColliderPlayer)
            {
                Vector2 direction = Bounds.Position - player.Bounds.Position;

                float pushBackIntensity = 0;

                if (player.dash.Length() >= 0)
                {
                    pushBackIntensity = 1000f;
                }

                // Normaliza a direção para obter apenas a direção unitária
                if (direction != Vector2.Zero)
                    direction.Normalize();

                // Define a intensidade inicial da força de repulsão
                pushBackIntensity += 600f;

                // Calcula a nova força resultante
                Vector2 newForce = direction * pushBackIntensity;

                // Ajusta `repulseForce` para reaproveitar a força existente:
                Vector2 preservedForce = Vector2.Dot(repulseForce, direction) * -direction; // Força projetada na direção do impacto
                repulseForce = preservedForce + newForce;

                repulseForce = Vector2.Clamp(repulseForce,
                                             new Vector2(-2000f, -2000f),
                                             new Vector2(2000f, 2000f));
                color = player.color;
                playerBounds = player.Bounds;
                isColliderPlayer = true;
            }
        }
    }
}