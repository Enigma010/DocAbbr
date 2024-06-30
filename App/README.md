# App
Contains the core business logic of the application and objects that allow external components to interact with the core business logic. Placing application business logic at this level means that it can be invoked by any method: API, console application, batch job, unit test, etc.

## Guidelines
The following describes guidelines for the architecture.

### Commands
* Represent a command or action happening to an entity and it's corresponding data. As an example cancelling an order in the system would have a command class defined as:

```
public class CancelOrderCmd {
    public Guid Id { get; set; }
    public string Reasone { get; set; }
}
```
* Represent correlated data to be passed into service or entity methods.
* Comprised of a verb then a noun and the suffix `Cmd` to represent a command.
* Used to more easily pass data rather than having a bunch of separate parameters passed in individually.

### Entities
* Entities should comprise the data as well as methods for safely changing the state of the data of the entity.
* Methods on entities should correspond to business actions that occur to the entity.  As an example for an order entity there might be a method called Cancel() that is invoked when an order is cancelled.
* All methods should adhere to [idempotence](https://en.wikipedia.org/wiki/Idempotence). While not important for the persistance of the data, this will become important when events are triggered from methods.
* Properties should allow read access to data but all data changes should be done through a method.  This allows for the safe guarding of the object state and allows multiple properties to be changes in consistant manners.
* Adhere to keeping the number of entities in a single API to a minimum.  The best practice is to have a single aggregate, or root, entity that your API deals with. Keeping the entities to a minimum will keep your projects smaller and more concise and avoid monolithic applications.
* Entities should keep all the business logic related internal state within their defintions.

### Events
* Events represent changes to entities that have occurred through a command.
* Events are emitted to signal changes throughout the system.
* Event defintions are a contract with other microservices. Contracts should be respected and only should be broken as a last resort. Breaking contracts requires a high degree of coordination across multiple microservices to not break the entire system.

### Repositories
* Keep the access patterns data storage agnostic. This will allow an easier transition between data stores.
* Repositories should implement paging.

### Services
* Services encapsulate interacting with the repository and entities.
* Services can interact with external systems.
* Services should include business logic that crosses entities or systems.  