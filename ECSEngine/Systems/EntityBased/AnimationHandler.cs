﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Engine.Componets;
using Engine.ECSCore;
using PlatformLibrary;

namespace Engine.Systems
{
    public class AnimationHandler : ECSCore.System
    {
        private Dictionary<string, AnimationTracker> animationData = new Dictionary<string, AnimationTracker>();
        private ContentManager content;

        #region ECS Methods

        public override bool SetSystemRequirments(Entity entity)
        {
            return entity.HasComponent<Sprite>() &&
                   entity.HasComponent<Transform>() &&
                   entity.HasComponent<Animation>();
        }

        public AnimationHandler(ContentManager contentManager)
        {
            content = contentManager;
        }

        public override void Initialize()
        {
            string[] files = Directory.GetFiles(content.RootDirectory + "\\Tilesets", "*.xnb", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                loadFromTileSet(content.Load<Tileset>("Tilesets\\" + Path.GetFileNameWithoutExtension(file)));
                Debug.WriteLine($"Loaded {Path.GetFileName(file)} tileset");  
            }

            foreach(Entity entity in Entities)
            {
                InitializeEntity(entity);
            }
        }

        public override void InitializeEntity(Entity entity) 
        {
            getAnimation(entity);
        }

        public override void RemoveFromSystem(Entity entity) { }

        public void UpdateAnimations(GameTime gameTime)
        {
            foreach (Entity entity in Entities)
            {
                AnimationTracker current = getAnimation(entity);
                Animation animation = entity.GetComponent<Animation>();

                if (current != null)
                {
                    Sprite sprite = entity.GetComponent<Sprite>();

                    current.TimeIntoAnimation += gameTime.ElapsedGameTime.TotalSeconds;

                    if (current.TimeIntoAnimation > current.TotalDuration)
                    {
                        current.TimeIntoAnimation = 0;
                    }

                    current.FrameNumber = (int)Math.Floor(current.TimeIntoAnimation / current.FrameDuration);

                    sprite.SpriteLocation = current.CurrentFrame;
                }
            }
        }

        #endregion

        #region Private Methods

        private AnimationTracker getAnimation(Entity entity)
        {
            Animation animation = entity.GetComponent<Animation>();

            if (animation.AnimationTracker.ContainsKey(animation.CurrentAnimation))
            {
                return animation.AnimationTracker[animation.CurrentAnimation];
            }

            if (animationData.ContainsKey(animation.CurrentAnimation)) {
                AnimationTracker animationTracker = new AnimationTracker(animationData[animation.CurrentAnimation].Parent);
                animationTracker.Frames = animationData[animation.CurrentAnimation].Frames;
                animation.AnimationTracker[animation.CurrentAnimation] = animationTracker;
                return animationTracker;               
            }

            Debug.WriteLine($"Animation '{animation.CurrentAnimation}' was not found");

            return null;
        }

        private void loadFromTileSet(Tileset tileset)
        {    
            for(int i = 0; i < tileset.Count; i++)
            {
                SpriteSheetAnimations spriteSheetAnimation = new SpriteSheetAnimations();
                spriteSheetAnimation.Width = tileset[i].Width;
                spriteSheetAnimation.Height = tileset[i].Width;
                spriteSheetAnimation.Margin = 0; // change
                spriteSheetAnimation.Spacing = 0; // change

                AnimationTracker animationTracker = new AnimationTracker(spriteSheetAnimation);

                string name = tileset[i].Animation.Name;

                foreach (TilesetFrame frame in tileset[i].Animation.Frames)
                {
                    animationTracker.AddFrame(frame.Duration, frame.Source.X, frame.Source.Y);
                }

                animationData[name] = animationTracker;
            }     
        }

        #endregion
    }

    #region Animation Data Structures

    public class SpriteSheetAnimations
    {
        public List<AnimationTracker> Animations { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Margin { get; set; }
        public int Spacing { get; set; }
    }

    public class AnimationTracker
    {
        public int FrameNumber { get; set; } = 0;
        public double TimeIntoAnimation { get; set; } = 0;
        public List<Frame> Frames { get; set; }
        public SpriteSheetAnimations Parent { get; set; }

        public float FrameDuration
        {
            get
            {
                return Frames[FrameNumber].Duration;
            }
        }

        public float TotalDuration
        {
            get
            {
                float duration = 0;

                foreach(Frame frame in Frames) 
                {
                    duration += frame.Duration;
                }

                return duration;
            }
        }

        public Rectangle CurrentFrame
        {
            get
            {
                return Frames[FrameNumber].FrameLocation;
            }
        }

        public AnimationTracker(SpriteSheetAnimations parent)
        {
            this.Parent = parent;
            Frames = new List<Frame>();
        }

        public void AddFrame(float duration, int spriteX, int spriteY)
        {
            Frames.Add(new Frame(duration, spriteX, spriteY, Parent));
        }
    }

    public class Frame
    {
        public float Duration { get; set; }
        public Rectangle FrameLocation { get; set; }
        public Frame(float duration, int spriteX, int spriteY, SpriteSheetAnimations parent)
        {
            Duration = duration;
            FrameLocation = new Rectangle(spriteX * (parent.Width + parent.Spacing) + parent.Margin,
                                          spriteY * (parent.Height + parent.Spacing) + parent.Margin,
                                          parent.Width, parent.Height);
        }
    }

    #endregion
}
