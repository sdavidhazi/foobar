using Impl.Model;
using Impl.Test.TestData;
using NUnit.Framework;

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
            string puzzle = PuzzleSmall;

            // Arrange
            string result;
            var solver = new Solver();
            using (new TestActionScope(nameof(Solve_Small_Table)))
            {
                result = solver.Solve(puzzle);
            }

            Assert.That(result, Is.Not.Null);
            var table = new Table(result);
            Assert.That(table.IsCorrect());
        }

        [Test]
        public void Solve_Mid_Table()
        {
            string puzzle = PuzzleMiddle;

            // Arrange
            var solver = new Solver();
            string result;
            using (new TestActionScope(nameof(Solve_Mid_Table)))
            {
                result = solver.Solve(puzzle);
            }

            Assert.That(result, Is.Not.Null);
            var table = new Table(result);

            Assert.That(table.IsCorrect());
        }

        [Test]
        public void Solve_Large_Table()
        {
            string puzzle = PuzzleLarge;

            // Arrange
            var solver = new Solver();
            string result;

            using (new TestActionScope(nameof(Solve_Large_Table)))
            {
                result = solver.Solve(puzzle);
            }

            Assert.That(result, Is.Not.Null);
            var table = new Table(result);

            Assert.That(table.IsCorrect());
        }

        [Test]
        public void Solve_Small_Table_WithReplacedPuzzles()
        {
            var puzzle = PuzzleSmall;

            var solver = new Solver();
            var testDataFactory = new TestDataFactory(solver);

            foreach (var transformedPuzzle in testDataFactory.TransformByReplace(puzzle))
            {
                string result;
                using (new TestActionScope(nameof(Solve_Small_Table_WithReplacedPuzzles)))
                {
                    result = solver.Solve(transformedPuzzle);
                }

                Assert.That(result, Is.Not.Null);
                var table = new Table(result);
                Assert.That(table.IsCorrect());
            }
        }

        [Test]
        public void Solve_Mid_Table_WithReplacedPuzzles()
        {
            var puzzle = PuzzleMiddle;

            var solver = new Solver();
            var testDataFactory = new TestDataFactory(solver);

            foreach (var transformedPuzzle in testDataFactory.TransformByReplace(puzzle))
            {
                string result;
                using (new TestActionScope(nameof(Solve_Mid_Table_WithReplacedPuzzles)))
                {
                    result = solver.Solve(transformedPuzzle);
                }

                Assert.That(result, Is.Not.Null);
                var table = new Table(result);
                Assert.That(table.IsCorrect());
            }
        }

        [Test]
        public void Solve_Small_Table_WithRotatedPuzzles()
        {
            var puzzle = PuzzleSmall;

            var solver = new Solver();
            var testDataFactory = new TestDataFactory(solver);

            foreach (var transformedPuzzle in testDataFactory.TransformByRotation(puzzle))
            {
                string result;
                using (new TestActionScope(nameof(Solve_Small_Table_WithRotatedPuzzles)))
                {
                    result = solver.Solve(transformedPuzzle);
                }

                Assert.That(result, Is.Not.Null);
                var table = new Table(result);
                Assert.That(table.IsCorrect());
            }
        }

        [Test]
        public void Solve_Mid_Table_WithRotatedPuzzles()
        {
            var puzzle = PuzzleMiddle;

            var solver = new Solver();
            var testDataFactory = new TestDataFactory(solver);

            foreach (var transformedPuzzle in testDataFactory.TransformByRotation(puzzle))
            {
                solver = new Solver();
                string result;
                using (new TestActionScope(nameof(Solve_Mid_Table_WithRotatedPuzzles)))
                {
                    result = solver.Solve(transformedPuzzle);
                }

                Assert.That(result, Is.Not.Null);
                var table = new Table(result);
                Assert.That(table.IsCorrect());
            }
        }
    }
}
