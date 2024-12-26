using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleBall.Scripts.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Collisions;

namespace BattleBall.Scripts.Entities
{
    public class InstantiateBallLight : IUpdateDrawable
    {
        public bool isDisposed { get; private set; } = false;
        CollisionComponent _collisionComponent;
        List<IUpdateDrawable> _updateDrawables;

        List<_Player> _players = new();
        _Field _field = null;

        _BallLIght _BallLIght = null;
        public bool _isExecute = false;

        public InstantiateBallLight(CollisionComponent collisionComponent, List<IUpdateDrawable> updateDrawables,
        _Field field, _Player p1, _Player p2)
        {
            _collisionComponent = collisionComponent;
            _updateDrawables = updateDrawables;
            _field = field;
            _players.Add(p1);
            _players.Add(p2);
        }

        public void Dispose()
        {
            isDisposed = true;
        }

        public void Draw(SpriteBatch spriteBatch) { }

        public void Update(GameTime gameTime)
        {
            if (_BallLIght == null && !_isExecute)
            {
                _isExecute = true;
                Random random = new Random();

                float x = random.Next((int)(_field.Bounds.BoundingRectangle.Left + _field.thickness), (int)(_field.Bounds.BoundingRectangle.Right - _field.thickness));
                float y = random.Next((int)(_field.Bounds.BoundingRectangle.Top + _field.thickness), (int)(_field.Bounds.BoundingRectangle.Bottom - _field.thickness));

                _BallLIght = new _BallLIght(new(new(x, y), 30), Color.Yellow, _players, this);

                _collisionComponent.Insert(_BallLIght);
                _updateDrawables.Add(_BallLIght);

                _isExecute = false;
            }
        }

        public void DestroyBallLight()
        {
            if (_BallLIght != null)
            {
                _BallLIght.Dispose();
            }
            _BallLIght = null;
        }
    }
}