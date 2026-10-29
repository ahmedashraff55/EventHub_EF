namespace EventHub.Entities;

public class Badge
{
    public int BadgeId { get; set; }
    public string BadgeNumber { get; set; } = string.Empty;
    public DateTime IssuedDate { get; set; }
    public BadgeTier Tier { get; set; }

    public int AttendeeId { get; set; }
    public Attendee Attendee { get; set; } = null!;
}
