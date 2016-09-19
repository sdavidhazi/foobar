using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Impl.Model;
using NUnit.Framework;
using Shouldly;

namespace Impl.Test
{
    [TestFixture]
    public class SolverTest
    {
        [Test]
        public void Solve_Small_Table()
        {
            string puzzle = @"  0    
 #   2 
      3
   0   
#      
 1   # 
    2  ";
            // Arrange
            var solver = new Solver();

            var result = solver.Solve(puzzle);
            Assert.That(result, Is.Not.Null);
            result.Replace('x', ' ');
            var table = new Table(result);
            //TODO
            //Assert.That(table.IsCorrect);
        }

        [Test]
        public void Solve_Mid_Table()
        {
            string puzzle = @"   #      1   
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
            // Arrange
            var solver = new Solver();

            var result = solver.Solve(puzzle);
            Assert.That(result, Is.Not.Null);
            result.Replace('x', ' ');
            var table = new Table(result);
            //TODO
            //Assert.That(table.IsCorrect);
        }

        [Test]
        public void Solve_Large_Table()
        {
            string puzzle = @"# #       1 #     ##  2 #
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
            // Arrange
            var solver = new Solver();

            var result = solver.Solve(puzzle);
            Assert.That(result, Is.Not.Null);
            result.Replace('x', ' ');
            var table = new Table(result);
            //TODO
            //Assert.That(table.IsCorrect);
        }

    }
}
