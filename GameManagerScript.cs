using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class WishlistItem
{
    public string itemName;
    public int itemCost;
    public WishlistItem(string _name, int _cost)
    {
        itemName = _name;
        itemCost = _cost;
    }
}

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] GameObject menuBackDropUI;

    [SerializeField] GameObject gameplayUI;
    [SerializeField] GameObject mainMenuUI;
    [SerializeField] GameObject difficultyMenuUI;
    [SerializeField] GameObject pauseMenuUI;

    [SerializeField] GameObject confirmExitUI;//exit the program option
    [SerializeField] GameObject confirmQuitUI;//quit to menu option
    [SerializeField] GameObject confirmStartNewGame;

    [SerializeField] GameObject character;
    [SerializeField] GameObject flyover;


    bool isPlaying = false;
    bool isPaused = false;


    WishlistItem[] wishlistItems = new WishlistItem[12];
    [SerializeField] TextMeshProUGUI[] wishlistButtonLabels = new TextMeshProUGUI[12];
    WishlistItem currentObjective;



    // Start is called before the first frame update
    void Start()
    {
        InitWishlist();
        character.SetActive(false);
        flyover.SetActive(true);
        mainMenuUI.SetActive(true);
        menuBackDropUI.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;

        for (int i = 0; i < wishlistButtonLabels.Length; i++)
        {
            wishlistButtonLabels[i].text = wishlistItems[i].itemName + " - $" + wishlistItems[i].itemCost + ".00";
        }
    }

    void InitWishlist()
    {
        wishlistItems[0] = new WishlistItem("NASSICA Yacht", 3206000);
        wishlistItems[1] = new WishlistItem("Lamborghini Countach", 2600000);
        wishlistItems[2] = new WishlistItem("Something Expensive", 2400000);
        wishlistItems[3] = new WishlistItem("Something Expensive", 1000000);
        wishlistItems[4] = new WishlistItem("Something Expensive", 750000);
        wishlistItems[5] = new WishlistItem("Something Expensive", 500000);
        wishlistItems[6] = new WishlistItem("Something Expensive", 200000);
        wishlistItems[7] = new WishlistItem("Something Expensive", 100000);
        wishlistItems[8] = new WishlistItem("Something Expensive", 50000);
        wishlistItems[9] = new WishlistItem("Something Expensive", 20000);
        wishlistItems[10] = new WishlistItem("Something Expensive", 10000);
        wishlistItems[11] = new WishlistItem("Something Expensive", 5000);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPlaying)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }










    void Wasted()
    {
        //play deathcam sequence

        Invoke("ConfirmQuitGame", 5f);//set this delay to whatever is suitable to allow the deathcam sequence
    }








    //>>>>>>>>>>>>>>>>>> All the following functions are to be called by buttons in the menus.
    
    public void Continue()
    {
        mainMenuUI.SetActive(false);
        difficultyMenuUI.SetActive(true);
    }


    public void QueryStartNewGame()
    {
        mainMenuUI.SetActive(false);
        confirmStartNewGame.SetActive(true);
    }


    public void CancelStartNewGame()
    {
        mainMenuUI.SetActive(true);
        confirmStartNewGame.SetActive(false);
    }


    public void ConfirmStartNewGame()
    {
        confirmStartNewGame.SetActive(false);
        PlayerPrefs.DeleteAll();
        Continue();
    }


    public void BeginGame(int itemRefNumber)
    {
        //set some gameplay vars from the params
        menuBackDropUI.SetActive(false);
        difficultyMenuUI.SetActive(false);
        gameplayUI.SetActive(true);

        flyover.SetActive(false);
        character.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;

        isPlaying = true;

        currentObjective = wishlistItems[itemRefNumber];
    }


    public void BackToMainMenu()
    {
        difficultyMenuUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }





    public void PauseGame()
    {
        character.GetComponent<CharacterController>().enabled = false;
        gameplayUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        menuBackDropUI.SetActive(true);
        isPaused = true;
        //disable cam controller
        Cursor.lockState = CursorLockMode.Confined;
    }



    public void ResumeGame()
    {
        print("unpause");
        character.GetComponent<CharacterController>().enabled = true;
        pauseMenuUI.SetActive(false);
        menuBackDropUI.SetActive(false);
        gameplayUI.SetActive(true);
        isPaused = false;
        //re-enable cam controller
        Cursor.lockState = CursorLockMode.Locked;
    }







    public void QueryQuitGame()
    {
        pauseMenuUI.SetActive(false);
        confirmQuitUI.SetActive(true);
    }

    public void ConfirmQuitGame()
    {
        SceneManager.LoadScene(0);//reloads the scene
    }


    public void CancelQuitGame()
    {
        confirmQuitUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }







    public void QueryExitProgram()
    {
        mainMenuUI.SetActive(false);
        confirmExitUI.SetActive(true);
    }


    public void ConfirmExitProgram()
    {
        Application.Quit();
    }

    public void CancelExitProgram()
    {
        mainMenuUI.SetActive(true);
        confirmExitUI.SetActive(false);
    }

}
