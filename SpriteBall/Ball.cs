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
        
        
        int lowerBubblesTime = 0;
        public static int posRoof = 0;
        

        public Ball(Texture2D texture) : base(texture)
        {

        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {

                Velocity.X = (float)Math.Cos(Angle) * Speed;
                Velocity.Y = (float)Math.Sin(Angle) * Speed;
                Position += Velocity * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

            mState = Mouse.GetState();

            //ถ้าObj ตัวนี้เป็น Shooter ถึงจะทำ ถ้าเป็น Board Ball ก็คืออยู่เฉยๆโง่ๆไป
            if (this.Name.Equals("ShootBall"))
            {

                if (mState.LeftButton == ButtonState.Pressed && mReleased == true)
                {
                    mReleased = false;
                    movement.X = mState.X - Position.X;
                    movement.Y = mState.Y - Position.Y;
                   
                   //หามุมจากบอลไปถึงเม้า  
                    Angle = (float)Math.Atan2(movement.Y, movement.X);
                    Speed = 300;    
                    

                    
                }

                if(mState.LeftButton == ButtonState.Released)
                {
                    
                    CollisionCheck(gameObjects, this);                  
                       
                }
            }
            //else นี้คือถ้ามันไม่ได้ชื่อ shootball แล้วก็เช็คว่ามันติดกับคนอื่นหรือเปล่า
            else
            {
                int count = 0;
                foreach(GameObject g in gameObjects)
                {
                    int sum = g.radius + 28;
                    if(Vector2.Distance(g.Position, this.Position) < sum)
                    {
                        count++;
                        
                    }
                   
                }


                if(count < 2)
                {
                    this.Position.Y += 10;
                }
                //Console.WriteLine(count);
                
            }

            // เช้คขอบซ้ายขวา (ใช้ Singleton.Gamewidth ไม่ได้ ไม่รู้ทำไม)
            if (Position.X <= 0 || Position.X + _texture.Width >= 450) 
            {
				Angle = -Angle;
				Angle += MathHelper.ToRadians(180);
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
                    // หา sum จาก รหัสมีของ obj ตัวที่จะเช็คด้วย + 28 หมายถึง จากรัศมี ออกไป28
                    int sum = g.radius + 28;

                    //distance คือระยะห่างระหว่าง Obj 2 ตัว ถ้ามัน < sum หมายความว่าชน 
                    if (Vector2.Distance(g.Position, current.Position) < sum)
                    {
                        // set speed = 0 คือให้บอลที่ยิงไป พอชนแล้ว จะหยุด
                        current.Speed = 0;
                        //ClusterCheck คือเช็คสี
                       if( ClusterCheck(current, g))
                        {
                            //กำหนดชื่อให้ obj ที่เช็คแล้ว 
                            current.Name = "CheckedBall";
                            g.Name = "CheckedBall";
                            CollisionCheck(gameObjects, g); //เช็คตัวถัดไปที่ติดกับมัน
                            CollisionCheck(gameObjects, current); //พอเช็คตัวถัดไปเสร็จให้มันเช็คตัวเองอีกรอบ นึกถึง depth first search หาลึกสุดๆแล้วกลับมาหาที่ตัวมันเองดูว่ามันไปไหนได้อีก
                            
                            
                            if(Singleton.Instance.count > 2)
                            {
                                current.IsActive = false;
                                g.IsActive = false;
                            }
                           

                        }

                       
                        
                        //else
                        //{
                        //    Console.WriteLine(Singleton.Instance.missCount);
                        //    Singleton.Instance.missCount++;
                        //}


                        if (!Singleton.Instance.isEndTurn)
                        {
                            //setให้จบตากรณีที่มันไม่ชนสีเหมือนกัน แล้วSet ค่าให้ตัว ปัจจุบันชื่อ Board 
                            //ก็คือ ball ที่อยู่บนกระดานที่ไม่ใช่shooter อ่ะ
                            current.Name = "Board";
                            
                            Singleton.Instance.isEndTurn = true;
                            
                            mReleased = true;
                        }
                      
                        
                    }

                  
                        
                    
                }
            }

 
        }// CollisionCheck

        public Boolean ClusterCheck(GameObject currebtObj, GameObject nextObj)
        {
            if(currebtObj.color == nextObj.color)
            {
                //Console.WriteLine("Before"+Singleton.Instance.count);
                Singleton.Instance.count++;
                //Console.WriteLine("Before" + Singleton.Instance.count);
                return true;
            }
            return false;

        }

        
    }
}
