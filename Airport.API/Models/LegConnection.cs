using System.Text.Json.Serialization;

namespace Airport.API.Models
{
    public class LegConnection
    {
        public int LegId { get; set; }
        public virtual Leg Leg { get; set; }

        public int NextLegId { get; set; }
        public virtual Leg NextLeg { get; set; }
    }
}
