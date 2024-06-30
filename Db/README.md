# Db
Data storage or database interfaces.  Note that for now the data storage was implemented to support the ease of development and thus wasn't unit tested.  As we have more ideas on how we want this to work this can be expanded or abandoned.

### Setup
The settings for MongoDB can be found in the `Api\appsettings.Development.json` under the `Db` node. No additional configuration is necessary to run the MongoDB with the default settings.

Next you'll need to create/modify the `Api\.env` file. Create the file with the following content to start:

```
MONGO_USERNAME: username
MONGO_PASSWORD: password
```

Change the `username` value to be the same value in the `Api\appsettings.Development.json` `Db` node, sub-node `Username` value.  

Change the `password` value to be the same value in the `Api\appsettings.Development.json` `Db` node, sub-node `Password` value.

Next you will also need to generate a replication key file so that MongoDB can handle rolling back transactions.  To do this open up a **wsl** command window and change directories to `MongoDb` from there run the command:

```
./lrun.sh
```

The output of this command is a file `MongoDb\Iac\Output\replica.key` this will be used when you start the docker containers for the API.

## Guidelines
The following describes guidelines for the architecture.

* This provides a contract between the repository and data storage to support changing data storage options. Don't allow repository definitions direct access to the specific data storage option.