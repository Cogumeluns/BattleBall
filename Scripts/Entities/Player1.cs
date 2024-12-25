using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace BattleBall
{
    public class Player : ICollisionActor
    {
        private const int SIDES = 256;
        private const int VELOCITY = 1000;
        private const int MAX_KEYS = 5;
        private const int DIRECTIONS = 4;
        private const float REPULSE_DECAY = 3f;
        public IShapeF Bounds { get; set; }
        private float radius;

        public Color color;
        private Keys[] keys = new Keys[MAX_KEYS];
        private bool[] moveDirection = new bool[DIRECTIONS];
        private bool isColliderBall = false;
        private Vector2 repulseForce = Vector2.Zero;
        public Vector2 dash = Vector2.Zero;
        private Field field;
        private Ball ball;

        float distance = 0;

        string name;

        public void SetFieldBounds(Field field, Ball ball)
        {
            this.field = field;
            this.ball = ball;
        }

        public Player() { }

        public Player(CircleF format, Color color, string name)
        {
            this.color = color;
            this.radius = format.Radius;
            Bounds = format;
            this.name = name;
        }

        public void SetKeys(Keys up, Keys down, Keys left, Keys right, Keys dash)
        {
            keys = [up, down, left, right, dash];
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            for (int i = 0; i < DIRECTIONS; i++)
            {
                moveDirection[i] = keyboardState.IsKeyDown(keys[i]);
            }

            if (keyboardState.IsKeyDown(keys[4]))
            {
                if (moveDirection[0])
                    dash.Y = -50;
                else if (moveDirection[1])
                    dash.Y = 50;

                if (moveDirection[2])
                    dash.X = -50;
                else if (moveDirection[3])
                    dash.X = 50;
            }

            Move(gameTime);
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.DrawCircle((CircleF)Bounds, SIDES, color, radius);
        }

        private void Move(GameTime gameTime)
        {
            Vector2 current = Bounds.Position;

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            float velocity = VELOCITY * deltaTime;
            Vector2 moveInput = Vector2.Zero;

            if (moveDirection[0]) moveInput.Y -= velocity;
            if (moveDirection[1]) moveInput.Y += velocity;
            if (moveDirection[2]) moveInput.X -= velocity;
            if (moveDirection[3]) moveInput.X += velocity;

            if (repulseForce.Length() > 1)
            {
                dash = Vector2.Zero;
            }

            current += (moveInput + repulseForce + dash) * 30 * deltaTime;

            if (repulseForce.Length() > 0)
            {
                repulseForce -= repulseForce * REPULSE_DECAY * deltaTime;
                if (repulseForce.Length() < 0.1f)
                    repulseForce = Vector2.Zero;
            }

            current = OnValidatePositionWhithField(current, deltaTime);
            if (isColliderBall)
                current = OnValidatePositionWhithBall(current, deltaTime);

            Bounds.Position = current;
            dash = Vector2.Zero;
        }

        public Vector2 OnValidatePositionWhithField(Vector2 current, float deltaTime)
        {
            bool[] isBorderField = new bool[4];
            isBorderField[0] = Math.Abs(current.Y - field.Bounds.BoundingRectangle.Top) < radius + distance;     // Topo
            isBorderField[1] = Math.Abs(current.Y - field.Bounds.BoundingRectangle.Bottom) < radius + distance;  // Base
            isBorderField[2] = Math.Abs(current.X - field.Bounds.BoundingRectangle.Left) < radius + distance;    // Esquerda
            isBorderField[3] = Math.Abs(current.X - field.Bounds.BoundingRectangle.Right) < radius + distance;   // Direita

            if (isBorderField[0])
            {
                repulseForce -= repulseForce / 2;
                current.Y = field.Bounds.BoundingRectangle.Top + radius + distance;
            }
            else if (isBorderField[1])
            {
                repulseForce -= repulseForce / 2;
                current.Y = field.Bounds.BoundingRectangle.Bottom - radius - distance; // Posiciona fora do jogador
            }
            if (isBorderField[2])
            {
                repulseForce -= repulseForce / 2;
                current.X = field.Bounds.BoundingRectangle.Left + radius + distance;   // Posiciona fora do jogador
            }
            else if (isBorderField[3])
            {
                repulseForce -= repulseForce / 2;
                current.X = field.Bounds.BoundingRectangle.Right - radius - distance; // Posiciona fora do jogador
            }

            return current;
        }

        public Vector2 OnValidatePositionWhithBall(Vector2 current, float deltaTime)
        {
            bool[] isBorderField = new bool[4];
            isBorderField[0] = Math.Abs(current.Y - ball.Bounds.BoundingRectangle.Top) < radius + distance;     // Topo
            isBorderField[1] = Math.Abs(current.Y - ball.Bounds.BoundingRectangle.Bottom) < radius + distance;  // Base
            isBorderField[2] = Math.Abs(current.X - ball.Bounds.BoundingRectangle.Left) < radius + distance;    // Esquerda
            isBorderField[3] = Math.Abs(current.X - ball.Bounds.BoundingRectangle.Right) < radius + distance;   // Direita

            if (isBorderField[0])
            {
                current.Y = ball.Bounds.BoundingRectangle.Top + radius + distance;
            }
            else if (isBorderField[1])
            {
                current.Y = ball.Bounds.BoundingRectangle.Bottom - radius - distance; // Posiciona fora do jogador
            }
            if (isBorderField[2])
            {
                current.X = ball.Bounds.BoundingRectangle.Left + radius + distance;   // Posiciona fora do jogador
            }
            else if (isBorderField[3])
            {
                current.X = ball.Bounds.BoundingRectangle.Right - radius - distance; // Posiciona fora do jogador
            }

            isColliderBall = false;
            return current;
        }

        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            if (collisionInfo.Other is Player enemy)
            {
                Vector2 direction = Bounds.Position - enemy.Bounds.Position;

                if (direction != Vector2.Zero)
                    direction.Normalize();

                float pushBackIntensity = 0;

                if (enemy.dash.Length() > 0)
                {
                    pushBackIntensity = 40f;
                }

                pushBackIntensity += 40f;
                repulseForce = direction * pushBackIntensity;

                System.Console.WriteLine($"Colisão entre {name} e {enemy.name}");
            }
            else if (collisionInfo.Other is Ball ball)
            {
                isColliderBall = true;
            }
            else
            {
                System.Console.WriteLine("Colisão com outro objeto");
            }

        }
    }
}