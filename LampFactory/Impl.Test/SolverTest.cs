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
    public class SolverTest
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

        [Test]
        public void Solve_Small_Table()
        {
            // Arrange
            var puzzle = PuzzleSmall;
            var solver = new Solver();

            // Act
            RunTestWithSinglePuzzle(solver, puzzle);
        }

        [Test]
        public void Solve_Mid_Table()
        {
            // Arrange
            var puzzle = PuzzleMiddle;
            var solver = new Solver();

            // Act
            RunTestWithSinglePuzzle(solver, puzzle);
        }

        [Test]
        public void Solve_Large_Table()
        {
            // Arrange
            var puzzle = PuzzleLarge;
            var solver = new Solver();

            // Act
            RunTestWithSinglePuzzle(solver, puzzle);
        }

        [Test]
        public void Solve_Small_Table_WithReplacedPuzzles()
        {
            // Arrange
            var puzzle = PuzzleSmall;
            var solver = new Solver();
            var random = new RNGRandomProvider();
            var testDataFactory = new TestDataFactory(solver, random);
            var puzzles = testDataFactory.TransformByReplace(puzzle);

            // Act
            RunTestWithMultiplePuzzles(solver, puzzles);
        }

        [Test]
        public void Solve_Mid_Table_WithReplacedPuzzles()
        {
            // Arrange
            var puzzle = PuzzleMiddle;
            var solver = new Solver();
            var random = new RNGRandomProvider();
            var testDataFactory = new TestDataFactory(solver, random);

            var puzzles = testDataFactory.TransformByReplace(puzzle);

            // Act
            RunTestWithMultiplePuzzles(solver, puzzles);
        }

        [Test]
        public void Solve_Large_Table_WithReplacedPuzzles()
        {
            // Arrange
            var puzzle = PuzzleLarge;
            var solver = new Solver();
            var random = new RNGRandomProvider();
            var testDataFactory = new TestDataFactory(solver, random);

            var puzzles = testDataFactory.TransformByReplace(puzzle);

            // Act
            RunTestWithMultiplePuzzles(solver, puzzles);
        }

        [Test]
        public void Solve_Small_Table_WithRotatedPuzzles()
        {
            // Arrange
            var puzzle = PuzzleSmall;
            var solver = new Solver();
            var random = new RNGRandomProvider();
            var testDataFactory = new TestDataFactory(solver, random);

            var puzzles = testDataFactory.TransformByRotation(puzzle);

            // Act
            RunTestWithMultiplePuzzles(solver, puzzles);
        }

        [Test]
        public void Solve_Mid_Table_WithRotatedPuzzles()
        {
            // Arrange
            var puzzle = PuzzleMiddle;
            var solver = new Solver();
            var random = new RNGRandomProvider();
            var testDataFactory = new TestDataFactory(solver, random);

            var puzzles = testDataFactory.TransformByRotation(puzzle);

            // Act
            RunTestWithMultiplePuzzles(solver, puzzles);
        }

        [Test]
        public void Solve_Large_Table_WithRotatedPuzzles()
        {
            // Arrange
            var puzzle = PuzzleLarge;
            var solver = new Solver();
            var random = new RNGRandomProvider();
            var testDataFactory = new TestDataFactory(solver, random);

            var puzzles = testDataFactory.TransformByRotation(puzzle);

            // Act
            RunTestWithMultiplePuzzles(solver, puzzles);
        }

        [Test]

        public void Solve_Random_Table_Size_20()
        {
            // Arrange
            const int size = 20;
            var solver = new Solver();
            var random = new RNGRandomProvider();
            var testDataFactory = new TestDataFactory(solver, random);

            var puzzle = testDataFactory.GenerateRandomPuzzle(size);

            RunTestWithSinglePuzzle(solver,puzzle);
        }

        [Test]

        public void Solve_Random_Table_Size_40()
        {
            // Arrange
            const int size = 40;
            var solver = new Solver();
            var random = new RNGRandomProvider();
            var testDataFactory = new TestDataFactory(solver, random);

            var puzzle = testDataFactory.GenerateRandomPuzzle(size);

            RunTestWithSinglePuzzle(solver, puzzle);
        }

        [Test]

        public void Solve_Random_Table_Size_60()
        {
            // Arrange
            const int size = 60;
            var solver = new Solver();
            var random = new RNGRandomProvider();
            var testDataFactory = new TestDataFactory(solver, random);

            var puzzle = testDataFactory.GenerateRandomPuzzle(size);

            RunTestWithSinglePuzzle(solver, puzzle);
        }

        [Test]

        public void Solve_Random_Table_Size_80()
        {
            // Arrange
            const int size = 80;
            var solver = new Solver();
            var random = new RNGRandomProvider();
            var testDataFactory = new TestDataFactory(solver, random);

            var puzzle = testDataFactory.GenerateRandomPuzzle(size);

            RunTestWithSinglePuzzle(solver, puzzle);
        }

        [Test]

        public void Solve_Random_Table_Size_100()
        {
            // Arrange
            const int size = 100;
            var solver = new Solver();
            var random = new RNGRandomProvider();
            var testDataFactory = new TestDataFactory(solver, random);

            var puzzle = testDataFactory.GenerateRandomPuzzle(size);

            RunTestWithSinglePuzzle(solver, puzzle);
        }


        private void RunTestWithSinglePuzzle(ISolver solver, string puzzle)
        {
            string result;
            using (new TestActionScope())
            {
                result = solver.Solve(puzzle);
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
