# USDB - User Storage Database

A distributed storage system that leverages multiple computers' RAM and filesystem storage via TCP communication. USDB allows you to store and retrieve data across a network of machines, automatically routing data between memory and disk storage based on size.

## Overview

USDB consists of two main components:

- **CNC (Command & Control)**: A REST API server that provides the interface for storing and retrieving objects
- **UserService**: Runs on storage nodes, managing local RAM and filesystem storage

The system uses TCP communication with compression (Zstd) and JSON serialization for efficient data transfer.

## Architecture

```text
Client → CNC (REST API) → UserService (TCP) → Storage (RAM/Filesystem)
```

## Quick Start

### 1. Start CNC Server

```bash
cd CNC
dotnet run
```

The CNC server will start on the configured port with Swagger UI available in development mode.

### 2. Start UserService on Storage Nodes

```bash
cd UserService
# Configure config.json with your settings
dotnet run
```

### 3. Store Data

```bash
# Add an object
curl -X POST "http://localhost:5114/Api/Storage/{username}" \
  -H "Content-Type: application/octet-stream" \
  --data-binary @your-file
```

### 4. Retrieve Data

```bash
# Get an object
curl "http://localhost:5114/Api/Storage/{username}/{object-id}" \
  --output retrieved-file
```

### 5. Delete Data

```bash
# Delete an object
curl -X DELETE "http://localhost:5114/Api/Storage/{username}/{object-id}"
```

## Configuration

### CNC (appsettings.json)

```json
{
  "Compression": {
    "Level": 6
  }
}
```

### UserService (config.json)

```json
{
  "Network": {
    "Port": 6969,
    "CNC": {
      "Port": 5114,
      "Address": "localhost"
    }
  },
  "DB": {
    "Directory": "/path/to/storage",
    "Limit": 2048,
    "MemoryStorageLimit": 1024
  },
  "Identity": {
    "Name": "NodeName"
  }
}
```

## Features

- **Automatic Storage Routing**: Large objects (>2.5MB) stored on disk, smaller ones in RAM
- **Compression**: Zstd compression for efficient network transfer
- **REST API**: Simple HTTP interface for client applications
- **Distributed**: Multiple UserService nodes can be managed by one CNC
- **Fallback Storage**: Automatic fallback between memory and disk storage