using UnityEngine;
using UnityEngine.UI;

public class MapSelect : MonoBehaviour
{
    public void Map1()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Map1");
    }
    
    public void Map2()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Map2");
    }

    public void Map3()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Map3");
    }
}
