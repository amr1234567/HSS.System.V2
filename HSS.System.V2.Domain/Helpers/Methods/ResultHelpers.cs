using FluentResults;

namespace HSS.System.V2.Domain.Helpers.Methods;

/// <summary>
/// Provides a set of helper extension methods for working with <see cref="Result"/> and <see cref="Result{T}"/> objects.
/// Includes asynchronous and synchronous methods for mapping results, ensuring conditions, and executing actions on success or failure.
/// </summary>
public static class ResultHelpers
{
    /********************************************
     *               Async Methods             *
     ********************************************/

    /// <summary>
    /// Maps the value from a <see cref="Result{TIn}"/> to a new value of type <typeparamref name="Tout"/> using the specified converter function.
    /// </summary>
    /// <typeparam name="TIn">The input type contained in the result.</typeparam>
    /// <typeparam name="Tout">The output type after conversion.</typeparam>
    /// <param name="task">A task that produces a <see cref="Result{TIn}"/>.</param>
    /// <param name="convertor">A function that converts the <typeparamref name="TIn"/> value to a <typeparamref name="Tout"/> value.</param>
    /// <returns>
    /// A task that produces a <see cref="Result{Tout}"/>. If the original result is a failure, the failure is propagated.
    /// Otherwise, the converted value is wrapped in a successful result.
    /// </returns>
    public static async Task<Result<Tout>> MapAsync<TIn, Tout>(
        this Task<Result<TIn>> task,
        Func<TIn, Tout> convertor)
    {
        var result = await task;
        if (result.IsFailed)
            return Result.Fail<Tout>(result.Errors);
        var convertorResult = convertor(result.Value);
        return Result.Ok(convertorResult);
    }

    public static async Task<Result> DiscardValueAsync<TIn>(
        this Task<Result<TIn>> task)
    {
        var result = await task;
        if (result.IsFailed)
            return Result.Fail(result.Errors);
        return Result.Ok();
    }

    /// <summary>
    /// Ensures that the given predicate on the result value is false. If the predicate returns true, a failure result is returned using the specified error factory.
    /// </summary>
    /// <typeparam name="T">The type contained in the result.</typeparam>
    /// <param name="task">A task that produces a <see cref="Result{T}"/>.</param>
    /// <param name="predicate">A function that evaluates the result value.</param>
    /// <param name="errorFactory">A function that produces a failure result if the predicate is met.</param>
    /// <returns>
    /// A task that produces the original successful result if the predicate is false, otherwise a failure result.
    /// </returns>
    public static async Task<Result<T>> EnsureNoneAsync<T>(
        this Task<Result<T>> task,
        Func<T, bool> predicate,
        Func<T, Result> errorFactory)
    {
        var result = await task;
        if (result.IsFailed)
            return Result.Fail<T>(result.Errors);
        if (predicate(result.Value))
            return Result.Fail<T>(errorFactory(result.Value).Errors);
        return result;
    }

    /// <summary>
    /// Ensures that the given predicate on the result value is false. If the predicate returns true, the specified error is returned.
    /// </summary>
    /// <typeparam name="T">The type contained in the result.</typeparam>
    /// <param name="task">A task that produces a <see cref="Result{T}"/>.</param>
    /// <param name="predicate">A function that evaluates the result value.</param>
    /// <param name="error">The error to return if the predicate is met.</param>
    /// <returns>
    /// A task that produces the original successful result if the predicate is false, otherwise a failure result.
    /// </returns>
    public static async Task<Result<T>> EnsureNoneAsync<T>(
        this Task<Result<T>> task,
        Func<T, bool> predicate,
        Error error)
    {
        var result = await task;
        if (result.IsFailed)
            return Result.Fail<T>(result.Errors);
        if (predicate(result.Value))
            return Result.Fail<T>(error);
        return result;
    }

    /// <summary>
    /// Ensures that the given predicate (which does not depend on the result value) is false.
    /// If the predicate returns true, the specified error is returned.
    /// </summary>
    /// <param name="task">A task that produces a <see cref="Result"/>.</param>
    /// <param name="predicate">A function that returns a boolean value.</param>
    /// <param name="error">The error to return if the predicate is true.</param>
    /// <returns>
    /// A task that produces the original successful result if the predicate is false, otherwise a failure result.
    /// </returns>
    public static async Task<Result> EnsureNoneAsync<T>(
        this Task<Result> task,
        Func<bool> predicate,
        Error error)
    {
        var result = await task;
        if (result.IsFailed)
            return Result.Fail(result.Errors);
        if (predicate())
            return Result.Fail(error);
        return result;
    }

    /// <summary>
    /// Ensures that none of the provided predicates (which do not depend on the result value) return true.
    /// </summary>
    /// <param name="task">A task that produces a <see cref="Result"/>.</param>
    /// <param name="predicates">
    /// An array of tuples where each tuple contains:
    /// <list type="bullet">
    /// <item><description>A condition function returning a boolean.</description></item>
    /// <item><description>An error to return if the condition is true.</description></item>
    /// </list>
    /// </param>
    /// <returns>
    /// A task that produces the original successful result if none of the conditions are met, otherwise a failure result.
    /// </returns>
    public static async Task<Result> EnsureNoneAsync(
        this Task<Result> task,
        params (Func<bool> condition, Error error)[] predicates)
    {
        var result = await task;
        if (result.IsFailed)
            return Result.Fail(result.Errors);
        foreach (var (condition, error) in predicates)
        {
            if (condition())
                return Result.Fail(error);
        }
        return result;
    }

    /// <summary>
    /// Evaluates a set of conditions on the result value, returning a failure if any condition is met.
    /// Unlike <see cref="EnsureSuccess{T}(Task{Result{T}}, (Func{T, bool}, Error)[])"/>, this method returns a successful result with no value if all conditions pass.
    /// </summary>
    /// <typeparam name="T">The type contained in the result.</typeparam>
    /// <param name="task">A task that produces a <see cref="Result{T}"/>.</param>
    /// <param name="predicates">
    /// An array of tuples where each tuple contains:
    /// <list type="bullet">
    /// <item><description>A condition function to evaluate the result value.</description></item>
    /// <item><description>An error to return if the condition is met.</description></item>
    /// </list>
    /// </param>
    /// <returns>
    /// A task that produces a <see cref="Result{T}"/> that is successful if all conditions pass, otherwise a failure result.
    /// </returns>
    public static async Task<Result<T>> EnsureNoneAsync<T>(
        this Task<Result<T>> task,
        params (Func<T, bool> condition, Error error)[] predicates)
    {
        var result = await task;
        if (result.IsFailed)
            return Result.Fail(result.Errors);
        var errors = new List<IError>();
        foreach (var (condition, error) in predicates)
        {
            if (condition(result.Value))
                errors.Add(error);
        }
        return errors.Count != 0 ? Result.Fail<T>(errors) : result;
    }

