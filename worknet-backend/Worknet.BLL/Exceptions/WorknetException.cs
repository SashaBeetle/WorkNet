namespace Worknet.BLL.Exceptions;
/// <summary>
/// Represents errors that occur during Worknet application execution,
/// </summary>
[Serializable] 
public class WorknetException : Exception
{
    /// <summary>
    /// Gets or sets detailed information about the error,
    /// which can include specific values or conditions that led to the exception.
    /// </summary>
    public string Details { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the exception occurred,
    /// useful for logging and debugging purposes.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="WorknetException"/> class.
    /// </summary>
    public WorknetException()
    {
        Timestamp = DateTime.UtcNow;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WorknetException"/> class
    /// with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public WorknetException(string message) : base(message)
    {
        Timestamp = DateTime.UtcNow;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WorknetException"/> class
    /// with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
    public WorknetException(string message, Exception innerException) : base(message, innerException)
    {
        Timestamp = DateTime.UtcNow;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WorknetException"/> class
    /// with specified error message, error code, and details.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="details">Detailed information about the error.</param>
    public WorknetException(string message, string details)
        : base(message)
    {
        Details = details;
        Timestamp = DateTime.UtcNow;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WorknetException"/> class
    /// with specified error message, error code, details, and inner exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="details">Detailed information about the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public WorknetException(string message, string details, Exception innerException)
        : base(message, innerException)
    {
        Details = details;
        Timestamp = DateTime.UtcNow;
    }
}