using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Internal;

namespace HelloWorld
{
    public static class Globals
    {
        // NOTE TO SELF: RANDOM.NEXT IS LIKE THIS [)
        // AS IN RAND.NEXT(0,2) CAN GENERATE ONLY 0 AND 1

        public const int mutationChance = 10; //1 In this number
        public const int eatingEUsage = 1;
        public const int processingSugEUsage = 1;
        public const int processingOxyEUsage = 1;
        public const int processingHydroEUsage = 1;
        public const int spittingEUsage = 1;
        public const int movingEUsage = 1;
        public const int multiplicationEUsage = 1;

        public const int energyFromSugar = 10;
        public const int energyFromOxygen = 10;
        public const int energyFromHydrogen = 10;

        public const char CellChar = 'C';
        public const char SugChar = 'S';
        public const char OxyChar = 'O';
        public const char HydroChar = 'H';
        public const int dnaCharNum = 15;
        public const string dnaChars = "ABCDEFGHIJKLMNO";
        //LIST OF DNA CHARACTERS
        // A = Eat (Collects any resource that is arround it)
        // B = Process Sugar (Turns stored Sugar into energy)
        // C = Process Oxygen (Turns stored Oxygen into energy)
        // D = Process Hydreogen (Turns stored Hydreogen into energy)
        // E = Release Sugar (Spits Sugar out of the cell)
        // F = Release Oxygen (Spits Oxygen out of the cell)
        // G = Release Hydreogen (Spits Hydreogen out of the cell)
        // H = Move Left (Moves the Cell to the Left)
        // I = Move Right (Moves the Cell to the Right)
        // J = Move Up (Moves the Cell Up)
        // K = Move Down (Moves the Cell Down)
        // L = Multiply (Creates a duplicate with same genes (possibility of mutation). Given energy is determined by the energy gene.)
        // M =
        // N =
        // O =

        public static EResource[] RemoveResource(int index, EResource[] resources)
        {
            EResource[] replaceResources = new EResource[resources.Length-1];
            int counter = 0;
            resources[index] = null;
            for (int i = 0; i < resources.Length; i++)
            {
                if (resources[i] == null)
                {
                    i++;
                }
                else
                {
                    replaceResources[counter] = resources[i];
                }
                counter++;
            }
            return replaceResources;
        }
        public static Cell[] RemoveCell(int index, Cell[] cells)
        {
            Cell[] replaceCells = new Cell[cells.Length-1];
            int counter = 0;
            cells[index] = null;
            for (int i = 0; i < cells.Length; i++)
            {
                if (cells[i] == null)
                {
                    i++;
                }
                else
                {
                    replaceCells[counter] = cells[i];
                }
                counter++;
            }
            return replaceCells;
        }

