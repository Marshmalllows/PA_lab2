# Lab 2 — 8-Puzzle Solver: BFS and A*

**Subject:** Algorithm Design  
**Year:** 2nd year, 1st semester, 2024  
**Author:** Maksym Poliukhovych

---

## Overview

This lab implements two search algorithms to solve the **8-puzzle** (sliding tile puzzle on a 3×3 grid):

- **BFS** — Breadth-First Search (uninformed, guarantees shortest path)
- **A\*** — A* Search (informed, uses Manhattan distance heuristic)

The puzzle is randomly generated at startup. The goal state is tiles 1–8 arranged left-to-right, top-to-bottom, with the empty cell in the bottom-right corner.

---

## Algorithms

### BFS

Explores nodes level by level using a FIFO queue. Guaranteed to find the shortest solution but explores significantly more nodes than A*.

**Termination conditions:**
- Solution found
- Memory usage exceeds 1 GB
- Runtime exceeds 30 minutes

### A*

Uses a priority queue ordered by `f(n) = g(n) + h(n)`:
- `g(n)` — path cost (depth)
- `h(n)` — Manhattan distance heuristic (sum of each tile's distance from its goal position)

Visited states are tracked to avoid revisiting. Finds an optimal solution much faster than BFS in practice.

---

## Solvability

An 8-puzzle is solvable if and only if the number of **inversions** in the flattened tile sequence (excluding the blank) is even. The program optionally filters unsolvable puzzles before attempting to solve.

---

## Project Structure

```
PA_lab2/
├── PA_lab2.sln
└── PA_lab2/
    ├── Program.cs       # Entry point, user prompts
    ├── EightPuzzle.cs   # BFS and A* implementations, solvability check
    ├── Node.cs          # Search node (state, parent, action, depth, path cost)
    ├── TreeNode.cs      # Tree node wrapper with Up/Down/Left/Right children
    └── Tree.cs          # Search tree with node counter
```

---

## How to Run

### Prerequisites

- [.NET 8+](https://dotnet.microsoft.com/download) SDK

### Steps

```bash
cd PA_lab2
dotnet run --project PA_lab2
```

### Usage

```
Select algorithm to solve 8-puzzle:
1.BFS
2.A*
Choice: 2
Must puzzle be solvable: yes
Do while not solved?: no

Here is your puzzle:
|3|1|2|
|4| |5|
|6|7|8|

Done! Here are steps:
Start Right Down Left Up ...
Time elapsed: 12ms
Iterations done: 47
Total nodes: 183
Nodes in memory: 121
Dead ends: 3
```

---

## Output Fields

| Field | Description |
|---|---|
| Steps | Sequence of moves applied to the blank tile |
| Time elapsed | Wall-clock time in milliseconds |
| Iterations done | Number of nodes dequeued/expanded |
| Total nodes | Nodes generated (including pruned duplicates, A* only) |
| Nodes in memory | Nodes currently held in the search tree |
| Dead ends | Nodes where all children were already visited (A* only) |

---

## Complexity

| Property | BFS | A* |
|---|---|---|
| Time | O(b^d) | O(b^d), much smaller in practice |
| Space | O(b^d) | O(b^d) |
| Optimal | Yes | Yes (admissible heuristic) |
| Heuristic | None | Manhattan distance |

where `b` = branching factor (≤4), `d` = solution depth.
