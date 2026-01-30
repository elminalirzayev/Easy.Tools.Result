using System;

namespace Easy.Tools.Result
{
    /// <summary>
    /// Represents the outcome of an operation, indicating success or failure.
    /// This is the base class for operations that do not return a value.
    /// </summary>
    public class Result
    {
        // Optimization: Cache the success result to avoid unnecessary allocations for void operations.
        private static readonly Result _success = new(true, Error.None);

        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// </summary>
        /// <param name="isSuccess">Indicates whether the operation was successful.</param>
        /// <param name="error">The error associated with the failure, if any.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when a success result contains an error or a failure result contains no error.
        /// </exception>
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
        /// Returns a cached instance of a successful result (Zero-Allocation).
        /// </summary>
        /// <returns>A successful <see cref="Result"/>.</returns>
        public static Result Success() => _success;

        /// <summary>
        /// Creates a failed result with the specified error.
        /// </summary>
        /// <param name="error">The error that caused the failure.</param>
        /// <returns>A failed <see cref="Result"/>.</returns>
        public static Result Failure(Error error) => new(false, error);

        /// <summary>
        /// Creates a successful result containing a value.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="value">The value to be returned.</param>
        /// <returns>A successful <see cref="Result{TValue}"/> containing the value.</returns>
        public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

        /// <summary>
        /// Creates a failed result for a specific value type.
        /// </summary>
        /// <typeparam name="TValue">The expected type of the value.</typeparam>
        /// <param name="error">The error that caused the failure.</param>
        /// <returns>A failed <see cref="Result{TValue}"/>.</returns>
        public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

        /// <summary>
        /// Deconstructs the Result into a boolean success flag and the Error.
        /// </summary>
        /// <param name="isSuccess">Output parameter indicating success.</param>
        /// <param name="error">Output parameter containing the error.</param>
        public void Deconstruct(out bool isSuccess, out Error error)
        {
            isSuccess = IsSuccess;
            error = Error;
        }
    }
}