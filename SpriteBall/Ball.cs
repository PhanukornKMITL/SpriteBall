using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteBall
{
    class Ball : GameObject
    {

        public float Speed;
        public float Angle;
        MouseState mState;
        bool mReleased = true;
        Vector2 mousePosition;
        Vector2 movement;

        public Ball(Texture2D texture) : base(texture)
        {

        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {

            Velocity.X = (float)Math.Cos(Angle) * Speed;
            Velocity.Y = (float)Math.Sin(Angle) * Speed;
            Position += Velocity * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

            mState = Mouse.GetState();

            
            if (this.Name.Equals("ShootBall"))
            {
                if (mState.LeftButton == ButtonState.Pressed && mReleased == true)
                {
                    movement.X = mState.X - Position.X;
                    movement.Y = mState.Y - Position.Y;
                    
                   
                   //หามุมจากบอลไปถึงเม้า  
                    Angle = (float)Math.Atan2(movement.Y, movement.X);
                    Speed = 300;    
                    
                    mReleased = false;
                }

                if(mState.LeftButton == ButtonState.Released)
                {
                    mReleased = true;   
                }
               

            }


            


           
                  


            base.Update(gameTime, gameObjects);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_texture , Position ,color );
            spriteBatch.End();

            base.Draw(spriteBatch);
        }

        public override void Reset()
        {

            if (this.Name.Equals("ShootBall"))
            {
                //Speed = 300;
                Position = new Vector2((Singleton.SCREENWIDTH * Singleton.TILESIZE - _texture.Width) / 2, 500);

                  //  Angle = 180;
            }
            base.Reset();
        }


    }
}
