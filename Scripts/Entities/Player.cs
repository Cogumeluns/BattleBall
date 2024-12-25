using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleBall.Scripts.Constantes;
using BattleBall.Scripts.Enum;
using BattleBall.Scripts.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace BattleBall.Scripts.Entities
{
    public class _Player : ICollisionActor, IUpdateDrawable
    {
        const int sides = 512;
        public IShapeF Bounds { get; set; }
        public float radius;
        private Color color;
        private Dictionary<PlayerKeys, Keys> playerKeys = new();
        public Vector2 velocity = Vector2.Zero;
        public Vector2 velocitydash = Vector2.Zero;
        public Vector2 pushBackIntensity = Vector2.Zero;

        public bool[] isborder = new bool[4];

        public float timeDash = 0;
        public float timePushBackDuration = 0;

        public _Player(CircleF circle, Color color)
        {
            Bounds = circle;
            radius = circle.Radius;
            this.color = color;
        }

        public void SetKeys(Keys up, Keys down, Keys left, Keys right, Keys dash)
        {
            playerKeys.Add(PlayerKeys.Up, up);
            playerKeys.Add(PlayerKeys.Down, down);
            playerKeys.Add(PlayerKeys.Left, left);
            playerKeys.Add(PlayerKeys.Right, right);
            playerKeys.Add(PlayerKeys.Dash, dash);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawCircle((CircleF)Bounds, sides, color, radius);
        }

        public void Update(GameTime gameTime)
        {
            TimeDurationDash(gameTime);
            TimeDurationPushBack(gameTime);
            KeysPress();
            Moviment(gameTime);

        }

        void TimeDurationPushBack(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timePushBackDuration > 0)
            {
                timePushBackDuration -= deltaTime;

                if (timePushBackDuration <= 0)
                {
                    timePushBackDuration = 0;
                    pushBackIntensity = Vector2.Zero;
                }
            }
        }

        void TimeDurationDash(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timeDash > 0)
            {
                timeDash -= deltaTime;

                if (timeDash <= 0)
                {
                    timeDash = 0;
                    velocitydash = Vector2.Zero;
                }
            }
        }

        void Moviment(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 current = Bounds.Position;

            current.X += (velocity.X + velocitydash.X + pushBackIntensity.X) * deltaTime;
            current.Y += (velocity.Y + velocitydash.Y + pushBackIntensity.Y) * deltaTime;

            Bounds.Position = current;
        }

        void KeysPress()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(playerKeys[PlayerKeys.Up]) && !isborder[0])
            {
                velocity.Y = -Physics.DEFAULT_VELOCITY;
            }
            else if (keyboardState.IsKeyDown(playerKeys[PlayerKeys.Down]) && !isborder[1])
            {
                velocity.Y = Physics.DEFAULT_VELOCITY;
            }
            else
            {
                velocity.Y = 0;
                isborder[0] = false;
                isborder[1] = false;
            }

            if (keyboardState.IsKeyDown(playerKeys[PlayerKeys.Left]) && !isborder[2])
            {
                velocity.X = -Physics.DEFAULT_VELOCITY;
            }
            else if (keyboardState.IsKeyDown(playerKeys[PlayerKeys.Right]) && !isborder[3])
            {
                velocity.X = Physics.DEFAULT_VELOCITY;
            }
            else
            {
                velocity.X = 0;
                isborder[2] = false;
                isborder[3] = false;
            }

            if (keyboardState.IsKeyDown(playerKeys[PlayerKeys.Dash]))
            {
                if (velocitydash.Length() == 0)
                {
                    if (velocity != Vector2.Zero)
                    {
                        Vector2 normalized = Vector2.Normalize(velocity);
                        velocitydash = normalized * Physics.DEFAULT_VELOCITY_DASH;
                        timeDash = Physics.DEFAULT_TIME_DASH_DURATION;
                    }
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

                pushBackIntensity = Physics.DEFAULT_PUSH_BACK_INTENSITY * someIntensity * direction;
                timePushBackDuration = Physics.DEFAULT_TIME_PUSH_BACK_DURATION;
            }
        }
    }
}