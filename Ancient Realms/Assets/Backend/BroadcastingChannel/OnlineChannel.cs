using System;
using System.Collections;
using System.Collections.Generic;
using Unisave;
using Unisave.Entities;
using Unisave.Facades;
using Unisave.Broadcasting;
using ESDatabase.Entities;

public class OnlineChannel : BroadcastingChannel
{
    public SpecificChannel WithoutParameters()
    {
        return SpecificChannel.From<OnlineChannel>();
    }
    public SpecificChannel ForPlayer(string entityID)
    {
        return SpecificChannel.From<OnlineChannel>(entityID);
    }
}
