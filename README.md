# Film Locations API Service 
## Overview
The **Film Locations Service** provides access to film locations in San Francisco, from [this public dataset](https://data.sfgov.org/resource/yitu-d5am.json). This API follows RESTful principles and allows fetching specific film locations in San Francisco. The application is organized into the following types of components, following a layered architecture pattern:
- **Controllers** (API layer),
- **Managers or Services** (Business logic layer),
- and **Repositories** (Data access layer).

## Endpoints
### **1. Get film location details by ID**
**Endpoint:** `GET /api/films/{id}`

Retrieves details of a specific film location by its ID.

#### **Request Parameters:**
| Parameter | Type    | In   | Required | Description |
|-----------|--------|------|----------|-------------|
| `string`      | string | path | ✅ Yes    | The unique identifier of the film location. |

#### **Response:**
- **200 OK** – Successfully retrieves the item.
- **404 Not Found** – If the item does not exist.

**Example Request:**
```sh
GET /api/films/a83fee9e-ed92-4738-95b6-0d4192016227
```

---
### **2. Search Films by Title**
**Endpoint:** `GET /api/films/search`

Searches for films based on their title.

#### **Query Parameters:**
| Parameter | Type   | In    | Required | Description |
|-----------|--------|------|----------|-------------|
| `title`   | string | query | ✅ Yes     | The title or partial title of the item. |

#### **Response:**
- **200 OK** – Returns a list of film locations matching the search criteria.

**Example Request:**
```sh
GET /api/films/search?title=Godzilla
```

## **Authentication & Rate Limits**
This API does not currently require authentication.

## **Error Handling**
The API returns standard HTTP response codes:
- **200 OK** – Request successful.
- **400 Bad Request** – Invalid request parameters.
- **404 Not Found** – The requested resource was not found.
- **500 Internal Server Error** – Unexpected server error.

## **Usage Notes**
- All responses are returned in **JSON format**.
- Query parameters are case-sensitive.