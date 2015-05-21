using System;
using System.Collections.Generic;

namespace MonoMinion.MapGenerators
{
    public enum CellState
    {
        Alive,
        Dead
    }

    public class CellularAutomataGenerator
    {
        #region CellularRule Struct
        /// <summary>
        /// Holds the Alive-Dead rules for the Cellular Automata generator 
        /// </summary>
        protected struct CellularRule
        {
            #region Variables & Properties
            /// <summary>
            /// The number of surround alive cells needed if this cell is alive
            /// </summary>
            public int CountNeededWhileAlive
            {
                get { return alive; }
                set
                {
                    // We default to the 4-5 rule if applicable
                    if (value > 0 && value <= 8)
                        alive = value;
                    else
                        alive = 4;
                }
            }
            private int alive;

            /// <summary>
            /// The number of surround alive cells needed if this cell is dead
            /// </summary>
            public int CountNeededWhileDead
            {
                get { return dead; }
                set
                {
                    // We default to the 4-5 rule if applicable
                    if (value > 0 && value <= 8)
                        dead = value;
                    else
                        dead = 5;
                }
            }
            private int dead;
            #endregion

            #region Constructors
            /// <summary>
            /// Creates a Cellular Automata Rule
            /// </summary>
            /// <param name="alive">The number of surround alive cells needed if this cell is alive</param>
            /// <param name="dead">The number of surround alive cells needed if this cell is dead</param>
            public CellularRule(int alive, int dead)
            {
                // We default to the 4-5 rule if applicable
                if (alive > 0 && alive <= 8)
                    this.alive = alive;
                else
                    this.alive = 4;

                // We default to the 4-5 rule if applicable
                if (dead > 0 && dead <= 8)
                    this.dead = dead;
                else
                    this.dead = 5;
            }
            #endregion
        }
        #endregion


        #region Variables
        public MapBlueprint Blueprint;
        protected CellState[,] noiseMap;
        protected int width;
        protected int height;

        protected int iterations;
        protected int wallChance;
        protected CellularRule[] rules;

        private int _randomSeed;
        private Random _rnd;
        #endregion


        public CellularAutomataGenerator(int iterations = 3, int wallChance = 50)
        {
            this.iterations = iterations;
            this.wallChance = wallChance;
            rules = new CellularRule[iterations];
            // We default to the 4-5 rule for each iteration
            for (int i = 0; i < iterations; i++)
                rules[i] = new CellularRule(4, 5);

            _rnd = new Random();
        }


        #region Initialization & Parameters
        /// <summary>
        /// Sets the random seed
        /// </summary>
        /// <param name="seed">The random seed to set</param>
        public void SetRandomSeed(int seed)
        {
            _randomSeed = seed;
            _rnd = new Random(seed);
        }

        /// <summary>
        /// Set the number of iterations (the number of passes the algorithm is set to run on each generation)
        /// </summary>
        /// <param name="iterations">The number of iterations</param>
        public void SetIterations(uint iterations)
        {
            if (iterations > 0 && this.iterations != (int)iterations)
            {
                this.iterations = (int)iterations;
                CellularRule[] oldRules = rules;
                rules = new CellularRule[iterations];
                // We default to the 4-5 rule for each iteration if applicable
                for (int i = 0; i < iterations; i++)
                {
                    if (i < oldRules.Length)
                        rules[i] = oldRules[i];
                    else
                        rules[i] = new CellularRule();
                }
            }
        }

        /// <summary>
        /// Sets the percentage chance of a wall being spawned in the preparation phase
        /// </summary>
        /// <param name="seed">The percentage</param>
        public void SetWallChance(int chance)
        {
            if (chance > 0 && chance <= 100)
                wallChance = chance;
        }

        /// <summary>
        /// Set a rule for all iterations
        /// </summary>
        /// <param name="alive">The number of surrounding cells that are alive if the cell is a alive</param>
        /// <param name="dead">The number of surrounding cells that are alive if the cell is dead</param>
        public void SetRules(int alive, int dead)
        {
            if ((alive > 0 && alive <= 8) && (dead > 0 && dead <= 8))
            {
                for (int i = 0; i < iterations; i++)
                    rules[i] = new CellularRule(alive, dead);
            }
        }

        /// <summary>
        /// Sets the rule for a specific iteration
        /// </summary>
        /// <param name="iteration">The iteration for this pass</param>
        /// <param name="alive">The number of surrounding cells that are alive if the cell is a alive</param>
        /// <param name="dead">The number of surrounding cells that are alive if the cell is dead</param>
        public void SetRule(int iteration, int alive, int dead)
        {
            if ((iteration > 0 && iteration <= iterations) && 
                (alive > 0 && alive <= 8) && (dead > 0 && dead <= 8))
            {
                rules[iteration - 1] = new CellularRule(alive, dead);
            }
        }
        #endregion


        #region Generator Methods
        /// <summary>
        /// Calculates how many of the surrounding cells are alive
        /// </summary>
        /// <param name="x">The cells X index position</param>
        /// <param name="y">The cells Y index position</param>
        /// <returns>The number of surrounding cells that are alive</returns>
        protected int calculateSurroundingAliveCells(int x, int y)
        {
            int cellCount = 0;

            for (int xx = -1; xx < 2; xx++)
            {
                for (int yy = -1; yy < 2; yy++)
                {
                    // We skip (i.e. assume CellState.Dead) if the current tile is the center tile or if the X or Y indexes are out of bounds 
                    if ((xx == 0 && yy == 0) ||
                        ((x + xx) < 0 || (x + xx) >= width) ||
                        ((y + yy) < 0 || (y + yy) >= height))
                    {
                        continue;
                    }

                    // Increase cell count if the current tile is alive
                    if (noiseMap[x + xx, y + yy] == CellState.Alive)
                        cellCount++;
                }
            }

            return cellCount;
        }
        #endregion


        public MapBlueprint Generate(int width, int height)
        {
            // PREPARATION
            this.width = width;
            this.height = height;
            Blueprint = new MapBlueprint(width, height);
            noiseMap = new CellState[width, height];
            // Start by randomly seeding a tile as being alive (0) or dead (1)
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    noiseMap[x,y] = (_rnd.Next(1, 101) >     wallChance ? CellState.Alive : CellState.Dead);

            // GENERATION
            // Next we iterate through the map array
            for (int i = 0; i < iterations; i++)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        int aliveCellCount = calculateSurroundingAliveCells(x, y);
                        // If current cell is alive
                        if (noiseMap[x, y] == CellState.Alive)
                        {
                            // and the number of surrounding cells that are alive is less than how many are needed while alive, we mark as dead
                            if (aliveCellCount < rules[i].CountNeededWhileAlive)
                                noiseMap[x, y] = CellState.Dead;
                        }
                        else
                        {
                            // or if the cell is dead the number of surround alive cells is more or equal to how many are needed while dead, we mark as alive
                            if (aliveCellCount >= rules[i].CountNeededWhileDead)
                                noiseMap[x, y] = CellState.Alive;
                        }
                    }
                }
            }

            // FINALIZATION
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (noiseMap[x, y] == CellState.Alive)
                        Blueprint.Map[x, y] = MapTileType.Ground;
                    else
                        Blueprint.Map[x, y] = MapTileType.Wall;

                }
            }

            return Blueprint;
        }
    }
}
