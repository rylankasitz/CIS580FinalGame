using Engine.Componets;
using Engine.ECSCore;
using Humper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using PlatformLibrary;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

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
                   entity.HasComponent<Componets.Transform>() &&
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
                Animation animation = entity.GetComponent<Animation>();

                if (animation.Enabled)
                {
                    if (animation.Playing)
                        animation.CurrentAnimation = animation.PlayeringAnimation;

                    AnimationTracker current = getAnimation(entity);

                    if (current != null)
                    {
                        Sprite sprite = entity.GetComponent<Sprite>();

                        current.TimeIntoAnimation += gameTime.ElapsedGameTime.TotalSeconds;

                        if (current.TimeIntoAnimation >= current.FrameDuration)
                        {
                            current.FrameNumber++;
                            current.TimeIntoAnimation = 0;

                            if (current.FrameNumber == current.Frames.Count)
                                current.FrameNumber = 0;

                            setCurrentCollisions(animation, entity, current);
                        }

                        sprite.SpriteLocation = current.CurrentFrame;

                        if (current.FrameNumber == current.Frames.Count - 1 && animation.Playing)
                        {
                            animation.Playing = false;
                            current.FrameNumber = 0;
                        }
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        private void setCurrentCollisions(Animation animation, Entity entity, AnimationTracker current)
        {
            animation.CurrentCollisions = new Dictionary<string, BoxCollision>();
            foreach (KeyValuePair<string, BoxCollision> pair in current.Frames[current.FrameNumber].Colliders)
            {
                if (pair.Key == "Collision")
                {
                    BoxCollision boxCollision = entity.GetComponent<BoxCollision>();
                    Componets.Transform transform = entity.GetComponent<Componets.Transform>();

                    List<IBox> newBoxes = new List<IBox>();

                    for (int i = 0; i < pair.Value.Boxes.Count; i++)
                    {
                        BoxData boxData = (BoxData) pair.Value.Boxes[i].Data;
                        IBox newIbox = boxCollision.World.Create(transform.Position.X + transform.Scale.X * boxData.Position.X,
                                                                 transform.Position.Y + transform.Scale.Y * boxData.Position.Y,
                                                                 transform.Scale.X * boxData.Scale.X,
                                                                 transform.Scale.Y * boxData.Scale.Y).AddTags(boxData.Layer);
                        newIbox.Data = boxData;

                        if (boxCollision.Boxes.Count > i) 
                        {
                            boxCollision.World.Remove(boxCollision.Boxes[i]);
                            boxCollision.Boxes.Remove(boxCollision.Boxes[i]);
                        }

                        newBoxes.Add(newIbox);
                    }

                    boxCollision.Boxes = newBoxes;
                }
                else
                {
                    animation.CurrentCollisions.Add(pair.Key, pair.Value);
                }
            }
        }

        private AnimationTracker getAnimation(Entity entity)
        {
            Animation animation = entity.GetComponent<Animation>();

            if (animation.AnimationTracker.ContainsKey(animation.CurrentAnimation))
            {
                animation.AnimationScale = new Vector(animation.AnimationTracker[animation.CurrentAnimation].Parent.Width, 
                    animation.AnimationTracker[animation.CurrentAnimation].Parent.Height);
                return animation.AnimationTracker[animation.CurrentAnimation];
            }

            if (animationData.ContainsKey(animation.CurrentAnimation)) {
                AnimationTracker animationTracker = new AnimationTracker(animationData[animation.CurrentAnimation].Parent);
                animationTracker.Frames = animationData[animation.CurrentAnimation].Frames;
                animation.AnimationTracker[animation.CurrentAnimation] = animationTracker;
                animation.AnimationScale = new Vector(animationTracker.Parent.Width, animationTracker.Parent.Height);
                return animationTracker;               
            }

            //Debug.WriteLine($"Animation '{animation.CurrentAnimation}' was not found for entity '{entity.Name}'");

            return null;
        }

        private void loadFromTileSet(Tileset tileset)
        {    
            for(int i = 0; i < tileset.Count; i++)
            {
                SpriteSheetAnimations spriteSheetAnimation = new SpriteSheetAnimations();
                spriteSheetAnimation.Width = tileset[i].Width;
                spriteSheetAnimation.Height = tileset[i].Height;
                spriteSheetAnimation.Margin = tileset[i].Margin;
                spriteSheetAnimation.Spacing = tileset[i].Spacing;

                AnimationTracker animationTracker = new AnimationTracker(spriteSheetAnimation);

                string name = tileset[i].Animation.Name;

                foreach (TilesetFrame frame in tileset[i].Animation.Frames)
                {
                    Dictionary<string, BoxCollision> colliders = new Dictionary<string, BoxCollision>();
                    foreach(KeyValuePair<string, List<BoxCol>> collider in tileset[frame.Id].BoxColliders)
                    {
                        BoxCollision collision = new BoxCollision();
                        collision.World = SceneManager.GetCurrentScene().World;
                        for (int j = 0; j < collider.Value.Count; j++)
                        {

                            BoxData box = new BoxData();
                            box.Position = new Vector(collider.Value[j].Rectangle.X / (float)tileset[frame.Id].Width,
                                                      collider.Value[j].Rectangle.Y / (float)tileset[frame.Id].Height);
                            box.Scale = new Vector(collider.Value[j].Rectangle.Width / (float)tileset[frame.Id].Width,
                                                   collider.Value[j].Rectangle.Height / (float)tileset[frame.Id].Height);
                            box.TriggerOnly = collider.Value[j].TriggerOnly;

                            IBox ibox = collision.World.Create(0, 0, box.Scale.X, box.Scale.Y).AddTags(box.Layer);
                            ibox.Data = box;
                            collision.World.Remove(ibox);

                            collision.Boxes.Add(ibox);

                        }
                        colliders.Add(collider.Key, collision);
                    }

                    animationTracker.AddFrame(frame.Duration, frame.Source.X, frame.Source.Y, colliders);
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

        public void AddFrame(float duration, int spriteX, int spriteY, Dictionary<string, BoxCollision> colliders)
        {
            Frames.Add(new Frame(duration, spriteX, spriteY, Parent, colliders));
        }
    }

    public class Frame
    {
        public float Duration { get; set; }
        public Rectangle FrameLocation { get; set; }
        public Dictionary<string, BoxCollision> Colliders { get; set; }
        public Frame(float duration, int spriteX, int spriteY, SpriteSheetAnimations parent, Dictionary<string, BoxCollision> colliders)
        {
            Duration = duration;
            FrameLocation = new Rectangle(spriteX * (parent.Width + parent.Spacing) + parent.Margin,
                                          spriteY * (parent.Height + parent.Spacing) + parent.Margin,
                                          parent.Width, parent.Height);
            Colliders = colliders;
        }
    }

    #endregion
}
