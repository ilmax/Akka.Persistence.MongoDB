﻿using System;
using System.Text;
using Akka.Configuration;
using Akka.Hosting;
using Akka.Persistence.Hosting;
using Akka.Persistence.MongoDb.Snapshot;

#nullable enable
namespace Akka.Persistence.MongoDb.Hosting;

public class MongoDbSnapshotOptions : SnapshotOptions
{
    private static readonly Config Default = MongoDbPersistence.DefaultConfiguration()
        .GetConfig(MongoDbSnapshotSettings.SnapshotStoreConfigPath);

    public MongoDbSnapshotOptions() : this(true)
    {
    }

    public MongoDbSnapshotOptions(bool isDefault, string identifier = "mongodb") : base(isDefault)
    {
        Identifier = identifier;
        AutoInitialize = true;
    }

    public virtual Type Type { get; } = typeof(MongoDbSnapshotStore);
    
    /// <summary>
    /// Connection string used to access the MongoDb, also specifies the database.
    /// </summary>
    public string ConnectionString { get; set; } = "";

    /// <summary>
    /// Name of the collection for the event journal or snapshots
    /// </summary>
    public string? Collection { get; set; }

    /// <summary>
    /// Transaction
    /// </summary>
    public bool? UseWriteTransaction { get; set; }

    /// <summary>
    /// When true, enables BSON serialization (which breaks features like Akka.Cluster.Sharding, AtLeastOnceDelivery, and so on.)
    /// </summary>
    public bool? LegacySerialization { get; set; }

    /// <summary>
    /// Timeout for individual database operations.
    /// </summary>
    /// <remarks>
    /// Defaults to 10s.
    /// </remarks>
    public TimeSpan? CallTimeout { get; set; }

    public override string Identifier { get; set; }
    protected override Config InternalDefaultConfig { get; } = Default;

    protected override StringBuilder Build(StringBuilder sb)
    {
        sb.AppendLine($"class = {Type.AssemblyQualifiedName.ToHocon()}");
        
        sb.AppendLine($"connection-string = {ConnectionString.ToHocon()}");
        
        if(UseWriteTransaction is not null)
            sb.AppendLine($"use-write-transaction = {UseWriteTransaction.ToHocon()}");
        
        if(Collection is not null)
            sb.AppendLine($"collection = {Collection.ToHocon()}");
        
        if(LegacySerialization is not null)
            sb.AppendLine($"legacy-serialization = {LegacySerialization.ToHocon()}");
        
        if(CallTimeout is not null)
            sb.AppendLine($"call-timeout = {CallTimeout.ToHocon()}");

        return base.Build(sb);
    }
}