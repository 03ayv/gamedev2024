using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using project.Animations;
using project.Interfaces;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project
{
    public class LeshyLeaf: IGameObject
    {
        private Texture2D leshyLeafTexture;
        private Animation walking;
        private Vector2 position;
        public LeshyLeaf(Texture2D texture)
        {
            leshyLeafTexture = texture;
            walking = new Animation();
            walking.AddFrame(new AnimationFrame(new Rectangle(0,40,32,32)));
            walking.AddFrame(new AnimationFrame(new Rectangle(32,40, 32, 32)));
            walking.AddFrame(new AnimationFrame(new Rectangle(64, 40, 32, 32)));
            walking.AddFrame(new AnimationFrame(new Rectangle(96, 40, 32, 32)));
            walking.AddFrame(new AnimationFrame(new Rectangle(128, 40, 32, 32)));
            walking.AddFrame(new AnimationFrame(new Rectangle(160, 40, 32, 32)));
            walking.AddFrame(new AnimationFrame(new Rectangle(192, 40, 32, 32)));
            walking.AddFrame(new AnimationFrame(new Rectangle(224, 40, 32, 32)));

            position = new Vector2(0, 300);
        }

        public void Update(GameTime gameTime)
        {
            walking.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //adjust size (1.0f = original size)
            float scale = 4f;

            //walking animation
            spriteBatch.Draw(leshyLeafTexture, position, walking.CurrentFrame.SourceRectangle, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
    }
}
