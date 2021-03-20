using System.Runtime.Serialization;

namespace BattleshipStateTracker.Models
{
    public class AttackPosition
    {
        [DataMember(Name = "xPosition")]
        public int XPosition { get; set; }
        [DataMember(Name = "yPosition")]
        public int YPosition { get; set; }
    }
}