    /// <summary>
    /// Executes the specified error handler actions if the asynchronous result is a failure.
    /// </summary>
    /// <typeparam name="T">The type contained in the result.</typeparam>
    /// <param name="task">A task that produces a <see cref="Result{T}"/>.</param>
    /// <param name="predicates">
    /// An array of error handler actions to be executed if the result is a failure.
    /// Each action receives the list of errors.
    /// </param>
    /// <returns>
    /// A task that produces a failure result if the original result was a failure; otherwise, a successful result.
    /// </returns>
    public static async Task<Result<T>> OnFailureAsync<T>(
        this Task<Result<T>> task,
        params Action<List<IError>>[] predicates)
    {
        var result = await task;
        if (result.IsFailed)
        {
            foreach (var predicate in predicates)
            {
                predicate(result.Errors);
            }
            return Result.Fail<T>(result.Errors);
        }
        return Result.Ok();
    }

    /// <summary>
    /// Executes the specified error handler actions if the asynchronous result is a failure.
    /// </summary>
    /// <param name="task">A task that produces a <see cref="Result"/>.</param>
    /// <param name="errorHandlers">
    /// An array of error handler actions to be executed if the result is a failure.
    /// Each action receives the list of errors.
    /// </param>
    /// <returns>
    /// A task that produces a failure result if the original result was a failure; otherwise, the original successful result.
    /// </returns>
    public static async Task<Result> OnFailureAsync(
        this Task<Result> task,
        params Action<List<IError>>[] errorHandlers)
    {
        var result = await task;
        if (result.IsFailed)
        {
            foreach (var handler in errorHandlers)
            {
                handler(result.Errors);
            }
            return Result.Fail(result.Errors);
        }
        return result;
    }

    /// <summary>
    /// Executes the specified actions if the asynchronous result is successful.
    /// </summary>
    /// <param name="task">A task that produces a <see cref="Result"/>.</param>
    /// <param name="predicates">An array of actions to execute on success.</param>
    /// <returns>
    /// A task that produces the original successful result if all actions are executed; otherwise, a failure result.
    /// </returns>
    public static async Task<Result> OnSuccessAsync(
        this Task<Result> task,
        params Action[] predicates)
    {
        var result = await task;
        if (result.IsSuccess)
        {
            foreach (var predicate in predicates)
            {
                predicate();
            }
            return Result.Ok();
        }
        return Result.Fail(result.Errors);
    }

    /// <summary>
    /// If the asynchronous operation (returning a <see cref="Result"/>) is successful, 
    /// executes the provided asynchronous functions (side effects) and then returns the original result.
    /// If the result is a failure, no functions are executed and the failure is propagated.
    /// </summary>
    /// <param name="task">A task returning a <see cref="Result"/>.</param>
    /// <param name="taskPredicates">
    /// An array of asynchronous functions to execute if the result is successful.
    /// Each function takes no parameters and returns a <see cref="Task"/>.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> that produces the original <see cref="Result"/> if successful; otherwise, a failure result with its errors.
    /// </returns>
    public static async Task<Result> OnSuccessAsync(
        this Task<Result> task,
        params Func<Task>[] taskPredicates)
    {
        var result = await task.ConfigureAwait(false);
        if (result.IsSuccess)
        {
            foreach (var predicate in taskPredicates)
            {
                await predicate().ConfigureAwait(false);
            }
            return result;
        }
        return Result.Fail(result.Errors);
    }

    /// <summary>
    /// If the asynchronous operation (returning a <see cref="Result{T}"/>) is successful, 
    /// executes the provided asynchronous functions (side effects) that receive the successful value,
    /// then returns the original result.
    /// If the result is a failure, no functions are executed and the failure is propagated.
    /// </summary>
    /// <typeparam name="T">The type of the successful value contained in the <see cref="Result{T}"/>.</typeparam>
    /// <param name="task">A task returning a <see cref="Result{T}"/>.</param>
    /// <param name="taskPredicates">
    /// An array of asynchronous functions to execute if the result is successful.
    /// Each function receives the successful value and returns a <see cref="Task"/>.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> that produces the original <see cref="Result{T}"/> if successful; otherwise, a failure result with its errors.
    /// </returns>
    public static async Task<Result<T>> OnSuccessAsync<T>(
        this Task<Result<T>> task,
        params Func<T, Task>[] taskPredicates)
    {
        var result = await task.ConfigureAwait(false);
        if (result.IsSuccess)
        {
            foreach (var predicate in taskPredicates)
            {
                await predicate(result.Value).ConfigureAwait(false);
            }
            return result;
        }
        return Result.Fail(result.Errors);
    }

    /// <summary>
    /// Executes the specified actions with the result value if the asynchronous result is successful.
    /// </summary>
    /// <typeparam name="T">The type contained in the result.</typeparam>
    /// <param name="task">A task that produces a <see cref="Result{T}"/>.</param>
    /// <param name="predicates">An array of actions that take the result value and execute on success.</param>
    /// <returns>
    /// A task that produces a successful result containing the original value if all actions execute; otherwise, a failure result.
    /// </returns>
    public static async Task<Result<T>> OnSuccessAsync<T>(
        this Task<Result<T>> task,
        params Action<T>[] predicates)
    {
        var result = await task;
        if (result.IsSuccess)
        {
            foreach (var predicate in predicates)
            {
                predicate(result.Value);
            }
            return result;
        }
        return Result.Fail<T>(result.Errors);
    }


    /// <summary>
    /// Provides a fallback mechanism for asynchronous operations that produce a <see cref="Result{T}"/>.
    /// If the first task fails, the alternate task is executed.
    /// If both fail, the errors from both tasks are combined.
    /// </summary>
    /// <typeparam name="T">The type contained in the result.</typeparam>
    /// <param name="task">The primary asynchronous operation returning a <see cref="Result{T}"/>.</param>
    /// <param name="anotherTask">The alternate asynchronous operation to execute if the primary operation fails.</param>
    /// <returns>
    /// A task that produces the successful result from the primary operation, or, if that fails, from the alternate operation.
    /// If both operations fail, a failure result containing the union of errors is returned.
    /// </returns>
    public static async Task<Result<T>> FallbackAsync<T>(
         this Task<Result<T>> task, Task<Result<T>> anotherTask)
    {
        var result = await task.ConfigureAwait(false);
        if (result.IsFailed)
        {
            var anotherTaskResult = await anotherTask.ConfigureAwait(false);
            if (anotherTaskResult.IsFailed)
            {
                return Result.Fail<T>(result.Errors.Union(anotherTaskResult.Errors));
            }
            return anotherTaskResult;
        }
        return result;
    }

