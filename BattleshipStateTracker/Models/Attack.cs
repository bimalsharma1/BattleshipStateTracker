using System.Runtime.Serialization;

namespace BattleshipStateTracker.Models
{
    public class Attack
    {
        [DataMember(Name = "boardName")]
        public string BoardName { get; set; }
        [DataMember(Name = "attackPosition")]
        public AttackPosition AttackPosition { get; set; }
    }
}
