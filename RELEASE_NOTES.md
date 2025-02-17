#### 1.5.28 September 11th 2024 ####

* [Bump Akka.NET to 1.5.28](https://github.com/akkadotnet/akka.net/releases/tag/1.5.28)
* [Bump Akka.Persistence.Hosting to 1.5.28](https://github.com/akkadotnet/Akka.Hosting/releases/tag/1.5.28)
* [Bump MongoDB.Driver to 2.27.0](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/380)
* [Add large snapshot support](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/383)

**Support For Large (Greater Than 16 Megabytes) Snapshot Store**

> [!NOTE]
> 
> GridFS is considered as an advanced feature, it will not be supported by Akka.Hosting.

We added a new SnapshotStore that supports GridFS. To use it, you will need to set it through manual HOCON setting.

```text
akka.persistence.snapshot-store.mongodb.class = "Akka.Persistence.MongoDb.Snapshot.MongoDbGridFsSnapshotStore, Akka.Persistence.MongoDb"
```

#### 1.5.26 July 15th 2024 ####

* [Bump Akka.NET to 1.5.26](https://github.com/akkadotnet/akka.net/releases/tag/1.5.26)
* [Bump Akka.Persistence.Hosting to 1.5.25](https://github.com/akkadotnet/Akka.Hosting/releases/tag/1.5.25)
* [Fix failure in CurrentEventByTag when there are no events](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/374)
* [Fix CurrentEventByTag never completes](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/379)

#### 1.5.12.1 September 15 2023 ####

* [Bump Akka.Persistence.Hosting to 1.5.12.1](https://github.com/akkadotnet/Akka.Hosting/releases/tag/1.5.12.1)
* [Bump MongoDB.Driver to 2.21.0](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/339)
* [Remove byte rot code that might have caused issue #313](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/347)
* [Implement transaction on both read and write operation](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/347)
* [Make transaction defaults to enabled](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/348)

**Breaking Behavior Change**

In this version, we're turning transaction on by default. If you're more concerned with database write and read performance compared to data consistency and correctness, you can move back to the old behavior by setting this flag in the HOCON configuration:

```hocon
akka.persistence.journal.mongodb.use-write-transaction = off
akka.persistence.snapshot-store.mongodb.use-write-transaction = off
```

Or by setting them inside the hosting options:

```csharp
var journalOptions = new MongoDbJournalOptions(true) 
    {
        UseWriteTransaction = false
    };
var snapshotOptions = new MongoDbSnapshotOptions(true)
    {
        UseWriteTransaction = false
    };
```

#### 1.5.12 August 10 2023 ####

* [Bump Akka.Persistence.Hosting from 1.5.8.1 to 1.5.12](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/337)
* [Bump AkkaVersion from 1.5.11 to 1.5.12](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/336)
* [Separate Akka.Hosting and core Akka version](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/335)
* [Bump XunitVersion from 2.4.2 to 2.5.0](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/332)
* [Move to using Build Props file and central package management.](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/333)
* [Bump MongoDB.Driver from 2.19.1 to 2.20.0](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/326)
* [Adding Hosting Extensions for Akka.Persistence.MongoDB](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/331)
 
#### 1.5.8 June 30 2023 ####

* [Bump Akka.NET to 1.5.8](https://github.com/akkadotnet/akka.net/releases/tag/1.5.8)
* [Add indexed tags support](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/318)
* [Add CancellationToken suppport to all driver calls](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/328)

#### 1.5.7 May 31 2023 ####

* [Bump Akka.NET to 1.5.7](https://github.com/akkadotnet/akka.net/releases/tag/1.5.7)
* [Bump MongoDb.Driver to 2.19.1](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/311)

#### 1.5.1.1 March 24 2023 ####

* [fixed ObjectSerializer initialization for backward compatibility](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/310) - this solves a compatibility problem in older Akka.Persistence.MongoDb applications that was introduced by updating MongoDb.Driver to 2.19.0.

#### 1.5.1 March 21 2023 ####
* [All writes are now performed via MongoDb transactions](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/301)
* [Bump MongoDb.Driver to 2.19.0](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/308)
* [Bump Akka.NET to 1.5.1](https://github.com/akkadotnet/akka.net/releases/tag/1.5.1)

#### 1.5.0 March 03 2023 ####
* [Bump Akka.NET to 1.5.0](https://github.com/akkadotnet/akka.net/releases/tag/1.5.0)

#### 1.4.48 January 24 2023 ####
* [Bump Akka.NET to 1.4.48](https://github.com/akkadotnet/akka.net/releases/tag/1.4.48)
* [Bump MongoDb.Driver to 2.17.0](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/284)

#### 1.4.40 July 27 2022 ####
* [Bump Akka.NET to 1.4.40](https://github.com/akkadotnet/akka.net/releases/tag/1.4.40)
* [Fix HighestSequenceNr query not returning proper value](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/267)
* [Bump MongoDb.Driver to 2.17.0](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/273)

#### 1.4.40-RC1 July 1 2022 ####
* [Fix HighestSequenceNr query not returning proper value](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/267)
* [Bump MongoDb.Driver to 2.16.1](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/266)

#### 1.4.39 June 6 2022 ####
* [Bump Akka.NET version to 1.4.39](https://github.com/akkadotnet/akka.net/releases/tag/1.4.39)
* [Fix compatibility with Akka.Cluster.Sharding in persistence mode](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/259)
* [Bump MongoDb.Driver to 2.15.1](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/255)
* [Fix BsonTimestamp causes NRE to be thrown](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/249)

#### 1.4.38-beta2 May 27 2022 ####

* [Fix compatibility with Akka.Cluster.Sharding in persistence mode](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/259)
* [Bump Akka.NET version to 1.4.38](https://github.com/akkadotnet/akka.net/releases/tag/1.4.38)
* [Bump MongoDb.Driver to 2.15.1](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/255)

#### 1.4.38-beta1 April 15 2022 ####

* [Fix BsonTimestamp causes NRE to be thrown](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/249)

#### 1.4.37 April 15 2022 ####

* [Bump Akka.NET version to 1.4.37](https://github.com/akkadotnet/akka.net/releases/tag/1.4.37)
* [Bump MongoDb.Driver to 2.15.0](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/245)

#### 1.4.31 December 21 2021 ####

* [Bump Akka.NET version to 1.4.31](https://github.com/akkadotnet/akka.net/releases/tag/1.4.31)
* [Bump MongoDb.Driver to 2.14.1](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/234)
* [Fix MongoDB InsertManyAsync to support ordering](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/224)

#### 1.4.25 September 9 2021 ####

* [Bump Akka.NET version to 1.4.25](https://github.com/akkadotnet/akka.net/releases/tag/1.4.25)
* [Bump MongoDb.Driver to 2.13.1](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/216)

#### 1.4.21 July 07 2021 ####

* [Bump Akka.NET version to 1.4.21](https://github.com/akkadotnet/akka.net/releases/tag/1.4.21)
* [Bump MongoDb.Driver to 2.12.4](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/209)
* [Change table auto-initialize default value to true](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/212)

#### 1.4.19 May 04 2021 ####

* [Bump MongoDb.Driver to 2.12.2](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/197)
* [Bump Akka.NET version to 1.4.19](https://github.com/akkadotnet/akka.net/releases/tag/1.4.19)
* [Add programmatic setup support](https://github.com/akkadotnet/Akka.Persistence.MongoDB/pull/199)

Please [read the Akka.Persistence.MongoDb README.md on how to use the new `MongoDbPersistenceSetup` feature to programmatically configure your `MongoDbClient`](https://github.com/akkadotnet/Akka.Persistence.MongoDB#programmatic-configuration).
