# SimpleApp

This application is a simple basic CRUD app to demonstrate core CRUD (Create, Read, Update, Delete) functionalities for managing Users, Projects, and Tasks. It also implements user registration and login, along with secure, token-based authentication to protect access to project and task management features.

**Core Functionalities:**

* **User Management:**
    * Users can register new accounts and log in to receive an authentication token.
    * Basic user operations are managed through the registration and login processes.
* **Project Management:**
    * Authenticated users can create new projects.
    * Users can retrieve a list of their own projects.
    * Users can update and delete existing projects.
* **Task Management:**
    * Authenticated users can create new tasks within existing projects.
    * Users can retrieve tasks associated with a specific project.
    * Users can update and delete existing tasks.
* **Authentication:**
    * Secure, token-based authentication is implemented to protect project and task management endpoints.
    * Only logged-in users with valid tokens can perform these operations.
* **Data Persistence:**
    * A database is used to store user, project, and task data.
* **Web Interface (Optional):**
    * I didn't have time to do the UI
* **API Endpoints:**
    * API endpoints are set up for all CRUD operations and protected with authentication.

## Testing

I tested my API endpoints using Swagger UI.

1.  **Obtain a JWT:**
    * Register a new user using the `/api/auth/register` endpoint.
    * Log in using the `/api/auth/login` endpoint to obtain a JWT token.

2.  **Authorize in Swagger UI:**
    * Once you have the JWT token, use the "Authorize" button in Swagger UI to add the token to the `Authorization` header for subsequent requests.

3.  **Perform CRUD Operations:**
    * After authorization, you can use the Swagger UI to execute the authenticated CRUD operations on the Users, Projects, and Tasks endpoints.

## API Endpoints

### User Authentication

* **POST /api/auth/register:** Register a new user.
    * Request body : 
`{
  "name": "string",
  "email": "string",
  "password": "string"
}`
    * Response: User object.
* **POST /api/auth/login:** Log in and receive a JWT.
    * Request body: 
`{
  "email": "string",
  "password": "string"
}`
    * Response: JWT token string.

### Users

* **GET /api/users/{id}:** Get a user by ID (requires authentication).
* **GET /api/users:** Get all users (requires authentication).
* **POST /api/users:** Create a new user (requires authentication).
    * Request body:
  `{
  "userId": 0,
  "name": "string",
  "email": "user@example.com",
  "password": "string",
  "projects": []
}`
* **PUT /api/users/{id}:** Update a user (requires authentication).
    * Request body:
  `{
    "userId": 7,
    "name": "string",
    "email": "user@example.com",
    "password": "string",
    "projects": []
 }`
* **DELETE /api/users/{id}:** Delete a user (requires authentication).

### Projects

* **POST /api/projects:** Create a new project (requires authentication).
    * Request body:
  `{
  "projectId": 0,
  "name": "NovProekt2",
  "description": "Proektttdgbh",
  "userId": 9,
  "tasks": []
}`
* **GET /api/projects:** Retrieve a list of projects (requires authentication).
* **PUT /api/projects/{id}:** Update a project (requires authentication).
    * Request body:
  `{
  "projectId": 0,
  "name": "string",
  "description": "string"
}`
* **DELETE /api/projects/{id}:** Delete a project (requires authentication).

### Tasks

* **POST /api/tasks:** Create a new task (requires authentication).
    * Request body:
`{
  "title": "string",
  "description": "string",
  "dueDate": "2025-02-26T13:38:56.804Z",
  "projectId": 0
}`
* **GET /api/tasks/{projectId}:** Retrieve tasks for a project (requires authentication).
* **PUT /api/tasks/{id}:** Update a task (requires authentication).
    * Request body:
`{
  "taskId": 0,
  "title": "string",
  "description": "string",
  "dueDate": "2025-02-26T13:39:17.591Z",
  "projectId": 0
}`
* **DELETE /api/tasks/{id}:** Delete a task (requires authentication).


