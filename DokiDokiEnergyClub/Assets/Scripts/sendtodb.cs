using UnityEngine;
using MongoDB.Driver;

public class sendtodb
{
    private MongoClient client;
    private IMongoDatabase database;

    public sendtodb()
    {
        // Replace with your MongoDB connection string and database name
        string connectionString = "your-mongodb-connection-string";
        string databaseName = "your-database-name";

        client = new MongoClient(connectionString);
        database = client.GetDatabase(databaseName);
    }

    public void SendData<T>(string collectionName, T data)
    {
        var collection = database.GetCollection<T>(collectionName);
        collection.InsertOne(data);
        Debug.Log("Data sent to MongoDB successfully.");
    }
}
