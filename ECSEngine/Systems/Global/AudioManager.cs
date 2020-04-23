using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Engine.ECSCore;
using Newtonsoft.Json;

namespace Engine.Systems
{
    public static class AudioManager
    {
        private static Dictionary<string, SoundEffect> soundEffects;

        public static void LoadContent(ContentManager content)
        {
            soundEffects = new Dictionary<string, SoundEffect>();

            string[] files = Directory.GetFiles(content.RootDirectory + "\\Audio", "*.xnb", SearchOption.AllDirectories);

            for (int i = 0; i < files.Length; i++)
            {
                string filename = Path.GetFileNameWithoutExtension(files[i]);
                soundEffects[filename] = content.Load<SoundEffect>("Audio\\" + filename);
            }
        }

        public static void Play(string audioName)
        {
            if (soundEffects.ContainsKey(audioName))
            {
                soundEffects[audioName].Play();
            }
            else
            {
                Debug.WriteLine("Failed to find audio clip " + audioName);
            }   
        }

    }
}
