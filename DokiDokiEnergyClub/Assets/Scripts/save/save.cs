using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class save : MonoBehaviour
{
    private saveToLocal saveLocal;

    // Sleep hier je Canvas Image object in via de Inspector
    public Image saveImage;

    void Start()
    {
        saveLocal = FindFirstObjectByType<saveToLocal>();
        if (saveLocal == null)
        {
            Debug.LogWarning("saveToLocal component not found in the scene.");
        }
        if (saveImage == null)
        {
            Debug.LogWarning("saveImage (Canvas Image) is not assigned!");
        }
    }

    void Update()
    {
        if (saveLocal != null && saveImage != null && Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverUIObject(saveImage.gameObject))
            {
                StartCoroutine(SaveWithWait());
            }
        }
    }

    private IEnumerator SaveWithWait()
    {
        // Start SaveDataToJson (now also saves prefab data)
        saveLocal.SaveDataToJson();
        // No need to call PrefabSerializer separately
        // Wacht 1 frame zodat het bestand zeker geschreven is
        yield return null;
        // Daarna syncen naar database
        saveLocal.SaveAndSyncData();
        Debug.Log("Save (with wait) triggered by clicking on the save image.");
    }

    // Helper functie om te checken of de muis op het specifieke UI-object is
    private bool IsPointerOverUIObject(GameObject target)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var raycastResults = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        foreach (var result in raycastResults)
        {
            if (result.gameObject == target)
                return true;
        }
        return false;
    }
}

// ============================================================
// Gemaakt door: Miro Vaassen
// ============================================================
