# ApiTemplate
## Description
A template that can easily be cloned and modified to quickly stand-up an API.

## Components
This template consists of the following components:

* **[Api](./Api/README.md)** - The application API
* **[App](./App/README.md)** - The application business logic
* **[AppCore](./AppCore/README.md)**: The application core
* **[AppTests](./AppTests/README.md)** - Unit tests for the application
* **[Db](./Db/README.md)** - Interfaces to abstract interaction with permanent storage like a database
* **[EventBus](./EventBus/README.md)** - Infrastructure used to work with the event bus
* **[MongoDB](./MongoDb/README.md)** - Permanent storage implementation using a document data store of MongoDB

## Setup
For windows you need to have the following software installed:

* Docker
* Ubuntu (You can get this from the windows store)

Once you've installed Ubuntu you need to set it as the default [wsl](https://learn.microsoft.com/en-us/windows/wsl/) distribution.  To do that use the following command to get a list of the distributions:

```
wsl -l
```

You'll see something like this:

```
Ubuntu-20.04 (Default)
docker-desktop-data
docker-desktop
```

From there find the entry for Ubuntu, in the above example it's `Ubuntu-20.04` once you have this use the value with the `wsl -s` to set the default distribution to Ubuntu, as an example from the above:

```
wsl -s Ubuntu-20.04
```

Next you'll need to create/modify the `Api\.env` file. Create the file with the following content to start:

```
App: template-
```

The `App` value gets appended to the docker container names that are build and are required to be unique.  The value `template-` above is just used as a placeholder.

There are setups for both the [EventBus](./EventBus/README.md#setup) and the [MongoDB](./Db/README.md#setup), review both of these and complete any necessary steps.

## Running the API
To run the API you need to do the following:

1. Run the [EventBus](./EventBus/README.md#running-the-eventbus).

2. Open up Visual Studio and open the solution `Api\Api.sln`.  Under the start-up projects select `docker-compose` and press the play button. It should open up a swagger API page that you can use to run HTTP REST commands.

## Code Evaluation
Code is evaluated by both using [Unit Tests](./AppTests/README.md#unit-tests) and [Test Coverage](./AppTests/README.md#test-coverage).  See the [guidelines](./AppTests/README.md#guidelines) for details.