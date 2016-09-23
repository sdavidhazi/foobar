using System;
using System.Collections.Generic;
using System.IO;
using Contracts;
using Impl.Model;
using Impl.Test.Random;
using Impl.Test.TestData;
using NUnit.Framework;
using NUnit.Util;

namespace Impl.Test
{
    [TestFixture]
    public partial class SolverTest
    {
        #region Puzzle Constants
        const string PuzzleSmall = @"  0    
 #   2 
      3
   0   
#      
 1   # 
    2  ";

        const string PuzzleMiddle = @"   #      1   
 1 #   1    # 
  1 1 21   #  
#       #   2#
      1 2  2  
   00         
 ##      2 #  
  # #      #0 
         3#   
  #  0 #      
2#   #       #
  3   #0 # 3  
 #    #   # # 
   #      #   ";

        const string PuzzleLarge = @"# #       1 #     ##  2 #
  # # 1#     10#     #   
2 1    1   #   ##  3  ###
 # #          1   ## #   
    # 0  #       0  #  3 
# 0#     ##   #          
1  #  #  2#  0   #0 #  # 
    0 #       # #     0# 
  1    ## ##    #        
 01         #2    0##    
 1 # # #   0    # ##    3
 #    3  2 2 2# #     #  
1        #     #        2
  #     # 0# # 2  3    # 
2    1# #    0   # 2 0 # 
    ###    ##         #0 
        2    #1 #0    #  
 1#     # #       # 3    
 #  3 ##   #  ##  1  #  #
          2   ##     #2 #
 1  2  #       #  0 1    
   # ##   1          # # 
###  #  21   0   2    # 2
   3     1##     ## # #  
# #  1#     # 3       1 #";

        const string Puzzle_25x25_Challenging = @"   ##  ## 2     #   1 2  
 #    #   #  #  ### #  # 
2 ##      #  ## ## 1  #  
    ### 2### # #      # #
2#  #    #          ##  #
  0  # # # # 0#  # # #   
 #      ###     ##   # # 
 ##  ## 1  #  0 #  #    #
###   ##   #  #  0#  0  #
   #          2   ####   
  #  # 1#1 # 1    #  ###0
 ### 1    1# ## ## # #   
                         
   # # ## ## #1    1 ### 
1###  #    2 # 0#1 #  #  
   ####   1          #   
#  2  #2  #  #   ##   ###
#    #  # 0  #  2 ##  ## 
 # #   ##     ###      # 
   # # #  #2 # # # #  2  
#  ##          #    #  #1
# #      # # ###1 ###    
  #  1 ## ##  #      ## 2
 #  # ###  #  #   #    # 
  2 2   #     1 ##  ##   ";

        #endregion

        private const int TimeoutShort = 5000;
        private const int TimeoutLong = 25000;

        [Test]
        [Ignore("For test puzzle generation only")]
        public void GeneratePuzzles()
        {            
            var testDataFactory = new TestDataFactory(new ParallelSolver(), new RngRandomProvider());

            var sizes = new[] { 70, 100, 120, 200 };
            const int count = 20;

            foreach (var size in sizes)
            {
                for (var i = 0; i < count; i++)
                {
                    var puzzle = testDataFactory.GenerateRandomPuzzle(size, size * 16, 0.3);

                    string time = DateTime.UtcNow.ToString("hhmmss");
                    string directory = $@"c:\Temp\Puzzles\{size}\{time}";

                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    string path = $@"{directory}\{i}.txt";
                    File.WriteAllText(path, puzzle);
                }
            }
        }

        [Test]
        [Timeout(TimeoutShort)]
        public void Solve_Challenging_Table_25x25()
        {
            // Arrange
            var puzzle = Puzzle_25x25_Challenging;
            var solver = CreateSolver();

            // Act
            RunTestWithSinglePuzzle(solver, puzzle);
        }

        [Test]
        [Timeout(TimeoutShort)]
        public void Solve_Small_Table()
        {
            // Arrange
            var puzzle = PuzzleSmall;
            var solver = CreateSolver();

            // Act
            RunTestWithSinglePuzzle(solver, puzzle);
        }

        [Test]
        [Timeout(TimeoutShort)]
        public void Solve_Mid_Table()
        {
            // Arrange
            var puzzle = PuzzleMiddle;
            var solver = CreateSolver();

            // Act
            RunTestWithSinglePuzzle(solver, puzzle);
        }

