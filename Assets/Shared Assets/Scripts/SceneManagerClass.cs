using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Puzzle
{
    public class SceneManagerClass : MonoSingleton<SceneManagerClass>
    {
        Scene scene;

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }

        private void Start()
        {
            scene = SceneManager.GetActiveScene();
        }

        public void LoadSceneByNumber(int index)
        {
            SceneManager.LoadScene(index);
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetSceneByName(scene.name).buildIndex);
        }
    }
}
