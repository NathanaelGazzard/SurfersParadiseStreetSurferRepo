using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleLoadSceneScript : MonoBehaviour
{
    [SerializeField] int sceneToLoad;
    [SerializeField] float delay;



    void Start()
    {
        Invoke("LoadScene", delay);
    }


    void LoadScene() 
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
