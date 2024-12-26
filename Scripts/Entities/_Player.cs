using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleBall.Scripts.Constants;
using BattleBall.Scripts.Enum;
using BattleBall.Scripts.Interfaces;
using BattleBall.Scripts.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace BattleBall.Scripts.Entities
{
    public class _Player : ICollisionActor, IUpdateDrawable
    {
        // ICollisionActor
        public IShapeF Bounds { get; set; }
        // IUpdateDrawable -> IBaseDisposable
        public bool isDisposed { get; private set; } = false;
        public Color color;
        private Dictionary<PlayerKeys, Keys> playerKeys = new();
        public Vector2 velocitydash = Vector2.Zero;
        public Vector2 velocity = Vector2.Zero;
        public Vector2 pushBackIntensity = Vector2.Zero;
        public int Lives { get; set; }
        public float timeDash = 0;
        public float timePushBackDuration = 0;
        public bool[] isColliderBorderField = new bool[4]; // TOP, BOTTOM, LEFT, RIGHT

        public _Player(CircleF circle, Color color)
        {
            Bounds = circle;
            this.color = color;
            Lives = 3;
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
            CircleF circle = (CircleF)Bounds;
            spriteBatch.DrawCircle(circle, Physics.SIDES, color, circle.Radius);
        }

        public void Update(GameTime gameTime)
        {
            UpdateDurations(gameTime);
            KeysPress();
            UpdateMovement(gameTime);

        }

        void UpdateDurations(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            UpdatePushBackDuration(deltaTime);
            UpdateDashDuration(deltaTime);
        }

        void UpdatePushBackDuration(float deltaTime)
        {
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

        void UpdateDashDuration(float deltaTime)
        {
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

        void UpdateMovement(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 totalVelocity = velocity + velocitydash + pushBackIntensity;

            UpdatePosition(deltaTime, totalVelocity);
        }

        void UpdatePosition(float deltaTime, Vector2 totalVelocity)
        {
            Bounds.Position += totalVelocity * deltaTime;
        }

        /// Eventos dos Botões

        void KeysPress()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            HandleVerticalMovement(keyboardState);
            HandleHorizontalMovement(keyboardState);
            HandleDash(keyboardState);
        }

        void HandleVerticalMovement(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(playerKeys[PlayerKeys.Up]) && !isColliderBorderField[0])
            {
                velocity.Y = -Physics.DEFAULT_VELOCITY_PLAYER;
            }
            else if (keyboardState.IsKeyDown(playerKeys[PlayerKeys.Down]) && !isColliderBorderField[1])
            {
                velocity.Y = Physics.DEFAULT_VELOCITY_PLAYER;
            }
            else
            {
                velocity.Y = 0;
                ResetVerticalBorders();
            }
        }

        void HandleHorizontalMovement(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(playerKeys[PlayerKeys.Left]) && !isColliderBorderField[2])
            {
                velocity.X = -Physics.DEFAULT_VELOCITY_PLAYER;
            }
            else if (keyboardState.IsKeyDown(playerKeys[PlayerKeys.Right]) && !isColliderBorderField[3])
            {
                velocity.X = Physics.DEFAULT_VELOCITY_PLAYER;
            }
            else
            {
                velocity.X = 0;
                ResetHorizontalBorders();
            }
        }

        void HandleDash(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(playerKeys[PlayerKeys.Dash]))
            {
                if (velocitydash.Length() == 0 && velocity != Vector2.Zero)
                {
                    Vector2 normalized = Vector2.Normalize(velocity);
                    velocitydash = normalized * Physics.DEFAULT_VELOCITY_DASH;
                    timeDash = Physics.DEFAULT_TIME_DASH_DURATION;
                }
            }
        }

        void ResetVerticalBorders()
        {
            isColliderBorderField[0] = false;
            isColliderBorderField[1] = false;
        }

        void ResetHorizontalBorders()
        {
            isColliderBorderField[2] = false;
            isColliderBorderField[3] = false;
        }

        // ICollisionActor
        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            if (collisionInfo.Other is _Player player)
            {
                pushBackIntensity = PhysicForce.ApplyPushBack(Bounds.Position, player, Physics.DEFAULT_PUSH_BACK_INTENSITY_PLAYER);
                timePushBackDuration = Physics.DEFAULT_TIME_PUSH_BACK_DURATION;
            }
        }

        public void Dispose()
        {
            isDisposed = true;
        }
    }
}