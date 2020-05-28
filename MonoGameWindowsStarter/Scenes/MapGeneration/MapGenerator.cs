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
        private Room currentRoom;
        private HallWay currentHallway;

        private List<HallWay> hallways;
        
        public MapGenerator()
        {
            Rooms = new Room[roomCount];
            random = new Random();
            hallways = new List<HallWay>();

            createRooms();
            createHallways();
        }

        #region Public Methods

        public void LoadFloor()
        {
            Rooms[0].Load();
            currentRoom = Rooms[0];

            foreach(Room room in Rooms)
            {
                room.Load();
            }
        }

        public void UpdateFloor(Player player)
        {
            foreach(Room room in Rooms)
            {
                if (currentRoom != room && room.Intersects(player))
                {
                    currentHallway.Unload();
                    room.Load();
                    currentRoom = room;
                    currentHallway = null;
                    break;
                }
            }

            foreach(HallWay hallWay in hallways)
            {
                if (currentHallway != hallWay && hallWay.Intersects(player))
                {
                    currentRoom.Unload();
                    hallWay.Load();
                    currentHallway = hallWay;
                    currentRoom = null;
                    break;
                }
            }
        }

        #endregion

        #region Private Methods

        private void createRooms()
        {
            int i = 0;
            while (i < roomCount)
            {
                bool intersects = false;
                Room room = new Room(random.Next(0, (int)(maxRoomSize * roomCount * spaceModifier)),
                                     random.Next(0, (int)(maxRoomSize * roomCount * spaceModifier)),
                                     random.Next(minRoomSize, maxRoomSize),
                                     random.Next(minRoomSize, maxRoomSize), i);

                for (int j = 0; j < i; j++)
                {
                    if (room.Intersects(Rooms[j], minRoomDistance))
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

                while(closestRoom != null && !connectRooms(Rooms[i], closestRoom))
                {
                    otherRooms.Remove(closestRoom);
                    closestRoom = Rooms[i].FindClosestRoom(otherRooms.ToArray());
                }
            }
        }

        private bool connectRooms(Room room1, Room room2)
        {
            HallWay hallWay = new HallWay(room1, room2);

            if (!hallWay.Intersects(Rooms))
            {
                hallWay.SetDoors();
                room1.HallWays.Add(hallWay);
                hallways.Add(hallWay);

                return true;
            }

            return false;
        }

        #endregion
    }
}
