using System;
using ESDatabase.Entities;
using Unisave;

namespace ESDatabase.Classes
{
    public class LoginResponse
    {
        [Fillable] public PlayerData playerData {get;set;}
        [Fillable] public PriceData priceData {get;set;}
        [Fillable] public string devBlog {get;set;}
    }
}

