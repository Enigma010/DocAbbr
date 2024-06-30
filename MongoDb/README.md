# MongoDB
Db implementation using MongoDB.  MongoDB was choosen for the ease of development.  Since MongoDB is a document database no formal database definion is required.  This means that tables and columns can be added as simple as adding new classes and properties on the classes in the source code.

## Guidelines
The following describes guidelines for the architecture.

* This provides a contract between the repository and data storage to support changing data storage options. Don't allow repository definitions direct access to the specific data storage option.