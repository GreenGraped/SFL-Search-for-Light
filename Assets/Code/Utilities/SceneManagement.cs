using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        // 씬 이름을 받아서 해당 씬을 로드합니다.
        SceneManager.LoadScene(sceneName);
    }

    // 씬을 비동기적으로 로드하는 함수
    public void LoadSceneAsync(string sceneName)
    {
        // 씬 이름을 받아서 비동기적으로 해당 씬을 로드합니다.
        StartCoroutine(LoadSceneAsyncCoroutine(sceneName));
    }

    private IEnumerator LoadSceneAsyncCoroutine(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // 씬이 로드될 때까지 기다립니다.
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    // 현재 씬을 리로드하는 함수
    public void ReloadCurrentScene()
    {
        // 현재 활성화된 씬을 다시 로드합니다.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 다음 씬을 로드하는 함수 (빌드 인덱스를 기반으로)
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextSceneIndex);
    }

    // 이전 씬을 로드하는 함수 (빌드 인덱스를 기반으로)
    public void LoadPreviousScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int previousSceneIndex = (currentSceneIndex - 1 + SceneManager.sceneCountInBuildSettings) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(previousSceneIndex);
    }
}
