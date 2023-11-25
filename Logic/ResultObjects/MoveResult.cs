
using SolveChess.Logic.Attributes;
using SolveChess.Logic.Chess.Utilities;

namespace SolveChess.Logic.ResultObjects;

public class MoveResult
{
	
	public StatusCode Status { get; set; }
	public string Message { get; set; } = "";

	public Move? Move { get; set; }

	public Exception? Exception { get; set; }

	public bool Succeeded { get { return Status == StatusCode.SUCCESS; } }

	public MoveResult(StatusCode status)
	{
		Status = status;
	}

	public MoveResult(StatusCode status, Move move, string message = "") : this(status, message)
	{
		Move = move;
	}

	public MoveResult(StatusCode status, string message) : this(status)
	{
		Message = message;
	}

	public MoveResult(StatusCode status, Exception exception) : this(status)
	{
		Exception = exception;
	}

}


