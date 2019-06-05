# Video Rental Store

This is the test assignment accomplishment for creation of the video rental store management system. The assignment itself can be found in the VideoRentalStore_assignment.md.

## Architecture

The application consists of four components:

* VideoStore - the core business logic,
* UI - console user interface to the application,
* Price - certain implementation of the price policy used by the VideoStore,
* Data - simple in-memory implementation of the data storage.

The core component is VideoStore, three other components are plug-ins for it. When necessary, they can be built and deployed independently.

## Components

**UI** just uses the VideoStore API presenting the simplest possible user interface in the console.

`PricePolicy` in **Price** component implements `IPricePolicy` interface residing in the **VideoStore**. This enables to easily adjust the price calculation to the changing business needs without recompiling other parts of the application.

`SimpleRepository` in the **Data** component implements `IRepository` interface residing in the **VideoStore**. This enables easy switching to some or another database rebuilding only this component.

The solution contains also the suite of unit tests in the **VideoStoreTest** project.