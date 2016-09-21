using System;
using System.Collections.Generic;
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

        #endregion

        private const int TimeoutShort = 5000;
        private const int TimeoutLong = 60000;

        [Test]
        [Timeout(TimeoutShort)]
        public void Solve_Small_Table()
        {
            // Arrange
            var puzzle = PuzzleSmall;
            var solver = new Solver();

            // Act
            RunTestWithSinglePuzzle(solver, puzzle);
        }

        [Test]
        [Timeout(TimeoutShort)]
        public void Solve_Mid_Table()
        {
            // Arrange
            var puzzle = PuzzleMiddle;
            var solver = new Solver();

            // Act
            RunTestWithSinglePuzzle(solver, puzzle);
        }

        [Test]
        [Timeout(TimeoutShort)]
        public void Solve_Large_Table()
        {
            // Arrange
            var puzzle = PuzzleLarge;
            var solver = new Solver();

            // Act
            RunTestWithSinglePuzzle(solver, puzzle);
        }

        [Test]
        [Timeout(TimeoutLong)]
        public void Solve_Small_Replaced_Puzzles()
        {
            // Arrange
            var puzzle = PuzzleSmall;
            var solver = new Solver();
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
            var solver = new Solver();
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
            var solver = new Solver();
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
            var solver = new Solver();
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
            var solver = new Solver();
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
            var solver = new Solver();
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
            var solver = new Solver();
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
            var solver = new Solver();
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
            var solver = new Solver();
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
            var solver = new Solver();
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
            var solver = new Solver();
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
            var solver = new Solver();
            var puzzles = WellKnownPuzzles.Puzzles_14x14;

            RunTestWithMultiplePuzzles(solver, puzzles);
        }

        [Test]
        [Timeout(TimeoutLong)]
        public void Solve_WellKnown_Puzzle_20x20()
        {
            // Arrange
            var solver = new Solver();
            var puzzles = WellKnownPuzzles.Puzzles_20x20;

            RunTestWithMultiplePuzzles(solver, puzzles);
        }

        [Test]
        [Timeout(TimeoutLong)]
        public void Solve_WellKnown_Puzzle_25x25()
        {
            // Arrange
            var solver = new Solver();
            var puzzles = WellKnownPuzzles.Puzzles_25x25;

            RunTestWithMultiplePuzzles(solver, puzzles);
        }

        [Test]
        [Timeout(TimeoutLong)]
        public void Solve_WellKnown_Puzzle_30x30()
        {
            // Arrange
            var solver = new Solver();
            var puzzles = WellKnownPuzzles.Puzzles_30x30;

            RunTestWithMultiplePuzzles(solver, puzzles);
        }

        [Test]
        [Timeout(TimeoutLong)]
        public void Solve_WellKnown_Puzzle_40x40()
        {
            // Arrange
            var solver = new Solver();
            var puzzles = WellKnownPuzzles.Puzzles_40x40;

            RunTestWithMultiplePuzzles(solver, puzzles);
        }

        [Test]
        [Timeout(TimeoutLong)]
        public void Solve_WellKnown_Puzzle_70x70()
        {
            // Arrange
            var solver = new Solver();
            var puzzles = WellKnownPuzzles.Puzzles_70x70;

            RunTestWithMultiplePuzzles(solver, puzzles);
        }

        private void RunTestWithSinglePuzzle(ISolver solver, string puzzle)
        {
            string result;
            using (new TestActionScope())
            {
                result = solver.Solve(puzzle);
                if (result != null)
                    Console.WriteLine("Result:\n{0}", result.Replace(' ', 'x'));
            }

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
