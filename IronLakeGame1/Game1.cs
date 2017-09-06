using System.Collections.Generic;
using System.Linq;
using IronLake;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace IronLakeGame1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameScene _gameScene;

        public Game1()
        {
            _gameScene = new GameScene(this);
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //MakePlayer
            const int playerWidth = 40;
            const int playerHeight = 40;

            var playerTexture = new Texture2D(GraphicsDevice, playerWidth, playerHeight);
            var playerTextureData = new Color[playerWidth * playerHeight];
            for (var i = 0; i < playerTextureData.Length; ++i)
                playerTextureData[i] = Color.Chocolate;

            playerTexture.SetData(playerTextureData);

            _gameScene.GameObjects.Add(new Player()
                .SetPosition(new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2))
                .Add(new SpriteRenderer(playerTexture))
                .Add(new BoxCollider(playerWidth, playerHeight)));

            //MakeBoxes
            const int boxWidth = 80;
            const int boxHeight = 80;

            var texture2D = new Texture2D(GraphicsDevice, boxWidth, boxHeight);
            var data = new Color[boxWidth * boxHeight];
            for (var i = 0; i < data.Length; ++i)
                data[i] = Color.Red;

            texture2D.SetData(data);

            _gameScene.GameObjects.Add(new Box()
                .Add(new SpriteRenderer(texture2D))
                .Add(new BoxCollider(boxWidth, boxHeight)));

            _gameScene.GameObjects.Add(new Box()
                .SetPosition(new Vector2(GraphicsDevice.Viewport.Width - 80, GraphicsDevice.Viewport.Height - 80))
                .Add(new SpriteRenderer(texture2D))
                .Add(new BoxCollider(boxWidth, boxHeight)));
            
            _gameScene.GameObjects.ForEach(go => go.Activate(_gameScene));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (!IsActive)
                return;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            _gameScene.GameObjects.ForEach(go => go.Update(gameTime.ElapsedGameTime.TotalSeconds, (GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height)));

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _gameScene.GameObjects.ForEach(go =>
            {
                var sr = go.GetComponent<SpriteRenderer>();
                _spriteBatch.Draw(sr.Texture2D, go.Transform.Position, Color.White);
            });
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}