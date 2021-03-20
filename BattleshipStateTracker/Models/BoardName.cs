using System.Runtime.Serialization;

namespace BattleshipStateTracker.Models
{
    public class BoardName
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
