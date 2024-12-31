using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using project.Input;
using project.Interfaces;
using project.Tiles;
using SharpDX.Direct2D1.Effects;
using System;
using System.Collections.Generic;
using System.IO;

namespace project
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //leshy leaf sprite
        private Texture2D leshyLeafTexture;
        LeshyLeaf leshyLeaf;

        //enemies
        private Texture2D porcupineTexture;
        private Texture2D dragonflyTexture;
        private Texture2D squirrelTexture;
        private List<IGameObject> enemies = new List<IGameObject>();

        //game over
        private bool gameOver = false;
        private Color backgroundColor = Color.RosyBrown;

        //camera
        private Vector2 cameraPosition;

        //tileset
        private Texture2D bg1Texture;
        private Texture2D bg2Texture;
        private Texture2D bg3Texture;
        private Texture2D decorsTexture;
        private Texture2D tilesetTexture;
        private List<Rectangle> tiles;
        private int[,] tileMap1;
        private int[,] tileMap2;
        private int[,] tileMap3;
        private int[,] tileMap4;
        private int[,] tileMap5;

        private List<Rectangle> bgTiles;      
        private List<Rectangle> decorTiles;   
        private List<Rectangle> tilesetTiles;

        private int tileWidth = 16;
        private int tileHeight = 16;
        private float scale = 5f;

        //camera stays within background
        private int mapWidth;
        private int mapHeight;

        //tilemanager
        private TileManager tileManager;

        //extras
        private Texture2D keyTexture;
        private Key key;
        private Texture2D coinTexture;
        private Coin coin;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        private int[,] LoadTileMap(string path)
        {
            string[] lines = File.ReadAllLines(path);
            int rows = lines.Length;
            int cols = lines[0].Split(',').Length;
            var map = new int[rows, cols];

            Console.WriteLine($"Loading tilemap: {path}");
            Console.WriteLine($"ons: { rows}x{ cols}");

            for (int y = 0; y < rows; y++)
            {
                string[] cells = lines[y].Split(',');
                for (int x = 0; x < cols; x++)
                {
                    map[y, x] = int.Parse(cells[x]);
                }
            }
            return map;
        }


        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //load tilesets
            bg1Texture = Content.Load<Texture2D>("BG1");
            bg2Texture = Content.Load<Texture2D>("BG2");
            bg3Texture = Content.Load<Texture2D>("BG3");
            decorsTexture = Content.Load<Texture2D>("Decors");
            tilesetTexture = Content.Load<Texture2D>("Tileset");
            

            tiles = new List<Rectangle>();
            //tile rectangle = 16px/16px
            int tileWidth = 16;
            int tileHeight = 16;
            int columns = bg2Texture.Width / tileWidth;
            int rows = bg2Texture.Height / tileHeight;

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    tiles.Add(new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight));
                }
            }

            //load csv
            tileMap1 = LoadTileMap("Data/Forest_TileLayer1.csv");
            tileMap2 = LoadTileMap("Data/Forest_TileLayer2.csv");
            tileMap3 = LoadTileMap("Data/Forest_TileLayer3.csv");
            tileMap4 = LoadTileMap("Data/Forest_TileLayer4.csv");
            tileMap5 = LoadTileMap("Data/Forest_TileLayer5.csv");

            tileManager = new TileManager(tileMap4, 16, 16, 5f);

            //game object textures
            leshyLeafTexture = Content.Load<Texture2D>("LeshyLeaf");
            porcupineTexture = Content.Load<Texture2D>("Porcupine");
            dragonflyTexture = Content.Load<Texture2D>("Dragonfly");
            squirrelTexture = Content.Load<Texture2D>("Squirrel");
            //extras
            keyTexture = Content.Load<Texture2D>("Key");
            coinTexture = Content.Load<Texture2D>("Key"); //key = dungeon collectibles
            InitializeGameObjects();

            //initialize tile lists
            bgTiles = CreateTileRectangles(bg1Texture, tileWidth, tileHeight);
            decorTiles = CreateTileRectangles(decorsTexture, tileWidth, tileHeight);
            tilesetTiles = CreateTileRectangles(tilesetTexture, tileWidth, tileHeight);

        }

        private void InitializeGameObjects()
        {
            leshyLeaf = new LeshyLeaf(leshyLeafTexture, new KeyboardReader(), tileManager);
            
            enemies = new List<IGameObject>
            {
                new Porcupine(porcupineTexture, new Vector2(800, 1185)),
                new Porcupine(porcupineTexture, new Vector2(1700, 1185)),
                new Dragonfly(dragonflyTexture, new Vector2(400, 950)),
                new Dragonfly(dragonflyTexture, new Vector2(1500, 1000)),
                new Squirrel(squirrelTexture, new Vector2(300, 1185), leshyLeaf),
                new Squirrel(squirrelTexture, new Vector2(2000, 1185), leshyLeaf)
            };

            key = new Key(keyTexture, new Vector2(2200, 1190));
            coin = new Coin(coinTexture, new Vector2(800, 1190));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (!gameOver)
            {
                leshyLeaf.Update(gameTime);

                foreach (var enemy in enemies)
                {
                    enemy.Update(gameTime);

                    if (leshyLeaf.GetBounds().Intersects(enemy.GetBounds()))
                    {
                        gameOver = true;
                        break;
                    }
                }
            }

            //collect key to go to next level
            key.Update(gameTime);

            //check collision with leshyleaf
            if (!key.IsCollected() && key.GetBounds().Intersects(leshyLeaf.GetBounds()))
            {
                key.Collect();
                //go to next level
            }

            //collect coin
            coin.Update(gameTime);

            //check collision with leshyleaf
            if (!coin.IsCollected() && coin.GetBounds().Intersects(leshyLeaf.GetBounds()))
            {
                coin.Collect();
            }

            base.Update(gameTime);
        }

        private void DrawTiles(SpriteBatch spriteBatch)
        {
            //draw all layers
            DrawTileLayer(spriteBatch, bg1Texture, tileMap1, tileWidth, tileHeight, scale);
            DrawTileLayer(spriteBatch, bg2Texture, tileMap2, tileWidth, tileHeight, scale);
            DrawTileLayer(spriteBatch, bg3Texture, tileMap3, tileWidth, tileHeight, scale);
            DrawTileLayer(spriteBatch, tilesetTexture, tileMap4, tileWidth, tileHeight, scale);
            DrawTileLayer(spriteBatch, decorsTexture, tileMap5, tileWidth, tileHeight, scale);
        }

        private void DrawTileLayer(SpriteBatch spriteBatch, Texture2D texture, int[,] tileMap, int tileWidth, int tileHeight, float scale)
        {
            if (tileMap == null || texture == null) return;

            //get the correct tile
            List<Rectangle> tileList;
            if (texture == bg1Texture || texture == bg2Texture || texture == bg3Texture)
                tileList = bgTiles;
            else if (texture == decorsTexture)
                tileList = decorTiles;
            else
                tileList = tilesetTiles;

            //calculate visible area based on camera position
            int startX = Math.Max(0, (int)(cameraPosition.X / (tileWidth * scale)));
            int startY = Math.Max(0, (int)(cameraPosition.Y / (tileHeight * scale)));
            int endX = Math.Min(tileMap.GetLength(1), startX + (GraphicsDevice.Viewport.Width / (int)(tileWidth * scale)) + 2);
            int endY = Math.Min(tileMap.GetLength(0), startY + (GraphicsDevice.Viewport.Height / (int)(tileHeight * scale)) + 2);

            for (int y = startY; y < endY; y++)
            {
                for (int x = startX; x < endX; x++)
                {
                    int tileIndex = tileMap[y, x];
                    if (tileIndex == -1) continue;

                    if (tileIndex >= 0 && tileIndex < tileList.Count)
                    {
                        Vector2 position = new Vector2(x * tileWidth * scale, y * tileHeight * scale);
                        spriteBatch.Draw(texture, position, tileList[tileIndex], Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                    }
                }
            }
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backgroundColor);

            //camera position centers leshyleaf horizontally
            cameraPosition = new Vector2(
                leshyLeaf.GetBounds().Center.X - GraphicsDevice.Viewport.Width / 3,
                //leshyLeaf.GetBounds().Center.Y - GraphicsDevice.Viewport.Height * 0.65f //vertically too
                GraphicsDevice.Viewport.Height * 1.9f
                );

            //clamp camera to stay within map boundaries
            ClampCamera();

            //camera transform matrix
            Matrix cameraTransform = Matrix.CreateTranslation(-cameraPosition.X, -cameraPosition.Y, 0);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: cameraTransform);

            DrawTiles(_spriteBatch);
            leshyLeaf.Draw(_spriteBatch);
            foreach (var enemy in enemies)
            {
                enemy.Draw(_spriteBatch);
            }
            key.Draw(_spriteBatch);
            coin.Draw(_spriteBatch);

            _spriteBatch.End();
            
            base.Draw(gameTime);
        }

        private List<Rectangle> CreateTileRectangles(Texture2D texture, int tileWidth, int tileHeight)
        {
            var tileList = new List<Rectangle>();
            int columns = texture.Width / tileWidth;
            int rows = texture.Height / tileHeight;

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    tileList.Add(new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight));
                }
            }
            return tileList;
        }

        //clamp camera within map
        private void ClampCamera()
        {
            //calculate map dimensions
            float mapWidthInPixels = (tileMap1.GetLength(1) * 16 * 5) - GraphicsDevice.Viewport.Width / 21f; //adjust for ending point
            float mapHeightInPixels = (tileMap1.GetLength(0) * 16 * 5f); 
                                                                
            float minX = GraphicsDevice.Viewport.Width / 50; //adjust for starting point
            float maxX = mapWidthInPixels - GraphicsDevice.Viewport.Width + minX;
            float maxY = mapHeightInPixels - GraphicsDevice.Viewport.Height;

            //clamp camera position
            cameraPosition.X = Math.Clamp(cameraPosition.X, minX, maxX);
            cameraPosition.Y = Math.Clamp(cameraPosition.Y, 0, maxY);
        }

    }
}
