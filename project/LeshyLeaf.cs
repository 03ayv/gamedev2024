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
        Texture2D leshyLeafTexture;
        Animation animation;
        public LeshyLeaf(Texture2D texture)
        {
            leshyLeafTexture = texture;
            animation = new Animation();
            animation.AddFrame(new AnimationFrame(new Rectangle(0,32,32,32)));
            animation.AddFrame(new AnimationFrame(new Rectangle(32,32, 32, 32)));
            animation.AddFrame(new AnimationFrame(new Rectangle(64, 32, 32, 32)));
            animation.AddFrame(new AnimationFrame(new Rectangle(96, 32, 32, 32)));
            animation.AddFrame(new AnimationFrame(new Rectangle(128, 32, 32, 32)));
            animation.AddFrame(new AnimationFrame(new Rectangle(160, 32, 32, 32)));
            animation.AddFrame(new AnimationFrame(new Rectangle(192, 32, 32, 32)));
            animation.AddFrame(new AnimationFrame(new Rectangle(224, 32, 32, 32)));
            animation.AddFrame(new AnimationFrame(new Rectangle(256, 32, 32, 32)));
        }

        public void Update()
        {
            animation.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(leshyLeafTexture, new Vector2(10, 10), animation.CurrentFrame.SourceRectangle, Color.White);
        }
    }
}
