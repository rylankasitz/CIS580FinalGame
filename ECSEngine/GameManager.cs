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

        private Renderer renderer;
        private CollisionHandler collisionHandler;
        private PhysicsHandler physicsHandler;
        private AnimationHandler animationHandler;
        private StateHandler stateHandler;
        private ParticleSystemHandler particleSystemHandler;
        private ParallaxHandler parallaxHandler;

        public GameManager(GameLayout game, string contentDirectory)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = contentDirectory;

            Template = new Template();

            gameLayout = game;

            // Systems
            systems.Add(renderer = new Renderer());
            systems.Add(collisionHandler = new CollisionHandler());
            systems.Add(physicsHandler = new PhysicsHandler());
            systems.Add(animationHandler = new AnimationHandler(Content));
            systems.Add(stateHandler = new StateHandler());
            systems.Add(particleSystemHandler = new ParticleSystemHandler());
            systems.Add(parallaxHandler = new ParallaxHandler());
        }

        #region Monogame Methods

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = WindowWidth;
            graphics.PreferredBackBufferHeight = WindowHeight;
            graphics.ApplyChanges();

            WindowWidth = WindowManager.Width;
            WindowHeight = WindowManager.Width;

            Camera.Intitialize(GraphicsDevice.Viewport, WindowWidth, WindowHeight);
            MapManager.Content = Content;

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

            AudioManager.LoadContent(Content);

            particleSystemHandler.LoadContent(Content);
            renderer.LoadContent(Content, textures);
            parallaxHandler.LoadContent(textures);
        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {
            InputManager.NewKeyboardState = Keyboard.GetState();
            InputManager.NewMouseState = Mouse.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            physicsHandler.HandlePhysics();
            collisionHandler.CheckCollisions();
            animationHandler.UpdateAnimations(gameTime);
            stateHandler.UpdateStateMachine(gameTime);
            particleSystemHandler.UpdateParticleSystems(gameTime);
            parallaxHandler.UpdateParallax(gameTime);

            SceneManager.UpdateScene(gameTime);

            InputManager.OldKeyboardState = InputManager.NewKeyboardState;
            InputManager.OldMouseState = InputManager.NewMouseState;

            gameLayout.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(WindowManager.BackgroundColor);

            parallaxHandler.Draw(spriteBatch, this);

            spriteBatch.Begin(SpriteSortMode.BackToFront, null, SamplerState.PointClamp, null, null, null, Camera.GetTransformation());

            renderer.Draw(spriteBatch);

            particleSystemHandler.DrawParticleSystems(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion
    }
}
