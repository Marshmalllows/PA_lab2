using System.Diagnostics;

namespace PA_lab2;

public class EightPuzzle
{
    public int[][] Table { get; }
    
    private Stopwatch Stopwatch { get; set; }

    private Tree Nodes { get; set; }

    private List<Node>? Solution { get; set; }

    private static long NodeSize { get; } = sizeof(int) * 11 + sizeof(char) * 4 + IntPtr.Size * 2;

    private long UsedMemory => NodeSize * Nodes.Count;

    public EightPuzzle()
    {
        var random = new Random();
        var puzzle = new int[3][];
        var elements = new List<int>();

        for (var i = 0; i < puzzle.Length; i++)
        {
            puzzle[i] = new int[3];
            for (var j = 0; j < puzzle[i].Length; j++)
            {
                int number;

                do
                {
                    number = random.Next(9);
                } while (elements.Contains(number));
                
                elements.Add(number);
                puzzle[i][j] = number;
            }
        }
        
        Table = puzzle;
        Stopwatch = new Stopwatch();
        Nodes = new Tree(new TreeNode(new Node(Table, null, "Start")));
        Nodes.Count++;
        Solution = null;
    }

    public bool BreadthFirstSearch()
    {
        Stopwatch.Start();
        var queue = new Queue<TreeNode>();
        var currentLeaf = Nodes.Root;
        var isSolved = currentLeaf.Node.IsSolved();
        var iterations = 0;

        while (!isSolved)
        {
            if (UsedMemory > 1024L * 1024L * 1024 || Stopwatch.Elapsed.TotalMinutes >= 30)
            {
                Console.WriteLine("The algorithm could not find a solution");
                Stopwatch.Stop();
                return false;
            }

            iterations++;
            
            var availableActions = currentLeaf.Node.GetActions();
            for (var i = 0; i < availableActions.Length; i++)
            {
                if (!availableActions[i]) continue;
                string action;
                switch (i)
                {
                    case 0:
                        action = "Up";
                        currentLeaf.Up = new TreeNode(new Node(currentLeaf.Node.State, currentLeaf.Node, action));
                        Nodes.Count++;
                        queue.Enqueue(currentLeaf.Up);
                        break;
                    case 1:
                        action = "Right";
                        currentLeaf.Right = new TreeNode(new Node(currentLeaf.Node.State, currentLeaf.Node, action));
                        Nodes.Count++;
                        queue.Enqueue(currentLeaf.Right);
                        break;
                    case 2:
                        action = "Down";
                        currentLeaf.Down = new TreeNode(new Node(currentLeaf.Node.State, currentLeaf.Node, action));
                        Nodes.Count++;
                        queue.Enqueue(currentLeaf.Down);
                        break;
                    default:
                        action = "Left";
                        currentLeaf.Left = new TreeNode(new Node(currentLeaf.Node.State, currentLeaf.Node, action));
                        Nodes.Count++;
                        queue.Enqueue(currentLeaf.Left);
                        break;
                }
            }


            currentLeaf = queue.Dequeue();
            
            isSolved = currentLeaf.Node.IsSolved();
        }

        Stopwatch.Stop();

        Solution = new List<Node>();
        var foundSolution = false;

        do
        {
            Solution.Add(currentLeaf.Node);
            if (currentLeaf.Node.ParentNode is null)
            {
                foundSolution = true;
            }
            else
            {
                currentLeaf.Node = currentLeaf.Node.ParentNode;
            }
        } while (!foundSolution);

        Solution.Reverse();
        Console.WriteLine("Done! Here are steps:");
        foreach (var node in Solution)
        {
            Console.Write(node.Action + " ");
        }
        Console.WriteLine($"\nTime elapsed: {Stopwatch.Elapsed.TotalMilliseconds}ms");
        Console.WriteLine($"Iterations done: {iterations}");
        Console.WriteLine($"Total nodes: {Nodes.Count}");
        Console.WriteLine($"Nodes in memory: {Nodes.Count}");
        Console.WriteLine("Dead ends: 0");
        return true;
    }

