using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private Rectangle deelRectangle;
        private int schuifOp_X = 0;
        public LeshyLeaf(Texture2D texture)
        {
            leshyLeafTexture = texture;
            deelRectangle = new Rectangle(schuifOp_X, 32, 32, 32);
        }

        public void Update()
        {
            schuifOp_X += 32;
            if (schuifOp_X > 256)
                schuifOp_X = 0;
            deelRectangle.X = schuifOp_X;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(leshyLeafTexture, new Vector2(10, 10), deelRectangle, Color.White);
        }
    }
}