    /// <summary>
    /// Provides a fallback mechanism for asynchronous operations that produce a <see cref="Result{T}"/>.
    /// If the first task fails, the alternate task is executed.
    /// If both fail, the errors from both tasks are combined.
    /// </summary>
    /// <typeparam name="T">The type contained in the result.</typeparam>
    /// <param name="task">The primary asynchronous operation returning a <see cref="Result{T}"/>.</param>
    /// <param name="anotherTask">The alternate asynchronous operation to execute if the primary operation fails.</param>
    /// <returns>
    /// A task that produces the successful result from the primary operation, or, if that fails, from the alternate operation.
    /// If both operations fail, a failure result containing the union of errors is returned.
    /// </returns>
    public static async Task<Result> FallbackAsync<T>(
         this Task<Result<T>> task, Task<Result> anotherTask)
    {
        var result = await task.ConfigureAwait(false);
        if (result.IsFailed)
        {
            var anotherTaskResult = await anotherTask.ConfigureAwait(false);
            if (anotherTaskResult.IsFailed)
            {
                return Result.Fail(result.Errors.Union(anotherTaskResult.Errors));
            }
            return anotherTaskResult;
        }
        return Result.Ok();
    }

    /// <summary>
    /// Switches the operation to an alternate asynchronous task.
    /// The final result is that of the alternate task.
    /// Any errors from the first task are appended to the errors of the alternate task if it fails.
    /// </summary>
    /// <typeparam name="TIn">The type contained in the first result.</typeparam>
    /// <typeparam name="TResult">The type contained in the alternate result.</typeparam>
    /// <param name="task">The primary asynchronous operation returning a <see cref="Result{TIn}"/>.</param>
    /// <param name="anotherTask">The alternate asynchronous operation returning a <see cref="Result{TResult}"/>.</param>
    /// <returns>
    /// A task that produces the result from the alternate operation.
    /// If the first task fails, its errors are added to the alternate task's errors (if it fails).
    /// </returns>
    public static async Task<Result<TResult>> SwitchMap<TIn, TResult>(
         this Task<Result<TIn>> task, Task<Result<TResult>> anotherTask)
    {
        var result = await task.ConfigureAwait(false);
        var errors = new List<IError>();
        if (result.IsFailed)
        {
            errors.AddRange(result.Errors);
        }
        var anotherTaskResult = await anotherTask.ConfigureAwait(false);
        if (anotherTaskResult.IsFailed)
        {
            anotherTaskResult.Errors.AddRange(errors);
        }
        return anotherTaskResult;
    }

    /// <summary>
    /// Merges the results of two asynchronous operations and then executes a mapping function.
    /// Both tasks must succeed for the mapping function to be executed.
    /// </summary>
    /// <typeparam name="TInFirst">The type of the value in the first result.</typeparam>
    /// <typeparam name="TInSecond">The type of the value in the second result.</typeparam>
    /// <param name="firstTask">The first asynchronous operation returning a <see cref="Result{TInFirst}"/>.</param>
    /// <param name="secondTask">The second asynchronous operation returning a <see cref="Result{TInSecond}"/>.</param>
    /// <param name="mapper">
    /// A function that takes the successful values from both tasks and returns an asynchronous <see cref="Result"/>.
    /// </param>
    /// <returns>
    /// A task that produces a <see cref="Result"/> from the mapping function if both tasks are successful;
    /// otherwise, a failure result containing the errors from the failed task.
    /// </returns>
    public static async Task<Result> MergeThenAsync<TInFirst, TInSecond>(
        this Task<Result<TInFirst>> firstTask, Task<Result<TInSecond>> secondTask, Func<TInFirst, TInSecond, Task<Result>> mapper)
    {
        var firstResult = await firstTask.ConfigureAwait(false);
        if (firstResult.IsFailed)
            return Result.Fail(firstResult.Errors);
        var secondResult = await secondTask.ConfigureAwait(false);
        if (secondResult.IsFailed)
            return Result.Fail(secondResult.Errors);

        return await mapper(firstResult.Value, secondResult.Value).ConfigureAwait(false);
    }

    /// <summary>
    /// Merges the results of two asynchronous operations and then executes a mapping function.
    /// Both tasks must succeed for the mapping function to be executed.
    /// </summary>
    /// <typeparam name="TInFirst">The type of the value in the first result.</typeparam>
    /// <typeparam name="TInSecond">The type of the value in the second result.</typeparam>
    /// <param name="firstTask">The first asynchronous operation returning a <see cref="Result{TInFirst}"/>.</param>
    /// <param name="secondTask">The second asynchronous operation returning a <see cref="Result{TInSecond}"/>.</param>
    /// <param name="mapper">
    /// A function that takes the successful values from both tasks and returns an asynchronous <see cref="Result"/>.
    /// </param>
    /// <returns>
    /// A task that produces a <see cref="Result"/> from the mapping function if both tasks are successful;
    /// otherwise, a failure result containing the errors from the failed task.
    /// </returns>
    public static async Task<Result<TOut>> MergeThenAsync<TInFirst, TInSecond, TOut>(
        this Task<Result<TInFirst>> firstTask, Task<Result<TInSecond>> secondTask, Func<TInFirst, TInSecond, Task<Result<TOut>>> mapper)
    {
        var firstResult = await firstTask.ConfigureAwait(false);
        if (firstResult.IsFailed)
            return Result.Fail(firstResult.Errors);
        var secondResult = await secondTask.ConfigureAwait(false);
        if (secondResult.IsFailed)
            return Result.Fail(secondResult.Errors);

        return await mapper(firstResult.Value, secondResult.Value).ConfigureAwait(false);
    }

    /// <summary>
    /// Merges the results of two asynchronous operations and then executes a mapping function.
    /// Both tasks must succeed for the mapping function to be executed.
    /// </summary>
    /// <typeparam name="TInFirst">The type of the value in the first result.</typeparam>
    /// <typeparam name="TInSecond">The type of the value in the second result.</typeparam>
    /// <param name="firstTask">The first asynchronous operation returning a <see cref="Result{TInFirst}"/>.</param>
    /// <param name="secondTask">The second asynchronous operation returning a <see cref="Result{TInSecond}"/>.</param>
    /// <param name="mapper">
    /// A function that takes the successful values from both tasks and returns an asynchronous <see cref="Result"/>.
    /// </param>
    /// <returns>
    /// A task that produces a <see cref="Result"/> from the mapping function if both tasks are successful;
    /// otherwise, a failure result containing the errors from the failed task.
    /// </returns>
    public static async Task<Result<TOut>> MergeThenAsync<TInFirst, TInSecond, TOut>(
        this Task<Result<TInFirst>> firstTask, Task<Result<TInSecond>> secondTask, Func<TInFirst, TInSecond, Result<TOut>> mapper)
    {
        var firstResult = await firstTask.ConfigureAwait(false);
        if (firstResult.IsFailed)
            return Result.Fail(firstResult.Errors);
        var secondResult = await secondTask.ConfigureAwait(false);
        if (secondResult.IsFailed)
            return Result.Fail(secondResult.Errors);

        return mapper(firstResult.Value, secondResult.Value);
    }

