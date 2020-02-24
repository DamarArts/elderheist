using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public PlayerMovement PlayerScript;
    //public Patrol GuardMovement;
    public float GuardAccel, GuardSpeed, GuardStop, fireRate, MinDistance;
    public bool Hard, Easy, Medium;
    public GameObject Canvas;
    public MainMenu MenuScript;

    private void Start()
    {
        MenuScript = GameObject.FindGameObjectWithTag("Menu").GetComponent<MainMenu>();
        //SceneManager.LoadSceneAsync("Level1-ElderHeist");
        gameObject.SetActive(true);
        Time.timeScale = 1;

    }

    public void SetHard()
    {
        SceneManager.LoadScene("Level1-ElderHeist");
        Hard = true;
        MenuScript.Hard = true;
        //GuardMovement.SetHard();

       DontDestroyOnLoad(this.gameObject);
       Canvas.SetActive(false);
        
    }
    public void SetMedium()
    {
        SceneManager.LoadScene("Level1-ElderHeist");
        Medium = true;
        MenuScript.Medium = true;
        //GuardMovement.SetMedium();

        DontDestroyOnLoad(this.gameObject);
        Canvas.SetActive(false);


    }
    public void SetEasy()
    {
        SceneManager.LoadScene("Level1-ElderHeist");
        Easy = true;
        MenuScript.Easy = true;
        //GuardMovement.SetEasy();

        DontDestroyOnLoad(this.gameObject);
        Canvas.SetActive(false);

    }
    public void SetExit()
    {
        Application.Quit();
    }

}
