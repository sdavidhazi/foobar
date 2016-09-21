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
        #region Wrong Puzzle Constants
        const string WrongPuzzleSmall = @"4 0    
 #   2 
      3
   0   
#      
 1   # 
    2  ";

        const string WrongPuzzleMiddle = @"   #      1   
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
   #      #  4";

        const string WrongPuzzleLarge = @"4 #       1 #     ##  2 #
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
        public void Solve_Handles_Wrong_Small_Table()
        {
            // Arrange
            var puzzle = WrongPuzzleSmall;
            var solver = new Solver();

            // Act
            var result = solver.Solve(puzzle);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Solve_Handles_Wrong_Mid_Table()
        {
            // Arrange
            var puzzle = WrongPuzzleMiddle;
            var solver = new Solver();

            // Act
            var result = solver.Solve(puzzle);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Solve_Handles_Wrong_Large_Table()
        {
            // Arrange
            var puzzle = WrongPuzzleLarge;
            var solver = new Solver();

            // Act
            var result = solver.Solve(puzzle);
            Assert.That(result, Is.Null);
        }
    }
}
