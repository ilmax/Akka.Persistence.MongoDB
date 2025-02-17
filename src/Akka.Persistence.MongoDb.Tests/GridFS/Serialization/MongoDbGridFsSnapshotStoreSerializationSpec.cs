﻿using Akka.Configuration;
using Akka.Persistence.TCK.Serialization;
using Akka.Util.Internal;
using Xunit;
using Xunit.Abstractions;

#nullable enable
namespace Akka.Persistence.MongoDb.Tests.GridFS.Serialization;

[Collection("MongoDbSpec")]
public class MongoDbSnapshotStoreSerializationSpec : SnapshotStoreSerializationSpec, IClassFixture<DatabaseFixture>
{
    public static readonly AtomicCounter Counter = new AtomicCounter(0);

    private readonly ITestOutputHelper _output;

    public MongoDbSnapshotStoreSerializationSpec(ITestOutputHelper output, DatabaseFixture databaseFixture)
        : base(CreateSpecConfig(databaseFixture, Counter.GetAndIncrement()), nameof(MongoDbSnapshotStoreSerializationSpec), output)
    {
        _output = output;
        output.WriteLine(databaseFixture.MongoDbConnectionString(Counter.Current));
    }

    private static Config CreateSpecConfig(DatabaseFixture databaseFixture, int id)
    {
        var specString = @"
                akka.test.single-expect-default = 3s
                akka.persistence {
                    publish-plugin-commands = on
                    snapshot-store {
                        plugin = ""akka.persistence.snapshot-store.mongodb""
                        mongodb {
                            class = ""Akka.Persistence.MongoDb.Snapshot.MongoDbGridFsSnapshotStore, Akka.Persistence.MongoDb""
                            connection-string = """ + databaseFixture.MongoDbConnectionString(id) + @"""
                            auto-initialize = on
                            collection = ""SnapshotStore""
                        }
                    }
                }";

        return ConfigurationFactory.ParseString(specString);
    }
}