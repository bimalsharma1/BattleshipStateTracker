using System.Runtime.Serialization;

namespace BattleshipStateTracker.Models
{
    public class Ship
    {
        [DataMember(Name = "boardName")]
        public string BoardName { get; set; }
        [DataMember(Name = "shipName")]
        public string ShipName { get; set; }
        [DataMember(Name = "xPosition")]
        public int XPosition { get; set; }
        [DataMember(Name = "yPosition")]
        public int YPosition { get; set; }
    }
}
