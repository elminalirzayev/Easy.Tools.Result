using System;

namespace Easy.Tools.Result
{
    /// <summary>
    /// Extension methods for functional operations on Result objects.
    /// </summary>
    public static class ResultExtensions
    {
        /// <summary>
        /// Matches the success or failure status of the result and executes the corresponding function.
        /// Useful for returning API responses (e.g., Ok() or BadRequest()).
        /// </summary>
        public static TOut Match<TOut>(this Result result, Func<TOut> onSuccess, Func<Error, TOut> onFailure)
        {
            return result.IsSuccess ? onSuccess() : onFailure(result.Error);
        }

        /// <summary>
        /// Matches the success or failure status of the result and executes the corresponding function.
        /// </summary>
        public static TOut Match<TValue, TOut>(this Result<TValue> result, Func<TValue, TOut> onSuccess, Func<Error, TOut> onFailure)
        {
            return result.IsSuccess ? onSuccess(result.Value) : onFailure(result.Error);
        }

        /// <summary>
        /// Maps the value of a successful result to a new value using the mapping function.
        /// If the result is a failure, returns a new failure result with the original error.
        /// </summary>
        public static Result<TNewValue> Map<TValue, TNewValue>(this Result<TValue> result, Func<TValue, TNewValue> mappingFunc)
        {
            return result.IsSuccess
                ? Result.Success(mappingFunc(result.Value))
                : Result.Failure<TNewValue>(result.Error);
        }

        /// <summary>
        /// Executes an action if the result is successful, without changing the result.
        /// Useful for logging or side effects.
        /// </summary>
        public static Result<TValue> Tap<TValue>(this Result<TValue> result, Action<TValue> action)
        {
            if (result.IsSuccess)
            {
                action(result.Value);
            }
            return result;
        }

        /// <summary>
        /// Ensures a condition is met. If the predicate returns false, returns a failure result with the specified error.
        /// </summary>
        public static Result<TValue> Ensure<TValue>(this Result<TValue> result, Func<TValue, bool> predicate, Error error)
        {
            if (result.IsFailure) return result;

            return predicate(result.Value)
                ? result
                : Result.Failure<TValue>(error);
        }
    }
}