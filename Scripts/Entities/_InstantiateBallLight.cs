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
        CollisionComponent _collisionComponent;
        List<IUpdateDrawable> _updateDrawables;
        List<IUpdateDrawable> _tempRemoveUpdateDrawables;

        List<IDeath> _players = new();
        _Field _field = null;

        _BallLIght _BallLIght = null;

        public InstantiateBallLight(CollisionComponent collisionComponent, List<IUpdateDrawable> updateDrawables, List<IUpdateDrawable> _tempRemoveUpdateDrawables,
        _Field field, IDeath p1, IDeath p2)
        {
            _collisionComponent = collisionComponent;
            _updateDrawables = updateDrawables;
            _field = field;
            _players.Add(p1);
            _players.Add(p2);
            this._tempRemoveUpdateDrawables = _tempRemoveUpdateDrawables;
        }

        public void Draw(SpriteBatch spriteBatch) { }

        public void Update(GameTime gameTime)
        {
            if (_BallLIght == null)
            {
                Random random = new Random();

                float x = random.Next((int)(_field.Bounds.BoundingRectangle.Left + _field.thickness), (int)(_field.Bounds.BoundingRectangle.Right - _field.thickness));
                float y = random.Next((int)(_field.Bounds.BoundingRectangle.Top + _field.thickness), (int)(_field.Bounds.BoundingRectangle.Bottom - _field.thickness));

                _BallLIght = new _BallLIght(new(new(x, y), 30), Color.Yellow, _players, _tempRemoveUpdateDrawables);

                _collisionComponent.Insert(_BallLIght);
                _updateDrawables.Add(_BallLIght);
            }
        }
    }
}