    /// <summary>
    /// Merges the results of two asynchronous operations and then executes a mapping function.
    /// Both tasks must succeed for the mapping function to be executed.
    /// </summary>
    /// <typeparam name="TInFirst">The type of the value in the first result.</typeparam>
    /// <typeparam name="TInSecond">The type of the value in the second result.</typeparam>
    /// <param name="firstTask">The first asynchronous operation returning a <see cref="Result{TInFirst}"/>.</param>
    /// <param name="secondTask">The second asynchronous operation returning a <see cref="Result{TInSecond}"/>.</param>
    /// <param name="mapper">
    /// A function that takes the successful values from both tasks and returns an asynchronous <see cref="Result"/>.
    /// </param>
    /// <returns>
    /// A task that produces a <see cref="Result"/> from the mapping function if both tasks are successful;
    /// otherwise, a failure result containing the errors from the failed task.
    /// </returns>
    public static async Task<Result> MergeThenAsync<TInFirst, TInSecond>(
        this Task<Result<TInFirst>> firstTask, Task<Result<TInSecond>> secondTask, Func<TInFirst, TInSecond, Result> mapper)
    {
        var firstResult = await firstTask.ConfigureAwait(false);
        if (firstResult.IsFailed)
            return Result.Fail(firstResult.Errors);
        var secondResult = await secondTask.ConfigureAwait(false);
        if (secondResult.IsFailed)
            return Result.Fail(secondResult.Errors);

        return mapper(firstResult.Value, secondResult.Value);
    }

    /// <summary>
    /// Merges the results of two asynchronous operations and then executes a mapping function to produce a new value.
    /// Both tasks must succeed for the mapping function to be executed.
    /// </summary>
    /// <typeparam name="TInFirst">The type of the value in the first result.</typeparam>
    /// <typeparam name="TInSecond">The type of the value in the second result.</typeparam>
    /// <typeparam name="TOut">The type of the value produced by the mapper function.</typeparam>
    /// <param name="firstTask">The first asynchronous operation returning a <see cref="Result{TInFirst}"/>.</param>
    /// <param name="secondTask">The second asynchronous operation returning a <see cref="Result{TInSecond}"/>.</param>
    /// <param name="mapper">
    /// A function that takes the successful values from both tasks and returns an asynchronous <see cref="Result{TOut}"/>.
    /// </param>
    /// <returns>
    /// A task that produces a <see cref="Result{TOut}"/> from the mapping function if both tasks are successful;
    /// otherwise, a failure result containing the errors from the failed task.
    /// </returns>
    public static async Task<Result<TOut>> MergeThenAsync<TInFirst, TInSecond, TOut>(
       this Task<Result<TInFirst>> firstTask, Task<Result<TInSecond>> secondTask, Func<TInFirst, TInSecond, Task<TOut>> mapper)
    {
        var firstResult = await firstTask.ConfigureAwait(false);
        if (firstResult.IsFailed)
            return Result.Fail<TOut>(firstResult.Errors);
        var secondResult = await secondTask.ConfigureAwait(false);
        if (secondResult.IsFailed)
            return Result.Fail<TOut>(secondResult.Errors);

        var resultOut = await mapper(firstResult.Value, secondResult.Value).ConfigureAwait(false);
        return Result.Ok(resultOut);
    }

    /// <summary>
    /// Merges the results of two asynchronous operations and then executes a mapping function to produce a new value.
    /// Both tasks must succeed for the mapping function to be executed.
    /// </summary>
    /// <typeparam name="TInFirst">The type of the value in the first result.</typeparam>
    /// <typeparam name="TInSecond">The type of the value in the second result.</typeparam>
    /// <typeparam name="TOut">The type of the value produced by the mapper function.</typeparam>
    /// <param name="firstTask">The first asynchronous operation returning a <see cref="Result{TInFirst}"/>.</param>
    /// <param name="secondTask">The second asynchronous operation returning a <see cref="Result{TInSecond}"/>.</param>
    /// <param name="mapper">
    /// A function that takes the successful values from both tasks and returns an asynchronous <see cref="Result{TOut}"/>.
    /// </param>
    /// <returns>
    /// A task that produces a <see cref="Result{TOut}"/> from the mapping function if both tasks are successful;
    /// otherwise, a failure result containing the errors from the failed task.
    /// </returns>
    public static async Task<Result<TOut>> MergeThenAsync<TInFirst, TInSecond, TOut>(
       this Task<Result<TInFirst>> firstTask, Task<Result<TInSecond>> secondTask, Func<TInFirst, TInSecond, TOut> mapper)
    {
        var firstResult = await firstTask;
        if (firstResult.IsFailed)
            return Result.Fail<TOut>(firstResult.Errors);
        var secondResult = await secondTask;
        if (secondResult.IsFailed)
            return Result.Fail<TOut>(secondResult.Errors);

        var resultOut = mapper(firstResult.Value, secondResult.Value);
        return Result.Ok(resultOut);
    }

    /// <summary>
    /// Merges the results of two asynchronous operations and then executes a mapping function to produce a new value.
    /// Both tasks must succeed for the mapping function to be executed.
    /// </summary>
    /// <typeparam name="TInFirst">The type of the value in the first result.</typeparam>
    /// <typeparam name="TInSecond">The type of the value in the second result.</typeparam>
    /// <typeparam name="TOut">The type of the value produced by the mapper function.</typeparam>
    /// <param name="firstTask">The first asynchronous operation returning a <see cref="Result{TInFirst}"/>.</param>
    /// <param name="secondTask">The second asynchronous operation returning a <see cref="Result{TInSecond}"/>.</param>
    /// <param name="mapper">
    /// A function that takes the successful values from both tasks and returns an asynchronous <see cref="Result{TOut}"/>.
    /// </param>
    /// <returns>
    /// A task that produces a <see cref="Result{TOut}"/> from the mapping function if both tasks are successful;
    /// otherwise, a failure result containing the errors from the failed task.
    /// </returns>
    public static async Task<Result<TOut>> MergeThenAsync<TInFirst, TInSecond, TOut>(
       this Task<Result<TInFirst>> firstTask, Func<Task<Result<TInSecond>>> secondTask, Func<TInFirst, TInSecond, TOut> mapper)
    {
        var firstResult = await firstTask;
        if (firstResult.IsFailed)
            return Result.Fail<TOut>(firstResult.Errors);
        var secondResult = await secondTask();
        if (secondResult.IsFailed)
            return Result.Fail<TOut>(secondResult.Errors);

        var resultOut = mapper(firstResult.Value, secondResult.Value);
        return Result.Ok(resultOut);
    }

