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
        DungeonScene dungeonScene;
        StartScene playScene;

        public override void AddScenes()
        {
            SceneManager.AddScene(dungeonScene = new DungeonScene());
            SceneManager.AddScene(playScene = new StartScene());

            dungeonScene.Name = "Dugneon";
            playScene.Name = "Play";
        }

        public override void Initialize()
        {
            SceneManager.LoadScene("Play");

            WindowManager.BackgroundColor = new Color(25, 23, 22);
            WindowManager.AmbientColor = Color.Gray;

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
            // Toggle Debug
            if (InputManager.KeyDown(Keys.F1))
            {
                WindowManager.ShowCamerDetails = !WindowManager.ShowCamerDetails;
                WindowManager.ShowLightDetails = !WindowManager.ShowLightDetails;
            }
        }
    }
}
