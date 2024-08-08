using Airport.API.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Airport.API.Models
{
    public class Leg
    {
        public int LegId { get; set; }
        public LegType LegType { get; set; }
        public TimeSpan CrossingTime { get; set; }
        public bool IsOccupied { get => FlightId.HasValue; }
        public int? FlightId { get; set; }
        public virtual Flight Flight { get; set; }
        public virtual ICollection<LegConnection> NextLegs { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        static Leg()
        {
            LegHelpers.isLegOccupied = [];
            for (var legId = LegIds.Leg1;  legId <= LegIds.Leg9;legId++)
            {
                LegHelpers.isLegOccupied.TryAdd(legId, false);
            }
        }
    }
}
