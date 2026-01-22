namespace Easy.Tools.Result
{
    /// <summary>
    /// Represents the outcome of an operation, indicating success or failure.
    /// This is the base class for operations that do not return a value.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// </summary>
        /// <param name="isSuccess">Indicates whether the operation was successful.</param>
        /// <param name="error">The error associated with the failure, if any.</param>
        /// <exception cref="InvalidOperationException">Thrown when success result has an error or failure result has no error.</exception>
        protected Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None)
            {
                throw new InvalidOperationException("A success result cannot contain an error.");
            }

            if (!isSuccess && error == Error.None)
            {
                throw new InvalidOperationException("A failure result must contain an error.");
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        /// <summary>
        /// Gets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Gets a value indicating whether the operation failed.
        /// </summary>
        public bool IsFailure => !IsSuccess;

        /// <summary>
        /// Gets the error associated with the failure. Returns <see cref="Error.None"/> if successful.
        /// </summary>
        public Error Error { get; }

        /// <summary>
        /// Creates a successful result.
        /// </summary>
        /// <returns>A new instance of <see cref="Result"/> indicating success.</returns>
        public static Result Success() => new(true, Error.None);

        /// <summary>
        /// Creates a failed result with the specified error.
        /// </summary>
        /// <param name="error">The error that caused the failure.</param>
        /// <returns>A new instance of <see cref="Result"/> indicating failure.</returns>
        public static Result Failure(Error error) => new(false, error);

        /// <summary>
        /// Creates a successful result containing a value.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="value">The value to return.</param>
        /// <returns>A new instance of <see cref="Result{TValue}"/>.</returns>
        public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

        /// <summary>
        /// Creates a failed result with a value type.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="error">The error that caused the failure.</param>
        /// <returns>A new instance of <see cref="Result{TValue}"/>.</returns>
        public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
    }
}