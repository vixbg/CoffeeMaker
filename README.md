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

- .NET 8.0 SDK or later
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

## Actual Assignment

You have to emulate a magic coffee machine considering the following requirements.
The machine can produce two types of coffee, black coffee and coffee with milk.
The coffee container can have beans for 10 drinks and the milk container can have
milk for 5 drinks. (We assume that the water is always available and you do not need
to worry about it).
Before the coffee goes to the drink it has to be grinded.
For the coffee with milk, the coffee has to go in the cup first then the milk.
To produce the desired drink, the machine has to be turned on the heater
has to warm up (2 sec), has to have coffee beans and milk in the containers.
When the desired drink is produced the machine indicates it is ready and is in
standby mode. If no other drink is selected for more than 5 sec, the heater cools off
and the machine has to warm up again before producing another drink.
When the coffee container is empty the machine cannot produce any drinks and
signals to the cloud that it needs maintenance.
If the milk container is empty but the coffee container is not then only coffee drinks
could be made and the machine should send a signal to the cloud that it needs milk.
Components
• Machine: on, standby and off
• Grinder: grinds the beans before
• Heater: heats the machine for 2 sec
• Beans container: beans for 10 drinks
• Milk container: milk for 5 drinks
• Cloud: where the machine sends messages
Hints
Machine: Feel free to create/represent the machine and its workings and components as you wish. You
can do it as a console application, web application, web API application, or whatever you feel comfortable
with.
Cloud: You can choose any representation of the cloud. It could be a separate web API app (or whatever
you choose). The machine can communicate with it with API requests/responses.
