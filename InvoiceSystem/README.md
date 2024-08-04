Invoice System API

## Overview

The Invoice System API is a RESTful API designed to manage invoices, including creating invoices, processing payments, and handling overdue invoices. The application is built using .NET Core and is containerized using Docker.

## Features

- **Create Invoice**: Add new invoices with specific amounts and due dates.
- **Pay Invoice**: Process payments against invoices.
- **Process Overdue Invoices**: Automatically manage overdue invoices, including adding late fees and updating invoice status.

## Assumptions

- The user has Docker and Docker Compose installed on their machine.
- The API is used in a development environment, not production.
- The due_date field in invoice creation is expected to be in YYYY-MM-DD format.
- The API expects payments to be made in full, without handling partial payments (unless explicitly defined in the functionality).
- Overdue processing assumes that invoices with a due_date earlier than the current date plus the overdue_days specified in the request are considered overdue.

## Added Functionality

- **Partial Payments**: The API supports partial payments, where users can make payments less than the total amount due, and the system will track the remaining balance.
- **Invoice History**: Each invoice tracks a history of payments and status changes, allowing users to see the lifecycle of an invoice.
- **Custom Late Fee Calculation**: The overdue invoice processing allows for custom late fee calculations based on the overdue period.
- **Swagger UI**: Integrated Swagger UI for easier API testing and documentation.
- **Dockerization**: The application is fully Dockerized, allowing for easy setup and deployment using Docker Compose.

## Prerequisites

Before you begin, ensure you have the following installed on your machine:

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Git](https://git-scm.com/)

## Setup Instructions

### 1. Clone the Repository

Clone the repository to your local machine using Git:
git clone https://github.com/navya/invoice-system.git
cd invoice-system
### 2. Build and Run Locally

#### Running with .NET SDK

If you want to run the project without Docker:

1. Navigate to the project directory:
cd InvoiceSystem
2. Restore the dependencies:
dotnet restore

3. Build the project:
dotnet build

4. Run the application:

dotnet run

5. Open your browser and navigate to:

- http://localhost:8080 for HTTP
- https://localhost:8081 for HTTPS

#### Running with Docker

If you prefer to use Docker to run the application:

1. Build the Docker image:
docker-compose build

2. Run the Docker container:
docker-compose up

3. Access the application:

- Open your browser and navigate to http://localhost:8080 for HTTP or https://localhost:8081 for HTTPS.

## API Endpoints

### 1. Create an Invoice

- **URL**: /api/invoices
- **Method**: POST
- **Request Body**:
{
"amount": 199.99,
"due_date": "2024-09-11"
}

- **Response**:

{
"id": "1234"
}

### 2. Get All Invoices

- **URL**: /api/invoices
- **Method**: GET
- **Response**:

### 3. Pay an Invoice

- **URL**: /api/invoices/{id}/payments
- **Method**: POST
- **Request Body**:

{
"amount": 159.99
}


- **Response**: 200 OK

### 4. Process Overdue Invoices

- **URL**: /api/invoices/process-overdue
- **Method**: POST
- **Request Body**:

{
"late_fee": 10.5,
"overdue_days": 10
}
- **Response**: 200 OK

## Running Tests

To run the unit tests for this project:

1. Navigate to the test project directory:

cd InvoiceSystemAPI.Tests

2. Run the tests:

dotnet test

or

Do Test -> Run all Tests in the Visual Studio.
Make sure that all the tests are passed.


## Docker Setup

The project is containerized using Docker. Below are the details on how to set up and run the project using Docker:

### Dockerfile

The Dockerfile is set up to build the application, publish it, and run it in a container. The relevant ports are exposed to allow access to the API.

### Docker Compose

The docker-compose.yml file is used to manage the Docker containers and services. It includes configuration for building and running the application.

