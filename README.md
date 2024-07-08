# BasketOrderServiceApp
A scalable microservices architecture for managing online basket and order processes, utilizing .NET 6, Docker, RabbitMQ, and Redis.

## Overview
The BasketOrderServiceApp is designed to implement a portion of the e-commerce site's basket and order processing functionalities. It's structured as a set of microservices developed using .NET 6, communicating through a gateway API that interacts with other services via message queues.

## Architecture
- **API Gateway**: Serves as the single entry point for all client requests and communicates with other services through a message queue.
- **Basket Service**: Manages shopping basket operations and stores basket data in Redis.
- **Order Service**: Handles order processing and persists order information in a database.

### Services Communication
- **Add Basket**: Requests from the gateway API are sent to the Basket Service via a message queue. The Basket Service then stores this information in Redis.
- **Send Order**: The gateway API retrieves basket data from Redis and forwards it to the Order Service through a message queue for order processing.

## Technologies
- **.NET 6**: Core framework for building the microservices.
- **Redis**: Used for caching basket data.
- **RabbitMQ**: Message broker for services communication.
- **Docker**: Containerization of services.
- **ELK/Graylog/Splunk**: Logging and monitoring solutions.
- **FluentValidation**: For validating incoming requests to the services.

## Prerequisites
- Docker and Docker Compose
- .NET SDK 6.0 or higher
- An IDE like Visual Studio 2019 or later, or VS Code

## Running Tests
To conduct load testing for handling 100,000 orders at a time:
- I used **JMeter** to simulate the high traffic and observe the performance.
- Test failed due to wrong configuration of RabbitMq container at first try.
- And also failed because of Redis connection timeout configuration.


## Setup Instructions

1. **Clone the repository:**
   ```bash
   git clone https://github.com/kochallibrhm/BasketOrderServiceApp.git
   cd BasketOrderServiceApp

2. **Build and run the services using Docker Compose:**
   ```bash
   docker-compose up --build