        public static bool AreCoordsEmpty(int x, int y, World world)
        {
            foreach (EResource resource in world.existingResources)
            {
                if (resource.X == x && resource.Y == y)
                {
                    return false;
                }
            }
            foreach (Cell cell in world.existingCells)
            {
                if (cell.CellX == x && cell.CellY == y)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class World 
    {
        public Cell[] existingCells;
        public EResource[] existingResources;

        public const int WorldSX = 10;
        public const int WorldSY = 10;
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Setup World
            World world = new World();
            EResource testOxy = new EResource();
            testOxy.type = "Oxygen";
            testOxy.X = 1;
            testOxy.Y = 2;
            string DNA1 = "KI";
            string DNA2 = "KI";
            string DNA3 = "HJ";
            string DNA4 = "EFGL";
            string DNA5 = "AB";
            string DNA6 = "AC";
            string DNA7 = "AD";
            string[] DNA = {DNA1, DNA2, DNA3, DNA4, DNA5, DNA6, DNA7};
            Cell firstCell = new Cell(DNA, 100, 100, 0,0, 50);
            world.existingCells = new Cell[] {firstCell};
            world.existingResources = new EResource[] {testOxy};
            bool simRunning = true;

            while (simRunning == true)
            {
                Console.Clear();
                // Processing Loop
                foreach (Cell cell in world.existingCells)
                {
                    cell.tick(world);
                }

                // Drawing Loop
                for (int i = 0; i < 10; i++) // Y
                {
                    for (int j = 0; j < 10; j++) // X
                    {
                        bool printed = false;
                        // Loop Through All of the cells. 
                        for (int l = 0; l < world.existingCells.Length; l++)
                        {
                            // If they are at this coordinate.
                            if (world.existingCells[l].CellX == j && world.existingCells[l].CellY == i)
                            {
                                // Print them.
                                Console.Write(Globals.CellChar);
                                printed = true;
                            }
                        }

                        // Loop Through All of the resources
                        for (int l = 0; l < world.existingResources.Length; l++)
                        {
                            // If they are at this coordinate.
                            if (world.existingResources[l].X == j && world.existingResources[l].Y == i)
                            {
                                // Print them.
                                switch (world.existingResources[l].type)
                                {
                                    case "Sugar":
                                        Console.Write(Globals.SugChar);
                                        printed = true;
                                        break;
                                    case "Oxygen":
                                        Console.Write(Globals.OxyChar);
                                        printed = true;
                                        break;
                                    case "Hydrogen":
                                        Console.Write(Globals.HydroChar);
                                        printed = true;
                                        break;
                                    default:
                                        Console.Write('#');
                                        break;
                                }
                            }
                        }
                        if (!printed)
                        {
                            Console.Write("_");
                        }
                    }
                    // Move to next line.
                    Console.WriteLine();
                }
                Console.ReadKey();
            }
        }
    }

    public class EResource
    {
        public string type;
        public int X;
        public int Y;
    }


    public class Cell
    {
        // DNA1 On Birth
        // DNA2 Passive Loop
        // DNA3 On Touch Cell
        // DNA4 Nearing Death (Last Instruction before dying of age)
        // DNA5 On Touch Sugar
        // DNA6 On Touch Oxygen
        // DNA7 On Touch Hydreogen

        public string[] DNA;
        public int[] DNACounter;
        public bool[] DNAActive;
        public int givenEGene;
        public int age;
        public int deathmark;

        public int energy;
        public int sugar;
        public int oxygen;
        public int hydrogen;

        public int CellX;
        public int CellY;

        public Cell(string[] GivenDNA, int GivenEnergy, int GivenEGene, int BornCellX, int BornCellY, int maxAge)
        {
            age = 0;
            deathmark = maxAge;
            energy = GivenEnergy;
            DNACounter = new int[] {0,0,0,0,0,0,0}; // Sets counter to 0 for all DNA.
            DNAActive = new bool[] {true, true, false, false, false, false, false};
            CellX = BornCellX;
            CellY = BornCellY;

            // Apply Mutation to dna.
            Random rand = new Random();
            int mutation = rand.Next(0,Globals.mutationChance);
            DNA = GivenDNA;
            givenEGene = GivenEGene;

            // Mutate!
            if (mutation == 0)
            {
                // Loop through all of the DNA
                for (int i = 0; i < DNA.Length; i++)
                {
                    int mutationLocation = rand.Next(0, DNA[i].Length);
                    int mutationLetter = rand.Next(0, Globals.dnaCharNum);
                    string mutatedDNA = DNA[i];

                    StringBuilder sb = new StringBuilder(mutatedDNA);
                    sb[mutationLocation] = Globals.dnaChars[mutationLetter];
                    mutatedDNA = sb.ToString();
                }

                // Mutate the givenEGene
                givenEGene = givenEGene + rand.Next(-10 * (Globals.mutationChance/2), 10 * (Globals.mutationChance/2));
            }
        }

        public void tick(World world)
        {
            // Check if about to die
            if (deathmark - age < 10)
            {
                DNAActive[3] = true;
            }

            // Check Cell Surroundings.

            // Starting with if touching other cell.
            for (int i = 0; i < world.existingCells.Length; i++)
            {
                // Checks left and right of cell.
                if (((world.existingCells[i].CellX == CellX+1 && world.existingCells[i].CellY == CellY) || (world.existingCells[i].CellX == CellX-1 && world.existingCells[i].CellY == CellY)))
                {
                    //
                    // #C#
                    //
                    DNAActive[2] = true;
                }
                // Check up and down of cell.
                else if (((world.existingCells[i].CellY == CellY+1 && world.existingCells[i].CellX == CellX) || (world.existingCells[i].CellY == CellY-1 && world.existingCells[i].CellX == CellX+1)))
                {
                    //  #
                    //  C
                    //  #
                    DNAActive[2] = true;
                }
                else
                {
                    // Isnt touching a cell.
                    DNAActive[2] = false;
                }
            }




            // Check if touching resources
            for (int i = 0; i < world.existingResources.Length; i++)
            {
                // Checks left and right of cell.
                if (((world.existingResources[i].X == CellX+1 && world.existingResources[i].Y == CellY) || (world.existingResources[i].X == CellX-1 && world.existingResources[i].Y == CellY)))
                {
                    //
                    // #C#
                    //
                    TouchingResource(world.existingResources[i]);
                }
                // Check up and down of cell.
                else if (((world.existingResources[i].Y == CellY+1 && world.existingResources[i].X == CellX) || (world.existingResources[i].Y == CellY-1 && world.existingResources[i].X == CellX+1)))
                {
                    //  #
                    //  C
                    //  #
                    TouchingResource(world.existingResources[i]);
                }
                else
                {
                    // Isnt touching a resource.
                    DNAActive[4] = false;
                    DNAActive[5] = false;
                    DNAActive[6] = false;
                }
            }

            // Process DNA
            for (int i = 0; i < DNAActive.Length; i++)
            {
                if (DNAActive[i] == true)
                {
                    string unprocessedDNA = DNA[i];
                    ProcessDNALetter(unprocessedDNA[DNACounter[i]], world);

                    // Increment the counter
                    DNACounter[i]++;

                    Console.WriteLine("test");
                    // If the counter is higher than the length of the DNA, reset it to 0.
                    if (DNACounter[i] >= DNA[i].Length)
                    {
                        DNACounter[i] = 0;
                        if (i == 0)
                        {
                            DNAActive[0] = false;
                        }
                    }
                }
            }

            // Die
            if (age >= deathmark)
            {
                for (int i = 0; i < world.existingCells.Length; i++)
                {
                    if (world.existingCells[i].CellX == CellX && world.existingCells[i].CellY == CellY)
                    {
                        Globals.RemoveCell(i, world.existingCells);
                    }
                }
                
            }
        }

        public void TouchingResource(EResource resource)
        {
            switch(resource.type)
            {
                case "Sugar":
                    DNAActive[4] = true;
                    break;
                case "Oxygen":
                    DNAActive[5] = true;
                    break;
                case "Hydrogen":
                    DNAActive[6] = true;
                    break;
                default:
                    break;
            }
        }
        public void ProcessDNALetter(char dnaL, World world)
        {
            switch (dnaL)
            {
                case 'A':
                    Eat(world);
                    break;
                case 'B':
                    Process("Sugar");
                    break;
                case 'C':
                    Process("Oxygen");
                    break;
                case 'D':
                    Process("Hydrogen");
                    break;
                case 'E':
                    Spit("Sugar", world);
                    break;
                case 'F':
                    Spit("Oxygen", world);
                    break;
                case 'G':
                    Spit("Hydrogen", world);
                    break;
                case 'H':
                    Move("Left", world);
                    break;
                case 'I':
                    Move("Right", world);
                    break;
                case 'J':
                    Move("Up", world);
                    break;
                case 'K':
                    Move("Down", world);
                    break;
                case 'L':
                    Multiply();
                    break;
                case 'M':
                    break;
                case 'N':
                    break;
                case 'O':
                    break;
                default:
                    break;
            }
        }

        public void Spit(string resource, World world)
        {
            Random rand = new Random();
            EResource newResource = new EResource();

            // Setup the resource and subtract from cell stockpile.
            switch (resource)
            {
                case "Sugar":
                    if (sugar >= 1)
                    {
                        sugar--;
                        newResource.type = "Sugar";
                    }
                    break;
                case "Oxygen":
                    if (oxygen >= 1)
                    {
                        oxygen--;
                        newResource.type = "Oxygen";
                    }
                    break;
                case "Hydrogen":
                    if (hydrogen >= 1)
                    {
                        hydrogen--;
                        newResource.type = "Hydrogen";
                    }
                    break;
                default:
                    if (oxygen >= 1)
                    {
                        oxygen--;
                        newResource.type = "Oxygen";
                    }
                    break;
            }

            bool found = false;
            int tries = 0;
            RandomNumber:
            // Pick random location
            int loc = rand.Next(0,4); // [0, 3]

            // Set the new resources coordinates.
            switch (loc)
            {
                case 0: // Up
                        if (!Globals.AreCoordsEmpty(CellX, CellY-1, world))
                        {
                            if (tries >= 4)
                            {
                                break;
                            }
                            else
                            {
                                tries++;
                                goto RandomNumber;
                            }
                        }
                        else
                        {
                            found = true;
                        }
                    break;
                case 1: // Down
                        if (!Globals.AreCoordsEmpty(CellX, CellY+1, world))
                        {
                            if (tries >= 4)
                            {
                                break;
                            }
                            else
                            {
                                tries++;
                                goto RandomNumber;
                            }
                        }
                        else
                        {
                            found = true;
                        }
                    break;
                case 2: // Left
                        if (!Globals.AreCoordsEmpty(CellX-1, CellY, world))
                        {
                            if (tries >= 4)
                            {
                                break;
                            }
                            else
                            {
                                tries++;
                                goto RandomNumber;
                            }
                        }
                        else
                        {
                            found = true;
                        }
                    break;
                case 3: // Right
                        if (!Globals.AreCoordsEmpty(CellX+1, CellY, world))
                        {
                            if (tries >= 4)
                            {
                                break;
                            }
                            else
                            {
                                tries++;
                                goto RandomNumber;
                            }
                        }
                        else
                        {
                            found = true;
                        }
                    break;
            }

            if (found)
            {
                // Add the resource to the world.
                Array.Resize(ref world.existingResources, world.existingResources.Length + 1);
                world.existingResources[world.existingResources.GetUpperBound(0)] = newResource;
            }
        }
        public void Move(string where, World world)
        {
            bool isValid = false;
            int addX = 0;
            int addY = 0;
            switch (where)
            {
                case "Up":
                    if (Globals.AreCoordsEmpty(CellX, CellY-1, world))
                    {
                        isValid = true;
                        addY = -1;
                    }
                    break;
                case "Down":
                    if (Globals.AreCoordsEmpty(CellX, CellY+1, world))
                    {
                        isValid = true;
                        addY = 1;
                    }
                    break;
                case "Left":
                    if (Globals.AreCoordsEmpty(CellX-1, CellY, world))
                    {
                        isValid = true;
                        addX = -1;
                    }
                    break;
                case "Right":
                    if (Globals.AreCoordsEmpty(CellX+1, CellY, world))
                    {
                        isValid = true;
                        addX = 1;
                    }
                    break;
                default:
                    break;
            }

            if (isValid == true && energy >= Globals.movingEUsage)
            {
                energy -= Globals.movingEUsage;
                CellX += addX;
                CellY += addY;
            }
        }
        public void Multiply()
        {
            bool isValid = false;
            Random rand = new Random();
            int counter = 0;

            SetDir:
            if (counter >= 4)
            {
                return;
            }
            int dir = rand.Next(0,4);
            int addX = 0;
            int addY = 0;
            switch (dir)
            {
                case 0:
                    if (Globals.AreCoordsEmpty(CellX, CellY-1, world))
                    {
                        isValid = true;
                        addY = -1;
                    }
                    else
                    {
                        counter++;
                        goto SetDir;
                    }
                    break;
                case 1:
                    if (Globals.AreCoordsEmpty(CellX, CellY+1, world))
                    {
                        isValid = true;
                        addY = 1;
                    }
                    else
                    {
                        counter++;
                        goto SetDir;
                    }
                    break;
                case 2:
                    if (Globals.AreCoordsEmpty(CellX-1, CellY, world))
                    {
                        isValid = true;
                        addX = -1;
                    }
                    else
                    {
                        counter++;
                        goto SetDir;
                    }
                    break;
                case 3:
                    if (Globals.AreCoordsEmpty(CellX+1, CellY, world))
                    {
                        isValid = true;
                        addX = 1;
                    }
                    else
                    {
                        counter++;
                        goto SetDir;
                    }
                    break;
                default:
                    break;
            }

            if (isValid == true && energy >= Globals.multiplicationEUsage)
            {
                energy -= Globals.movingEUsage;

                // Give The birthed cell the maximum energy it can.
                int varx = energy - givenEGene;
                int maxenergy = givenEGene - varx;
                energy -= maxenergy;
                // Create Cell
                Cell newCell = new Cell(DNA, maxenergy, givenEGene, CellX += addX. CellY += addY, deathmark);
                // Add the cell to the world.
                Array.Resize(ref world.existingCells, world.existingCells.Length + 1);
                world.existingCells[world.existingCells.GetUpperBound(0)] = newCell;
            }
        }
        public void Process(string resource)
        {
            switch (resource)
            {
                case "Sugar":
                    if (energy >= Globals.processingSugEUsage)
                    {
                        energy -= Globals.processingSugEUsage;
                        sugar--;
                        energy += Globals.energyFromSugar;
                    }
                    break;
                case "Oxygen":
                    if (energy >= Globals.processingOxyEUsage)
                    {
                        energy -= Globals.processingOxyEUsage;
                        sugar--;
                        energy += Globals.energyFromOxygen;
                    }
                    break;
                case "Hydrogen":
                    if (energy >= Globals.processingHydroEUsage)
                    {
                        energy -= Globals.processingHydroEUsage;
                        sugar--;
                        energy += Globals.energyFromHydrogen;
                    }
                    break;
                default:
                    break;
            }
        }
        public void Eat(World world)
        {
            if (energy >= Globals.eatingEUsage)
            {
                energy -= Globals.eatingEUsage;
            }

            // Check if touching resources
            for (int i = 0; i < world.existingResources.Length; i++)
            {
                // Checks left and right of cell.
                if (((world.existingResources[i].X == CellX+1 && world.existingResources[i].Y == CellY) || (world.existingResources[i].X == CellX-1 && world.existingResources[i].Y == CellY)))
                {
                    //
                    // #C#
                    //
                    EndEat(world.existingResources[i], i, world);
                }
                // Check up and down of cell.
                else if (((world.existingResources[i].Y == CellY+1 && world.existingResources[i].X == CellX) || (world.existingResources[i].Y == CellY-1 && world.existingResources[i].X == CellX+1)))
                {
                    //  #
                    //  C
                    //  #
                    EndEat(world.existingResources[i], i, world);
                }
                else
                {
                    // Isnt touching a resource.
                }
            }
        }

        public void EndEat(EResource resource, int index ,World world)
        {
            switch (resource.type)
            {
                case "Sugar":
                    sugar++;
                    break;
                case "Oxygen":
                    oxygen++;
                    break;
                case "Hydrogen":
                    hydrogen++;
                    break;
                default:
                    break;
            }
            world.existingResources = Globals.RemoveResource(index, world.existingResources);
        }
    }
}