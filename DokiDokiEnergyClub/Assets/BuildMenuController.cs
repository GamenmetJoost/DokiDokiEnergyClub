using UnityEngine;
using UnityEngine.UI;

public class BuildMenuController : MonoBehaviour
{
    public GameObject buildMenu; // Assign the "buildMenu" panel in the Inspector
    public Button openMenuButton; // Assign the main button in the Inspector

    void Start()
    {
        // Ensure build menu is hidden at start
        if (buildMenu != null)
            buildMenu.SetActive(false);

        // Add listener for the button
        if (openMenuButton != null)
            openMenuButton.onClick.AddListener(ToggleBuildMenu);
    }


    void ToggleBuildMenu()
    {
        if (buildMenu != null)
            buildMenu.SetActive(!buildMenu.activeSelf); // Toggle menu visibility
    }
}