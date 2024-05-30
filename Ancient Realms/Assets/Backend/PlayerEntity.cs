using System;
using System.Collections;
using System.Collections.Generic;
using Unisave;
using Unisave.Entities;
using Unisave.Facades;

[EntityCollectionName("PlayerTable")]
public class PlayerEntity : Entity
{
    public string publicKey;
    public DateTime lastLoginAt = DateTime.UtcNow;
}