# EduHub – Student Course Management API

EduHub is a simple **Student Course Management REST API** built with ASP.NET Core Web API.

The system allows students to register, log in, view courses, and enroll in courses. Admins can manage courses.

##  Technologies

* C#
* ASP.NET Core Web API
* .NET 10
* Entity Framework Core
* SQL Server
* JWT Authentication
* Swagger / OpenAPI

##  Features

### Student

* Register and login
* View available courses
* Enroll in courses
* View enrolled courses

### Admin

* Create courses
* Update courses
* Delete courses
* View courses

##  API Endpoints

| Method | Endpoint              | Description        |
| ------ | --------------------- | ------------------ |
| POST   | `/api/Auth/SignUp`    | Register           |
| POST   | `/api/Auth/LogIn`     | Login              |
| GET    | `/api/Courses`        | Get all courses    |
| GET    | `/api/Courses/{id}`   | Get course         |
| POST   | `/api/Courses`        | Create course      |
| PUT    | `/api/Courses/{id}`   | Update course      |
| DELETE | `/api/Courses/{id}`   | Delete course      |
| POST   | `/api/Enrollments`    | Enroll in course   |
| GET    | `/api/Enrollments/my` | Get my enrollments |

