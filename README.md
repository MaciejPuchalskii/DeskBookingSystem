## DeskBookingSystem
This application is a desk booking system that allows users to reserve desks in various office locations. Administrators have the ability to manage locations, desks, and reservations. The application also includes JWT-based authentication for secure access and role-based authorization for administrators.

Features:
- JWT Authentication and Authorization (Admin role management)
- Desk Management: Adding, removing, and updating desks.
- Location Management: Adding and removing locations.
- Desk Reservation: Users can reserve desks for a specified time period.
- FluentValidation for DTO validation
- Test-driven development with unit tests for service and controller logic
- Separate business logic using services and repositories

## Setup Instructions:

### 1. Clone the repository:

### 2. Navigate to the project directory:

cd ./DeskBookingSystem

### 3. Configure the database:

dotnet ef database update

### 4. Run the application

### 5. Register:
Set isAdmin = true (to test all functions)

### 6. Authorize with token:
Click authorize in Swagger and enter: "Bearer {token generated after login}"
