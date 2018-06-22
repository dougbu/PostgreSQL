# PostgreSQL bug
## Default execution
By default the program will attempt to use PostgreSQL with a connection string from the environment:
1. `$env:ConnectionString='Server=localhost;Database=hello_world;User Id={user};Password={password};Maximum Pool Size=1024;NoResetOnClose=true;Enlist=false;Max Auto Prepare=3'`
    - Substitute your username and password into the above PowerShell command or equivalent for your console
2.  `dotnet msbuild /t:rebuild`
3.  `dotnet run --no-build`

### Expected output
```
Starting...
Failed in attempt 7.

Unhandled Exception: Microsoft.EntityFrameworkCore.DbUpdateException: An error occurred while updating the entries. See the inner exception for details. ---> Npgsql.PostgresException: 22P02: invalid input syntax for integer: "orange7"
   at Npgsql.NpgsqlConnector.<>c__DisplayClass161_0.<<ReadMessage>g__ReadMessageLong|0>d.MoveNext()
--- End of stack trace from previous location where exception was thrown ---
   at Npgsql.NpgsqlConnector.<>c__DisplayClass161_0.<<ReadMessage>g__ReadMessageLong|0>d.MoveNext() in C:\projects\npgsql\src\Npgsql\NpgsqlConnector.cs:line 1032
--- End of stack trace from previous location where exception was thrown ---
   at Npgsql.NpgsqlDataReader.NextResult(Boolean async, Boolean isConsuming) in C:\projects\npgsql\src\Npgsql\NpgsqlDataReader.cs:line 444
   at Npgsql.EntityFrameworkCore.PostgreSQL.Update.Internal.NpgsqlModificationCommandBatch.ConsumeAsync(RelationalDataReader reader, CancellationToken cancellationToken) in C:\projects\EFCore.PG\src\EFCore.PG\Update\Internal\NpgsqlModificationCommandBatch.cs:line 207
   --- End of inner exception stack trace ---
   at Npgsql.EntityFrameworkCore.PostgreSQL.Update.Internal.NpgsqlModificationCommandBatch.ConsumeAsync(RelationalDataReader reader, CancellationToken cancellationToken) in C:\projects\EFCore.PG\src\EFCore.PG\Update\Internal\NpgsqlModificationCommandBatch.cs:line 216
   at Microsoft.EntityFrameworkCore.Update.ReaderModificationCommandBatch.ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(DbContext _, ValueTuple`2 parameters, CancellationToken cancellationToken)
   at Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal.NpgsqlExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken) in C:\projects\EFCore.PG\src\EFCore.PG\Storage\Internal\NpgsqlExecutionStrategy.cs:line 72
   at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IReadOnlyList`1 entriesToSave, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
   at PostgreSQL.Program.Main(String[] args) in C:\dd\Projects\PostgreSQL\Program.cs:line 40
   at PostgreSQL.Program.<Main>(String[] args)
```

#### Workaround
Increase the "Max Auto Prepare" setting to 4 or more.

## Using other databases
The program also supports InMemory, LocalDb, Sqlite, and SqlServer databases. For example to use LocalDb:
1. `dotnet msbuild /t:rebuild /p:Database=LocalDb`
2. `dotnet run --no-build`

### Note
To use SqlServer, change the hard-coded Server name to match your configuration. See line 73 in Program.cs.

### Expected output
```
Starting...
Done
```
