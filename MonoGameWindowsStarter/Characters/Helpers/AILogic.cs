using Engine;
using Engine.Componets;
using Microsoft.Xna.Framework;
using MonoGameWindowsStarter.Entities;
using MonoGameWindowsStarter.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Characters.Helpers
{
    public class AILogic
    {
        #region Public Variables

        public string CurrentState { get; set; } = "Start";
        public Enemy AI;

        #endregion

        #region Private Variables

        private Player player;
        private Random random;

        private float movementElapsedTime;
        private float attackElapsedTime;
        private float waitElapsedTime;

        private float attackDuration;
        private float movementDuration;
        private float waitDuration;

        private float attackAccuracy;
        private Vector movementDirection;

        private bool stateSwitch;

        #endregion

        #region ECS Methods

        public AILogic(Enemy character)
        {
            AI = character;
            MainScene scene = (MainScene)SceneManager.GetCurrentScene();
            player = scene.Player;
            random = new Random();

            movementElapsedTime = 0;
            attackElapsedTime = 0;
            waitElapsedTime = 0;

            attackDuration = 0;
            movementDuration = 0;
            waitDuration = 0;

            movementDirection = new Vector(0, 0);

            stateSwitch = true;
        }

        public void Update(GameTime gameTime)
        {
            // State Switch
            if (stateSwitch)
            {
                stateSwitch = false;
                AI.Character.OnStateSwitch(CurrentState);
            }

            // Attack
            if (attackElapsedTime < attackDuration)
            {
                Vector2 dir = getPlayerDirection() * 
                    new Vector2(random.Next(1, (int)((1 - attackAccuracy) * 100) + 2), 
                                random.Next(1, (int)((1 - attackAccuracy) * 100) + 2));
                dir.Normalize();

                AI.Character.Attack(AI, AI.Transform.Position, new Vector(dir.X, dir.Y));
            }
            else if (attackDuration != 0)
            {
                stateSwitch = true;
                attackDuration = 0;
            }

            // Move
            if (movementElapsedTime < movementDuration)
            {
                AI.Physics.Velocity = movementDirection * AI.Character.MoveSpeed * AI.Character.AIMoveSpeedMod;
            }
            else if (movementDuration != 0)
            {
                AI.Physics.Velocity = new Vector(0, 0);
                stateSwitch = true;
                movementDuration = 0;
            }

            if (waitElapsedTime > waitDuration && waitDuration != 0)
            {
                stateSwitch = true;
                waitDuration = 0;
            }


            movementElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            attackElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            waitElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        #endregion

        #region AI Templates

        public void BasicMovementTemplate(string lastState, Vector attackRange, float accuracy)
        {
            switch (lastState)
            {
                case "Start":
                    Wait(.1f);
                    CurrentState = "Waiting";
                    break;

                case "Waiting":
                    Wait(.5f);
                    if (InRangeOfPlayer((int)attackRange.X, (int)attackRange.Y))
                        CurrentState = "Attacking";
                    else
                        CurrentState = "Moving";
                    break;

                case "Moving":
                    MoveToPlayer(.3f);
                    if (InRangeOfPlayer((int)attackRange.X, (int)attackRange.Y))
                        CurrentState = "Attacking";
                    else
                        CurrentState = "Waiting";
                    break;

                case "Attacking":
                    AttackPlayer(.3f, accuracy);
                    if (InRangeOfPlayer((int)attackRange.X, (int)attackRange.Y))
                        CurrentState = "Waiting";
                    else
                        CurrentState = "Moving";
                    break;

                default:
                    break;
            }
        }

        #endregion

        #region Custom AI Methods

        public void MoveToPlayer(float duration)
        {
            MoveTo(player.Transform.Position, duration);
        }

        public void MoveTo(Vector position, float duration)
        {          
            Vector2 d = position - AI.Transform.Position;
            d.Normalize();

            movementDirection = new Vector(d.X, d.Y);
            movementDuration = duration;
            movementElapsedTime = 0;
        }

        public void AttackPlayer(float duration, float accuracy)
        {
            attackDuration = duration;
            attackElapsedTime = 0;
            attackAccuracy = accuracy;
        }

        public void Wait(float waitTime)
        {
            waitDuration = waitTime;
            waitElapsedTime = 0;
        }

        public bool InRangeOfPlayer(int distance)
        {
            Vector2 diff = player.Transform.Position - AI.Transform.Position;
            int magnitude = (int)diff.Length();

            return magnitude < distance;
        }

        public bool InRangeOfPlayer(int distanceX, int distanceY)
        {
            Vector2 diff = player.Transform.Position - AI.Transform.Position;

            return Math.Abs(diff.X) < distanceX && Math.Abs(diff.Y) < distanceY;
        }

        #endregion

        #region Private Methods

        private Vector getPlayerDirection()
        {
            Vector2 d = (player.Transform.Position + player.Transform.Scale/2) - 
                (AI.Transform.Position + AI.Transform.Scale /2);
            d.Normalize();
            return new Vector(d.X, d.Y);
        }

        private Vector randomPointOnCircle(Vector position, int radius)
        {
            int angle = random.Next(360);
            int x = (int)(position.X + radius * Math.Cos(angle));
            int y = (int)(position.Y + radius * Math.Sin(angle));

            return new Vector(x, y);
        }

        #endregion
    }
}