    /// <summary>
    /// If the <paramref name="task"/> <see cref="Result{TFirst}"/> is successful, 
    /// invokes <paramref name="newTask"/> on its value to get a new <see cref="Result{TSecond}"/>.
    /// Otherwise, returns a failed <see cref="Result{TSecond}"/> carrying over the original errors.
    /// </summary>
    /// <typeparam name="TFirst">The type of the successful value in the first result.</typeparam>
    /// <typeparam name="TSecond">The type of the successful value in the new result.</typeparam>
    /// <param name="task">An asynchronous operation returning <see cref="Result{TFirst}"/>.</param>
    /// <param name="newTask">A function that takes the successful value of type <typeparamref name="TFirst"/> 
    /// and returns a new asynchronous <see cref="Result{TSecond}"/>.</param>
    /// <returns>A new <see cref="Result{TSecond}"/>: 
    /// either the success from <paramref name="newTask"/> or the carried-over errors.</returns>
    public static async Task<Result<TSecond>> ThenAsync<TFirst, TSecond>(
        this Task<Result<TFirst>> task,
        Func<TFirst, Task<Result<TSecond>>> newTask)
    {
        var result = await task.ConfigureAwait(false);

        if (result.IsSuccess)
        {
            return await newTask(result.Value).ConfigureAwait(false);
        }

        // If the first task is not successful, carry over its errors to the new result.
        return Result.Fail<TSecond>(result.Errors);
    }

    /// <summary>
    /// If the <paramref name="task"/> <see cref="Result{TFirst}"/> is successful, 
    /// invokes <paramref name="newTask"/> on its value to get a new <see cref="Result{TSecond}"/>.
    /// Otherwise, returns a failed <see cref="Result{TSecond}"/> carrying over the original errors.
    /// </summary>
    /// <typeparam name="TFirst">The type of the successful value in the first result.</typeparam>
    /// <typeparam name="T">The type of the successful value in the new result.</typeparam>
    /// <param name="task">An asynchronous operation returning <see cref="Result{TFirst}"/>.</param>
    /// <param name="newTask">A function that takes the successful value of type <typeparamref name="TFirst"/> 
    /// and returns a new asynchronous <see cref="Result{TSecond}"/>.</param>
    /// <returns>A new <see cref="Result{TSecond}"/>: 
    /// either the success from <paramref name="newTask"/> or the carried-over errors.</returns>
    public static async Task<Result<T>> ThenAsync<T>(
        this Task<Result> task,
        Func<Task<Result<T>>> newTask)
    {
        var result = await task.ConfigureAwait(false);

        if (result.IsSuccess)
        {
            return await newTask().ConfigureAwait(false);
        }

        // If the first task is not successful, carry over its errors to the new result.
        return Result.Fail<T>(result.Errors);
    }

    /// <summary>
    /// If the <paramref name="task"/> <see cref="Result{TFirst}"/> is successful, 
    /// invokes <paramref name="newTask"/> on its value to get a new <see cref="Result{TSecond}"/>.
    /// Otherwise, returns a failed <see cref="Result{TSecond}"/> carrying over the original errors.
    /// </summary>
    /// <typeparam name="TFirst">The type of the successful value in the first result.</typeparam>
    /// <typeparam name="T">The type of the successful value in the new result.</typeparam>
    /// <param name="task">An asynchronous operation returning <see cref="Result{TFirst}"/>.</param>
    /// <param name="newTask">A function that takes the successful value of type <typeparamref name="TFirst"/> 
    /// and returns a new asynchronous <see cref="Result{TSecond}"/>.</param>
    /// <returns>A new <see cref="Result{TSecond}"/>: 
    /// either the success from <paramref name="newTask"/> or the carried-over errors.</returns>
    public static async Task<Result<T>> ThenAsync<T>(
        this Task<Result> task,
        Task<Result<T>> newTask)
    {
        var result = await task.ConfigureAwait(false);

        if (result.IsSuccess)
        {
            var result2 = await newTask.ConfigureAwait(false);
            return result2;
        }

        // If the first task is not successful, carry over its errors to the new result.
        return Result.Fail<T>(result.Errors);
    }

    /// <summary>
    /// If the <paramref name="task"/> <see cref="Result{TFirst}"/> is successful, 
    /// invokes <paramref name="newTask"/> on its value to get a new <see cref="Result{TSecond}"/>.
    /// Otherwise, returns a failed <see cref="Result{TSecond}"/> carrying over the original errors.
    /// </summary>
    /// <typeparam name="TFirst">The type of the successful value in the first result.</typeparam>
    /// <typeparam name="T">The type of the successful value in the new result.</typeparam>
    /// <param name="task">An asynchronous operation returning <see cref="Result{TFirst}"/>.</param>
    /// <param name="newTask">A function that takes the successful value of type <typeparamref name="TFirst"/> 
    /// and returns a new asynchronous <see cref="Result{TSecond}"/>.</param>
    /// <returns>A new <see cref="Result{TSecond}"/>: 
    /// either the success from <paramref name="newTask"/> or the carried-over errors.</returns>
    public static async Task<Result> ThenAsync(
        this Task<Result> task,
        Func<Task<Result>> newTask)
    {
        var result = await task.ConfigureAwait(false);

        if (result.IsSuccess)
        {
            return await newTask().ConfigureAwait(false);
        }

        // If the first task is not successful, carry over its errors to the new result.
        return Result.Fail(result.Errors);
    }


    /// <summary>
    /// If the <paramref name="task"/> <see cref="Result{TFirst}"/> is successful, 
    /// invokes <paramref name="newTask"/> on its value to get a new <see cref="Result{TSecond}"/>.
    /// Otherwise, returns a failed <see cref="Result{TSecond}"/> carrying over the original errors.
    /// </summary>
    /// <typeparam name="TFirst">The type of the successful value in the first result.</typeparam>
    /// <typeparam name="TSecond">The type of the successful value in the new result.</typeparam>
    /// <param name="task">An asynchronous operation returning <see cref="Result{TFirst}"/>.</param>
    /// <param name="newTask">A function that takes the successful value of type <typeparamref name="TFirst"/> 
    /// and returns a new asynchronous <see cref="Result{TSecond}"/>.</param>
    /// <returns>A new <see cref="Result{TSecond}"/>: 
    /// either the success from <paramref name="newTask"/> or the carried-over errors.</returns>
    public static async Task<Result<TSecond>> ThenAsync<TFirst, TSecond>(
        this Result<TFirst> result,
        Func<TFirst, Task<Result<TSecond>>> newTask)
    {
        if (result.IsSuccess)
        {
            return await newTask(result.Value).ConfigureAwait(false);
        }

        // If the first task is not successful, carry over its errors to the new result.
        return Result.Fail<TSecond>(result.Errors);
    }


