using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    public Animator Transition;
    [SerializeField] private float _transitionTime;

   

   
    public void LoadGameScene()
    {

        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
       
    }

    public void LoadMenuScene()
    {
        
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 1));
        
    }
    IEnumerator LoadLevel(int levelIndex)
    {

        Transition.SetTrigger("Start");

        yield return new WaitForSeconds(_transitionTime);

        SceneManager.LoadScene(levelIndex);
        
    }

    public void RestartGame()
    {

        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }
}
