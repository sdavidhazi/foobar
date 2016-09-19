using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;

namespace Impl.Test
{
    [TestFixture]
    public class SolverTest
    {
        [Test]
        public void Solve_Is_Not_Implemented()
        {
            // Arrange
            var solver = new Solver();

            // Act
            // Assert
            Should.Throw<NotImplementedException>(() => solver.Solve(""));
        }
    }
}
