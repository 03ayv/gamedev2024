using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using project.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using project.Interfaces;

namespace project
{
    public class Porcupine: IGameObject
    {
        private Texture2D porcupineTexture;
        private Animation walking;
        private Vector2 position;
        public Porcupine(Texture2D texture)
        {
            porcupineTexture = texture;
            walking = new Animation();
            walking.AddFrame(new AnimationFrame(new Rectangle(0, 0, 32, 32)));
            walking.AddFrame(new AnimationFrame(new Rectangle(32, 0, 32, 32)));
            walking.AddFrame(new AnimationFrame(new Rectangle(64, 0, 32, 32)));
            walking.AddFrame(new AnimationFrame(new Rectangle(96, 0, 32, 32)));
            walking.AddFrame(new AnimationFrame(new Rectangle(128, 0, 32, 32)));

            position = new Vector2(200, 300);
        }

        public void Update(GameTime gameTime)
        {
            walking.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //adjust size (1.0f = original size)
            float scale = 3f;

            //walking animation
            spriteBatch.Draw(porcupineTexture, position, walking.CurrentFrame.SourceRectangle, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.FlipHorizontally, 0f);
        }
    }
}
