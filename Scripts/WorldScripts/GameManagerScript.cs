using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


// defined a wishlist item object. 
public class WishlistItem
{
    public string itemName;
    public int itemCost;
    public bool isPurchased;
    public WishlistItem(string _name, int _cost)
    {
        itemName = _name;
        itemCost = _cost;
        if (PlayerPrefs.GetInt(_name, 0) == 0)
        {

            isPurchased = false;
        }
        else
        {
            isPurchased = false;
        }
    }
}

public class GameManagerScript : MonoBehaviour
{

    //UI
    [SerializeField] GameObject fakeLoadScreen;

    [SerializeField] GameObject menuBackDropUI;

    [SerializeField] GameObject gameplayUI;
    [SerializeField] GameObject mainMenuUI;
    [SerializeField] GameObject difficultyMenuUI;
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject creditScreen;

    [SerializeField] GameObject confirmExitUI;//exit the program option
    [SerializeField] GameObject confirmQuitUI;//quit to menu option
    [SerializeField] GameObject confirmStartNewGame;

    [SerializeField] GameObject playerRef;
    [SerializeField] GameObject flyover; // the seagul that flys over the scene

    // Mission UI
    [SerializeField] GameObject missionUI;
    [SerializeField] GameObject interactionUI;


    bool isPlaying = false;
    bool isPaused = false;


    WishlistItem[] wishlistItems = new WishlistItem[8];
    [SerializeField] TextMeshProUGUI[] wishlistButtonLabels = new TextMeshProUGUI[12];
    WishlistItem currentObjective;

    [SerializeField] Text goalProgressText;
    int goalCost;
    int currentCash = 0;




    // Start is called before the first frame update
    void Start()
    {
        fakeLoadScreen.SetActive(true);

        missionUI.SetActive(false);
        interactionUI.SetActive(false);

        InitWishlist();
        playerRef.SetActive(false);
        flyover.SetActive(true);
        mainMenuUI.SetActive(true);
        menuBackDropUI.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;

        for (int i = 0; i < wishlistButtonLabels.Length; i++)
        {
            wishlistButtonLabels[i].text = wishlistItems[i].itemName + " - $" + wishlistItems[i].itemCost + ".00";
        }

        Invoke("DisableFakeLoadScreen", 1f);
    }

    void DisableFakeLoadScreen()
    {
        fakeLoadScreen.SetActive(false);
    }


    void InitWishlist()
    {
        // SUPER IMPORTANT: THE SAVE FUNCTIONALITY WILL NOT WORK UNLESS ALL THESE ITEMS HAVE UNIQUE NAMES
        wishlistItems[0] = new WishlistItem("Yacht", 15000);
        wishlistItems[1] = new WishlistItem("Supercar", 12000);
        wishlistItems[2] = new WishlistItem("Camper Van", 8000);
        wishlistItems[3] = new WishlistItem("Jet Ski", 7000);
        wishlistItems[4] = new WishlistItem("Artwork", 6000);
        wishlistItems[5] = new WishlistItem("Spa", 5000);
        wishlistItems[6] = new WishlistItem("TV", 3000);
        wishlistItems[7] = new WishlistItem("Guitar", 2000);
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


    //All the following functions are to be called by buttons in the menus.

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


    public void ConfirmStartNewGame()
    {
        confirmStartNewGame.SetActive(false);
        PlayerPrefs.DeleteAll();
        Continue();
    }


    public void BeginGame(int itemRefNumber)
    {
        // >>> set some gameplay vars from the relevent wishlist item

        menuBackDropUI.SetActive(false);
        difficultyMenuUI.SetActive(false);
        gameplayUI.SetActive(true);

        flyover.SetActive(false);
        playerRef.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;

        isPlaying = true;
        currentObjective = wishlistItems[itemRefNumber];
        goalCost = currentObjective.itemCost;
        ReceiveMoney(0);
        MissionGeneration ms = GameObject.FindGameObjectWithTag("Player").GetComponent<MissionGeneration>();

        ms.GenMissions();
    }


    public void CreditScreen()
    {
        mainMenuUI.SetActive(false);
        creditScreen.SetActive(true);
    }


    public void BackToMainMenu()
    {
        creditScreen.SetActive(false);
        confirmExitUI.SetActive(false);
        confirmQuitUI.SetActive(false);
        confirmStartNewGame.SetActive(false);
        difficultyMenuUI.SetActive(false);

        mainMenuUI.SetActive(true);
    }


    public void PauseGame()
    {
        gameplayUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        menuBackDropUI.SetActive(true);
        isPaused = true;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
    }


    public void ResumeGame()
    {
        playerRef.GetComponent<CharacterController>().enabled = true;
        pauseMenuUI.SetActive(false);
        menuBackDropUI.SetActive(false);
        gameplayUI.SetActive(true);
        isPaused = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }









    public void QueryQuitGame()
    {
        pauseMenuUI.SetActive(false);
        confirmQuitUI.SetActive(true);
    }

    public void ConfirmQuitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);//reloads the scene
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




    public void ReceiveMoney(int amountReceived)
    {
        currentCash += amountReceived;
        goalProgressText.text = "$" + currentCash.ToString() + " / $" + goalCost.ToString();
        if (currentCash >= goalCost)
        {
            PlayerWon();
        }
    }


    void PlayerWon()
    {
        //uhm, activate the win sequence
        print("Player won!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
    }
}
