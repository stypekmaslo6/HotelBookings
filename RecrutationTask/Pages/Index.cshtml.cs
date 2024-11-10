using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using RecrutationTask;

public class IndexModel : PageModel
{
    private readonly IWebHostEnvironment _environment;

    public IndexModel(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public List<Hotel> Hotels { get; set; }
    public List<Hotel> FilteredHotels { get; set; }
    public List<Bookings> Occupancies { get; set; }
    public List<Bookings> OccupanciesJson { get; set; }
    public string SelectedHotel { get; set; }
    public string SelectedRoomType { get; set; }
    public DateTime? CheckInDate { get; set; }
    public DateTime? CheckOutDate { get; set; }

    // Filter hotels based on user input and occupancy
    public void OnGet(string hotel, string roomType, DateTime? checkIn, DateTime? checkOut)
    {
        LoadHotelData();
        LoadOccupancyData();

        SelectedHotel = hotel;
        SelectedRoomType = roomType;
        CheckInDate = checkIn;
        CheckOutDate = checkOut;

        // Sprawdzenie, czy daty są określone
        bool isDateSelected = checkIn.HasValue && checkOut.HasValue;

        // Filtruj hotele na podstawie wybranego hotelu i typu pokoju
        FilteredHotels = Hotels
            .Where(h => (string.IsNullOrEmpty(hotel) || h.Name == SelectedHotel))  // Filtruj po nazwie hotelu
            .Select(h => new Hotel
            {
                Id = h.Id,
                Name = h.Name,
                RoomTypes = h.RoomTypes
                    .Where(rt => string.IsNullOrEmpty(roomType) || rt.Code == SelectedRoomType)  // Filtruj pokoje po wybranym typie
                    .Select(rt => new RoomType
                    {
                        Code = rt.Code,
                        Description = rt.Description,
                        Amenities = rt.Amenities,
                        Features = rt.Features,
                        AvailableRooms = isDateSelected
                            ? GetAvailableRooms(h.Id, rt.Code, checkIn, checkOut)  // Jeśli daty wybrane, oblicz dostępność pokoi
                            : null  // Jeśli brak dat, ustaw AvailableRooms na null
                    }).ToList()
            })
            .Where(h => h.RoomTypes.Any())  // Sprawdź, czy po filtrze są dostępne pokoje
            .ToList();
    }

    private void LoadHotelData()
    {
        var jsonFilePath = Path.Combine(_environment.WebRootPath, "json", "hotels.json");
        var jsonData = System.IO.File.ReadAllText(jsonFilePath);
        Hotels = JsonConvert.DeserializeObject<List<Hotel>>(jsonData);
    }

    private void LoadOccupancyData()
    {
        var occupancyFilePath = Path.Combine(_environment.WebRootPath, "json", "bookings.json");
        var occupancyData = System.IO.File.ReadAllText(occupancyFilePath);
        OccupanciesJson = JsonConvert.DeserializeObject<List<Bookings>>(occupancyData);
        Occupancies = OccupanciesJson.Select(o => new Bookings
        {
            HotelId = o.HotelId,
            RoomType = o.RoomType,
            RoomRate = o.RoomRate,
            Arrival = o.Arrival,
            Departure = o.Departure,
            ArrivalDateTime = (DateTime)(!string.IsNullOrEmpty(o.Arrival)
            ? DateTime.ParseExact(o.Arrival, "yyyyMMdd", null)
            : (DateTime?)null),
            DepartureDateTime = (DateTime)(!string.IsNullOrEmpty(o.Departure)
            ? DateTime.ParseExact(o.Departure, "yyyyMMdd", null)
            : (DateTime?)null)
        }).ToList();
    }

    // Metoda, która liczy wszystkie pokoje danego typu w danym hotelu
    private int GetTotalRoomsOfType(string hotelId, string roomType)
    {
        // Zakładamy, że Hotels to lista obiektów Hotel, która ma listę Rooms
        var hotel = Hotels.FirstOrDefault(h => h.Id == hotelId);
        if (hotel == null) return 0;

        return hotel.Rooms.Count(r => r.RoomType == roomType);
    }

    // Metoda sprawdzająca, czy są dostępne pokoje danego typu w wybranym zakresie dat
    private int GetAvailableRooms(string hotelId, string roomType, DateTime? checkIn, DateTime? checkOut)
    {
        if (checkIn == null || checkOut == null)
        {
            return GetTotalRoomsOfType(hotelId, roomType); // Jeśli brak dat, zwróć całą liczbę pokoi
        }

        int totalRooms = GetTotalRoomsOfType(hotelId, roomType);

        // Liczba zajętych pokoi w wybranym zakresie dat
        int occupiedRooms = Occupancies.Count(o =>
            o.HotelId == hotelId &&
            o.RoomType == roomType &&
            o.ArrivalDateTime < checkOut && // Pokój jest zajęty przed wybraną datą wyjazdu
            o.DepartureDateTime > checkIn   // Pokój jest zajęty po wybranej dacie przyjazdu
        );

        Console.WriteLine(totalRooms-occupiedRooms);

        return totalRooms - occupiedRooms;
    }

}