    /// <summary>
    /// If the asynchronous operation returning a <see cref="Result{TFirst}"/> is successful, 
    /// invokes the specified follow-up asynchronous task and returns its non-generic <see cref="Result"/>.
    /// If the first operation fails, its errors are propagated.
    /// </summary>
    /// <typeparam name="TFirst">The type of the successful value in the first result.</typeparam>
    /// <param name="task">The asynchronous operation returning a <see cref="Result{TFirst}"/>.</param>
    /// <param name="newTask">
    /// A function that takes the successful value from the first result and returns a follow-up asynchronous <see cref="Result"/>.
    /// </param>
    /// <returns>
    /// A task producing a <see cref="Result"/>. If the first operation fails, the returned result is a failure with its errors.
    /// Otherwise, the result from <paramref name="newTask"/> is returned.
    /// </returns>
    public static async Task<Result> ThenAsync<TFirst>(
        this Task<Result<TFirst>> task,
        Func<TFirst, Task<Result>> newTask)
    {
        var firstResult = await task.ConfigureAwait(false);
        if (firstResult.IsFailed)
        {
            // Propagate existing errors
            return Result.Fail(firstResult.Errors);
        }

        // If first step succeeded, run newTask
        return await newTask(firstResult.Value).ConfigureAwait(false);
    }

    /// <summary>
    /// If the synchronous <see cref="Result{TFirst}"/> is successful, 
    /// invokes the specified follow-up asynchronous task (returning a non-generic <see cref="Result"/>)
    /// for its side effects and returns the original result.
    /// If the follow-up task fails, errors from both operations are combined.
    /// </summary>
    /// <typeparam name="TFirst">The type of the successful value in the first result.</typeparam>
    /// <param name="firstResult">The first synchronous <see cref="Result{TFirst}"/>.</param>
    /// <param name="newTask">
    /// A function that takes the successful value from the first result and returns an asynchronous <see cref="Result"/>.
    /// </param>
    /// <returns>
    /// A task producing a <see cref="Result{TFirst}"/>. On success, the original result is preserved.
    /// On failure, errors from both operations are combined.
    /// </returns>
    public static async Task<Result<TFirst>> ThenWithFirstReturnAsync<TFirst>(
        this Result<TFirst> firstResult,
        Func<TFirst, Task<Result>> newTask)
    {
        if (firstResult.IsFailed)
        {
            // Propagate existing errors
            return Result.Fail<TFirst>(firstResult.Errors);
        }
        // If first step succeeded, run newTask
        var secondResult = await newTask(firstResult.Value).ConfigureAwait(false);
        if (secondResult.IsFailed)
        {
            // Combine errors from both operations
            return Result.Fail<TFirst>(firstResult.Errors.Union(secondResult.Errors));
        }
        return firstResult;
    }

    /// <summary>
    /// If the synchronous <see cref="Result{TFirst}"/> is successful, 
    /// invokes the specified follow-up asynchronous task (returning a <see cref="Result{TSecond}"/>)
    /// for its side effects and returns the original result.
    /// If the follow-up task fails, errors from both operations are combined.
    /// </summary>
    /// <typeparam name="TFirst">The type of the successful value in the first result.</typeparam>
    /// <typeparam name="TSecond">The type of the value returned by the follow-up task.</typeparam>
    /// <param name="firstResult">The first synchronous <see cref="Result{TFirst}"/>.</param>
    /// <param name="newTask">
    /// A function that takes the successful value from the first result and returns an asynchronous <see cref="Result{TSecond}"/>.
    /// </param>
    /// <returns>
    /// A task producing a <see cref="Result{TFirst}"/> that preserves the original value on success,
    /// or a failure result with combined errors if the follow-up fails.
    /// </returns>
    public static async Task<Result<TFirst>> ThenWithFirstReturnAsync<TFirst, TSecond>(
        this Result<TFirst> firstResult,
        Func<TFirst, Task<Result<TSecond>>> newTask)
    {
        if (firstResult.IsFailed)
        {
            // Propagate existing errors
            return Result.Fail<TFirst>(firstResult.Errors);
        }
        // If first step succeeded, run newTask
        var secondResult = await newTask(firstResult.Value).ConfigureAwait(false);
        if (secondResult.IsFailed)
        {
            // Combine errors from both operations
            return Result.Fail<TFirst>(firstResult.Errors.Union(secondResult.Errors));
        }
        return firstResult;
    }

    /// <summary>
    /// If the asynchronous operation returning a <see cref="Result{TFirst}"/> is successful,
    /// invokes the specified follow-up asynchronous task (returning a <see cref="Result{TSecond}"/>)
    /// for its side effects and returns the original result.
    /// If the follow-up task fails, errors from both operations are combined.
    /// </summary>
    /// <typeparam name="TFirst">The type of the successful value in the first result.</typeparam>
    /// <typeparam name="TSecond">The type of the value returned by the follow-up task.</typeparam>
    /// <param name="firstTask">The asynchronous operation returning a <see cref="Result{TFirst}"/>.</param>
    /// <param name="newTask">
    /// A function that takes the successful value from the first result and returns an asynchronous <see cref="Result{TSecond}"/>.
    /// </param>
    /// <returns>
    /// A task producing a <see cref="Result{TFirst}"/> that preserves the original value on success,
    /// or a failure result with combined errors if the follow-up fails.
    /// </returns>
    public static async Task<Result<TFirst>> ThenWithFirstReturnAsync<TFirst, TSecond>(
        this Task<Result<TFirst>> firstTask,
        Func<TFirst, Task<Result<TSecond>>> newTask)
    {
        var firstResult = await firstTask.ConfigureAwait(false);
        if (firstResult.IsFailed)
        {
            // Propagate existing errors
            return Result.Fail<TFirst>(firstResult.Errors);
        }
        // If first step succeeded, run newTask
        var secondResult = await newTask(firstResult.Value).ConfigureAwait(false);
        if (secondResult.IsFailed)
        {
            // Combine errors from both operations
            return Result.Fail<TFirst>(firstResult.Errors.Union(secondResult.Errors));
        }
        return firstResult;
    }

