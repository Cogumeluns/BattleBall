using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleBall.Scripts.Constants;
using BattleBall.Scripts.Enum;
using BattleBall.Scripts.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace BattleBall.Scripts.Entities
{
    public class Player : ICollisionActor, IUpdateDrawable
    {
        // ICollisionActor
        public IShapeF Bounds { get; set; }
        // IUpdateDrawable -> IBaseDisposable
        public bool isDisposed { get; private set; } = false;
        public Color color;
        public Dictionary<PlayerKeys, Func<KeyboardState, bool>> playerKeys = new();
        public Vector2 velocitydash = Vector2.Zero;
        public Vector2 velocity = Vector2.Zero;
        public Vector2 pushBackIntensity = Vector2.Zero;
        public int Lives { get; set; }
        public float timeDash = 0;
        public float timePushBackDuration = 0;
        public bool[] isColliderBorderField = new bool[4]; // TOP, BOTTOM, LEFT, RIGHT

        public Player(CircleF circle, Color color)
        {
            Bounds = circle;
            this.color = color;
            Lives = 3;
        }

        public void Damage(int damage)
        {
            Lives -= damage;

            if (Lives <= 0)
            {
                Dispose();
            }
        }

        public void SetKeys(Dictionary<PlayerKeys, Func<KeyboardState, bool>> events)
        {
            playerKeys = events;
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
            if (playerKeys[PlayerKeys.Up](keyboardState) && !isColliderBorderField[0])
            {
                velocity.Y = -Physics.DEFAULT_VELOCITY_PLAYER;
            }
            else if (playerKeys[PlayerKeys.Down](keyboardState) && !isColliderBorderField[1])
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
            if (playerKeys[PlayerKeys.Left](keyboardState) && !isColliderBorderField[2])
            {
                velocity.X = -Physics.DEFAULT_VELOCITY_PLAYER;
            }
            else if (playerKeys[PlayerKeys.Right](keyboardState) && !isColliderBorderField[3])
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
            if (playerKeys[PlayerKeys.Dash](keyboardState))
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
            if (collisionInfo.Other is Player player)
            {
                pushBackIntensity = ApplyPushBack(Bounds.Position, player, Physics.DEFAULT_PUSH_BACK_INTENSITY_PLAYER);
                timePushBackDuration = Physics.DEFAULT_TIME_PUSH_BACK_DURATION;

                AdjustPosition(player);
            }
        }

        public Vector2 ApplyPushBack(Vector2 currentCollider, Player playerCollider, float force)
        {
            Vector2 direction = currentCollider - playerCollider.Bounds.Position;

            direction.Normalize();

            float someIntensity = 1;

            if (playerCollider.timeDash != 0)
            {
                someIntensity = 1.5f;
            }

            return force * someIntensity * direction;
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

        public void Dispose()
        {
            isDisposed = true;
        }
    }
}