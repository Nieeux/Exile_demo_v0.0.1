using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        GameManager.Instance = this;
       
    }
    public void StartPlay()
    {
        SceneManager.LoadSceneAsync(1);
        //SceneManager.LoadScene(1);
    }
    public void Relife()
    {
        LoadingScenes.Instance.Show = true;
        UnityEngine.AI.NavMesh.RemoveAllNavMeshData();
        //Scene scene = SceneManager.GetActiveScene();
        //SceneManager.LoadScene(scene.name);
        StartCoroutine(Restarts());
    }
    public void ReturnToMenu()
    {
        Time.timeScale = 1;
        LoadingScenes.Instance.Show = true;
        UnityEngine.AI.NavMesh.RemoveAllNavMeshData();
        StartCoroutine(returnmenu());

    }
    private IEnumerator Restarts()
    {
        yield return new WaitForSeconds(1);
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);

    }
    private IEnumerator returnmenu()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(0);

    }
}
