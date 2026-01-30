using System;

namespace Easy.Tools.Result
{
    /// <summary>
    /// Represents the outcome of an operation that returns a value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value returned.</typeparam>
    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{TValue}"/> class.
        /// </summary>
        /// <param name="value">The value to be returned.</param>
        /// <param name="isSuccess">Indicates whether the operation was successful.</param>
        /// <param name="error">The error associated with the failure.</param>
        protected internal Result(TValue? value, bool isSuccess, Error error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        /// <summary>
        /// Gets the value returned by the operation.
        /// </summary>
        /// <value>The result value if successful.</value>
        /// <exception cref="InvalidOperationException">
        /// Thrown if accessing the value of a failed result.
        /// </exception>
        public TValue Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException($"The value of a failure result cannot be accessed. Error: {Error.Code}");

        /// <summary>
        /// Implicitly converts a value to a success result.
        /// If the value is null, it returns a Failure with <see cref="Error.NullValue"/>.
        /// </summary>
        /// <param name="value">The value to convert into a result.</param>
        /// <returns>A success result containing the value, or a failure result if null.</returns>
        public static implicit operator Result<TValue>(TValue? value)
        {
            return value is not null
                ? Success(value)
                : Failure<TValue>(Error.NullValue);
        }

        /// <summary>
        /// Implicitly converts an <see cref="Error"/> to a failed <see cref="Result{TValue}"/>.
        /// </summary>
        /// <param name="error">The error to convert into a result.</param>
        /// <returns>A failed result containing the specified error.</returns>
        public static implicit operator Result<TValue>(Error error)
        {
            return Failure<TValue>(error);
        }
    }
}