using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lvl_End : MonoBehaviour
{
    //this script is main script for loading levels and other scripts are accessing static variables from here

    public GameObject Blue_bg; //blue background that appears when portal is activated
    public GameObject Portal_Unlocked; //[portal unlocked] ui that appears on screen when portal is unlocked
    public AudioClip Lvl_Finished; //sound that appears when portal is unlocked
    private AudioSource Source;
    [SerializeField] private Collider2D Col; //collider not allowing player to enter teleport, its part of stone teleport at the end of level
    [SerializeField] private Collider2D Teleport; //trigger for teleportation
    static public bool Level_Passed = false;
    bool Sound_Play = true;

    public Animator transition; //black fade animation
    public float transitionTime = 1f; //black fade animation

    public int nextSceneLoad; //prefab that stores last level you passed
    static public int Num_Of_Con = 0;//number of fulfilled conditions (buttons activated)
    static public int Num_Of_Con_Needed;//number of conditions needed (buttons activated)

    static public int Num_Of_Mon = 0;//number of monsters you killed
    static public int Num_Of_Mon_Needed;//number of monsters you have to kill

    private void Start()
    {
        nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1;
        Source = GetComponent<AudioSource>();

        //parameters for each level (there are currently 6 levels in Paxo Adventures but you can add here as much as you like)

        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            Num_Of_Con_Needed = 1;
            Num_Of_Mon_Needed = 0;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            Num_Of_Con_Needed = 2;
            Num_Of_Mon_Needed = 1;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            Num_Of_Con_Needed = 3;
            Num_Of_Mon_Needed = 2;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            Num_Of_Con_Needed = 3;
            Num_Of_Mon_Needed = 1;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            Num_Of_Con_Needed = 2;
            Num_Of_Mon_Needed = 5;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 6) //last level in this game
        {
            Num_Of_Con_Needed = 2;
            Num_Of_Mon_Needed = 0;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 7)
        {
            Num_Of_Con_Needed = 2;
            Num_Of_Mon_Needed = 0;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 8)
        {
            Num_Of_Con_Needed = 2;
            Num_Of_Mon_Needed = 0;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 9)
        {
            Num_Of_Con_Needed = 2;
            Num_Of_Mon_Needed = 0;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 10)
        {
            Num_Of_Con_Needed = 2;
            Num_Of_Mon_Needed = 0;
        }
    }


    void Update()
    {
        if (Num_Of_Con >= Num_Of_Con_Needed && Num_Of_Mon >= Num_Of_Mon_Needed) //what happens when teleport is unlocked
        {
            
            Level_Passed = true;
            Blue_bg.SetActive(true);
            Col.enabled = false;
            Teleport.enabled = true;
            Portal_Unlocked.SetActive(true);
            if (Sound_Play == true)
            {
                Sound_Play = false;
                Source.PlayOneShot(Lvl_Finished);
            }

        }
        else
        {
            Level_Passed = false;
            Blue_bg.SetActive(false);
            Col.enabled = true;
            Teleport.enabled = false;
            Portal_Unlocked.SetActive(false);
            Sound_Play = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D teleporter) //what happens when players enter portal (enters trigger collider in portal)
    {
        if (teleporter.gameObject.tag == "Player" && Blue_bg.activeSelf == true) //second condition check if portal is unlocked
        {

            LoadNextLevel();
                if (nextSceneLoad > PlayerPrefs.GetInt("levelAt"))
                {
                    PlayerPrefs.SetInt("levelAt", nextSceneLoad); //sets prefab to next level
                }
            Num_Of_Con = 0;
            Num_Of_Mon = 0;
            Weapon.ammo1 = 30; //player ammo (from another script)
            Weapon.ammo2 = 20;
            Weapon.ammo3 = 10;

        }
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(nextSceneLoad));
    }

    IEnumerator LoadLevel (int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
}
