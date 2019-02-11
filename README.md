# AsyncInn
Deployment URL: https://asyncinn-mbg.azurewebsites.net/

Welcome to the Async Inn! This is a complete front and back end hotel managment system, using ASP.NET Core, Entity Framework, and SQL database. It allows a hotel admin to access the hotel's database to view and change data regarding hotels, rooms, room layouts, room amenities, and types of amenities.

## Database
![schema](https://github.com/mbgoseco/AsyncInn/blob/master/assets/SchemaAsyncInn.png)
- The relational database consists of five tables, three of them joined tables. The contents of which are as follows:

Hotels
- ID: Primary Key
- Name: Name of each hotel.
- Address: Address of each hotel.
- Phone: Phone number of each hotel.

Room
- ID: Primary Key
- Name: The name associated to a particular room design.
- Layout (enum List) - Types of floor plans. Can be studio, one bedroom, or two bedroom.

HotelRoom
- HotelID (Composite Key): Associates different designs of rooms to a single hotel.
- RoomNumber (Composite Key): Associates a room number to many types of room designs.
- RoomID (Foreign Key): A unique identifier linked to the ID of the Room table.
- Rate: Price per day of each room.
- Pet Friendly: A boolean flag for whether or not a room is pet friendly.

Amenities
- ID: Primary Key
- Name: The name of a room amenity.

RoomAmenities
- AmenitiesID (Composite Key): The list of amenities a particular room can have.
- RoomID (Composite Key): Reference to a particular room design and its associated amenities.

## Changes
- 1/29/2019 - Dependency Injection implemented as a middleware repository between the database and the web app for Hotels, Rooms, and Amenities. CRUD operations for HotelRooms and RoomAmenities tables fixed.
- 1/30/2019 - Search bar and total asset count added to Hotels, Rooms, and Amenities databases.
- 2/1/2019 - HotelRooms table properly displays name of each room type. Tables now display more user friendly information.
- 2/3/2019 - Deleting hotels also deletes associated hotel room entries, and deleting rooms also deletes related hotel room entries and room amenites entries.

## Directions
Users will be redirected to the hotel database home page. From there, a navigation bar directs you to the different database tables that you can access. Each link begins with a view of the entire table with options to create, edit, delete, or view details of each entry.

## Screenshots
Home
![home](https://github.com/mbgoseco/AsyncInn/blob/master/assets/home.PNG)
View
![database](https://github.com/mbgoseco/AsyncInn/blob/master/assets/database.PNG)
Create
![create](https://github.com/mbgoseco/AsyncInn/blob/master/assets/create.PNG)
Edit
![edit](https://github.com/mbgoseco/AsyncInn/blob/master/assets/edit.PNG)
Delete
![delete](https://github.com/mbgoseco/AsyncInn/blob/master/assets/delete.PNG)
