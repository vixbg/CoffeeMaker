# CoffeeMaker

# Magic Coffee Machine V3

This project simulates a virtual coffee machine as a part of the interview process for a famous coffee brand.
It includes a backend API that manages coffee-making operations and a front-end MVC application for user interaction.

## Features

- Turn on/off the coffee machine.
- Make black coffee or coffee with milk.
- Refill beans and milk containers.
- Notify a cloud service when maintenance is needed.

## Getting Started

Clone the repository to your local machine using:

https://github.com/vixbg/CoffeeMaker.git

### Prerequisites

- .NET 5.0 SDK or later
- Visual Studio 2019 or later

### Running the Project

1. Open the solution in Visual Studio.
2. Set multiple startup projects to run both the backend API and the MVC project.
3. Run the solution.

## Usage

The front-end MVC application provides a simple interface with buttons to interact with the coffee machine. 
It displays messages from the backend API representing the machine's responses.

## Backend API

The backend API manages the state of the coffee machine and performs operations such as making coffee and refilling containers.

## Cloud Service

The cloud service is a separate API that receives notifications for maintenance and refilling operations.


