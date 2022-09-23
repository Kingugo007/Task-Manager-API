# Task Manager API

Task Manager API is a RESTful API built with ASP.NET Core web API, and I used mockapi.io to save my data. The task manager provides access to can be integrated into other applications that require the task manager's services.

## Allowed HTTP requests
```
* Get: Get a list of tasks
* Delete: To delete the resource
* put: update resources
* post: update resource
```

## Description Of Usual Server Responses
```
200 OK - the request was successful (some API calls may return 201 instead).
201 Created - the request was successful and a resource was created.
204 No Content - the request was successful but there is no representation to return (i.e. the response is empty).
400 Bad Request - the request could not be understood or was missing the required parameters.
401 Unauthorized - authentication failed or user doesn't have permissions for the requested operation.
403 Forbidden - access denied.
404 Not Found - resource was not found.
405 Method Not Allowed - requested method is not supported for resources.
```
# REST API
## Get a List of things
```
Request
GET /tasks/
Accept: 'application/json' https://localhost:7130/api/tasks/
```
this Get request by default returns 10 tasks, this is made possible
by adding pagination to make sure responses are easier to handle.
```
[
    {
        "startDate": "2043-01-29T23:47:17.119Z",
        "dueDate": "2169-07-04T23:47:17.119Z",
        "endDate": "2164-12-26T23:47:17.119Z",
        "daysOverDue": 6,
        "daysLate": 0,
        "taskName": "zMNQfc>HiK",
        "taskDescription": "Explicabo minus animi ipsa nobis ipsam ex voluptatem quis.\nExcepturi aut sint ipsum et libero rerum minima.\nDolor vero aut vel doloremque voluptatem repudiandae incidunt non.\nNihil quas ipsam consequuntur commodi id et quia voluptate aut.",
        "allottedTimeInDays": 46177,
        "elapsedTimeInDays": 44526,
        "taskStatus": false
    }
]
```
## Get a specific task
```
Get https://localhost:7130/api/tasks/id
This request gets a specific task using the task id

Response
{
    "startDate": "2022-09-22T03:31:58.1526577+01:00",
    "dueDate": "2022-10-07T03:31:58.1526577+01:00",
    "endDate": "2022-10-02T03:31:58.1526577+01:00",
    "daysOverDue": 0,
    "daysLate": 5,
    "taskName": "DTo",
    "taskDescription": "Just testing creatDTo",
    "allottedTimeInDays": 15,
    "elapsedTimeInDays": 10,
    "taskStatus": true
}
```
## Create a task
POST /tasks/
```
Post end point
HTTP Request: DELETE https://localhost:7130/api/tasks/id
```
```
## Response sample
HTTP/1.1 201 Created
Status: 201 Created
Connection: close
Content-Type: application/json
````
## Delete a specific task
This request deletes a specific task matching the route parameter
```
HTTP Request: DELETE https://localhost:7130/api/tasks/id
```

# Reference
### Read more about pagination [Pagination](https://developer.atlassian.com/server/confluence/pagination-in-the-rest-api/)
### Task Model
```
* string taskName 
* string taskDescription
* DateTime startDate 
* int allottedTimeInDays
* int elapsedTimeInDays
* bool taskStatus 
* string taskId 
```
