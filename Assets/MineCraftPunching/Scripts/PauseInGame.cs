using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseInGame : MonoBehaviour {
    [SerializeField]
    private GameObject sliderAireForce;
   
    [SerializeField]
    private GameObject menuPauseObject;
    [SerializeField]
    private KeyCode keyMenuPause= KeyCode.Escape;
    [SerializeField]
    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController firstPersonController;

    [HideInInspector]
    public bool isActive;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(isActive == true)
        {
            menuPauseObject.SetActive(true);
            sliderAireForce.SetActive(false);
           
            firstPersonController.m_MouseLook.CursorIsLocked = false;
            firstPersonController.m_MouseLook.UpdateCursorLock();

            Time.timeScale = 0;
        }
        else
        {
            menuPauseObject.SetActive(false);
            sliderAireForce.SetActive(true);
           
            firstPersonController.m_MouseLook.CursorIsLocked = true;
            firstPersonController.RotateView();
            

            Time.timeScale = 1;
        }
	
        if(Input.GetKeyDown(keyMenuPause))
        {
            ResumeButton();
        }

	}

    public void ResumeButton()
    {
        isActive = !isActive;
    }
    public void ReloadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
