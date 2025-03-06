namespace Infrastructure.Exceptions;

/// <summary>
/// Represents an exception that is thrown when a product is not found.
/// </summary>
public class ProductNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProductNotFoundException"/> class.
    /// </summary>
    public ProductNotFoundException() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductNotFoundException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public ProductNotFoundException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductNotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public ProductNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}
