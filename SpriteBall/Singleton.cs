using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteBall
{
    
    class Singleton
    {
        private static Singleton instance;
        public const int TILESIZE = 30;
        public const int BALLSIZE = 56; 

        public const int GAMEWIDTH = 10;
        public const int GAMEHEIGHT = 20;

        public const int SCREENWIDTH = GAMEWIDTH + 5;
        public const int SCREENHEIGHT = GAMEHEIGHT;

        public int Score;
        public int Level;
        public int LineDeleted;

        public KeyboardState PreviousKey, CurrentKey;

        public int[,] GameBoard;
        public int count;
        public bool isEndTurn = true;

        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }
                return instance;
            }
        }

        
    }
    
}
