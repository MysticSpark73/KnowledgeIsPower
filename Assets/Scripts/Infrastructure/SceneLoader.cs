using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public class SceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;

        public SceneLoader(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void LoadScene(string sceneName, Action onLoaded = null) => 
            _coroutineRunner.StartCoroutine(LoadSceneRoutine(sceneName, onLoaded));

        private IEnumerator LoadSceneRoutine(string sceneName, Action onLoaded = null)
        {
            if (SceneManager.GetActiveScene().name == sceneName)
            {
                onLoaded?.Invoke(); 
                yield break;
            }
            
            AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(sceneName);

            while (loadSceneOperation != null && !loadSceneOperation.isDone)
            {
                yield return null;
            }
            
            onLoaded?.Invoke();
        }
    }
}