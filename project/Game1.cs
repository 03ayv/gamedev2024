using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using project.Input;
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

        //porcupine sprite
        private Texture2D porcupineTexture;
        Porcupine porcupine;

        //dragonfly sprite
        private Texture2D dragonflyTexture;
        Dragonfly dragonfly;

        //squirrel sprite
        private Texture2D squirrelTexture;
        Squirrel squirrel;

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

            //game object textures
            leshyLeafTexture = Content.Load<Texture2D>("LeshyLeaf");
            porcupineTexture = Content.Load<Texture2D>("Porcupine");
            dragonflyTexture = Content.Load<Texture2D>("Dragonfly");
            squirrelTexture = Content.Load<Texture2D>("Squirrel");
            InitializeGameObjects();

            //initialize tile lists
            bgTiles = CreateTileRectangles(bg1Texture, tileWidth, tileHeight);
            decorTiles = CreateTileRectangles(decorsTexture, tileWidth, tileHeight);
            tilesetTiles = CreateTileRectangles(tilesetTexture, tileWidth, tileHeight);
        }

        private void InitializeGameObjects()
        {
            leshyLeaf = new LeshyLeaf(leshyLeafTexture, new KeyboardReader());
            porcupine = new Porcupine(porcupineTexture);
            dragonfly = new Dragonfly(dragonflyTexture);
            squirrel = new Squirrel(squirrelTexture);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (!gameOver)
            {
                leshyLeaf.Update(gameTime);
                porcupine.Update(gameTime);
                dragonfly.Update(gameTime);
                squirrel.Update(gameTime);

                //enemy collisions
                if (leshyLeaf.GetBounds().Intersects(porcupine.GetBounds()) ||
                    leshyLeaf.GetBounds().Intersects(dragonfly.GetBounds()) ||
                    leshyLeaf.GetBounds().Intersects(squirrel.GetBounds()))
                {
                    gameOver = true;
                    backgroundColor = Color.DarkRed;
                }
            }

            base.Update(gameTime);
        }

        private void DrawTiles(SpriteBatch spriteBatch)
        {
            int tileWidth = 16;
            int tileHeight = 16;
            float scale = 5f;

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
            //cameraPosition = new Vector2(leshyLeaf.GetBounds().Center.X - GraphicsDevice.Viewport.Width / 6, 0);
            cameraPosition = new Vector2(leshyLeaf.GetBounds().Center.X - GraphicsDevice.Viewport.Width / 6, GraphicsDevice.Viewport.Height * 2
);

            //camera transform matrix
            Matrix cameraTransform = Matrix.CreateTranslation(-cameraPosition.X, -cameraPosition.Y, 0);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: cameraTransform);

            DrawTiles(_spriteBatch);
            leshyLeaf.Draw(_spriteBatch);
            porcupine.Draw(_spriteBatch);
            dragonfly.Draw(_spriteBatch);
            squirrel.Draw(_spriteBatch);

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
    }
}
