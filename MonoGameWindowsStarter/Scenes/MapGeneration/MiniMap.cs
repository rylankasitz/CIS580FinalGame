using ECSEngine.Systems;
using Engine;
using Engine.Componets;
using Engine.ECSCore;
using Engine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameWindowsStarter.Scenes;
using MonoGameWindowsStarter.Scenes.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Scenes.Rooms
{
    public class MiniMap
    {
        private Vector offset = new Vector(-WindowManager.Width, -WindowManager.Height);
        private float scale = 1.5f;

        private Room[] rooms;
        private List<MiniMapObj> miniMapObjs;

        public MiniMap(Room[] rooms) 
        {
            this.rooms = rooms;

            miniMapObjs = new List<MiniMapObj>();
            offset += Camera.Position;

            createMinimap();
        }

        public void MoveTo(Vector position)
        {
            foreach(MiniMapObj miniMapObj in miniMapObjs)
            {
                miniMapObj.GetComponent<Transform>().Position += position;
            }
        }

        private void createMinimap()
        {
            for (int i = 0; i < rooms.Length; i++)
            {              
                miniMapObjs.Add(createMinimapObj(new Vector(rooms[i].Dimensions.X, rooms[i].Dimensions.Y),
                                                 new Vector(rooms[i].Dimensions.Width, rooms[i].Dimensions.Height)));

                foreach(HallWay hallWay in rooms[i].HallWays)
                {
                    miniMapObjs.Add(createMinimapObj(new Vector(hallWay.HorizonalConnector.X, hallWay.HorizonalConnector.Y),
                                                     new Vector(hallWay.HorizonalConnector.Width, hallWay.HorizonalConnector.Height)));
                    miniMapObjs.Add(createMinimapObj(new Vector(hallWay.VerticalConnector.X, hallWay.VerticalConnector.Y),
                                                     new Vector(hallWay.VerticalConnector.Width, hallWay.VerticalConnector.Height)));
                }
            }
        }

        private MiniMapObj createMinimapObj(Vector pos, Vector sc)
        {
            MiniMapObj obj = SceneManager.GetCurrentScene().CreateEntity<MiniMapObj>();
            obj.Transform.Position = new Vector(pos.X, pos.Y) * scale + offset;
            obj.Transform.Scale = new Vector(sc.X, sc.Y) * scale;

            return obj;
        }
    }

    [Sprite(ContentName: "MiniMapRoom", Layer: .12f)]
    [Transform(X: 0, Y: 0, Width: 6, Height: 6)]
    public class MiniMapObj : Entity
    {
        public Transform Transform;
        public Sprite Sprite;

        public override void Initialize()
        {
            Transform = GetComponent<Transform>();
            Sprite = GetComponent<Sprite>();
        }

        public override void Update(GameTime gameTime)
        {
           
        }
    }
}
