using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.Extensions.Logging;

namespace HSS.System.V2.Services.Helpers;

public static class LoggerExtensions
{
    /// <summary>
    /// Logs an exception with a structured context and optional payload.
    /// </summary>
    public static void LogDescriptiveError<TLoggedIn, TPayload>(
        this ILogger<TLoggedIn> logger,
        Exception exception,
        string operation,
        TPayload? payload = default,
        string? payloadName = null)
    {
        // Build a short JSON representation of the payload (if any)
        string payloadJson = payload is null
            ? "{}"
            : JsonSerializer.Serialize(payload, new JsonSerializerOptions
            {
                WriteIndented = false,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                ReferenceHandler = ReferenceHandler.Preserve
            });

        // Use structured logging: include operation name and payload
        logger.LogError(
            exception,
            "Error during operation '{Operation}'.\n Payload ({PayloadName}): {PayloadJson}.\n Error Message: \"{message}\"",
            operation,
            payloadName ?? typeof(TPayload).Name,
            payloadJson,
            exception.Message
        );
    }

    /// <summary>
    /// Logs an exception with a structured context and optional payload.
    /// </summary>
    public static void LogDescriptiveError<TLoggedIn>(
        this ILogger<TLoggedIn> logger,
        Exception exception,
        string operation,
        string? payloadName = null)
    {
        // Use structured logging: include operation name and payload
        logger.LogError(
            exception,
            "Error during operation '{Operation}'.\n Error Message: \"{message}\"",
            operation,
            exception.Message
        );
    }
}
