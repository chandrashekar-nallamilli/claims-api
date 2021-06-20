using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Claims_Api.Models
{
    public class Claim
    {
        public Claim(Guid id, string name,int year, Type type, decimal damageCost, DateTime created, DateTime lastModified)
        {
            Id = id;
            Year = year;
            Name = name;
            Type = type;
            DamageCost = damageCost;
            Created = created;
            LastModified = lastModified;
        }

        public Guid Id { get; }
        public int Year { get; }
        public string Name { get; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Type Type { get; }
        public decimal DamageCost { get; }
        public DateTime Created { get; }
        public DateTime LastModified { get; }
    }
    
    public enum Type
    {
        Collision,
        Grounding,
        BadWeather,
        Fire
    }
}