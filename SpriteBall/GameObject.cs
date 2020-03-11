using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteBall
{
    class GameObject
    {
        protected Texture2D _texture;

        public int numCollision;
        public Vector2 Position;
        public Color color;
        public float Rotation;
        public Vector2 Scale;
        public float Speed;
        public Vector2 Velocity;
        public bool isCollide,Oncelling;

        public string Name;

        public bool IsActive;
        public int radius; 

        public GameObject(Texture2D texture)
        {
            _texture = texture;
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Rotation = 0f;
            IsActive = true;
            radius = 30; 
             
        }

        public virtual void Update(GameTime gameTime, List<GameObject> gameObjects)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        public virtual void Reset()
        {

        }
    }
}