    /// <summary>
    /// If the asynchronous operation returning a <see cref="Result{TFirst}"/> is successful,
    /// invokes the specified follow-up asynchronous task (returning a non-generic <see cref="Result"/>)
    /// for its side effects and returns the original result.
    /// If the follow-up task fails, errors from both operations are combined.
    /// </summary>
    /// <typeparam name="TFirst">The type of the successful value in the first result.</typeparam>
    /// <param name="firstTask">The asynchronous operation returning a <see cref="Result{TFirst}"/>.</param>
    /// <param name="newTask">
    /// A function that takes the successful value from the first result and returns an asynchronous <see cref="Result"/>.
    /// </param>
    /// <returns>
    /// A task producing a <see cref="Result{TFirst}"/> that preserves the original value on success,
    /// or a failure result with combined errors if the follow-up fails.
    /// </returns>
    public static async Task<Result<TFirst>> ThenWithFirstReturnAsync<TFirst>(
       this Task<Result<TFirst>> firstTask,
       Func<TFirst, Task<Result>> newTask)
    {
        var firstResult = await firstTask.ConfigureAwait(false);
        if (firstResult.IsFailed)
        {
            // Propagate existing errors
            return Result.Fail<TFirst>(firstResult.Errors);
        }
        // If first step succeeded, run newTask
        var secondResult = await newTask(firstResult.Value).ConfigureAwait(false);
        if (secondResult.IsFailed)
        {
            // Combine errors from both operations
            return Result.Fail<TFirst>(firstResult.Errors.Union(secondResult.Errors));
        }
        return firstResult;
    }

    /// <summary>
    /// If the synchronous <see cref="Result{TFirst}"/> is successful,
    /// invokes the specified follow-up asynchronous task (returning a non-generic <see cref="Result"/>)
    /// for its side effects and returns the follow-up result.
    /// If the first result is a failure, its errors are propagated.
    /// </summary>
    /// <typeparam name="TFirst">The type of the successful value in the first result.</typeparam>
    /// <param name="firstResult">The synchronous <see cref="Result{TFirst}"/>.</param>
    /// <param name="newTask">
    /// A function that takes the successful value from the first result and returns an asynchronous <see cref="Result"/>.
    /// </param>
    /// <returns>
    /// A task producing a non-generic <see cref="Result"/> that is the result of the follow-up operation if the first result is successful;
    /// otherwise, a failure result containing the errors from the first result.
    /// </returns>
    public static async Task<Result> ThenAsync<TFirst>(
        this Result<TFirst> firstResult,
        Func<TFirst, Task<Result>> newTask)
    {
        if (firstResult.IsFailed)
        {
            // Propagate existing errors
            return Result.Fail(firstResult.Errors);
        }

        // If first step succeeded, run newTask
        return await newTask(firstResult.Value).ConfigureAwait(false);
    }


    /// <summary>
    /// Evaluates the specified condition on the asynchronous result value.
    /// If the condition is met, executes the action with the calculated value.
    /// </summary>
    /// <typeparam name="TIn">The type contained in the result.</typeparam>
    /// <typeparam name="TAction">The type of the value produced by the condition function.</typeparam>
    /// <param name="task">A task that produces a <see cref="Result{TIn}"/>.</param>
    /// <param name="condition">
    /// A function that takes the result value and returns a tuple:
    /// <c>(bool ConditionMet, TAction Value)</c>.
    /// The boolean indicates whether the condition is met, and the value is passed to the action.
    /// </param>
    /// <param name="action">An action to execute if the condition is met, receiving the calculated value.</param>
    /// <returns>
    /// A task that produces a successful result with the original value if the operation is successful;
    /// otherwise, a failure result.
    /// </returns>
    public static async Task<Result<TIn>> OnConditionsSuccessAsync<TIn, TAction>(
        this Task<Result<TIn>> task,
        Func<TIn, (bool ConditionMet, TAction Value)> condition,
        Action<TAction> action)
    {
        var result = await task;
        if (result.IsSuccess)
        {
            // Invoke the condition function to get a tuple: (isConditionMet, calculatedValue)
            var (conditionMet, value) = condition(result.Value);
            if (conditionMet)
            {
                // Execute the action with the calculated value
                action(value);
            }
            // Return the original successful result (or modify if needed)
            return Result.Ok(result.Value);
        }

        return Result.Fail<TIn>(result.Errors);
    }

    /// <summary>
    /// Evaluates the specified condition on the asynchronous result value.
    /// If the condition is met, executes the action with the calculated value.
    /// Otherwise, adds the provided error to the result and returns a failure.
    /// </summary>
    /// <typeparam name="TIn">The type contained in the result.</typeparam>
    /// <typeparam name="TAction">The type of the value produced by the condition function.</typeparam>
    /// <param name="task">A task that produces a <see cref="Result{TIn}"/>.</param>
    /// <param name="condition">
    /// A function that takes the result value and returns a tuple: 
    /// <c>(bool ConditionMet, TAction Value)</c>. 
    /// The boolean indicates whether the condition is met, and the value is passed to the action.
    /// </param>
    /// <param name="action">An action to execute if the condition is met, receiving the calculated value.</param>
    /// <param name="error">The error to add if the condition is not met.</param>
    /// <returns>
    /// A task that produces a successful result with the original value if the condition is met; 
    /// otherwise, a failure result with the specified error.
    /// </returns>
    public static async Task<Result<TIn>> OnConditionsSuccessAsync<TIn, TAction>(
        this Task<Result<TIn>> task,
        Func<TIn, (bool ConditionMet, TAction Value)> condition,
        Action<TAction> action,
        IError error)
    {
        var result = await task;
        if (result.IsSuccess)
        {
            // Invoke the condition function to get a tuple: (isConditionMet, calculatedValue)
            var (conditionMet, value) = condition(result.Value);
            if (conditionMet)
            {
                // Execute the action with the calculated value
                action(value);
            }
            else
            {
                result.Errors.Add(error);
                return Result.Fail<TIn>(result.Errors);
            }
            // Return the original successful result (or modify if needed)
            return Result.Ok(result.Value);
        }

        return Result.Fail<TIn>(result.Errors);
    }

    /********************************************
     *            Synchronous Methods          *
     ********************************************/

