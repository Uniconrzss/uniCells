using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Internal;

namespace Simulation
{
    public static class Globals
    {
        // NOTE TO SELF: RANDOM.NEXT IS LIKE THIS [)
        // AS IN RAND.NEXT(0,2) CAN GENERATE ONLY 0 AND 1

        public const int maxX = 50;
        public const int maxY = 30;
        public const int mutationChance = 2; //1 In this number
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

        public const int maxResourceCapacity = 5;
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
        // M = Activate Reserve Gene (Activates the 8th Gene in the Cell)
        // N =
        // O =

        public static EResource[] RemoveResource(int index, EResource[] resources)
        {
            List<EResource> replaceResources = new List<EResource>(resources);
            replaceResources.RemoveAt(index);

            return replaceResources.ToArray();
        }
        public static Cell[] RemoveCell(int index, Cell[] cells)
        {
            List<Cell> replaceCells = new List<Cell>(cells);
            replaceCells.RemoveAt(index);

            return replaceCells.ToArray();
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

        public const int WorldSX = Globals.maxX;
        public const int WorldSY = Globals.maxY;
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Setup World
            World world = new World();
            int[] RcoordsX = {1, 4, 7, 6, 8, 10, 13, 17, 2};
            int[] RcoordsY = {5, 3, 8, 9, 4, 1, 9, 14, 12};
            string[] RsourceT = {"Sugar", "Sugar", "Sugar", "Sugar", "Sugar", "Sugar", "Sugar", "Sugar", "Sugar"};
            EResource[] allResources = new EResource[9];
            for (int i = 0; i < RsourceT.Length; i++)
            {
                allResources[i] = new EResource();
                allResources[i].type = RsourceT[i];
                allResources[i].X = RcoordsX[i];
                allResources[i].Y = RcoordsY[i];
            }
            // Goes towards the down and right
            // string DNA1 = "KI";
            // string DNA2 = "KIKIL";
            // string DNA3 = "HJ";
            // string DNA4 = "EFGL";
            // string DNA5 = "AB";
            // string DNA6 = "AC";
            // string DNA7 = "AD";
            // string[] DNA = {DNA1, DNA2, DNA3, DNA4, DNA5, DNA6, DNA7};
            // Cell firstCell = new Cell(DNA, 100, 100, 0,0, 50);
            // world.existingCells = new Cell[] {firstCell};

            // Plant Attempt 1:
            // string DNA1 = "J";
            // string DNA2 = "AC";
            // string DNA3 = "HI";
            // string DNA4 = "KLFGE";
            // string DNA5 = "";
            // string DNA6 = "A";
            // string DNA7 = "";
            // string DNA8 = "";
            // string[] DNA = {DNA1, DNA2, DNA3, DNA4, DNA5, DNA6, DNA7, DNA8};
            // Cell plantCell = new Cell(DNA, 40, 40, 15, 15, 10);

            // Bacterium:
            string DNA11 = "JKHI";
            string DNA21 = "AL";
            string DNA31 = "JKHI";
            string DNA41 = "L";
            string DNA51 = "AB";
            string DNA61 = "AC";
            string DNA71 = "AD";
            string DNA81 = "";
            string[] DNAa = {DNA11, DNA21, DNA31, DNA41, DNA51, DNA61, DNA71, DNA81};
            Cell bacterium = new Cell(DNAa, 40, 50, 10, 10, 10, true);

            // Sugar Eater:
            string DNA1 = "HKIJ";
            string DNA2 = "ABF";
            string DNA3 = "HKIJ";
            string DNA4 = "L";
            string DNA5 = "ABF";
            string DNA6 = "";
            string DNA7 = "";
            string DNA8 = "";
            string[] DNAsge = {DNA1, DNA2, DNA3, DNA4, DNA5, DNA6, DNA7, DNA8};
            Cell sugarEater = new Cell(DNAsge, 50, 50, 5, 5, 12, true);
            Cell sugarEater1 = new Cell(DNAsge, 50, 50, 6, 7, 12, true);
            Cell sugarEater2 = new Cell(DNAsge, 50, 50, 6, 6, 12, true);
            Cell sugarEater3 = new Cell(DNAsge, 50, 50, 7, 7, 12, true);

            // Dummy Cell
            string[] dummyDNA = new string[] {"A","A","A","A","A","A","A"};
            Cell dummyCell = new Cell(dummyDNA, 999, 999, 0, 0, 999, true);
            world.existingCells = new Cell[] {sugarEater, sugarEater1, sugarEater2, sugarEater3, bacterium};

            world.existingResources = allResources;
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
                for (int i = 0; i < Globals.maxY; i++) // Y
                {
                    for (int j = 0; j < Globals.maxX; j++) // X
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
                                        Console.WriteLine($"Error while printing resoruce: {l}");
                                        Console.WriteLine($"Resource Type: {world.existingResources[l].type}");
                                        Console.WriteLine($"Resource Coordinates: [{world.existingResources[l].X} {world.existingResources[l].Y}]");
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

        public Cell(string[] GivenDNA, int GivenEnergy, int GivenEGene, int BornCellX, int BornCellY, int maxAge, bool spawned)
        {
            age = 0;
            deathmark = maxAge;
            energy = GivenEnergy;
            DNACounter = new int[] {0,0,0,0,0,0,0,0}; // Sets counter to 0 for all DNA.
            DNAActive = new bool[] {true, true, false, false, false, false, false, false};
            DNA = GivenDNA;
            givenEGene = GivenEGene;

            CellX = BornCellX;
            CellY = BornCellY;

            if (!spawned)
            {
                Random rand = new Random();
                deathmark = maxAge + rand.Next(-1 * (10 - Globals.mutationChance), 10 * (10 - Globals.mutationChance));

                // Apply Mutation to dna.
                int mutation = rand.Next(0,Globals.mutationChance);
                DNA = GivenDNA;
                givenEGene = GivenEGene;

                // Mutate!
                if (mutation == 0)
                {
                    // Loop through all of the DNA
                    for (int i = 0; i < DNA.Length; i++)
                    {
                        rand = new Random();
                        mutation = rand.Next(0,Globals.mutationChance);
                        if (DNA[i].Length > 0)
                        {
                            int mutationLocation = rand.Next(0, DNA[i].Length);
                            int mutationLetter = rand.Next(0, Globals.dnaCharNum);
                            string mutatedDNA = DNA[i];
                            StringBuilder sb = new StringBuilder(mutatedDNA);
                            sb[mutationLocation] = Globals.dnaChars[mutationLetter];
                            mutatedDNA = sb.ToString();
                            Console.WriteLine($"Mutated a Gene! Old: {DNA[i]} New: {mutatedDNA}");
                            DNA[i] = mutatedDNA;
                        }

                        // Add possibility of adding another letter to a gene.
                        // This makes it possible for DNA to get either longer or shorter.
                        int geneLengthPossibility = rand.Next(0,3);

                        // Make Longer (1)
                        if (geneLengthPossibility == 1)
                        {
                            DNA[i]+=Globals.dnaChars[rand.Next(0, Globals.dnaCharNum)];
                        }

                        // Make Shorter (2)
                        if (geneLengthPossibility == 2 && DNA[i].Length > 0)
                        {
                            DNA[i] = DNA[i].Substring(0, DNA[i].Length-1);
                        }

                    }

                    // Mutate the givenEGene
                    givenEGene = givenEGene + rand.Next(-10 * (Globals.mutationChance/2), 10 * (Globals.mutationChance/2));
                }
            }
        }

        public void tick(World world)
        {
            // Check if about to die
            if (deathmark - age < deathmark/2)
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
                if (DNAActive[i] == true && DNA[i].Length > 0)
                {
                    // If the counter is higher than the length of the DNA, reset it to 0.
                    if (DNACounter[i] >= DNA[i].Length)
                    {
                        DNACounter[i] = 0;
                        if (i == 0)
                        {
                            DNAActive[0] = false;
                        }
                    }
                    string unprocessedDNA = DNA[i];
                    ProcessDNALetter(unprocessedDNA[DNACounter[i]], world);
                    // Increment the counter
                    DNACounter[i]++;
                    
                }
            }

            // Die
            if (age >= deathmark || energy <= 0 || sugar > Globals.maxResourceCapacity || oxygen > Globals.maxResourceCapacity || hydrogen > Globals.maxResourceCapacity)
            {
                for (int i = 0; i < world.existingCells.Length; i++)
                {
                    if (world.existingCells[i].CellX == CellX && world.existingCells[i].CellY == CellY)
                    {
                        // Delete cell
                        world.existingCells = Globals.RemoveCell(i, world.existingCells);

                        // Spit out resources.
                        for (int l = 0; l < sugar; l++)
                        {
                            Spit_FREE("Sugar", world);
                        }
                        for (int l = 0; l < oxygen; l++)
                        {
                            Spit_FREE("Oxygen", world);
                        }
                        for (int l = 0; l < hydrogen; l++)
                        {
                            Spit_FREE("Hydrogen", world);
                        }

                        // Break out of loop
                        i = world.existingCells.Length;
                        break;
                    }
                }
            }

            // Age Up
            age++;

            // Print stats
            Console.WriteLine($"Cell Stats: X:{CellX} Y:{CellY} Repreducing?:{deathmark - age < deathmark/2} Age:{age}/{deathmark} DNA:{DNA[0]} {DNA[1]} {DNA[2]} {DNA[3]} {DNA[4]} {DNA[5]} {DNA[6]} Energy: {energy} Oxygen:{oxygen} Sugar:{sugar} Hydrogen:{hydrogen}");
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
                    Multiply(world);
                    break;
                case 'M':
                    DNAActive[7] = true;
                    break;
                case 'N':
                    break;
                case 'O':
                    break;
                default:
                    break;
            }
        }