        [Test]
        [Timeout(TimeoutShort)]
        public void Solve_Large_Table()
        {
            // Arrange
            var puzzle = PuzzleLarge;
            var solver = CreateSolver();

            // Act
            RunTestWithSinglePuzzle(solver, puzzle);
        }

        [Test]
        [Timeout(TimeoutLong)]
        public void Solve_Small_Replaced_Puzzles()
        {
            // Arrange
            var puzzle = PuzzleSmall;
            var solver = CreateSolver();
            var random = new RngRandomProvider();
            var testDataFactory = new TestDataFactory(new Solver(), random);
            var puzzles = testDataFactory.TransformByReplace(puzzle);

            // Act
            RunTestWithMultiplePuzzles(solver, puzzles);
        }

        [Test]
        [Timeout(TimeoutLong)]
        public void Solve_Mid_Replaced_Puzzles()
        {
            // Arrange
            var puzzle = PuzzleMiddle;
            var solver = CreateSolver();
            var random = new RngRandomProvider();
            var testDataFactory = new TestDataFactory(new Solver(), random);

            var puzzles = testDataFactory.TransformByReplace(puzzle);

            // Act
            RunTestWithMultiplePuzzles(solver, puzzles);
        }

        [Test]
        [Timeout(TimeoutLong)]
        public void Solve_Large_Replaced_Puzzles()
        {
            // Arrange
            var puzzle = PuzzleLarge;
            var solver = CreateSolver();
            var random = new RngRandomProvider();
            var testDataFactory = new TestDataFactory(new Solver(), random);

            var puzzles = testDataFactory.TransformByReplace(puzzle);

            // Act
            RunTestWithMultiplePuzzles(solver, puzzles);
        }

        [Test]
        [Timeout(TimeoutLong)]
        public void Solve_Small_Rotated_Puzzles()
        {
            // Arrange
            var puzzle = PuzzleSmall;
            var solver = CreateSolver();
            var random = new RngRandomProvider();
            var testDataFactory = new TestDataFactory(new Solver(), random);

            var puzzles = testDataFactory.TransformByRotation(puzzle);

            // Act
            RunTestWithMultiplePuzzles(solver, puzzles);
        }

        [Test]
        [Timeout(TimeoutLong)]
        public void Solve_Mid_Rotated_Puzzles()
        {
            // Arrange
            var puzzle = PuzzleMiddle;
            var solver = CreateSolver();
            var random = new RngRandomProvider();
            var testDataFactory = new TestDataFactory(new Solver(), random);

            var puzzles = testDataFactory.TransformByRotation(puzzle);

            // Act
            RunTestWithMultiplePuzzles(solver, puzzles);
        }

        [Test]
        [Timeout(TimeoutLong)]
        public void Solve_Large_Rotated_Puzzles()
        {
            // Arrange
            var puzzle = PuzzleLarge;
            var solver = CreateSolver();
            var random = new RngRandomProvider();
            var testDataFactory = new TestDataFactory(new Solver(), random);

            var puzzles = testDataFactory.TransformByRotation(puzzle);

            // Act
            RunTestWithMultiplePuzzles(solver, puzzles);
        }

        [Test]
        [Timeout(TimeoutShort)]
        public void Solve_Random_Puzzle_20x20()
        {
            // Arrange
            const int size = 20;
            var solver = CreateSolver();
            var random = new RngRandomProvider();
            var testDataFactory = new TestDataFactory(new Solver(), random);

            var puzzle = testDataFactory.GenerateRandomPuzzle(size);

            RunTestWithSinglePuzzle(solver, puzzle);
        }

        [Test]
        [Timeout(TimeoutShort)]
        public void Solve_Random_Puzzle_40x40()
        {
            // Arrange
            const int size = 40;
            var solver = CreateSolver();
            var random = new RngRandomProvider();
            var testDataFactory = new TestDataFactory(new Solver(), random);

            var puzzle = testDataFactory.GenerateRandomPuzzle(size);

            RunTestWithSinglePuzzle(solver, puzzle);
        }

        [Test]
        [Timeout(TimeoutShort)]
        public void Solve_Random_Puzzle_60x60()
        {
            // Arrange
            const int size = 60;
            var solver = CreateSolver();
            var random = new RngRandomProvider();
            var testDataFactory = new TestDataFactory(new Solver(), random);

            var puzzle = testDataFactory.GenerateRandomPuzzle(size);

            RunTestWithSinglePuzzle(solver, puzzle);
        }

