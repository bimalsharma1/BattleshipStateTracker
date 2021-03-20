using System.Runtime.Serialization;

namespace BattleshipStateTracker.Models
{
    public class Ship
    {
        [DataMember(Name = "shipName")]
        public string shipName { get; set; }
        [DataMember(Name = "xPos")]
        public int XPosition { get; set; }
        [DataMember(Name = "yPos")]
        public int YPosition { get; set; }
    }
}
