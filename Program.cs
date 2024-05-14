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
            
            // var adjecentChars = FindSurroundingNumbers(lineIndex, charIndex);
            var adjecentNumbers = FindSurroundingNumbers(lineIndex, charIndex);

            foreach (var adjecentNumber in adjecentNumbers)
            {
                Console.WriteLine(adjecentNumber);
            }
            // foreach (var adjecentChar in adjecentChars)
            // {
            //     if (adjecentChar.Value != null && char.IsDigit(adjecentChar.Value.GetValueOrDefault()))
            //     {
            //         Console.WriteLine($"{adjecentChar.Key} : {adjecentChar.Value}");
            //     }
            //     
            // }
        }
    }

    static List<string> FindSurroundingNumbers(int lineIndex, int charIndex)
    {
        List<string> surroundingNumbers = new List<string>();
        int maxLineIndex = _coordinates.GetLength(0) - 1;
        int maxCharIndex = _coordinates.GetLength(1) - 1;

        for (int xMovement = -1; xMovement <= 1; xMovement++)
        {
            for (int yMovement = -1; yMovement <= 1; yMovement++)
            {
                if(xMovement == 0 && yMovement == 0) continue; //If on the current position, just move to the next position
                int currentCol = charIndex + xMovement; //move left or right
                int currentRow = lineIndex + yMovement; //move up or down
                while (currentCol >= 0 && currentCol <= maxCharIndex && currentRow >= 0 && currentRow <= maxLineIndex &&
                       char.IsDigit(_coordinates[currentRow, currentCol]))
                {
                    string number = "";
                    //Found a digit, now look left and right for another digit
                    string leftNumber = "", rightNumber = "";
                    if (char.IsDigit(_coordinates[currentRow, currentCol - 1]))
                    {
                        leftNumber = _coordinates[currentRow, currentCol - 1].ToString();
                    }

                    if (char.IsDigit(_coordinates[currentRow, currentCol + 1]))
                    {
                        rightNumber = _coordinates[currentRow, currentCol + 1].ToString();
                    }

                    number = $"{leftNumber}{_coordinates[currentRow, currentCol].ToString()}{rightNumber}";
                    
                    surroundingNumbers.Add(number);
                    currentCol += xMovement;
                    currentRow += yMovement;
                }
            }
        }

        return surroundingNumbers;
    }
    
    static List<KeyValuePair<string, char?>> FindAjcentNumbers(int lineIndex, int charIndex)
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
        
        int maxLineIndex = _coordinates.GetLength(0) - 1;
        int maxCharIndex = _coordinates.GetLength(1) - 1;

        // Don't look north if lineIndex is 0
        if (lineIndex > 0)
        {
            adjacentChars["north"] = _coordinates[lineIndex - 1, charIndex];

            // Don't look west if charIndex is 0
            if (charIndex > 0)
                adjacentChars["northWest"] = _coordinates[lineIndex - 1, charIndex - 1];

            // Don't look east if charIndex is at the maximum
            if (charIndex < maxCharIndex)
                adjacentChars["northEast"] = _coordinates[lineIndex - 1, charIndex + 1];
        }

        // Don't look south if lineIndex is at the maximum
        if (lineIndex < maxLineIndex)
        {
            adjacentChars["south"] = _coordinates[lineIndex + 1, charIndex];

            // Don't look west if charIndex is 0
            if (charIndex > 0)
                adjacentChars["southWest"] = _coordinates[lineIndex + 1, charIndex - 1];

            // Don't look east if charIndex is at the maximum
            if (charIndex < maxCharIndex)
                adjacentChars["southEast"] = _coordinates[lineIndex + 1, charIndex + 1];
        }

        // Don't look west if charIndex is 0
        if (charIndex > 0)
            adjacentChars["west"] = _coordinates[lineIndex, charIndex - 1];

        // Don't look east if charIndex is at the maximum
        if (charIndex < maxCharIndex)
            adjacentChars["east"] = _coordinates[lineIndex, charIndex + 1];

        var result = adjacentChars.ToList();
        return result;
    }
}



