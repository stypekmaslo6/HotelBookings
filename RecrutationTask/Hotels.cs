using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace RecrutationTask
{
    public class Hotel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<RoomType> RoomTypes { get; set; }
        public List<Room> Rooms { get; set; }
    }

    public class RoomType
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public List<string> Amenities { get; set; }
        public List<string> Features { get; set; }
        public int? AvailableRooms { get; set; }
    }

    public class Room
    {
        public string RoomType { get; set; }
        public string RoomId { get; set; }
    }
}
