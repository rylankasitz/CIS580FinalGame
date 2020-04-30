using Engine.Componets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.ECSCore
{
    public delegate void HandleGrid(Entity entity1, Entity entity2);
    public class Grid
    {
        private const int NUM_CELLS = 160;
        private const int CELL_SIZE = 320;

        private Entity[][] cells;

        public Grid()
        {
            cells = new Entity[NUM_CELLS][];

            for (int x = 0; x < NUM_CELLS; x++)
            {
                cells[x] = new Entity[NUM_CELLS];
                for (int y = 0; y < NUM_CELLS; y++)
                {
                    cells[x][y] = null;
                }
            }
        }

        public void Remove(Entity current)
        {
            Transform transform = current.GetComponent<Transform>();

            int cellX = (int)(transform.Position.X / CELL_SIZE);
            int cellY = (int)(transform.Position.Y / CELL_SIZE);

            if (cells[cellX][cellY] == current)
            {
                cells[cellX][cellY] = current.Next;
            }

            if (current.Next != null)
            {
                current.Next.Prev = current.Prev;
            }

            if (current.Prev != null)
            {
                current.Prev.Next = current.Next;
            }
        }

        public void Add(Entity entity)
        {
            Transform transform = entity.GetComponent<Transform>();

            int cellX = (int)(transform.Position.X / CELL_SIZE);
            int cellY = (int)(transform.Position.Y / CELL_SIZE);

            entity.Prev = null;
            entity.Next = cells[cellX][cellY];
            cells[cellX][cellY] = entity;

            if (entity.Next != null)
            {
                entity.Next.Prev = entity;
            }
            entity.OldCellX = cellX;
            entity.OldCellY = cellY;
        }

        public void Handle(HandleGrid handleGrid)
        {
            for (int x = 0; x < NUM_CELLS; x++)
            {
                for (int y = 0; y < NUM_CELLS; y++)
                {
                    Entity entity1 = cells[x][y];
                    while (entity1 != null)
                    {
                        handleEntity(entity1, entity1.Next, handleGrid);

                        if (x > 0 && y > 0) handleEntity(entity1, cells[x - 1][y - 1], handleGrid);
                        if (x > 0) handleEntity(entity1, cells[x - 1][y], handleGrid);
                        if (y > 0) handleEntity(entity1, cells[x][y - 1], handleGrid);
                        if (x > 0 && y < NUM_CELLS - 1)
                        {
                            handleEntity(entity1, cells[x - 1][y + 1], handleGrid);
                        }

                        entity1 = entity1.Next;
                    }
                }
            }
        }

        private void handleEntity(Entity entity1, Entity entity2, HandleGrid handleGrid)
        {
            while (entity2 != null)
            {
                handleGrid(entity1, entity2);
                entity2 = entity2.Next;
            }
        }

        public void Move(Entity entity)
        {
            Transform newPosition = entity.GetComponent<Transform>();

            int cellX = (int)(newPosition.Position.X / CELL_SIZE);
            int cellY = (int)(newPosition.Position.Y / CELL_SIZE);

            if (entity.OldCellX == cellX && entity.OldCellY == cellY) return;

            if (entity.Prev != null)
            {
                entity.Prev.Next = entity.Next;
            }

            if (entity.Next != null)
            {
                entity.Next.Prev = entity.Prev;
            }

            if (cells[entity.OldCellX][entity.OldCellY] == entity)
            {
                cells[entity.OldCellX][entity.OldCellY] = entity.Next;
            }

            Add(entity);
        }
    }
}
