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
        Console.Write("Must puzzle be solvable:");
        var isSolvable = Console.ReadLine() == "yes";
        Console.Write("Do while not solved?:");
        var doWhileNotSolved = Console.ReadLine() == "yes";
        bool isSolved;
        
        do
        {
            var eightPuzzle = new EightPuzzle();

            while (isSolvable && !EightPuzzle.IsSolvable(eightPuzzle.Table))
            {
                eightPuzzle = new EightPuzzle();
            }
            
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

            isSolved = (choice == 1 ? eightPuzzle.BreadthFirstSearch() : eightPuzzle.AStar()) || !doWhileNotSolved;
        } while (!isSolved);
    }
}