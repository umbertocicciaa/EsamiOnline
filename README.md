# EsamiOnline

[![.NET Build Status](https://github.com/umbertocicciaa/EsamiOnline/actions/workflows/ci.yml/badge.svg)](https://github.com/umbertocicciaa/EsamiOnline/actions/workflows/ci.yml)

A project for managing online exams using C# gRPC and Docker.

## Table of Contents

- [Installation](#installation)
- [Usage](#usage)

## Installation

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/get-started)

### Steps

1. Clone the repository:
   ```sh
   git clone https://github.com/umbertocicciaa/EsamiOnline.git
   cd EsamiOnline
   ```
2. Build the Docker image:
   ```sh
   docker build -t esamionline .
   ```
3. Run the Docker container:
   ```sh
   docker run -d -p 8080:80 esamionline
   ```

## Usage

1. Access the application at `http://localhost:8080`.
2. Follow the instructions on the screen to manage your online exams.


---
