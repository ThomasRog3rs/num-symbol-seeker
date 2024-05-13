using System;
using System.IO;
using System.Linq;

class Program
{
    private static char[,] _coordinates = new char[140, 140];
    
    static void Main(string[] args)
    {
        string calibrationDocument =
            "/home/tom/RiderProjects/num-symbol-seeker/num-symbol-seeker/EngineSchematic.txt";
            
        string[] lines = File.ReadAllLines(calibrationDocument);
        Dictionary<string, char> symbolPositions = new Dictionary<string, char>();
        
        int currentLine = 1;
        foreach (var line in lines)
        {
            int currentChar = 1;
            foreach (char character in line.ToCharArray())
            {
                _coordinates[currentLine - 1, currentChar - 1] = character;
                if (character != '.' && !char.IsDigit(character))
                {
                    symbolPositions.Add($"{currentLine}-{currentChar}", character);
                }
                currentChar++;
            }
            Console.WriteLine(line);
            currentLine++;
        }
        
        Console.WriteLine(_coordinates);
        for (var i = 0; i < _coordinates.GetLength(0); i++)
        {
            for (var j = 0; i < _coordinates.GetLength(1); i++)
            {
                Console.WriteLine(_coordinates[i, j]);
            }
        }
        foreach (var keyValuePair in symbolPositions)
        {
            Console.WriteLine($"The symbol {keyValuePair.Value} was found at position {keyValuePair.Key}");
            string[] positionParts = keyValuePair.Key.Split('-');
            int lineIndex = Int32.Parse(positionParts[0]) - 1;
            int charIndex = Int32.Parse(positionParts[1]) - 1;
        }
    }

    static List<int> findAjcentNumbers(int lineIndex, int charIndex)
    {
        Dictionary<string, char?> adjacentChars = new Dictionary<string, char?>
        {
            {"north", null},
            {"northEast", null},
            {"east", null},
            {"southEast", null},
            {"south", null},
            {"southWest", null},
            {"west", null},
            {"northWest", null},
        };
        
        //Don't look north
        if (lineIndex == 0)
        {
            adjacentChars["east"] = _coordinates[lineIndex, charIndex + 1];
            adjacentChars["southEast"] = _coordinates[lineIndex - 1, charIndex + 1];
            adjacentChars["south"] = _coordinates[lineIndex - 1, charIndex];
            adjacentChars["southWest"] = _coordinates[lineIndex - 1, charIndex - 1];
            adjacentChars["west"] = _coordinates[lineIndex, charIndex - 1];
        }

        //Don't look south
        if (lineIndex == 139)
        {
            adjacentChars["north"] = _coordinates[lineIndex + 1, charIndex];
            adjacentChars["northEast"] = _coordinates[lineIndex + 1, charIndex + 1];
            adjacentChars["east"] = _coordinates[lineIndex, charIndex + 1];
            adjacentChars["west"] = _coordinates[lineIndex, charIndex - 1];
            adjacentChars["northWest"] = _coordinates[lineIndex + 1, charIndex - 1];
        }
        
        return new List<int>();
    }
}



