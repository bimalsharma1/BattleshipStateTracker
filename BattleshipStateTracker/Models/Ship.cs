using System.Runtime.Serialization;

namespace BattleshipStateTracker.Models
{
    public class Ship
    {
        [DataMember(Name = "boardName")]
        public string BoardName { get; set; }
        [DataMember(Name = "shipName")]
        public string ShipName { get; set; }
        [DataMember(Name = "startPosition")]
        public Position StartPosition { get; set; }
        [DataMember(Name = "endPosition")]
        public Position EndPosition { get; set; }
        [DataMember(Name = "orientation")]
        public string Orientation { get; set; }
    }
}
