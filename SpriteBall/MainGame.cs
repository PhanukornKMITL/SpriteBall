using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpriteBall
{
   
    public class MainGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont _font;
        List<GameObject> _gameObjects;
        private int _numObject;
        Random rnd = new Random();
        private Texture2D ballTexture,bgTexture,woodTexture,winTexture,loseTexture,finalResult,gun;
        string _text = "";
        GameObject ballObj,celingObj;
        Vector2 _ballPosition;
        float timePass = 0f;
        Texture2D ceiling;
        int rand;




        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

       
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = Singleton.SCREENWIDTH * Singleton.TILESIZE;
            graphics.PreferredBackBufferHeight = Singleton.SCREENHEIGHT * Singleton.TILESIZE;
            graphics.ApplyChanges();

            base.Initialize();
        }

        
        protected override void LoadContent()
        {
           
            spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("gameFont");
           Singleton.Instance.gameState = Singleton.GameState.PLAYING;
            //Texture2D ballTexture = this.Content.Load<Texture2D>("Ball");
            bgTexture = Content.Load<Texture2D>("BG");
            woodTexture = Content.Load<Texture2D>("wood");
            ballTexture = this.Content.Load<Texture2D>("sprite");
            winTexture = Content.Load<Texture2D>("youwin");
            loseTexture = Content.Load<Texture2D>("youlose");
            gun = Content.Load<Texture2D>("gun");

            Singleton.Instance.GameBoard = new int[8,4];
            _gameObjects = new List<GameObject>();
            Color _color = new Color();
           
            

            celingObj = new Celing(woodTexture)
            {
                Name = "Ceiling",
                color =  Color.White,
                Position = new Vector2(0,0)
            };

            _gameObjects.Add(celingObj);

            

            
            
            for(int i = 0; i < 8; i++)
            {

                for (int j = 0; j < 4; j++)
                {

                    switch (Singleton.Instance.GameBoard[i, j])
                    {
                        case 0: _color = Color.Red; break;
                        case 1: _color = Color.Blue;  break;
                        case 2: _color = Color.Yellow;  break;
                        case 3: _color = Color.Green;  break;
                    }

                    if (j % 2 == 0)
                    {
                        if(j == 0)
                        _ballPosition = new Vector2(i * Singleton.BALLSIZE, j * Singleton.BALLSIZE +10 );
                        
                          else
                            _ballPosition = new Vector2(i * Singleton.BALLSIZE, j * Singleton.BALLSIZE -15 +10  );
                    }
                    
                    else
                    {
                        if( i != 7)
                        {
                            if(j == 1)
                            {

                                _ballPosition = new Vector2((i * Singleton.BALLSIZE)
                                                  + (j + 1 * Singleton.BALLSIZE) / 2, ((j * Singleton.BALLSIZE)
                                                  + (j - 1 * Singleton.BALLSIZE) / 2) + 20 + 10) ;
                            }
                            else
                                _ballPosition = new Vector2((i * Singleton.BALLSIZE)
                                                  + (j + 1 * Singleton.BALLSIZE) / 2, ((j * Singleton.BALLSIZE)
                                                  + (j - 1 * Singleton.BALLSIZE) / 2) + 5 +10);
                        }   
                    }

                    ballObj = new Ball(ballTexture)
                    {
                        Name = "Board",
                        color = randomColor(),
                        Position = _ballPosition

                    };


                    _gameObjects.Add(ballObj);
                }
            }


            Reset();
            


            
        }

        protected  void Reset()
        {
            GameObject shootBall = null;
            Singleton.Instance.isEndTurn = false;
            //ยิง miss 3 ครั้งแล้วร่วง
            if (Singleton.Instance.count < 2)
            {

                Singleton.Instance.missCount++;
                
            }
           
            Singleton.Instance.count = 0;
            rand = rnd.Next(0, 11);
            if (rand != 1)
            {
                 shootBall = new Ball(ballTexture)
                {
                    Name = "ShootBall",
                    Position = new Vector2((graphics.PreferredBackBufferWidth - ballTexture.Width) / 2, 500),
                    color = randomColor()
                };
            }
            else
            {
                shootBall = new Ball(ballTexture)
                {
                    Name = "SpecialBall",
                    Position = new Vector2((graphics.PreferredBackBufferWidth - ballTexture.Width) / 2, 500),
                    color = Color.Purple
                };
            }
            

            _gameObjects.Add(shootBall);

            //RemoveAll ตัวที่ไม่ได้ Active อยู่
            _gameObjects.RemoveAll(g => g.IsActive == false);
            
            //เปลี่ยนชื่อ ball ทั้งหมด ที่ชื่อว่า checkedBall กลับไปเป็น Board เหมือนเดิม
            _gameObjects.Where(w => w.Name == "CheckedBall").ToList().ForEach(s => s.Name = "Board");
            _gameObjects.Where(w => w.Name == "Board").ToList().ForEach(s => s.numCollision = 0);
           

            foreach (GameObject s in _gameObjects)
            {
                
                s.Reset();
            }

        }
        protected override void UnloadContent()
        {
            
        }

        
        protected override void Update(GameTime gameTime)
        {
            
            switch(Singleton.Instance.gameState)
            {
                    case Singleton.GameState.PLAYING:

                    finalResult = null;
                     timePass += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    

              
               if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                        Singleton.Instance.gameState = Singleton.GameState.PAUSE;

            _numObject = _gameObjects.Count;
                    Console.WriteLine(_numObject);

                    for (int i = 0; i < _numObject; i++)
            {

                if (_gameObjects[i].IsActive)
                    _gameObjects[i].Update(gameTime, _gameObjects);
            }

            if (Singleton.Instance.isEndTurn)
            {
                Reset();
            }

            if(Singleton.Instance.missCount >= 3  || timePass >= 30)
            {
                _gameObjects.Where(w => w.Name == "Board" || w.Name=="Ceiling").ToList().ForEach(s => s.Position.Y += 30);
                Singleton.Instance.missCount = 0;
                        timePass = 0;
            }

                    _gameObjects.Where(w => w.Name=="DownBall").ToList().ForEach(s => s.Position.Y += 10);
                    _gameObjects.RemoveAll(w => w.Name.Equals("DownBall") && w.Position.Y > 700);

                    CheckWin();
                    break;

                case Singleton.GameState.PAUSE:

                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))

                        Singleton.Instance.gameState = Singleton.GameState.PLAYING;
                        break;

                case Singleton.GameState.LOSE:
                    _text = "YOU LOSE";
                    finalResult = loseTexture;
                    break;

                case Singleton.GameState.WIN:
                    _text = "YOU WIN";
                    finalResult = winTexture;
                    break;



            }
            


            


            base.Update(gameTime);
        }

        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.DrawString(_font,_text, new Vector2(300, 400), Color.Red);
          
            
            spriteBatch.Draw(bgTexture, new Vector2(0, 0),null);
            
            
            _numObject = _gameObjects.Count;
            if (finalResult != null)
            {
                spriteBatch.Draw(finalResult, new Vector2((graphics.PreferredBackBufferWidth - finalResult.Width) / 2, (graphics.PreferredBackBufferHeight - finalResult.Height) / 2), Color.White);
            }
            spriteBatch.Draw(gun, new Vector2((graphics.PreferredBackBufferWidth - gun.Width) / 2, 410),null,Color.White);

            spriteBatch.End();

            for (int i = 0; i < _numObject; i++)
            {
               if (_gameObjects[i].IsActive) _gameObjects[i].Draw(spriteBatch);
             }
           


            base.Draw(gameTime);
        }

      

        
       public void CheckWin()
        {
            if(_gameObjects.Count < 3)
            {
                Singleton.Instance.gameState = Singleton.GameState.WIN;
            }
        }

        public Color randomColor()
        {
            int rand = rnd.Next(0, 4);
            switch (rand)
            {
                case 0: return Color.Red; break;
                case 1: return Color.Blue; break;
                case 2: return Color.Yellow; break;
                case 3: return Color.Green; break;
            }
            return Color.Moccasin;
            
        }

        
    }
}
