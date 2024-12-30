using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.Tiles
{
    public class TileManager
    {
        private int[,] collisionLayer;
        private int tileWidth;
        private int tileHeight;
        private float scale;

        public TileManager(int[,] collisionLayer, int tileWidth, int tileHeight, float scale)
        {
            this.collisionLayer = collisionLayer;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            this.scale = scale;
        }

        private bool IsSolidTile(int tileIndex)
        {
            //tilelayer4 solid tiles (ground/wall)
            return tileIndex == 0 || tileIndex == 1 || tileIndex == 2 || 
                   tileIndex == 8 || tileIndex == 10;
        }

        public bool CheckCollision(Rectangle bounds, bool checkWallsOnly = false)
        {
            Rectangle scaledBounds = new Rectangle(
                (int)(bounds.X / scale),
                (int)(bounds.Y / scale),
                (int)(bounds.Width / scale),
                (int)(bounds.Height / scale)
            );

            int startX = scaledBounds.Left / tileWidth;
            int endX = scaledBounds.Right / tileWidth;
            int startY = scaledBounds.Top / tileHeight;
            int endY = scaledBounds.Bottom / tileHeight;

            for (int y = startY; y <= endY; y++)
            {
                for (int x = startX; x <= endX; x++)
                {
                    if (y >= 0 && y < collisionLayer.GetLength(0) &&
                        x >= 0 && x < collisionLayer.GetLength(1))
                    {
                        int tileIndex = collisionLayer[y, x];
                        if (IsSolidTile(tileIndex))
                        {
                            Rectangle tileRect = new Rectangle(
                                x * tileWidth * (int)scale,
                                y * tileHeight * (int)scale,
                                tileWidth * (int)scale,
                                tileHeight * (int)scale
                            );

                            if (bounds.Intersects(tileRect))
                            {
                                if (checkWallsOnly)
                                {
                                    //check for wall collisions
                                    bool isLeftCollision = bounds.Right > tileRect.Left && bounds.Left < tileRect.Left;
                                    bool isRightCollision = bounds.Left < tileRect.Right && bounds.Right > tileRect.Right;
                                    
                                    //ignore collision if above tile
                                    if (bounds.Bottom < tileRect.Top + (tileRect.Height * 0.25f))
                                    {
                                        return false;
                                    }
                                    
                                    return isLeftCollision || isRightCollision;
                                }
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public bool IsOnGround(Rectangle bounds)
        {
            Rectangle groundCheck = new Rectangle(
                bounds.X,
                bounds.Y + bounds.Height + 1,
                bounds.Width,
                1);

            return CheckCollision(groundCheck);
        }

        public Rectangle GetSolidTileBounds(Rectangle bounds)
        {
            Rectangle scaledBounds = new Rectangle(
                (int)(bounds.X / scale),
                (int)(bounds.Y / scale),
                (int)(bounds.Width / scale),
                (int)(bounds.Height / scale)
            );

            int startX = scaledBounds.Left / tileWidth;
            int endX = scaledBounds.Right / tileWidth;
            int startY = scaledBounds.Top / tileHeight;
            int endY = scaledBounds.Bottom / tileHeight;

            for (int y = startY; y <= endY; y++)
            {
                for (int x = startX; x <= endX; x++)
                {
                    if (y >= 0 && y < collisionLayer.GetLength(0) &&
                        x >= 0 && x < collisionLayer.GetLength(1))
                    {
                        int tileIndex = collisionLayer[y, x];
                        if (IsSolidTile(tileIndex))
                        {
                            return new Rectangle(
                                x * tileWidth * (int)scale,
                                y * tileHeight * (int)scale,
                                tileWidth * (int)scale,
                                tileHeight * (int)scale
                            );
                        }
                    }
                }
            }
            return Rectangle.Empty;
        }
    }
}
