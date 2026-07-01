using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ChinookDal.Interceptors;

/// <summary>
/// Interceptor zum Protokollieren von SQL-Abfragen und deren Ausführungszeiten.
/// Demonstriert die Verwendung von EF Core Interceptoren für die Überwachung von Datenbankoperationen.
/// </summary>
public class LoggingInterceptor : DbCommandInterceptor
{
    public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result)
    {
        LogCommand("Executing", command);
        return base.ReaderExecuting(command, eventData, result);
    }

    public override async ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = default)
    {
        LogCommand("Executing Async", command);
        return await base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
    }

    public override DbDataReader ReaderExecuted(
        DbCommand command,
        CommandExecutedEventData eventData,
        DbDataReader result)
    {
        LogCommandExecuted("Executed", command, eventData);
        return base.ReaderExecuted(command, eventData, result);
    }

    public override async ValueTask<DbDataReader> ReaderExecutedAsync(
        DbCommand command,
        CommandExecutedEventData eventData,
        DbDataReader result,
        CancellationToken cancellationToken = default)
    {
        LogCommandExecuted("Executed Async", command, eventData);
        return await base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
    }

    public override InterceptionResult<int> NonQueryExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<int> result)
    {
        LogCommand("NonQuery Executing", command);
        return base.NonQueryExecuting(command, eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        LogCommand("NonQuery Executing Async", command);
        return await base.NonQueryExecutingAsync(command, eventData, result, cancellationToken);
    }

    public override int NonQueryExecuted(
        DbCommand command,
        CommandExecutedEventData eventData,
        int result)
    {
        LogCommandExecuted("NonQuery Executed", command, eventData);
        return base.NonQueryExecuted(command, eventData, result);
    }

    public override async ValueTask<int> NonQueryExecutedAsync(
        DbCommand command,
        CommandExecutedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        LogCommandExecuted("NonQuery Executed Async", command, eventData);
        return await base.NonQueryExecutedAsync(command, eventData, result, cancellationToken);
    }

    public override void CommandFailed(DbCommand command, CommandErrorEventData eventData)
    {
        LogCommandFailed(command, eventData);
        base.CommandFailed(command, eventData);
    }

    public override async Task CommandFailedAsync(
        DbCommand command,
        CommandErrorEventData eventData,
        CancellationToken cancellationToken = default)
    {
        LogCommandFailed(command, eventData);
        await base.CommandFailedAsync(command, eventData, cancellationToken);
    }

    private static void LogCommand(string operation, DbCommand command)
    {
        var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
        var sql = TruncateSql(command.CommandText);
        Console.WriteLine($"[{timestamp}] {operation}: {sql}");
    }

    private static void LogCommandExecuted(string operation, DbCommand command, CommandExecutedEventData eventData)
    {
        var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
        var sql = TruncateSql(command.CommandText);
        var duration = eventData.Duration.TotalMilliseconds;
        Console.WriteLine($"[{timestamp}] {operation} in {duration:F2}ms: {sql}");
    }

    private static void LogCommandFailed(DbCommand command, CommandErrorEventData eventData)
    {
        var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
        var sql = TruncateSql(command.CommandText);
        var duration = eventData.Duration.TotalMilliseconds;
        var error = eventData.Exception.Message;
        Console.WriteLine($"[{timestamp}] FAILED after {duration:F2}ms: {sql}");
        Console.WriteLine($"[{timestamp}] Error: {error}");
    }

    private static string TruncateSql(string sql)
    {
        const int maxLength = 100;
        if (sql.Length <= maxLength)
            return sql.Replace("\r\n", " ").Replace("\n", " ");
        
        return sql.Substring(0, maxLength).Replace("\r\n", " ").Replace("\n", " ") + "...";
    }
}