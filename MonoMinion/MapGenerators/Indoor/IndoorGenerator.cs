/*
 * BASED OFF OF
 * - http://bigbadwofl.me/random-dungeon-generator/
 * - http://jsfiddle.net/bigbadwaffle/YeazH/
 */

using Microsoft.Xna.Framework;
using MonoMinion.Helpers;
using System;
using System.Collections.Generic;

namespace MonoMinion.MapGenerators.Indoor
{
    public class IndoorGenerator
    {
        public readonly int ROOM_CLOSEST_START = 1000;

        public readonly int CONDENSE_ITERATIONS;

        protected Point areaSize;
        protected List<Rectangle> rooms;
        // TODO: Refactor this into an enum
        protected bool?[,] map;
        public bool?[,] Map { get { return map; } }

        public int RoomCount { get { return rooms.Count; } }

        public IndoorGenerator(Point size, int iterations)
        {
            CONDENSE_ITERATIONS = iterations;

            areaSize = size;
            rooms = new List<Rectangle>();
            map = new bool?[size.X, size.Y];
        }

        public static IndoorGenerator GenerateMap(Point mapSize, Point roomCountRange, Point minRoomSize, Point maxRoomSize, int condenseIterations, int maxAttemptsPerRoom = 100, bool skipCorridors = false)
        {
            // Verify ranges
            if (mapSize.Equals(Point.Zero))
                throw new Exception("Map size is invalid or too small.");
            if (roomCountRange.Equals(Point.Zero) || roomCountRange.X > roomCountRange.Y)
                throw new Exception("Room count range is invalid or too small.");
            if (minRoomSize.Equals(Point.Zero) || (minRoomSize.X > maxRoomSize.X || minRoomSize.Y > maxRoomSize.Y))
                throw new Exception("Room size range is invalid or too small.");

            IndoorGenerator mapGen = new IndoorGenerator(mapSize, condenseIterations);
            int roomCount = Minion.Instance.Random.Next(roomCountRange.X, roomCountRange.Y + 1);

            // Generate the rooms
            int roomAttempts = 0;
            for (int i = 0; i < roomCount; i++)
            {
                // TODO: Refactor this travesty
                // If we've exceeded the generation maxAttemptsPerRoom we stop so we don't go into an infinite loop
                if (roomAttempts > roomCount * maxAttemptsPerRoom)
                {
                    roomCount = mapGen.rooms.Count;
                    break;
                }
                roomAttempts++;

                Rectangle room = new Rectangle(
                    Minion.Instance.Random.Next(1, mapSize.X - maxRoomSize.X),
                    Minion.Instance.Random.Next(1, mapSize.Y - maxRoomSize.Y),
                    Minion.Instance.Random.Next(minRoomSize.X, maxRoomSize.X),
                    Minion.Instance.Random.Next(minRoomSize.Y, maxRoomSize.Y)
                );

                // If we collide we regenerate the room
                if (mapGen.doesCollide(room))
                {
                    i--;
                    continue;
                }
                // Decrease the size of the room by one to make sure no rooms are directly next to each other
                room.Width--;
                room.Height--;
                mapGen.rooms.Add(room);
            }

            // Condense the area by moving the rooms closer to each other
            mapGen.condenseArea(condenseIterations);

            // Make every room tile into a floor tile
            for (int i = 0; i < roomCount; i++)
            {
                for (int x = mapGen.rooms[i].X; x < mapGen.rooms[i].X + mapGen.rooms[i].Width; x++)
                {
                    for (int y = mapGen.rooms[i].Y; y < mapGen.rooms[i].Y + mapGen.rooms[i].Height; y++)
                    {
                        mapGen.map[x, y] = false;
                    }
                }
            }

            
            // TODO: Make these into a separate class (or use Rectangle) instead of applying the corridors directly to the map grid
            if (!skipCorridors)
            {
                // Build corridors between rooms
                for (int i = 0; i < roomCount; i++)
                {
                    Rectangle closestRoom = mapGen.findClosestRoom(mapGen.rooms[i]);
                    if (closestRoom.Equals(Rectangle.Empty))
                        continue;

                    Point pointA = new Point(
                        Minion.Instance.Random.Next(mapGen.rooms[i].X, mapGen.rooms[i].X + mapGen.rooms[i].Width + 1),
                        Minion.Instance.Random.Next(mapGen.rooms[i].Y, mapGen.rooms[i].Y + mapGen.rooms[i].Height + 1)
                    );

                    Point pointB = new Point(
                        Minion.Instance.Random.Next(closestRoom.X, closestRoom.X + closestRoom.Width + 1),
                        Minion.Instance.Random.Next(closestRoom.Y, closestRoom.Y + closestRoom.Height + 1)
                    );

                    while ((pointB.X != pointA.X) || (pointB.Y != pointA.Y))
                    {
                        if (pointB.X != pointA.X)
                        {
                            if (pointB.X > pointA.X)
                                pointB.X--;
                            else
                                pointB.X++;
                        }
                        else if (pointB.Y != pointA.Y)
                        {
                            if (pointB.Y > pointA.Y)
                                pointB.Y--;
                            else
                                pointB.Y++;
                        }

                        // Set as a floor tile
                        mapGen.map[pointB.X, pointB.Y] = false;
                    }
                }
            }

            // TODO: This is problematic and ugly as sin, fix
            // Find and set wall tiles around the rooms (NOTE THE ROOM SIZE IS ALWAYS THE WALKABLE AREA, I.E. THE SURROUNDING WALLS ARE BIGGER THAN THE ROOM MIN/MAX)
            for (int x = 0; x < mapSize.X; x++)
            {
                for (int y = 0; y < mapSize.Y; y++)
                {
                    if (mapGen.map[x, y] == false)
                    {
                        for (int xx = x - 1; xx <= x + 1; xx++)
                        {
                            if (xx < mapGen.map.GetLength(0))
                            {
                                for (int yy = y - 1; yy <= y + 1; yy++)
                                {
                                    if (yy < mapGen.map.GetLength(1) && mapGen.map[xx, yy] == null)
                                        mapGen.map[xx, yy] = true;
                                }
                            }
                        }
                    }
                }
            }

            return mapGen;
        }

