using ECSEngine.Systems;
using Engine;
using Engine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameWindowsStarter.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter
{
    public class Game : GameLayout
    {
        MainScene mainScene;
        GameoverScene gameoverScene;
        PlayScene playScene;
        EndScreen endScreen;

        public override void AddScenes()
        {
            SceneManager.AddScene(mainScene = new MainScene());
            SceneManager.AddScene(gameoverScene = new GameoverScene());
            SceneManager.AddScene(playScene = new PlayScene());
            SceneManager.AddScene(endScreen = new EndScreen());

            mainScene.Name = "Main";
            gameoverScene.Name = "Gameover";
            playScene.Name = "Play";
            endScreen.Name = "End Screen";
        }

        public override void Initialize()
        {
            SceneManager.LoadScene("Play");

            Camera.Position.X = WindowManager.Width / 2;
            Camera.Position.Y = WindowManager.Height / 2;

            WindowManager.BackgroundColor = new Color(25, 23, 22);

            //WindowManager.Debug = true;

            // Add input axises in code temperary
            InputManager.InputAxises.Add("MoveY", new InputAxis(PositiveKey: Keys.S, NegativeKey: Keys.W, LeftThumbStickY: true));
            InputManager.InputAxises.Add("MoveX", new InputAxis(PositiveKey: Keys.D, NegativeKey: Keys.A, LeftThumbStickX: true));
            InputManager.InputAxises.Add("Roll", new InputAxis(PositiveKey: Keys.Space, PositiveButton: Buttons.B));
            InputManager.InputAxises.Add("Attack", new InputAxis(PositiveKey: Keys.R, PositiveButton: Buttons.RightTrigger));
            InputManager.InputAxises.Add("AimY", new InputAxis(PositiveKey: Keys.Up, NegativeKey: Keys.Down, RightThumbStickY: true));
            InputManager.InputAxises.Add("AimX", new InputAxis(PositiveKey: Keys.Left, NegativeKey: Keys.Right, RightThumbStickX: true));
            InputManager.InputAxises.Add("CharacterPickup", new InputAxis(PositiveKey: Keys.Q, PositiveButton: Buttons.X));
            InputManager.InputAxises.Add("Start", new InputAxis(PositiveKey: Keys.Enter, PositiveButton: Buttons.Start));
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
