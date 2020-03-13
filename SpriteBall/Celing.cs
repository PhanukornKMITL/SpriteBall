using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteBall
{
    class Celing : GameObject
    {

        public Celing(Texture2D texture) : base(texture)
        {
            
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
            
            base.Update(gameTime, gameObjects);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
           spriteBatch.Begin();
            spriteBatch.Draw(_texture, Position, color);
            spriteBatch.End();

            base.Draw(spriteBatch);
        }

        public override void Reset()
        {
            

        }

       
    }
}
