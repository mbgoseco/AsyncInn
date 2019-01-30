# AsyncInn
Welcome to the Async Inn! This is a work in progress of a front and back end hotel managment system, using ASP.NET Core and Entity Framework. It allows a hotel admin to access the hotel's database to view and change data regarding hotels, rooms, room layouts, room amenities, and types of amenities.

## Database
![schema]((https://github.com/mbgoseco/AsyncInn/blob/master/assets/SchemaAsyncInn.png)
- The relational database consists of five tables, three of them joined tables. The contents of which are as follows.

Hotels
- ID: Primary Key
- Name: Name of each hotel.
- Address: Address of each hotel.
- Phone: Phone number of each hotel.
Room
-- ID: Primary Key
-- Name: The name associated to a particular room design.
-- Layout (enum List) - Types of floor plans. Can be studio, one bedroom, or two bedroom.
- HotelRoom
HotelID (Composite Key): Associates different designs of rooms to a single hotel.
RoomNumber (Composite Key): Associates a room number to many types of room designs.
RoomID (Foreign Key): A unique identifier linked to the ID of the Room table.
Rate: Price per day of each room.
Pet Friendly: A boolean flag for whether or not a room is pet friendly.
- Amenities
ID: Primary Key
Name: The name of a room amenity.
- RoomAmenities
AmenitiesID (Composite Key): The list of amenities a particular room can have.
RoomID (Composite Key): Reference to a particular room design and its associated amenities.

## Changes
- 1/29/2019 - Dependency Injection implemented as a middleware repository between the database and the web app for Hotels, Rooms, and Amenities.

## Directions
Users will be redirected to the hotel database home page. From there, a navigation bar directs you to the different database tables that you can access. Each link begins with a view of the entire table with options to create, edit, delete, or view details of each entry.

## Screenshots
Home
![home](https://github.com/mbgoseco/AsyncInn/blob/master/assets/home.PNG)
View
![database](https://github.com/mbgoseco/AsyncInn/blob/master/assets/database.PNG)
Create
![create](https://github.com/mbgoseco/AsyncInn/blob/master/assets/database.PNG)
Edit
![edit](https://github.com/mbgoseco/AsyncInn/blob/master/assets/database.PNG)
Delete
![delete](https://github.com/mbgoseco/AsyncInn/blob/master/assets/database.PNG)

