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
    public class ControllerBallLight : IUpdateDrawable
    {
        public bool isDisposed { get; private set; } = false;

        private readonly Random _random = new();
        CollisionComponent _collisionComponent;
        List<IUpdateDrawable> _tempUpdateDrawables;
        List<Player> _players = new();
        Field _field = null;

        BallLight _ballLight = null;
        public bool _isExecute = false;
        public bool isVisible { get; set; } = true;

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

        public void Draw(SpriteBatch spriteBatch)
        {
            // Nenhuma lógica de desenho necessária no momento
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
            // Gera posições aleatórias dentro dos limites do campo
            float x = _random.Next((int)(_field.Bounds.BoundingRectangle.Left + _field.thickness + 30),
                                   (int)(_field.Bounds.BoundingRectangle.Right - _field.thickness - 30));
            float y = _random.Next((int)(_field.Bounds.BoundingRectangle.Top + _field.thickness + 30),
                                   (int)(_field.Bounds.BoundingRectangle.Bottom - _field.thickness - 30));

            _ballLight = new BallLight(new(new(x, y), 30), Color.Yellow, _players, this);

            // Insere a bola no sistema de colisão e atualização
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
    }
}