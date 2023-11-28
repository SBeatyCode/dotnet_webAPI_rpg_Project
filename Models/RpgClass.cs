using System.Text.Json.Serialization;

namespace rpg_Class_Project.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RpgClass
    {
        Novice = 0,
        Warrior = 1, 
        Wizarad = 2,
        Cleric = 3,
        Rouge = 4,
        Bard = 5,
        Paladin = 6, 
        Druid = 7
    }
}