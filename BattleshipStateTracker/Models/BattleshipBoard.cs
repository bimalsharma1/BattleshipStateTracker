using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BattleshipStateTracker.Models
{
    public class BattleshipBoard
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "board")]
        public List<ShipPosition> Board { get; set; }
        [DataMember(Name = "ships")]
        public List<Ship> Ships { get; set; }
    }
}
