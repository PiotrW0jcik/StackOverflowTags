# StackOverflowTags

This project provides a REST API built with .NET 8 and C# to manage and serve a list of popular tags from the Stack Overflow API. The API allows users to fetch tags, calculate their percentage share in the Stack Overflow ecosystem, and sort them by name or percentage. Additionally, it provides functionality for refreshing the list of tags from the Stack Overflow API.

## Features

- Fetch and manage popular tags from the Stack Overflow API.
- Calculate the percentage share of tags in the total count.
- Paginated API with sorting options (by name and percentage).
- API for forcing a refresh of the tags from Stack Overflow.
- Dockerized solution for easy deployment.
- Continuous Integration setup with GitHub Actions.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet)
- [Docker](https://www.docker.com/get-started)
- [Redis](https://redis.io/)

## Docker Setup

To build and run the application in Docker, follow these steps:

1. **Build the Docker image:**

   ```bash
   docker-compose build

   
2. **Start the services (tags_service, redis):**
	```bash
   docker-compose up

The application will be accessible at http://localhost:5001

## API Endpoints

### 1. `GET /api/tags`

Fetches a paginated list of tags sorted by the specified criteria.

#### Query Parameters:

- `page`: Page number (default: 1).
- `pageSize`: Number of tags per page (default: 10).
- `sortBy`: Sorting criteria (`name` or `percentage`, default: `name`).
- `descending`: Boolean to sort in descending order (default: `false`).

### 2. `POST /api/tags/refresh`

Forces the refresh of tags by fetching new data from the Stack Overflow API.
