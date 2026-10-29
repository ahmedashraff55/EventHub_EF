using EventHub;
using EventHub.Entities;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("EventHub - EF Core database setup");

try
{
    using var db = new EventHubContext();

    bool created = db.Database.EnsureCreated();
    Console.WriteLine(created
        ? "Database and tables were created successfully."
        : "Database already exists. EF Core connected to it.");

    if (!db.Database.CanConnect())
    {
        Console.WriteLine("Could not connect. Check the SQL Server connection string.");
        return;
    }

    if (!db.Organizers.Any() && !db.Attendees.Any())
    {
        var organizer = new Organizer
        {
            Name = "EventHub Demo Organizer",
            CompanyName = "EventHub Events Ltd.",
            IsVerified = true,
            Profile = new OrganizerProfile
            {
                Biography = "A sample organizer profile for verifying the EventHub database.",
                WebsiteUrl = "https://example.com",
                LogoUrl = "https://example.com/logo.png"
            },
            Events =
            {
                new Event
                {
                    Title = "Technology Meetup",
                    Description = "A sample event created to demonstrate the EventHub model.",
                    StartDate = new DateTime(2027, 3, 15, 10, 0, 0),
                    EndDate = new DateTime(2027, 3, 15, 16, 0, 0),
                    MaximumAttendees = 100
                }
            }
        };

        var attendee = new Attendee
        {
            FullName = "Alex Morgan",
            Email = "alex.morgan@example.com",
            Street = "10 Example Street",
            City = "Alexandria",
            Country = "Egypt",
            PostalCode = "21500",
            Badge = new Badge
            {
                BadgeNumber = "EH-0001",
                IssuedDate = DateTime.Today,
                Tier = BadgeTier.VIP
            }
        };

        db.Organizers.Add(organizer);
        db.Attendees.Add(attendee);
        db.SaveChanges();
        Console.WriteLine("Sample organizer, profile, event, attendee, and badge inserted.");
    }

    Console.WriteLine();
    Console.WriteLine($"Organizers: {db.Organizers.Count()}");
    Console.WriteLine($"Organizer profiles: {db.OrganizerProfiles.Count()}");
    Console.WriteLine($"Events: {db.Events.Count()}");
    Console.WriteLine($"Attendees: {db.Attendees.Count()}");
    Console.WriteLine($"Badges: {db.Badges.Count()}");

    var organizers = db.Organizers.Include(o => o.Profile).Include(o => o.Events).ToList();
    Console.WriteLine("\nOrganizers and their events:");
    foreach (var organizer in organizers)
    {
        Console.WriteLine($"- {organizer.Name} | Company: {organizer.CompanyName ?? "N/A"} | Verified: {organizer.IsVerified}");
        Console.WriteLine($"  Profile website: {organizer.Profile?.WebsiteUrl ?? "No website"}");
        foreach (var ev in organizer.Events)
            Console.WriteLine($"  Event: {ev.Title} | Start: {ev.StartDate:g} | Max attendees: {ev.MaximumAttendees}");
    }

    var attendees = db.Attendees.Include(a => a.Badge).ToList();
    Console.WriteLine("\nAttendees and badges:");
    foreach (var attendee in attendees)
    {
        Console.WriteLine($"- {attendee.FullName} | {attendee.Email}");
        Console.WriteLine($"  Address: {attendee.FullAddress}");
        Console.WriteLine(attendee.Badge is null
            ? "  Badge: not issued"
            : $"  Badge: {attendee.Badge.BadgeNumber} | Tier: {attendee.Badge.Tier} | Issued: {attendee.Badge.IssuedDate:d}");
    }

    Console.WriteLine("\nDatabase verification completed.");
}
catch (Exception ex)
{
    Console.WriteLine("An error occurred while setting up or accessing the database.");
    Console.WriteLine(ex.Message);
    Console.WriteLine("Check that SQL Server/LocalDB is installed and the connection string is correct.");
}
