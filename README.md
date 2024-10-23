# (WORK IN PROGRESS) EsamiOnline

[![.NET Build Status](https://github.com/umbertocicciaa/EsamiOnline/actions/workflows/ci.yml/badge.svg)](https://github.com/umbertocicciaa/EsamiOnline/actions/workflows/ci.yml)

A project for managing online exams using C# gRPC and Docker.

## Table of Contents

- [Introduction](#introduction)
- [Technologies](#technologies)
- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
  
## Introduction

EsamiOnline is a distributed software system designed for managing online exams at a telematic university. The system facilitates online exams conducted at a designated lab, ensuring strict identification and security protocols. It includes both client and server components with a minimal GUI for ease of use.

## Technologies
- C#: The primary programming language used for development.
- .NET: The framework used for building and running the application.
- MongoDB: The database used for storing data.
- gRPC: Used for communication between client and server.
- xUnit: The testing framework used for unit tests.
- AutoMapper: A library used for object-to-object mapping.
- Docker: Used for containerization of the application.

## Features

1. View upcoming exam dates.
2. Book an exam slot (providing student ID and tax code).
3. Participate in an exam:
   - Students answer 10 questions prepared by the teacher, with multiple-choice answers.
   - Each question must be answered within 5 minutes, scoring 3 points for correct answers, 0 for incorrect, and -1 for unanswered.
   - At the end of the exam, students receive a report with correct answers and their scores.

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
