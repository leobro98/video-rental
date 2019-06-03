# Video Rental Store

For a video rental store we want to create a system for managing the rental administration. We want three primary functions:

* Have an inventory of films.
* Calculate the price for rentals.
* Keep track of the customers bonus points.

## Inventory

The inventory keeps track of all films in the store. We need to be able to:

* Add a film.
* Remove a film.
* Change the type of a film.
* List all films.
* List all available films (e.g. not rented at the moment).

The store has three types of films:

* New releases
* Regular films
* Old films

## Rental

Customers can rent the films that are available (not rented). Before renting, the customers can select for how many days they want to rent the film, after which a price will be calculated based on the film type and the number of days to rent. The customer is presented with a choice to accept or reject the rental terms. Upon accepting, the rental will begin and the selected film will now not be available.

## Price calculation

The price of rentals is based on the type of film rented and how many days the film is rented for. There are two fee levels:

* Premium fee – 40€
* Regular fee – 30€

The price calculation for different types of films is the following:

* New releases – Rental price is Premium fee times number of days rented.
* Regular films – Rental price is Regular fee once for the first 3 days plus Regular fee times the number of days over 3.
* Old films – Rental price is Regular fee once for the first 5 days plus Regular fee times the number of days over 5.

When a customer has selected a film to rent, the price will be calculated and shown to the customer. For example:

```
Matrix 11 (New release) 1 days 40 EUR
```

The program should be able to list the active rentals for a certain customer and the active rentals for all customers. For example:

```
Matrix 11 (New release) 1 days 40 EUR
Spider Man (Regular rental) 5 days 90 EUR
Spider Man 2 (Regular rental) 2 days 30 EUR
Out Of Africa (Old film) 7 days 90 EUR
Total price: 250 EUR
```

## Bonus points

Customers get bonus points when renting films. A new release gives 2 points and other films give one point per rental (regardless of the time rented).

The system keeps track of how many bonus points the customer has and the customers can use their accumulated bonus points to pay for **new release** rentals. 25 points pays the rental for one day. If the customer has enough bonus points for paying for a rental period, an option to use bonus points will be presented and the customer can pay with bonus points. When paying with bonus points, the rental receipt could look like this:

```
Matrix 11 (New release) 1 days (Paid with 25 bonus points)
Remaining bonus points: 3
```

## The implementation

* Use Console Application project type.
* Create a `main` method that demonstrates the various functions (do not write a UI).
* No persistence is required. All program state is kept in memory.
* Input validation is not the focus of this exercise and should be basic. Only valid values should be accepted and invalid values should result in a no-op.
* Do output with a `Console.WriteLine()`.