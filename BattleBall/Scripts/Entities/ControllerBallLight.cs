using System;
using System.Collections.Generic;
using BattleBall.Scripts.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Collisions;

namespace BattleBall.Scripts.Entities
{
    public class ControllerBallLight : IUpdateDrawable
    {
        // IUpdateDrawable
        public bool isDisposed { get; private set; } = false;
        // IUpdateDrawable
        public bool isVisible { get; set; } = true;

        private const float RADIUS = 30f;
        private readonly Random _random = new();
        CollisionComponent _collisionComponent;
        List<IUpdateDrawable> _tempUpdateDrawables;
        List<Player> _players = new();
        Field _field = null;

        public BallLight ballLight = null;

        public ControllerBallLight(CollisionComponent collisionComponent, List<IUpdateDrawable> updateDrawables,
        Field field, Player p1, Player p2)
        {
            _collisionComponent = collisionComponent;
            _tempUpdateDrawables = updateDrawables;
            _field = field;
            _players.Add(p1);
            _players.Add(p2);
        }

        public void Dispose()
        {
            if (isDisposed) return;

            DestroyBallLight();
            isDisposed = true;
        }

        public void Update(GameTime gameTime)
        {
            if (ballLight == null)
            {
                CreateBallLight();
            }
        }

        private void CreateBallLight()
        {
            float x = _random.Next((int)(_field.Bounds.BoundingRectangle.Left + _field.thickness + RADIUS),
                                   (int)(_field.Bounds.BoundingRectangle.Right - _field.thickness - RADIUS));
            float y = _random.Next((int)(_field.Bounds.BoundingRectangle.Top + _field.thickness + RADIUS),
                                   (int)(_field.Bounds.BoundingRectangle.Bottom - _field.thickness - RADIUS));

            ballLight = new BallLight(new(new(x, y), RADIUS), Color.Yellow, _players, this);

            // Insere a bola no sistema de colisão e atualização
            _collisionComponent.Insert(ballLight);
            _tempUpdateDrawables.Add(ballLight);
        }

        public void DestroyBallLight()
        {
            if (ballLight != null)
            {
                ballLight.Dispose();
                ballLight = null;
            }
        }

        public void Draw(SpriteBatch spriteBatch) { }
    }
}