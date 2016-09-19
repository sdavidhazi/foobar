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
    }
}