        [Test]
        [Timeout(TimeoutShort)]
        public void Solve_Random_Puzzle_80x80()
        {
            // Arrange
            const int size = 80;
            var solver = CreateSolver();
            var random = new RngRandomProvider();
            var testDataFactory = new TestDataFactory(new Solver(), random);

            var puzzle = testDataFactory.GenerateRandomPuzzle(size);

            RunTestWithSinglePuzzle(solver, puzzle);
        }

        [Test]
        [Timeout(TimeoutShort)]
        public void Solve_Random_Puzzle_100x100()
        {
            // Arrange
            const int size = 100;
            var solver = CreateSolver();
            var random = new RngRandomProvider();
            var testDataFactory = new TestDataFactory(new Solver(), random);

            var puzzle = testDataFactory.GenerateRandomPuzzle(size);

            RunTestWithSinglePuzzle(solver, puzzle);
        }

        [Test]
        [Timeout(TimeoutLong)]
        public void Solve_Random_Puzzle_200x200()
        {
            // Arrange
            const int size = 200;
            var solver = CreateSolver();
            var random = new RngRandomProvider();
            var testDataFactory = new TestDataFactory(new Solver(), random);

            var puzzle = testDataFactory.GenerateRandomPuzzle(size);

            RunTestWithSinglePuzzle(solver, puzzle);
        }

        [Test]
        [Timeout(TimeoutLong)]
        public void Solve_WellKnown_Puzzle_14x14()
        {
            // Arrange
            var solver = CreateSolver();
            var puzzles = WellKnownPuzzles.Puzzles_14x14;

            RunTestWithMultiplePuzzles(solver, puzzles);
        }

        [Test]
        [Timeout(TimeoutLong)]
        public void Solve_WellKnown_Puzzle_20x20()
        {
            // Arrange
            var solver = CreateSolver();
            var puzzles = WellKnownPuzzles.Puzzles_20x20;

            RunTestWithMultiplePuzzles(solver, puzzles);
        }

        [Test]
        [Timeout(TimeoutLong)]
        public void Solve_WellKnown_Puzzle_25x25()
        {
            // Arrange
            var solver = CreateSolver();
            var puzzles = WellKnownPuzzles.Puzzles_25x25;

            RunTestWithMultiplePuzzles(solver, puzzles);
        }

        [Test]
        [Timeout(TimeoutLong)]
        public void Solve_WellKnown_Puzzle_30x30()
        {
            // Arrange
            var solver = CreateSolver();
            var puzzles = WellKnownPuzzles.Puzzles_30x30;

            RunTestWithMultiplePuzzles(solver, puzzles);
        }

        [Test]
        [Timeout(TimeoutLong)]
        public void Solve_WellKnown_Puzzle_40x40()
        {
            // Arrange
            var solver = CreateSolver();
            var puzzles = WellKnownPuzzles.Puzzles_40x40;

            RunTestWithMultiplePuzzles(solver, puzzles);
        }

        [Test]
        [Timeout(TimeoutLong)]
        public void Solve_WellKnown_Puzzle_70x70()
        {
            // Arrange
            var solver = CreateSolver();
            var puzzles = WellKnownPuzzles.Puzzles_70x70;

            RunTestWithMultiplePuzzles(solver, puzzles);
        }

        public ISolver CreateSolver()
        {
            return new ParallelSolver();
        }

        private void RunTestWithSinglePuzzle(ISolver solver, string puzzle)
        {
            Console.WriteLine(string.Empty.PadLeft(50, '*'));
            Console.WriteLine("Puzzle:\n{0}", puzzle.Replace(' ', '-'));

            string result;
            using (new TestActionScope())
            {
                result = solver.Solve(puzzle);
            }

            if (result != null)
                Console.WriteLine("Result:\n{0}", result);

            Assert.That(result, Is.Not.Null);
            var table = new Table(result);
            Assert.That(table.IsCorrect());
        }

        private void RunTestWithMultiplePuzzles(ISolver solver, IEnumerable<string> puzzles)
        {
            foreach (var puzzle in puzzles)
            {
                RunTestWithSinglePuzzle(solver, puzzle);
            }
        }
    }
}
