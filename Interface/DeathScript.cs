using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathScript : MonoBehaviour
    
{
    public PlayerMovement _playerScript;

    public void SetStart()
    {
        SceneManager.LoadScene("Level1-ElderHeist");
    }
    public void SetRestart()
    {
        SceneManager.LoadScene("Level1-ElderHeist");
    }
    public void SetMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        var MainMenu = GameObject.FindGameObjectWithTag("Menu");
        Destroy(MainMenu);
    }
    public void SetExit()
    {
        Application.Quit();
    }
    public void SetResume()
    {
        Time.timeScale = 1;
        _playerScript.PauseScreen.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}

