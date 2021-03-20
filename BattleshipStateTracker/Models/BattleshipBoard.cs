using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BattleshipStateTracker.Models
{
    public class BattleshipBoard
    {
        [DataMember(Name = "id")]
        public Guid Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "board")]
        public List<Position> Board { get; set; }
        [DataMember(Name = "ships")]
        public List<Ship> Ships { get; set; }
    }

    public class Position
    {
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public bool Bombed { get; set; }
    }
}
