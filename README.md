# Hotel Booking Availability Recrutation Task

This project is a web application for browsing hotel rooms and checking room availability for selected dates and hotels. It allows users to filter hotels based on name, room type, and check-in/check-out dates.

## Table of Contents
- [Project Setup](#project-setup)
  - [Requirements](#requirements)
  - [Clone the Repository](#clone-the-repository)
  - [Database Setup](#database-setup)
  - [Run the Application](#run-the-application)
- [Usage](#usage)

## Project Setup

### Requirements
- [.NET Core SDK](https://dotnet.microsoft.com/download) (version 6.0 or higher)
- [Git](https://git-scm.com/)
- A code editor ([Visual Studio](https://visualstudio.microsoft.com/))

### Clone the Repository

To start using the project, first clone it to your local machine. Open a terminal or command prompt and enter:

```bash
git clone https://github.com/stypekmaslo6/HotelBookings.git
```

This will create a local copy of the project in a folder named `HotelBookings`.

### Run the Application

Once the repository is cloned and the database is set up, you can run the application:

```bash
cd HotelBookings
dotnet run
```

The application should now be running locally. You can access it by searching `http://localhost:7293` in your browser.

## Usage

1. Go to the home page to see a list of available hotels and their rooms.
2. Use the filter form to specify hotel name, room type, and dates.
3. Click "Check Availability" to see available rooms for the selected criteria.
