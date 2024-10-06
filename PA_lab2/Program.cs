namespace PA_lab2;

public static class Program
{
    public static void Main()
    {
        Console.Write("Select algorithm to solve 8-puzzle:\n" +
                      "1.BFS\n" +
                      "2.A*\n" +
                      "Choice:");
        var choice = int.Parse(Console.ReadLine()!);
        Console.Write("Do while not solved?:");
        var doWhileNotSolved = Console.ReadLine() == "yes";
        var isSolved = false;
        
        do
        {
            var eightPuzzle = new EightPuzzle();
            Console.Clear();
            Console.WriteLine("Here is your puzzle:");
            foreach (var row in eightPuzzle.Table)
            {
                Console.Write("|");
                foreach (var element in row)
                {
                    Console.Write(element == 0 ? " |" : $"{element}|");
                }

                Console.WriteLine();
            }

            if (choice == 1)
            {
                isSolved = eightPuzzle.BreadthFirstSearch();
            }
            else
            {
                eightPuzzle.AStar();
            }

            if (!doWhileNotSolved)
            {
                isSolved = true;
            }
        } while (!isSolved);
    }
}