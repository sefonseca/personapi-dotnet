namespace personapi_dotnet.Exceptions;

/// <summary>Excepción base de la aplicación; cada tipo define su código HTTP.</summary>
public abstract class AppException : Exception
{
    protected AppException(string message) : base(message) { }

    public abstract int StatusCode { get; }
}

/// <summary>El recurso solicitado no existe (HTTP 404).</summary>
public class NotFoundException : AppException
{
    public NotFoundException(string message) : base(message) { }

    public override int StatusCode => StatusCodes.Status404NotFound;
}

/// <summary>El recurso ya existe o tiene dependencias que impiden la operación (HTTP 409).</summary>
public class ConflictException : AppException
{
    public ConflictException(string message) : base(message) { }

    public override int StatusCode => StatusCodes.Status409Conflict;
}

/// <summary>Se violó una regla de negocio (HTTP 400).</summary>
public class BusinessRuleException : AppException
{
    public BusinessRuleException(string message) : base(message) { }

    public override int StatusCode => StatusCodes.Status400BadRequest;
}