        public void Spit_FREE(string resource, World world)
        {
            Random rand = new Random();
            EResource newResource = new EResource();
            newResource.type = resource;
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
                            newResource.Y = CellY-1;
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
                            newResource.Y = CellY+1;
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
                            newResource.X = CellX-1;
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
                            newResource.X = CellX+1;
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

        public void Spit(string resource, World world)
        {
            Random rand = new Random();
            EResource newResource = new EResource();
            bool isValid = false;

            // Setup the resource and subtract from cell stockpile.
            switch (resource)
            {
                case "Sugar":
                    if (sugar >= 1)
                    {
                        sugar--;
                        newResource.type = "Sugar";
                        isValid = true;
                    }
                    else
                    {
                        isValid = false;
                    }
                    break;
                case "Oxygen":
                    if (oxygen >= 1)
                    {
                        oxygen--;
                        newResource.type = "Oxygen";
                        isValid = true;
                    }
                    else
                    {
                        isValid = false;
                    }
                    break;
                case "Hydrogen":
                    if (hydrogen >= 1)
                    {
                        hydrogen--;
                        newResource.type = "Hydrogen";
                        isValid = true;
                    }
                    else
                    {
                        isValid = false;
                    }
                    break;
                default:
                    if (oxygen >= 1)
                    {
                        oxygen--;
                        newResource.type = "Oxygen";
                    }
                    else
                    {
                        return;
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
                            newResource.Y = CellY-1;
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
                            newResource.Y = CellY+1;
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
                            newResource.X = CellX-1;
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
                            newResource.X = CellX+1;
                            found = true;
                        }
                    break;
            }

            if (found && isValid)
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
                    if (Globals.AreCoordsEmpty(CellX, CellY-1, world) && CellY > 0)
                    {
                        isValid = true;
                        addY = -1;
                    }
                    break;
                case "Down":
                    if (Globals.AreCoordsEmpty(CellX, CellY+1, world) && CellY < Globals.maxY-1)
                    {
                        isValid = true;
                        addY = 1;
                    }
                    break;
                case "Left":
                    if (Globals.AreCoordsEmpty(CellX-1, CellY, world) && CellX > 0)
                    {
                        isValid = true;
                        addX = -1;
                    }
                    break;
                case "Right":
                    if (Globals.AreCoordsEmpty(CellX+1, CellY, world) && CellX < Globals.maxX-1)
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
        public void Multiply(World world)
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
                Cell newCell = new Cell(DNA, maxenergy, givenEGene, CellX += addX, CellY += addY, deathmark, false);
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
                    if (energy >= Globals.processingSugEUsage && sugar > 0)
                    {
                        energy -= Globals.processingSugEUsage;
                        sugar--;
                        oxygen++;
                        energy += Globals.energyFromSugar;
                    }
                    break;
                case "Oxygen":
                    if (energy >= Globals.processingOxyEUsage && oxygen > 0)
                    {
                        energy -= Globals.processingOxyEUsage;
                        oxygen--;
                        hydrogen++;
                        energy += Globals.energyFromOxygen;
                    }
                    break;
                case "Hydrogen":
                    if (energy >= Globals.processingHydroEUsage && hydrogen > 0)
                    {
                        energy -= Globals.processingHydroEUsage;
                        hydrogen--;
                        sugar++;
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