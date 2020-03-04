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

                if(Position.X <= 0 || Position.X + _texture.Width >= Singleton.SCREENHEIGHT * Singleton.TILESIZE)
                {
                    Angle = -Angle;
                }
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
                    CollisionCheck(gameObjects, this);
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
                
                Position = new Vector2((Singleton.SCREENWIDTH * Singleton.TILESIZE - _texture.Width) / 2, 500);

            }

            
        }

        public void CollisionCheck(List<GameObject> gameObjects,GameObject current)
        {
            GameObject boardObject;
            foreach (GameObject g in gameObjects)
            {
                if (g.Name != current.Name && current.IsActive == true)
                {
                    //boardObject = g;

                    int sum = g.radius + 28;

                    if (Vector2.Distance(g.Position, current.Position) < sum)
                    {
                        current.Speed = 0;
                       if( ClusterCheck(current, g))
                        {
                            current.Name = "CheckedBall";
                            g.Name = "CheckedBall";
                            CollisionCheck(gameObjects, g); //เช็คตัวถัดไป
                            CollisionCheck(gameObjects, current); //พอเช็คตัวถัดไปเสร็จให้มันเช็คตัวเองอีกรอบ
                            if(Singleton.Instance.count > 2)
                            {
                                current.IsActive = false;
                                g.IsActive = false;
                            }
                           
                        }

                        if (!Singleton.Instance.isEndTurn)
                        {
                            current.Name = "Board";
                            Singleton.Instance.isEndTurn = true;
                        }
                      
                        
                    }
                    
                }
            }

 
        }// CollisionCheck

        public Boolean ClusterCheck(GameObject currebtObj, GameObject nextObj)
        {
            if(currebtObj.color == nextObj.color)
            {
                Console.WriteLine("Before"+Singleton.Instance.count);
                Singleton.Instance.count++;
                Console.WriteLine("Before" + Singleton.Instance.count);
                return true;
            }
            return false;

        }

    }
}
