﻿- [Akka.Persistence.MongoDB](#akkapersistencemongodb)
    * [Setup](#setup)
    * [Configuration](#configuration)
    * [Programmatic configuration](#programmatic-configuration)
    * [Serialization](#serialization)
- [Large Snapshot Store Support](#large-snapshot-store-support)
    * [Configuring `MongoDbGridFSSnapshotStore`](#configuring-mongodbgridfssnapshotstore)
- [Performance Benchmarks](#performance-benchmarks)

# Akka.Persistence.MongoDB

Akka Persistence journal and snapshot store backed by MongoDB database.

> [!NOTE]
>
> The MongoDB operator to limit the number of documents in a query only accepts an integer while akka provides a long as maximum for the loading of events during the replay. Internally the long value is cast to an integer and if the value is higher than Int32.MaxValue, Int32.MaxValue is used. So if you have stored more than 2,147,483,647 events for a single PersistenceId, you may have a problem :wink:

## Setup

To activate the journal plugin, add the following lines to actor system configuration file:

```
akka.persistence.journal.plugin = "akka.persistence.journal.mongodb"
akka.persistence.journal.mongodb.connection-string = "<database connection string>"
akka.persistence.journal.mongodb.collection = "<journal collection>"
```

Similar configuration may be used to setup a MongoDB snapshot store:

```
akka.persistence.snapshot-store.plugin = "akka.persistence.snapshot-store.mongodb"
akka.persistence.snapshot-store.mongodb.connection-string = "<database connection string>"
akka.persistence.snapshot-store.mongodb.collection = "<snapshot-store collection>"
```

Remember that connection string must be provided separately to Journal and Snapshot Store. To finish setup simply initialize plugin using: `MongoDbPersistence.Get(actorSystem);`

## Configuration

Both journal and snapshot store share the same configuration keys (however they reside in separate scopes, so they are defined distinctly for either journal or snapshot store):

```hocon
akka.persistence {
	journal {
		plugin = "akka.persistence.journal.mongodb"
		mongodb {
			# qualified type name of the MongoDb persistence journal actor
			class = "Akka.Persistence.MongoDb.Journal.MongoDbJournal, Akka.Persistence.MongoDb"

			# connection string used for database access
			connection-string = ""

			# transaction
			use-write-transaction = off

			# should corresponding journal table's indexes be initialized automatically
			auto-initialize = on

			# dispatcher used to drive journal actor
			plugin-dispatcher = "akka.actor.default-dispatcher"

			# MongoDb collection corresponding with persistent journal
			collection = "EventJournal"

			# metadata collection
			metadata-collection = "Metadata"

			# For users with legacy data, who want to keep writing data to MongoDb using the original BSON format
			# and not the standard binary format introduced in v1.4.0 (see https://github.com/akkadotnet/Akka.Persistence.MongoDB/issues/72)
			# enable this setting via `legacy-serialization = on`.
			#
			# NOTE: this will likely break features such as Akka.Cluster.Sharding, IActorRef serialization, AtLeastOnceDelivery, and more.
			legacy-serialization = off
			
			# Per-call timeout setting - Journal will err on the side of caution and fail calls that take longer than this
			# to complete. This is to prevent the journal from blocking indefinitely if the database is slow or unresponsive.
			# If you experience frequent failures due to timeouts, you may want to increase this value.
			# Default: 10 seconds
			call-timeout = 10s
		}
	}

	snapshot-store {
		plugin = "akka.persistence.snapshot-store.mongodb"
		mongodb {
			# qualified type name of the MongoDB persistence snapshot actor
			class = "Akka.Persistence.MongoDb.Snapshot.MongoDbSnapshotStore, Akka.Persistence.MongoDb"

			# connection string used for database access
			connection-string = ""

			# transaction
			use-write-transaction = off

			# should corresponding snapshot's indexes be initialized automatically
			auto-initialize = on

			# dispatcher used to drive snapshot storage actor
			plugin-dispatcher = "akka.actor.default-dispatcher"

			# MongoDb collection corresponding with persistent snapshot store
			collection = "SnapshotStore"

			# For users with legacy data, who want to keep writing data to MongoDb using the original BSON format
			# and not the standard binary format introduced in v1.4.0 (see https://github.com/akkadotnet/Akka.Persistence.MongoDB/issues/72)
			# enable this setting via `legacy-serialization = on`.
			#
			# NOTE: this will likely break features such as Akka.Cluster.Sharding, IActorRef serialization, AtLeastOnceDelivery, and more.
			legacy-serialization = off

      # Per-call timeout setting - Journal will err on the side of caution and fail calls that take longer than this
      # to complete. This is to prevent the journal from blocking indefinitely if the database is slow or unresponsive.
      # If you experience frequent failures due to timeouts, you may want to increase this value.
      # Default: 10 seconds
      call-timeout = 10s
		}
	}

    query {
        mongodb {
            # Implementation class of the EventStore ReadJournalProvider
            class = "Akka.Persistence.MongoDb.Query.MongoDbReadJournalProvider, Akka.Persistence.MongoDb"

            # Absolute path to the write journal plugin configuration entry that this 
            # query journal will connect to. 
            # If undefined (or "") it will connect to the default journal as specified by the
            # akka.persistence.journal.plugin property.
            write-plugin = ""

            # The SQL write journal is notifying the query side as soon as things
            # are persisted, but for efficiency reasons the query side retrieves the events 
            # in batches that sometimes can be delayed up to the configured `refresh-interval`.
            refresh-interval = 3s
  
            # How many events to fetch in one query (replay) and keep buffered until they
            # are delivered downstreams.
            max-buffer-size = 500
        }
    }
}
```

## Programmatic configuration

You can programmatically override the connection string setting in the HOCON configuration by adding a `MongoDbPersistenceSetup` to the
`ActorSystemSetup` during `ActorSystem` creation. The `MongoDbPersistenceSetup` takes `MongoClientSettings` instances to be used to configure
MongoDB client connection to the server. The `connection-string` settings in the HOCON configuration will be ignored if any of these `MongoClientSettings`
exists inside the Setup object.

> [!NOTE]
> The HOCON configuration is still needed for this to work, only the `connection-string` setting in the configuration will be overriden.

Setting connection override for both snapshot store and journal:
```
// Set snapshotClientSettings or journalClientSettings to null if you do not use them.
var snapshotClientSettings = new MongoClientSettings();
var journalClientSettings = new MongoClientSettings();

// database names are not needed when its client setting is set to null
var snapshotDatabaseName = "theSnapshotDatabase"
var journalDatabaseName = "theJournalDatabase"

var setup = BootstrapSetup.Create()
  .WithConfig(myHoconConfig)
  .And(new MongoDbPersistenceSetup(snapshotDatabaseName, snapshotClientSettings, journalDatabaseName, journalClientSettings));

var actorSystem = ActorSystem.Create("actorSystem", setup);
```

Setting connection override only for snapshot store:
```
var snapshotClientSettings = new MongoClientSettings();
var snapshotDatabaseName = "theSnapshotDatabase"

var setup = BootstrapSetup.Create()
  .WithConfig(myHoconConfig)
  .And(new MongoDbPersistenceSetup(snapshotDatabaseName, snapshotClientSettings, null, null));

var actorSystem = ActorSystem.Create("actorSystem", setup);
```

Setting connection override only for journal:
```
var journalClientSettings = new MongoClientSettings();
var journalDatabaseName = "theJournalDatabase"

var setup = BootstrapSetup.Create()
  .WithConfig(myHoconConfig)
  .And(new MongoDbPersistenceSetup(null, null, journalDatabaseName, journalClientSettings));

var actorSystem = ActorSystem.Create("actorSystem", setup);
```

## Serialization

[Going from v1.4.0 onwards, all events and snapshots are saved as byte arrays using the standard Akka.Persistence format](https://github.com/akkadotnet/Akka.Persistence.MongoDB/issues/72).

However, in the event that you have one of the following use cases:

1. Legacy data all stored in the original BSON / "object" format;
2. A use case where BSON is preferable, i.e. so it can be queried directly via MongoDb queries rather than Akka.Persistence.Query; or
3. A requirement to keep all data in human-readable form.

Then you can disable binary serialization (enabled by default) via the following HOCON:

```
akka.persistence.mongodb{
   journal{
    legacy-serialization = off
  }

  snapshot-store{
   legacy-serialization = off
 }
}
```

Setting `legacy-serialization = on` will allow you to save objects in a BSON format.

**WARNING**: However, `legacy-serialization = on` will break Akka.NET serialization. `IActorRef`s, Akka.Cluster.Sharding, `AtLeastOnceDelivery` actors, and other built-in Akka.NET use cases can't be properly supported using this format. Use it at your own risk.

# Large Snapshot Store Support

MongoDb limits the size of documents it can store to 16 megabytes. If you know you will need to store snapshots larger than 16 megabytes, you can use the `Akka.Persistence.MongoDb.Snapshot.MongoDbGridFSSnapshotStore` snapshot store plugin.

> [!NOTE]
>
> `MongoDbGridFSSnapshotStore` is not designed for normal snapshot store plugin, it does not support transaction and read/write operations are slower on this plugin. Only use this plugin if you are having 16 megabyte limit problem.

Note that `MongoDbGridFSSnapshotStore` is considered as advanced optional plugin and it will not get an `Akka.Hosting` support. You will need to use HOCON configuration to use this plugin.

## Configuring `MongoDbGridFSSnapshotStore`

You will need to override the `class` property of the snapshot store `mongodb` HOCON settings to use this plugin. If you're using a custom snapshot-store plugin identifier, then you will need to change that instead. Note that the `use-write-transaction` setting is ignored on this plugin.

```
akka.persistence.snapshot-store.mongodb.class = "Akka.Persistence.MongoDb.Snapshot.MongoDbGridFSSnapshotStore, Akka.Persistence.MongoDb"
```

# Performance Benchmarks

```
Windows 10 (10.0.19045.4780/22H2/2022Update)
AMD Ryzen 9 3900X, 1 CPU, 24 logical and 12 physical cores, 64 GB RAM
.NET SDK 8.0.304
  [Host]     : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2
```

Databases running on Docker for Desktop with WSL2 integration with 10 virtual CPU and 8 GB RAM

All numbers are in msg/sec.

| **Test**        | **MongoDB** |
|:----------------|------------:|
| Persist         |         257 |
| PersistAsync    |        4587 |
| PersistAll      |        7322 |
| PersistAllAsync |       30435 |
| PersistGroup10  |        1239 |
| PersistGroup25  |        2290 |
| PersistGroup50  |        3367 |
| PersistGroup100 |        4108 |
| PersistGroup200 |        4404 |
| PersistGroup400 |        3777 |
| Recovering      |       30370 |
| RecoveringTwo   |       26693 |
| RecoveringFour  |       35551 |
| Recovering8     |       44192 |