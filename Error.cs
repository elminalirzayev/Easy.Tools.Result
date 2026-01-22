namespace Easy.Tools.Result
{
    /// <summary>
    /// Represents a domain error with a specific code and message.
    /// This class is immutable.
    /// </summary>
    public sealed class Error : IEquatable<Error>
    {
        /// <summary>
        /// Gets the unique code for the error.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Gets the description of the error.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets the static instance representing no error.
        /// </summary>
        public static readonly Error None = new(string.Empty, string.Empty);

        /// <summary>
        /// Gets the static instance representing a null value error.
        /// </summary>
        public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.");

        /// <summary>
        /// Initializes a new instance of the <see cref="Error"/> class.
        /// </summary>
        /// <param name="code">The error code.</param>
        /// <param name="message">The error message.</param>
        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        /// <summary>
        /// Implicitly converts a result to a string.
        /// </summary>
        public static implicit operator string(Error error) => error.Code;

        /// <inheritdoc />
        public bool Equals(Error? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Code == other.Code && Message == other.Message;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is Error error && Equals(error);

        /// <inheritdoc />
        public override int GetHashCode()
        {
#if NETSTANDARD2_0 || NETFRAMEWORK
            // Legacy Method: For .NET Standard 2.0 and .NET Framework 4.x
            // Since HashCode.Combine struct does not exist in these versions.
            return (Code.GetHashCode() * 397) ^ Message.GetHashCode();
#else
            // Modern Method: For .NET Core 2.1+, .NET 5+, .NET 8.0/9.0
            // HashCode.Combine is faster and more secure against hash collision attacks.
            return HashCode.Combine(Code, Message);
#endif

        }

        /// <inheritdoc />
        public override string ToString() => $"Code: {Code}, Message: {Message}";
    }
}