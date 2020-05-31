using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Engine.ECSCore;
using Engine.Systems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ECSEngine.Systems;
using Comora;
using System.Diagnostics;
using Penumbra;
using ECSEngine.Systems.EntityBased;
using GeonBit.UI;
using GeonBit.UI.Entities;

namespace Engine
{
    public class GameManager : Game
    {
        public int WindowWidth { get; set; } = 1280;
        public int WindowHeight { get; set; } = 720;

        public Template Template { get; set; }

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private List<ECSCore.System> systems = new List<ECSCore.System>();

        private GameLayout gameLayout;

        private Camera camera;
        private PenumbraComponent penumbra;

        private Renderer renderer;
        private CollisionHandler collisionHandler;
        private PhysicsHandler physicsHandler;
        private AnimationHandler animationHandler;
        private LightHandler lightHandler;

        public GameManager(GameLayout game, string contentDirectory)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = contentDirectory;

            Template = new Template();
            penumbra = new PenumbraComponent(this);

            Services.AddService(penumbra);

            gameLayout = game;

            // Systems
            systems.Add(renderer = new Renderer());
            systems.Add(collisionHandler = new CollisionHandler());
            systems.Add(physicsHandler = new PhysicsHandler());
            systems.Add(animationHandler = new AnimationHandler(Content));
            systems.Add(lightHandler = new LightHandler(penumbra));
        }

        #region Monogame Methods

        protected override void Initialize()
        {
            // Set window size
            WindowWidth = WindowManager.Width;
            WindowHeight = WindowManager.Height;

            graphics.PreferredBackBufferWidth = WindowWidth;
            graphics.PreferredBackBufferHeight = WindowHeight;
            graphics.ApplyChanges();

            // Initialize Camera
            camera = new Camera(GraphicsDevice);
            camera.Position = new Vector2(WindowManager.Width / 2, WindowHeight / 2);

            // Initialize GUI
            UserInterface.Initialize(Content, BuiltinThemes.lowres);

            // Set constants
            WindowManager.Camera = camera;
            WindowManager.Penumbra = penumbra;
            MapManager.Content = Content;

            // Initialize scenes
            gameLayout.AddScenes();

            SceneManager.systems = systems;
            SceneManager.Initialize(this);

            gameLayout.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

            spriteBatch = new SpriteBatch(GraphicsDevice);

            string[] files = Directory.GetFiles(Content.RootDirectory + "\\Sprites", "*.xnb", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                string filename = Path.GetFileNameWithoutExtension(file);
                textures[filename] = Content.Load<Texture2D>("Sprites\\" + filename);
            }

            penumbra.Initialize();
            camera.LoadContent();
            AudioManager.LoadContent(Content);
            renderer.LoadContent(Content, textures);
        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {
            // Debug Details
            camera.Debug.IsVisible = WindowManager.ShowCamerDetails;
            penumbra.Debug = WindowManager.ShowLightDetails;

            // Input States
            InputManager.NewKeyboardState = Keyboard.GetState();
            InputManager.NewMouseState = Mouse.GetState();
            InputManager.NewGamePadState = GamePad.GetState(PlayerIndex.One);

            // Update Libraries
            camera.Update(gameTime);
            UserInterface.Active.Update(gameTime);

            // Update Scene
            SceneManager.UpdateScene(gameTime);

            // System Updates
            physicsHandler.HandlePhysics();
            collisionHandler.CheckCollisions();
            animationHandler.UpdateAnimations(gameTime);
            lightHandler.UpdateLights();

            // Update Game
            gameLayout.Update(gameTime);

            // Old Input Stats
            InputManager.OldKeyboardState = InputManager.NewKeyboardState;
            InputManager.OldMouseState = InputManager.NewMouseState;
            InputManager.OldGamePadState = InputManager.NewGamePadState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            penumbra.BeginDraw();

            GraphicsDevice.Clear(WindowManager.BackgroundColor);

            // Draw Game
            spriteBatch.Begin(camera, SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);
            renderer.Draw(spriteBatch, penumbra);
            spriteBatch.End();

            // Update Lighting
            penumbra.Draw(gameTime);

            // Draw UI
            UserInterface.Active.Draw(spriteBatch);

            // Draw Debug
            spriteBatch.Draw(WindowManager.Camera.Debug);

            base.Draw(gameTime);
        }

        #endregion
    }
}
