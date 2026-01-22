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
        /// <value>The value of the result.</value>
        /// <exception cref="InvalidOperationException">Thrown if accessing the value of a failed result.</exception>
        public TValue Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("The value of a failure result can not be accessed.");

        /// <summary>
        /// Implicitly converts a value to a success result.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static implicit operator Result<TValue>(TValue? value)
        {
            // If the user tries to create a result from null, decide if it's an error or just null data.
            // Usually, creating a Success result from null is risky, but permitted in C#.
            // For safer code, we could return a specific error if value is null, but let's keep it standard.
            return value is not null
                ? Success(value)
                : Failure<TValue>(Error.NullValue);
        }
    }
}