using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolveChess.Logic.Chess.Utilities;
using SolveChess.Logic.Exceptions;

namespace SolveChess.Logic.Chess.Utilities.Tests;

[TestClass]
public class SquareTests
{

    [TestMethod]
    public void SquareTest_CreateWithNotationD3()
    {
        //Arrange
        var expectedRank = 5;
        var expectedFile = 3;

        //Act
        var square = new Square("D3");

        //Assert
        Assert.AreEqual(expectedRank, square.Rank);
        Assert.AreEqual(expectedFile, square.File);
    }

    [TestMethod]
    public void SquareTest_CreateWithNotationD5()
    {
        //Arrange
        var expectedRank = 3;
        var expectedFile = 3;

        //Act
        var square = new Square("D5");

        //Assert
        Assert.AreEqual(expectedRank, square.Rank);
        Assert.AreEqual(expectedFile, square.File);
    }

    [TestMethod]
    public void SquareTest_GetNotationOfD5()
    {
        //Arrange
        var square = new Square("D5");
        var expected = "d5";

        //Act
        var result = square.Notation;

        //Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void SquareTest_OutOfBoundsRank_ThrowsException()
    {
        //Arrange

        //Assert
        Assert.ThrowsException<ArgumentsOutOfBoundsException>(() =>
        {
            //Act
            var result = new Square(9, 1);
        });
    }

    [TestMethod]
    public void SquareTest_OutOfBoundsFile_ThrowsException()
    {
        //Arrange

        //Assert
        Assert.ThrowsException<ArgumentsOutOfBoundsException>(() =>
        {
            //Act
            var result = new Square(1, -2);
        });
    }

}