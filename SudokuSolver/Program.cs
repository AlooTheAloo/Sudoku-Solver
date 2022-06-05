using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver
{
    class Program
    {
        public static void Main(String[] args)
        {

            //Initial sudoku 2D array
            int[][] sudoku = new int[][]
            {
                new int[]{0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[]{0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[]{0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[]{0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[]{0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[]{0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[]{0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[]{0, 0, 0, 0, 0, 0, 0, 0, 8},
                new int[]{0, 0, 0, 0, 0, 0, 0, 0, 0}
            };
            FillArray(sudoku); // Text-based input for sudoku

            PrintSudoku(sudoku); //Shows the initial sodoku
            Console.WriteLine("Searching for solution... (this might take a while)"); 

            //Recursive method that solves the sudoku
            int[][] answer = SudokuSolver(sudoku, 0, 0); 

            // If there is no solution, show error message
            if (answer[0] == null) Console.WriteLine("\nThere is no solver for this soduku :(");            
            else PrintSudoku(answer); // Shows final sudoku if there is a solution


        }
        private static void FillArray(int[][] board)
        {
            //Goes through the 9x9 and asks for 9 characters
            for(int i = 0; i < board.Length; i++)
            {
                if (i < 0) i = 0;
                Console.WriteLine("Enter the line #" + (i + 1) + "(b to go back)"));
                String input  = Console.ReadLine(); // Read the input
                if (input.ToLower() == "b") i-= 2; // Go back if needed
                else if (input.Length != 9) { Console.WriteLine("That input is incorrect, please try again !"); i--; } //Wrong input 
            else for (int j = 0; j < input.Length; j++) // If input is correct, fill the board with it
                        board[i][j] = input[j] - '0';
                
            }
           
        }

        //Simple 2D printer
        private static void PrintSudoku(int[][] board)
        {
            Console.WriteLine();
            for (int i = 0; i < board.Length; i++)
            {
                for (int j = 0; j < board[i].Length; j++)
                    Console.Write(board[i][j] + " ");
                Console.WriteLine();
            }
        }


    private static int[][] SudokuSolver(int[][] board, int x, int y)
    {
        //Attempt value
        int[][] testBoard = new int[9][];
            
        //2D sudoku cloning
        for (int i = 0; i < testBoard.Length; i++)
        {
            testBoard[i] = new int[9];
            for (int j = 0; j < testBoard[i].Length; j++) 
                testBoard[i][j] = board[i][j];
        }

        if (y > 8) return testBoard; //Board complete, return it! (base case)
        while (testBoard[x][y] != 0)
        {
            if (x == 8 && y == 8) return testBoard; // Other base case
            y = (x == 8) ? y + 1 : y; //Add one to the y
            x = (x == 8) ? 0 : x + 1; //Return to last row if x is at full
        }
        //For ever possible number
        for (int i = 1; i <= 9; i++)
        {
            //Try it!
            testBoard[x][y] = i;
            //Does it work?
            bool validated = Validate(testBoard);
            //If not, try the next one
            if (!validated) continue;
            //If it did, go to the next!
            int[][] newSudoku = SudokuSolver(testBoard, x == 8 ? 0 : x + 1, (x == 8) ? y + 1 : y);
            if (newSudoku[0] == null) { continue; } //Solution with this number is not possible, go to next
            return newSudoku; //Done with 9 numbers, we return it!
        }
        return new int[9][]; //Did not find solution, return empty array
    }

    private static int[] nineNumbers = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };


    //Valid sudoku position?
    public static bool Validate(int[][] board)
    {
        for (int i = 0; i < 9; ++i)
        {
            //Row
            var row = new List<int>();
            for (int j = 0; j < 9; ++j) { if (board[i][j] == 0) continue; row.Add(board[i][j]); }
            if (!ValidateNine(row)) return false;
            
            //Column
            var col = new List<int>();
            for (int j = 0; j < 9; ++j) { if (board[j][i] == 0) continue; col.Add(board[j][i]); }
            if (!ValidateNine(col)) return false;

            //3x3 block
            var block = new List<int>();
            int br = (i / 3) * 3;
            int bc = (i % 3) * 3;
            for (int j = 0; j < 9; ++j) { if (board[br + j / 3][bc + j % 3] == 0) continue; block.Add(board[br + j / 3][bc + j % 3]); }
            if (!ValidateNine(block)) return false;
        }

        return true;
    }

    private static bool ValidateNine(IList<int> nine)
    {
        bool[] nums = new bool[9]; //9 Numbers represented as 9 bools
        for (int i = 0; i < nine.ToList().Count; i++)
        {
            if (!nums[nine.ToList()[i] - 1])
                nums[nine.ToList()[i] - 1] = true;
                else return false; //If they are seen more then once, return false
            }
            return true; //If not return true
        }
    }
}