    public bool AStar()
    {
        Stopwatch.Start();
        var priorityQueue = new PriorityQueue<TreeNode, int>();
        var currentLeaf = Nodes.Root;
        var states = new List<int[][]> { currentLeaf.Node.State };
        var isSolved = currentLeaf.Node.IsSolved();
        var iterations = 0;
        var totalNodes = 1;
        var deadEnds = 0;

        while (!isSolved)
        {
            if (UsedMemory > 1024L * 1024L * 1024 || Stopwatch.Elapsed.TotalMinutes >= 30)
            {
                Console.WriteLine("The algorithm could not find a solution");
                Stopwatch.Stop();
                return false;
            }

            iterations++;

            var availableActions = currentLeaf.Node.GetActions();
            var isDeadEnd = true;
            for (var i = 0; i < availableActions.Length; i++)
            {
                if (!availableActions[i]) continue;
                string action;
                switch (i)
                {
                    case 0:
                        action = "Up";
                        currentLeaf.Up = new TreeNode(new Node(currentLeaf.Node.State, currentLeaf.Node, action));
                        Nodes.Count++;
                        if (!states.Contains(currentLeaf.Up.Node.State))
                        {
                            priorityQueue.Enqueue(currentLeaf.Up, currentLeaf.Node.ManhattanDistance(currentLeaf.Up));
                            isDeadEnd = false;
                        }
                        else
                        {
                            currentLeaf.Up = null;
                            Nodes.Count--;
                        }
                        totalNodes++;
                        break;
                    case 1:
                        action = "Right";
                        currentLeaf.Right = new TreeNode(new Node(currentLeaf.Node.State, currentLeaf.Node, action));
                        Nodes.Count++;
                        if (!states.Contains(currentLeaf.Right.Node.State))
                        {
                            priorityQueue.Enqueue(currentLeaf.Right,
                                currentLeaf.Node.ManhattanDistance(currentLeaf.Right));
                            isDeadEnd = false;
                        }
                        else
                        {
                            currentLeaf.Right = null;
                            Nodes.Count--;
                        }
                        totalNodes++;
                        break;
                    case 2:
                        action = "Down";
                        currentLeaf.Down = new TreeNode(new Node(currentLeaf.Node.State, currentLeaf.Node, action));
                        Nodes.Count++;
                        if (!states.Contains(currentLeaf.Down.Node.State))
                        {
                            priorityQueue.Enqueue(currentLeaf.Down,
                                currentLeaf.Node.ManhattanDistance(currentLeaf.Down));
                            isDeadEnd = false;
                        }
                        else
                        {
                            currentLeaf.Down = null;
                            Nodes.Count--;
                        }
                        totalNodes++;
                        break;
                    default:
                        action = "Left";
                        currentLeaf.Left = new TreeNode(new Node(currentLeaf.Node.State, currentLeaf.Node, action));
                        Nodes.Count++;
                        if (!states.Contains(currentLeaf.Left.Node.State))
                        {
                            priorityQueue.Enqueue(currentLeaf.Left,
                                currentLeaf.Node.ManhattanDistance(currentLeaf.Left));
                            isDeadEnd = false;
                        }
                        else
                        {
                            currentLeaf.Left = null;
                            Nodes.Count--;
                        }
                        totalNodes++;
                        break;
                }
            }

            if (isDeadEnd)
            {
                deadEnds++;
            }
            
            currentLeaf = priorityQueue.Dequeue();
            
            isSolved = currentLeaf.Node.IsSolved();
        }

        Solution = new List<Node>();
        var foundSolution = false;

        do
        {
            Solution.Add(currentLeaf.Node);
            if (currentLeaf.Node.ParentNode is null)
            {
                foundSolution = true;
            }
            else
            {
                currentLeaf.Node = currentLeaf.Node.ParentNode;
            }
        } while (!foundSolution);

        Solution.Reverse();
        Console.WriteLine("Done! Here are steps:");
        foreach (var node in Solution)
        {
            Console.Write(node.Action + " ");
        }
        Console.WriteLine($"\nTime elapsed: {Stopwatch.Elapsed.TotalMilliseconds}ms");
        Console.WriteLine($"Iterations done: {iterations}");
        Console.WriteLine($"Total nodes: {totalNodes}");
        Console.WriteLine($"Nodes in memory: {Nodes.Count}");
        Console.WriteLine($"Dead ends: {deadEnds}");

        return true;
    }
    
    public static bool IsSolvable(int[][] puzzle)
    {
        var flatPuzzle = new int[9];
        var index = 0;

        foreach (var row in puzzle)
        {
            foreach (var number in row)
            {
                flatPuzzle[index++] = number;
            }
        }

        var inversions = CountInversions(flatPuzzle);
        return inversions % 2 == 0;
    }

    private static int CountInversions(int[] puzzle)
    {
        var inversions = 0;

        for (var i = 0; i < puzzle.Length; i++)
        {
            if (puzzle[i] == 0) continue;

            for (var j = i + 1; j < puzzle.Length; j++)
            {
                if (puzzle[j] == 0) continue; 
                if (puzzle[i] > puzzle[j])
                {
                    inversions++;
                }
            }
        }

        return inversions;
    }
}