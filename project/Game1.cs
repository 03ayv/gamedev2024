﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using project.Collectibles;
using project.Enemies;
using project.Input;
using project.Interfaces;
using project.Scenes;
using project.Tiles;
using SharpDX.Direct2D1.Effects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace project
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SceneManager sceneManager;

        //leshy leaf sprite
        public static LeshyLeaf LeshyLeaf { get; private set; }
        private Texture2D leshyLeafTexture;

        /*
        //enemies
        private Texture2D porcupineTexture;
        private Texture2D dragonflyTexture;
        private Texture2D squirrelTexture;
        private List<IGameObject> enemies = new List<IGameObject>();
        */

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

       /*
        //extras
        private Texture2D keyTexture;
        private Key key;
        private Texture2D coinTexture;
        private Coin coin;
        private List<Coin> coins = new List<Coin>();
       */
        private ScoreManager scoreManager;

        //manage levels
        private LevelManager levelManager;
        private LevelTransitionScreen transitionScreen;

        //start screen
        private StartScreen startScreen;
        private bool gameStarted = false;

        public static bool GameOver { get; set; }
        public static LevelManager LevelManager { get; private set; }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            GameOver = false;
            LevelManager = new LevelManager();
        }

        protected override void Initialize()
        {
            levelManager = new LevelManager();
            sceneManager = new SceneManager(GraphicsDevice, Content);
            
            //initialize scenes!
            sceneManager.AddScene("Level1", new GameScene1(GraphicsDevice, Content));
            
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

            //load tile content
            LoadTileContent();
            
            //load screens
            startScreen = new StartScreen(Content.Load<SpriteFont>("File"), GraphicsDevice);
            transitionScreen = new LevelTransitionScreen(Content.Load<SpriteFont>("File"), GraphicsDevice);
            
            //load initial scene (level 1)
            var scene = new GameScene1(GraphicsDevice, Content);
            scene.SetSpriteBatch(_spriteBatch);
            sceneManager.AddScene("Level1", scene);
            sceneManager.LoadScene("Level1");
        }

        private void InitializeGameObjects()
        {
            LeshyLeaf = new LeshyLeaf(leshyLeafTexture, new KeyboardReader(), tileManager);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (!gameStarted)
            {
                if (startScreen.Update(Vector2.Zero))
                {
                    gameStarted = true;
                }
                return;
            }

            if (levelManager.IsTransitioning)
            {
                HandleLevelTransition();
                return;
            }

            LeshyLeaf.Update(gameTime);
            UpdateCamera();
            sceneManager.Update(gameTime);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.RosyBrown);

            if (!gameStarted)
            {
                _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                startScreen.Draw(_spriteBatch, Vector2.Zero);
                _spriteBatch.End();
                return;
            }

            //main game with camera transform
            _spriteBatch.Begin(
                samplerState: SamplerState.PointClamp,
                transformMatrix: Matrix.CreateTranslation(-cameraPosition.X, -cameraPosition.Y, 0)
            );

            //draw tiles
            DrawTiles(_spriteBatch);
            
            //draw leshyleaf
            LeshyLeaf.Draw(_spriteBatch);
            
            //draw current scene
            sceneManager.Draw(gameTime);

            if (levelManager.IsTransitioning)
            {
                transitionScreen.Draw(_spriteBatch, cameraPosition);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void HandleLevelTransition()
        {
            if (transitionScreen.Update(cameraPosition))
            {
                levelManager.StartTransition();
                LoadTileContent();
                levelManager.CompleteTransition();
                string nextScene = $"Level{levelManager.CurrentLevel}";
                sceneManager.LoadScene(nextScene);
            }
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

        /*
        //generate multiple coins
        private List<Vector2> GenerateCoinPositions()
        {
            List<Vector2> positions = new List<Vector2>
            {
                new Vector2(50, 1195),
                new Vector2(150, 1195),
                new Vector2(250, 1160),
                new Vector2(400, 1120),
                new Vector2(500, 1120),
                new Vector2(650, 1195),
                new Vector2(750, 1195),
                new Vector2(850, 1195),
                new Vector2(930, 1100),
                new Vector2(1050, 1035),
                new Vector2(1150, 1035),
                new Vector2(1250, 1035),
                new Vector2(1350, 1000),
                new Vector2(1400, 970),
                new Vector2(1450, 950),
                new Vector2(1500, 940),
                new Vector2(1550, 930),
                new Vector2(1450, 1195),
                new Vector2(1550, 1195),
                new Vector2(1650, 1195),
            };
            return positions;
        }
        */

        private void LoadTileContent()
        {
            //load tiles
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

            //load csv for correct level
            tileMap1 = LoadTileMap("Data/Forest_TileLayer1.csv");
            tileMap2 = LoadTileMap("Data/Forest_TileLayer2.csv");
            tileMap3 = LoadTileMap("Data/Forest_TileLayer3.csv");
            tileMap4 = LoadTileMap("Data/Forest_TileLayer4.csv");
            tileMap5 = LoadTileMap("Data/Forest_TileLayer5.csv");
            
            tileManager = new TileManager(tileMap4, 16, 16, 5f);

            /*
            //game object textures
            leshyLeafTexture = Content.Load<Texture2D>("LeshyLeaf");
            porcupineTexture = Content.Load<Texture2D>("Porcupine");
            dragonflyTexture = Content.Load<Texture2D>("Dragonfly");
            squirrelTexture = Content.Load<Texture2D>("Squirrel");
            //extras
            keyTexture = Content.Load<Texture2D>("Key");
            coinTexture = Content.Load<Texture2D>("Key"); //key = dungeon collectibles
            */
            InitializeGameObjects();

            //initialize tile lists
            bgTiles = CreateTileRectangles(bg1Texture, tileWidth, tileHeight);
            decorTiles = CreateTileRectangles(decorsTexture, tileWidth, tileHeight);
            tilesetTiles = CreateTileRectangles(tilesetTexture, tileWidth, tileHeight);

            //score manager
            scoreManager = new ScoreManager(Content.Load<SpriteFont>("File"));

            //initialize leshyleaf before scenes
            leshyLeafTexture = Content.Load<Texture2D>("LeshyLeaf");
            LeshyLeaf = new LeshyLeaf(leshyLeafTexture, new KeyboardReader(), tileManager);
        }

        private void UpdateCamera()
        {
            cameraPosition = new Vector2(
                LeshyLeaf.GetBounds().Center.X - GraphicsDevice.Viewport.Width / 3,
                GraphicsDevice.Viewport.Height * 1.9f
            );
            ClampCamera();
        }
    }
}
