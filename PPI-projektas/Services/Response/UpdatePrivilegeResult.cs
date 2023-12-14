namespace PPI_projektas.Services.Response;

public class UpdatePrivilegeResult
{
      public Result Result { get; set; }
      
      public string? Message { get; set; }

      public UpdatePrivilegeResult(string? message = null)
      {
            Result = message == null ? Result.Success : Result.Failure;
            Message = message;
      }
}

public enum Result
{
      Success,
      Failure
}