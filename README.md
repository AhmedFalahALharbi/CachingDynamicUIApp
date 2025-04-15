# Caching and Dynamic UI Updates in .NET

This project demonstrates the implementation of various caching strategies in a .NET web application, with a focus on dynamic UI updates and image optimization. The application fetches data from an external API, caches it using both Redis and in-memory caching, and displays the data dynamically in the UI.

## Features

- External API integration (JSONPlaceholder)
- Distributed caching with Redis
- In-memory caching with IMemoryCache
- Dynamic UI updates with JavaScript and Fetch API
- Lazy loading and optimization of images
- Tiered caching strategy (memory → Redis → external API)

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Redis](https://redis.io/download) installed locally or access to a Redis Cloud instance (e.g., RedisLabs)
- Web browser with Developer Tools for testing and debugging

## Project Structure

```
CachingDynamicUIApp/
├── Controllers/           # API controllers for data access
├── Models/                # Data models (User, Post)
├── Services/              # Service classes for caching and API access
├── wwwroot/               # Static files for frontend
│   ├── css/               # Stylesheets
│   ├── js/                # JavaScript files
│   └── index.html         # Main HTML page
├── Program.cs             # Application entry point and configuration
└── appsettings.json       # Application settings including Redis connection
```

## Key Components

1. **Redis Caching**: Implemented using `StackExchange.Redis` library to provide distributed caching
2. **In-Memory Caching**: Using `IMemoryCache` for fast access to frequently requested data
3. **JSONPlaceholder Service**: Fetches data from the external API with caching
4. **Dynamic UI**: JavaScript-based UI that updates without page reloads
5. **Lazy Image Loading**: Images loaded only when visible in the viewport

## Setup and Run

### 1. Clone the Repository

```bash
git clone <repository-url>
cd CachingDynamicUIApp
```

### 2. Configure Redis

Update the Redis connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "Redis": "localhost:6379"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

If using Redis Cloud, replace `localhost:6379` with your Redis Cloud connection string.

### 3. Build and Run

```bash
dotnet build
dotnet run
```

The application will start and be accessible at:
- http://localhost:5234 (HTTP)

## Usage

1. **View Users**: Click the "Load Users" button to fetch and display users from the API
2. **View Posts**: Click the "Load Posts" button to fetch and display posts
3. **User Details**: Click on a user card to view detailed information about that user and their posts
4. **Observe Caching**: Notice how subsequent data loads are faster due to caching

## Cache Implementation Details

The application implements a tiered caching strategy:

1. **First Tier (IMemoryCache)**: Fastest, in-process memory cache with a short TTL (10 minutes)
2. **Second Tier (Redis)**: Distributed cache that persists across application restarts with a longer TTL (1 hour)
3. **Third Tier (External API)**: Original data source, accessed only when data is not available in caches

## Troubleshooting

### Redis Connection Issues

- Ensure Redis is running: `redis-cli ping` should return `PONG`
- Check your connection string in `appsettings.json`
- If using Redis Cloud, verify your account credentials and network access

### Build Errors

- Ensure all required NuGet packages are installed:
  ```bash
  dotnet add package StackExchange.Redis
  dotnet add package Microsoft.Extensions.Caching.Memory
  dotnet add package Newtonsoft.Json
  ```

- For nullable reference warnings, make sure model properties either:
  - Use the nullable reference type syntax (e.g., `string?`)
  - Include the `required` keyword if using C# 11+

### Runtime Issues

- Check browser console for JavaScript errors
- Verify that your API endpoints are functioning using browser DevTools or Postman
- If images don't load, check network requests and ensure paths are correct

## Future Enhancements

- Implement cache invalidation strategies
- Add paging for large data sets
- Implement real-time updates with SignalR
- Add auth/authorization for secure data access
- Implement WebP conversion for optimized images

## Performance Testing

You can observe the caching performance using browser Developer Tools:

1. Open DevTools and navigate to the Network tab
2. Load data with the "Load Users" or "Load Posts" button
3. Observe the request time
4. Click the same button again and compare the response time
5. You should see a significant performance improvement on subsequent requests

## License

[MIT License](LICENSE)

## Acknowledgments

- [JSONPlaceholder](https://jsonplaceholder.typicode.com/) for providing the free API for testing
- [StackExchange.Redis](https://github.com/StackExchange/StackExchange.Redis) for the Redis client library
