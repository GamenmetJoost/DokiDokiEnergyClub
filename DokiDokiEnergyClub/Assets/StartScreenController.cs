using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenController : MonoBehaviour
{
    public void OnstartClick()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
