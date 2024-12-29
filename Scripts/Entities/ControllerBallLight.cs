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
        private CollisionComponent _collisionComponent;
        private List<IUpdateDrawable> _tempUpdateDrawables;
        private List<Player> _players = new();
        private Field _field = null;
        private BallLight _ballLight = null;

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
            if (_ballLight == null)
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

            _ballLight = new BallLight(new(new(x, y), RADIUS), Color.Yellow, _players, this);

            _collisionComponent.Insert(_ballLight);
            _tempUpdateDrawables.Add(_ballLight);
        }

        public void DestroyBallLight()
        {
            if (_ballLight != null)
            {
                _ballLight.Dispose();
                _ballLight = null;
            }
        }

        public void Draw(SpriteBatch spriteBatch) { }
    }
}