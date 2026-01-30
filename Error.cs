using System;
using System.Collections.Generic;

namespace Easy.Tools.Result
{
    /// <summary>
    /// Represents a domain error with a specific code and message.
    /// This class is immutable and supports equality comparison.
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
        /// <param name="code">The unique error code.</param>
        /// <param name="message">The detailed error message.</param>
        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        /// <summary>
        /// Implicitly converts an <see cref="Error"/> to a <see cref="string"/> (returns the Code).
        /// </summary>
        /// <param name="error">The error instance to convert.</param>
        public static implicit operator string(Error error) => error?.Code ?? string.Empty;

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
            // Legacy Method
            return (Code.GetHashCode() * 397) ^ Message.GetHashCode();
#else
            // Modern Method
            return HashCode.Combine(Code, Message);
#endif
        }

        /// <inheritdoc />
        public override string ToString() => Code;

        /// <summary>
        /// Determines whether two specified <see cref="Error"/> objects have the same value.
        /// </summary>
        /// <param name="a">The first error to compare.</param>
        /// <param name="b">The second error to compare.</param>
        /// <returns><c>true</c> if the value of a is the same as the value of b; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Error? a, Error? b)
        {
            if (a is null && b is null) return true;
            if (a is null || b is null) return false;
            return a.Equals(b);
        }

        /// <summary>
        /// Determines whether two specified <see cref="Error"/> objects have different values.
        /// </summary>
        /// <param name="a">The first error to compare.</param>
        /// <param name="b">The second error to compare.</param>
        /// <returns><c>true</c> if the value of a is different from the value of b; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Error? a, Error? b) => !(a == b);
    }
}