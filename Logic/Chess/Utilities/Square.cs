
using SolveChess.Logic.Chess.Interfaces;
using SolveChess.Logic.Exceptions;

namespace SolveChess.Logic.Chess.Utilities;

public class Square : ISquare
{

    private readonly int _file;
    public int File { get { return _file; } }

    private readonly int _rank;
    public int Rank { get { return _rank; } }

    public string Notation { get { return $"{(char)('a' + _file)}{(char)('8' - _rank)}"; } }

    public Square(string notation)
    {
        if (notation.Length != 2)
            throw new InvalidSquareNotationException();

        char fileChar = char.ToUpper(notation[0]);
        char rankChar = notation[1];

        if(!char.IsLetter(fileChar) || !char.IsDigit(rankChar) || fileChar < 'A' || fileChar > 'H' || rankChar < '1' || rankChar > '8')
            throw new InvalidSquareNotationException();


        _file = fileChar - 'A';
        _rank = '8' - rankChar;
    }

    public Square(int rank, int file)
    {
        if (rank < 0 || rank > 7 || file < 0 || file > 7)
            throw new ArgumentsOutOfBoundsException();

        _rank = rank;
        _file = file;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        Square other = (Square)obj;

        return other.Rank == Rank && other.File == File;
    }

    public override int GetHashCode()
    {
        return (Rank * 10) + File;
    }

}

