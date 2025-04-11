using UnityEngine;

public class testsend : MonoBehaviour
{
    private Sendtodb sendToDb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Zoek de Sendtodb-component in de scene
        sendToDb = FindFirstObjectByType<Sendtodb>();
        if (sendToDb == null)
        {
            Debug.LogError("Sendtodb component not found in the scene!");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Called when the user clicks on the GameObject's collider
    void OnMouseDown()
    {
        Debug.Log("Sprite clicked!");

        if (sendToDb != null)
        {
            // Voorbeelddata die je wilt verzenden
            var data = new { message = "Hello, database!" };
            sendToDb.SendData("testCollection", data);
        }
    }
}
