using Engine;
using Engine.Componets;
using Engine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameWindowsStarter.Characters;
using MonoGameWindowsStarter.Characters.Helpers;
using MonoGameWindowsStarter.Entities;
using MonoGameWindowsStarter.GlobalValues;
using MonoGameWindowsStarter.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Scenes.Rooms
{
    public class MapGenerator
    {
        public Room[] Rooms { get; set; }

        private int roomCount = 6;
        private int maxRoomSize = 24;
        private int minRoomSize = 12;
        private int minRoomDistance = 5;
        private float spaceModifier = 1f;

        private Random random;
        
        public MapGenerator()
        {
            Rooms = new Room[roomCount];
            random = new Random();

            createRooms();
            createHallways();
        }

        public void LoadFloor()
        {
            foreach(Room room in Rooms)
            {
                room.Load();
            }
        }

        private void createRooms()
        {
            int i = 0;
            while(i < roomCount)
            {
                bool intersects = false;
                Room room = new Room(random.Next(0, (int)(maxRoomSize * roomCount * spaceModifier)),
                                     random.Next(0, (int)(maxRoomSize * roomCount * spaceModifier)),
                                     random.Next(minRoomSize, maxRoomSize),
                                     random.Next(minRoomSize, maxRoomSize), i);

                for (int j = 0; j < i; j++)
                {
                    if (room.Instercects(Rooms[j], minRoomDistance)) 
                        intersects = true;
                }

                if (!intersects)
                {
                    Rooms[i] = room;
                    i++;
                }
            }
        }

        private void createHallways()
        {
            for (int i = 0; i < roomCount; i++)
            {
                List<Room> otherRooms = new List<Room>(Rooms);
                otherRooms.Remove(Rooms[i]);

                Room closestRoom = Rooms[i].FindClosestRoom(otherRooms.ToArray());

                while (closestRoom != null && closestRoom.HasConnection(Rooms[i]))
                {
                    otherRooms.Remove(closestRoom);
                    closestRoom = Rooms[i].FindClosestRoom(otherRooms.ToArray());
                }

                if (closestRoom != null)
                {
                    Rooms[i].HallWays.Add(new HallWay(Rooms[i], closestRoom));
                }
            }
        }
    }
}
