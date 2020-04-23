using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Utils
{
    public class AnimationState
    {
        public string Idle { get { return "Idle" + currentCharacter;  } }
        public string Walk { get { return "Walk" + currentCharacter; } }

        private string currentCharacter = "Default";

        public AnimationState(string character)
        {
            currentCharacter = character;
        }

        public void UpdateCharacter(string character)
        {
            currentCharacter = character;
        }
    }
}
