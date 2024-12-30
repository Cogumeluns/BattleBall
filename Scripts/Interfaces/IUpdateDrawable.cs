using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace BattleBall.Scripts.Interfaces
{
    public interface IUpdateDrawable : IBaseDisposable
    {
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}