using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private int level;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayPause(0);
            MoveToScene(0);
        }
    }
    public void MoveToScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void PlayPause(int value)
    {
        Time.timeScale = value;
    }

    public Vector2 getRespawn()
    {
        var spawnDict = new Dictionary<int, Vector2>
        {
            { 1, new Vector2(-8.47f, -3.14f) }
        };

        return spawnDict[level];
    }
}
