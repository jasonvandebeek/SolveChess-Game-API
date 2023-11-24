using System;
using SolveChess.Logic.Attributes;
using SolveChess.Logic.DTO;

namespace SolveChess.Logic.ResultObjects;

public class MoveResult
{
	
	public StatusCode Status { get; set; }
	public string Message { get; set; } = "";

	public MoveDto? MoveDto { get; set; }

	public Exception? Exception { get; set; }

	public bool Succeeded { get { return Status == StatusCode.SUCCESS; } }

	public MoveResult(StatusCode status)
	{
		Status = status;
	}

	public MoveResult(StatusCode status, MoveDto move, string message = "") : this(status, message)
	{
		MoveDto = move;
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


