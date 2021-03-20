using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BattleshipStateTracker.Models
{
    public class BattleshipBoard
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "board")]
        public bool[,] Board { get; set; }
        [DataMember(Name = "ships")]
        public List<Ship> Ships { get; set; }
    }
}