        /* THE OLD VERSION THAT CONDENSES TOO MUCH IMHO (NOTE THE WHILE LOOP)
        /// <summary>
        /// Condenses the map rooms
        /// </summary>
        /// <param name="iterations">The number of iterations, lower means rooms will be further apart</param>
        protected void condenseArea(int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                for (int j = 0; j < rooms.Count; j++)
                {
                    int attempts = 0;
                    while (true)
                    {
                        Point oldPos = new Point(rooms[j].X, rooms[j].Y);
                        // Move room upwards and left until it collides with another room or hits the top
                        if (rooms[j].X > 1)
                            rooms[j] = new Rectangle(rooms[j].X - 1, rooms[j].Y, rooms[j].Width, rooms[j].Height);
                        if (rooms[j].Y > 1)
                            rooms[j] = new Rectangle(rooms[j].X, rooms[j].Y - 1, rooms[j].Width, rooms[j].Height);

                        if ((rooms[j].X == 1) && (rooms[j].Y == 1)) 
                            break;

                        if (doesCollide(rooms[j], j) || attempts >= 100)
                        {
                            rooms[j] = new Rectangle(oldPos.X, oldPos.Y, rooms[j].Width, rooms[j].Height);
                            break;
                        }
                        attempts++;
                    }
                }
            }
        }
        */


        /// <summary>
        /// Condenses the map rooms
        /// </summary>
        /// <param name="iterations">The number of iterations, lower means rooms will be further apart</param>
        protected void condenseArea(int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                for (int j = 0; j < rooms.Count; j++)
                {
                    Point oldPos = new Point(rooms[j].X, rooms[j].Y);
                    // Move room upwards and left until it collides with another room or hits the top
                    if (rooms[j].X > 1)
                        rooms[j] = new Rectangle(rooms[j].X - 1, rooms[j].Y, rooms[j].Width, rooms[j].Height);
                    if (rooms[j].Y > 1)
                        rooms[j] = new Rectangle(rooms[j].X, rooms[j].Y - 1, rooms[j].Width, rooms[j].Height);

                    if ((rooms[j].X == 1) && (rooms[j].Y == 1))
                        break;

                    if (doesCollide(rooms[j], j))
                    {
                        rooms[j] = new Rectangle(oldPos.X, oldPos.Y, rooms[j].Width, rooms[j].Height);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Finds the room that is the closest to the room that was passed in.
        /// If the rectangle is empty no room was found.
        /// </summary>
        /// <param name="room">The room we want to use</param>
        /// <returns>The closest found room or an empty Rectangle if nothing was found</returns>
        protected Rectangle findClosestRoom(Rectangle room)
        {
            Rectangle closestRoom = Rectangle.Empty;
            int closestDistance = 1000;

            Point midPoint = new Point(
                room.X + (room.Width / 2),
                room.Y + (room.Height / 2)
            );

            for (int i = 0; i < rooms.Count; i++)
            {
                // Don't check a room against itself
                if (rooms[i] == room)
                    continue;

                Point currentMidPoint = new Point(
                    rooms[i].X + (rooms[i].Width / 2),
                    rooms[i].Y + (rooms[i].Height / 2)
                );

                int distance = Math.Min(
                    Math.Abs(midPoint.X - currentMidPoint.X) - (room.Width / 2) -  (rooms[i].Width / 2),
                    Math.Abs(midPoint.Y - currentMidPoint.Y) - (room.Height / 2) - (rooms[i].Height / 2)
                );

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestRoom = rooms[i];
                }
            }

            return closestRoom;
        }

        /// <summary>
        /// Checks if a room does collide with the one passed in
        /// </summary>
        /// <param name="room">The room to check</param>
        /// <param name="skipIndex">The room array index to skip (if any, defaults to -1)</param>
        /// <returns>True on collision</returns>
        protected bool doesCollide(Rectangle room, int skipIndex = -1)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (i == skipIndex)
                    continue;

                if (room.Intersects(rooms[i]))
                    return true;
            }

            return false;
        }

        public void Draw(GameTime gameTime, int scaleX = 1, int scaleY = 1)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                Rectangle room = new Rectangle(
                    rooms[i].X * scaleX,
                    rooms[i].Y * scaleY,
                    rooms[i].Width * scaleX - scaleX,
                    rooms[i].Height * scaleY - scaleY
                );
                DrawingHelper.DrawRectangle(
                    room, 
                    1 * ((scaleX + scaleY) / 2),
                    (i % 2 == 0 ? 
                        (rooms[i].X % 2 == 0 ? 
                            (rooms[i].Y % 2 == 0 ? Color.DarkRed : Color.Purple) : 
                            (rooms[i].Y % 2 == 0 ? Color.DarkBlue : Color.Orange) 
                        ) :
                        (rooms[i].X % 2 == 0 ?
                            (rooms[i].Y % 2 == 0 ? Color.LightGreen : Color.DeepPink) :
                            (rooms[i].Y % 2 == 0 ? Color.GreenYellow : Color.Cyan)
                        )
                    ) * 0.5f, 
                    10
                );
            }
        }

    }
}