    /// <summary>
    /// Maps the value from a <see cref="Result{TIn}"/> to a new value of type <typeparamref name="TOut"/> using the specified converter function.
    /// </summary>
    /// <typeparam name="TIn">The input type contained in the result.</typeparam>
    /// <typeparam name="TOut">The output type after conversion.</typeparam>
    /// <param name="result">A <see cref="Result{TIn}"/>.</param>
    /// <param name="converter">A function that converts the input value to an output value.</param>
    /// <returns>
    /// A <see cref="Result{TOut}"/> containing the converted value if the original result is successful;
    /// otherwise, a failure result.
    /// </returns>
    public static Result<TOut> Map<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, TOut> converter)
    {
        if (result.IsFailed)
            return Result.Fail<TOut>(result.Errors);

        return Result.Ok(converter(result.Value));
    }

    /// <summary>
    /// Ensures that the given predicate on the result value is false.
    /// If the predicate returns true, the specified error is returned.
    /// </summary>
    /// <typeparam name="T">The type contained in the result.</typeparam>
    /// <param name="result">A <see cref="Result{T}"/>.</param>
    /// <param name="predicate">A function that evaluates the result value.</param>
    /// <param name="error">The error to return if the predicate is met.</param>
    /// <returns>
    /// The original successful result if the predicate is false; otherwise, a failure result.
    /// </returns>
    public static Result<T> OnSuccess<T>(
        this Result<T> result,
        Func<T, bool> predicate,
        Error error)
    {
        if (result.IsFailed)
            return result;

        return predicate(result.Value)
            ? Result.Fail<T>(error)
            : result;
    }

    /// <summary>
    /// Ensures that none of the provided predicates on the result value return true.
    /// Each predicate is evaluated sequentially; if any predicate returns true, the corresponding error is returned.
    /// </summary>
    /// <typeparam name="T">The type contained in the result.</typeparam>
    /// <param name="result">A <see cref="Result{T}"/>.</param>
    /// <param name="predicates">
    /// An array of tuples where each tuple contains:
    /// <list type="bullet">
    /// <item><description>A condition function to evaluate the result value.</description></item>
    /// <item><description>An error to return if the condition is met.</description></item>
    /// </list>
    /// </param>
    /// <returns>
    /// The original successful result if none of the predicates return true; otherwise, a failure result.
    /// </returns>
    public static Result<T> EnsureNone<T>(
        this Result<T> result,
        params (Func<T, bool> condition, Error error)[] predicates)
    {
        if (result.IsFailed)
            return result;

        foreach (var (condition, error) in predicates)
        {
            if (condition(result.Value))
                return Result.Fail<T>(error);
        }
        return result;
    }

    /// <summary>
    /// Ensures that the given predicate on the result value is false.
    /// If the predicate returns true, a failure result is returned using the specified error factory.
    /// </summary>
    /// <typeparam name="T">The type contained in the result.</typeparam>
    /// <param name="result">A <see cref="Result{T}"/>.</param>
    /// <param name="predicate">A function that evaluates the result value.</param>
    /// <param name="errorFactory">A function that produces a failure result if the predicate is met.</param>
    /// <returns>
    /// The original successful result if the predicate is false; otherwise, a failure result.
    /// </returns>
    public static Result<T> EnsureNone<T>(
        this Result<T> result,
        Func<T, bool> predicate,
        Func<T, Result> errorFactory)
    {
        if (result.IsFailed)
            return Result.Fail<T>(result.Errors);
        if (predicate(result.Value))
            return Result.Fail<T>(errorFactory(result.Value).Errors);
        return result;
    }

    /// <summary>
    /// Executes the specified error handler actions if the result is a failure.
    /// </summary>
    /// <typeparam name="T">The type contained in the result.</typeparam>
    /// <param name="result">A <see cref="Result{T}"/>.</param>
    /// <param name="errorHandlers">
    /// An array of actions to execute if the result is a failure.
    /// Each action receives the list of errors.
    /// </param>
    /// <returns>
    /// The original result.
    /// </returns>
    public static Result<T> OnFailed<T>(
        this Result<T> result,
        params Action<List<IError>>[] errorHandlers)
    {
        if (result.IsFailed)
        {
            foreach (var handler in errorHandlers)
            {
                handler(result.Errors);
            }
        }
        return result;
    }

    /// <summary>
    /// Executes the specified actions with the result value if the result is successful.
    /// </summary>
    /// <typeparam name="T">The type contained in the result.</typeparam>
    /// <param name="result">A <see cref="Result{T}"/>.</param>
    /// <param name="actions">
    /// An array of actions to execute on success, each receiving the result value.
    /// </param>
    /// <returns>
    /// The original result.
    /// </returns>
    public static Result<T> OnSuccess<T>(
        this Result<T> result,
        params Action<T>[] actions)
    {
        if (result.IsSuccess)
        {
            foreach (var action in actions)
            {
                action(result.Value);
            }
        }
        return result;
    }

    /// <summary>
    /// Evaluates the specified condition on the result value.
    /// If the condition is met, executes the action with the calculated value.
    /// </summary>
    /// <typeparam name="TIn">The type contained in the result.</typeparam>
    /// <typeparam name="TAction">The type of the value produced by the condition function.</typeparam>
    /// <param name="result">A <see cref="Result{TIn}"/>.</param>
    /// <param name="condition">
    /// A function that takes the result value and returns a tuple: 
    /// <c>(bool ConditionMet, TAction Value)</c>.
    /// The boolean indicates whether the condition is met, and the value is passed to the action.
    /// </param>
    /// <param name="action">An action to execute if the condition is met, receiving the calculated value.</param>
    /// <returns>
    /// The original result.
    /// </returns>
    public static Result<TIn> OnConditionsSuccess<TIn, TAction>(
        this Result<TIn> result,
        Func<TIn, (bool ConditionMet, TAction Value)> condition,
        Action<TAction> action)
    {
        if (result.IsSuccess)
        {
            var (conditionMet, value) = condition(result.Value);
            if (conditionMet)
            {
                action(value);
            }
        }
        return result;
    }

    /// <summary>
    /// Evaluates the specified condition on the result value.
    /// If the condition is met, executes the action with the calculated value.
    /// Otherwise, returns a failure result with the specified error.
    /// </summary>
    /// <typeparam name="TIn">The type contained in the result.</typeparam>
    /// <typeparam name="TAction">The type of the value produced by the condition function.</typeparam>
    /// <param name="result">A <see cref="Result{TIn}"/>.</param>
    /// <param name="condition">
    /// A function that takes the result value and returns a tuple: 
    /// <c>(bool ConditionMet, TAction Value)</c>.
    /// The boolean indicates whether the condition is met, and the value is passed to the action.
    /// </param>
    /// <param name="action">An action to execute if the condition is met, receiving the calculated value.</param>
    /// <param name="error">The error to return if the condition is not met.</param>
    /// <returns>
    /// The original result if the condition is met; otherwise, a failure result containing the specified error.
    /// </returns>
    public static Result<TIn> OnConditionsSuccess<TIn, TAction>(
        this Result<TIn> result,
        Func<TIn, (bool ConditionMet, TAction Value)> condition,
        Action<TAction> action,
        IError error)
    {
        if (result.IsSuccess)
        {
            var (conditionMet, value) = condition(result.Value);
            if (conditionMet)
            {
                action(value);
            }
            else
            {
                return Result.Fail<TIn>(error);
            }
        }
        return result;
    }
}
