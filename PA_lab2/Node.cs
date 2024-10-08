namespace PA_lab2;

public class Node
{
    public int[][] State { get; set; }

    public Node? ParentNode { get; set; }

    public string Action { get; set; }

    private int PathCost { get; set; }

    private int Depth { get; set; }

    public Node(int[][] table, Node? parentNode, string action)
    {
        var tableCopy = new int[table.Length][];
        for (var i = 0; i < table.Length; i++)
        {
            tableCopy[i] = new int[table[i].Length];
            for (var j = 0; j < table[i].Length; j++)
            {
                tableCopy[i][j] = table[i][j];
            }
        }

        State = tableCopy;
        ParentNode = parentNode;
        Action = action;
        
        switch (action)
        {
            case "Up":
                MoveUp();
                break;
            case "Right":
                MoveRight();
                break;
            case "Down":
                MoveDown();
                break;
            case "Left":
                MoveLeft();
                break;
        }

        if (parentNode is null)
        {
            PathCost = 0;
            Depth = 0;
        }
        else
        {
            PathCost = parentNode.PathCost + 1;
            Depth = parentNode.Depth + 1;
        }
    }
    
    public bool[] GetActions()
    {
        var actions = new bool[4];
        var emptyElement = GetEmptyElement();
        
        for (var i = 0; i < actions.Length; i++)
        {
            actions[i] = true;
        }

        if (emptyElement.row == 2)
        {
            actions[0] = false;
        }

        if (emptyElement.col == 0)
        {
            actions[1] = false;
        }

        if (emptyElement.row == 0)
        {
            actions[2] = false;
        }

        if (emptyElement.col == 2)
        {
            actions[3] = false;
        }

        return actions;
    }

    private (int row, int col) GetEmptyElement()
    {
        var empty = (0, 0);
        for (var i = 0; i < State.Length; i++)
        {
            for (var j = 0; j < State[i].Length; j++)
            {
                if (State[i][j] == 0)
                {
                    empty = (i, j);
                }
            }
        }

        return empty;
    }
    
    private void MoveUp()
    {
        var emptyElement = GetEmptyElement();
        (State[emptyElement.row][emptyElement.col], State[emptyElement.row + 1][emptyElement.col]) = 
            (State[emptyElement.row + 1][emptyElement.col], State[emptyElement.row][emptyElement.col]);
    }
    
    private void MoveDown()
    {
        var emptyElement = GetEmptyElement();
        (State[emptyElement.row][emptyElement.col], State[emptyElement.row - 1][emptyElement.col]) = 
            (State[emptyElement.row - 1][emptyElement.col], State[emptyElement.row][emptyElement.col]);
    }
    
    private void MoveRight()
    {
        var emptyElement = GetEmptyElement();
        (State[emptyElement.row][emptyElement.col], State[emptyElement.row][emptyElement.col - 1]) = 
            (State[emptyElement.row][emptyElement.col - 1], State[emptyElement.row][emptyElement.col]);
    }
    
    private void MoveLeft()
    {
        var emptyElement = GetEmptyElement();
        (State[emptyElement.row][emptyElement.col], State[emptyElement.row][emptyElement.col + 1]) = 
            (State[emptyElement.row ][emptyElement.col + 1], State[emptyElement.row][emptyElement.col]);
    }
    
    public bool IsSolved()
    {
        var isSolved = true;
        var current = 1;

        for (var i = 0; i < State.Length; i++)
        {
            for (var j = 0; j < State[i].Length; j++)
            {
                if (State[i][j] == current || (i == State.Length - 1 && j == State.Length - 1 && State[i][j] == 0))
                {
                    current++;
                }
                else
                {
                    isSolved = false;
                }
            }
        }

        return isSolved;
    }
    
    public int ManhattanDistance(TreeNode potentialStep)
    {
        var distance = 0;
        var puzzle = potentialStep.Node.State;

        for (var i = 0; i < puzzle.Length; i++)
        {
            for (var j = 0; j < puzzle[i].Length; j++)
            {
                if (puzzle[i][j] != 0)
                {
                    var target = ((puzzle[i][j] - 1) / 3, (puzzle[i][j] - 1) % 3);
                    distance += int.Abs(i - target.Item1) + int.Abs(j - target.Item2);
                }
                else
                {
                    distance += int.Abs(i - 2) + int.Abs(j - 2);
                }
            }
        }

        return distance + PathCost;
    }
}