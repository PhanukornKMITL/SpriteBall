using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;


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
        private Texture2D ballTexture;
        GameObject gObj;
        Vector2 _ballPosition;

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
            //Texture2D ballTexture = this.Content.Load<Texture2D>("Ball");
            ballTexture = this.Content.Load<Texture2D>("sprite");
            Texture2D rectTexture = new Texture2D(graphics.GraphicsDevice, 30, 100);
            Color[] data = new Color[30 * 100];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.White;
            rectTexture.SetData(data);
            Singleton.Instance.GameBoard = new int[8,4];
            _gameObjects = new List<GameObject>();
            Color _color = new Color();
            randomColor();
            for(int i = 0; i < 8; i++)
            {

                for (int j = 0; j < 4; j++)
                {

                    switch (Singleton.Instance.GameBoard[i, j])
                    {
                        case 0: _color = Color.Red; break;
                        case 1: _color = Color.Blue; break;
                        case 2: _color = Color.Yellow; break;
                        case 3: _color = Color.Green; break;
                    }

                    if (j % 2 == 0)
                    {
                        if(j == 0)
                        _ballPosition = new Vector2(i * Singleton.BALLSIZE, j * Singleton.BALLSIZE );
                        
                          else
                            _ballPosition = new Vector2(i * Singleton.BALLSIZE, j * Singleton.BALLSIZE -15  );
                    }
                    
                    else
                    {
                        if( i != 7)
                        {
                            if(j == 1)
                            {

                                _ballPosition = new Vector2((i * Singleton.BALLSIZE)
                                                  + (j + 1 * Singleton.BALLSIZE) / 2, ((j * Singleton.BALLSIZE)
                                                  + (j - 1 * Singleton.BALLSIZE) / 2) + 20);
                            }
                            else
                                _ballPosition = new Vector2((i * Singleton.BALLSIZE)
                                                  + (j + 1 * Singleton.BALLSIZE) / 2, ((j * Singleton.BALLSIZE)
                                                  + (j - 1 * Singleton.BALLSIZE) / 2) + 5);


                            /*_ballPosition = new Vector2((i * Singleton.BALLSIZE)
                                                 + (j + 1 * Singleton.BALLSIZE) / 2, j * Singleton.BALLSIZE);*/



                        }
                       
                        
                      
                    }

                    gObj = new Ball(ballTexture)
                    {
                        Name = "Board",
                        color = randomColor(),
                        Position = _ballPosition

                    };


                    _gameObjects.Add(gObj);
                }
            }


            Reset();
            


            
        }

        protected  void Reset()
        {
            Singleton.Instance.isEndTurn = false;

            GameObject shootBall = new Ball(ballTexture)
            {
                Name = "ShootBall",
                Position = new Vector2((graphics.PreferredBackBufferWidth - ballTexture.Width) / 2, 500),
                color = randomColor()
            };

            _gameObjects.Add(shootBall);


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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _numObject = _gameObjects.Count;
            for (int i = 0; i < _numObject; i++)
            {
                if (_gameObjects[i].IsActive)
                    _gameObjects[i].Update(gameTime, _gameObjects);
            }

           // if (Singleton.Instance.isEndTurn)
            //{
              //  Reset();
            //}

            base.Update(gameTime);
        }

        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _numObject = _gameObjects.Count;
            for (int i = 0; i < _numObject; i++)
            {
               if (_gameObjects[i].IsActive) _gameObjects[i].Draw(spriteBatch);
             }
                

           



            base.Draw(gameTime);
        }

        /*public void randomColor()
        {

            int color;
           

            for (int i = 0; i < Singleton.Instance.GameBoard.GetLength(0); i++)
            {

                for (int j = 0; j < Singleton.Instance.GameBoard.GetLength(1); j++)
                {
                   color = rnd.Next(0, 4);
                    Singleton.Instance.GameBoard[i, j] = color;
                    
                }


            }//forj

        }*/

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
