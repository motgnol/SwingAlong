using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region GAMEMANAGER
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }
    #endregion GAMEMANAGER

    #region VARIABLES
    private GameObject player;
    private AudioSource soundEffectPlayer;
    private AudioSource backgroundMusicPlayer;

    [SerializeField] private AudioClip[] audioClips;
    public AudioClip SoundEffect {get; set;}
    private AudioClip startBackgroundClip;
    private AudioClip mainBackgroundClip;
    private AudioClip boingClip;
    private AudioClip failClip;
    private AudioClip gotBonusClip;
    private AudioClip nextLevelClip;
    private AudioClip gameOverClip;


    public Vector3 startPosition {get; set;}

    //Screen size
    public float ScreenHalfHeightWorldUnits {get; set;}
    public float ScreenHalfWidthWorldUnits {get; set;}

    public Vector3 WaterResetLocation {get; set;}

    public bool IsGrounded {get; set;}
    public bool IsInWater {get; set;}
    public bool CanClimb {get; set;}
    public bool RotatedAlready {get; set;}
    public bool HasKey {get; set;}

    public int Health {get; set;} = 10;

    //Assets
    private GameObject key;
    private Image keyAssetImage;

    //Lights
    [SerializeField] private float lerpDuration = 2f;
    private float timeElapsed;
    //private int tempTimeRef = 0;
    
    private Light2D safeSpotLight;
    private float lightIntesityLow = 0f;
    private float lightIntesityHigh = 10f;

    //UI elements
    private Image ui_BackgroundImage;
    private TMP_Text welcomeText;
    private Button startButton;
    private TMP_Text numberOfLivesText;
    private TMP_Text wellDoneText;
    private TMP_Text gameOverText;
    private Button playAgainButton;
    private Button quitButton;

    private Image joystick;
    private Image jumpButton;
    #endregion VARIABLES
    
    
    void Awake()
    {
        if(_instance !!= null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        //Get location of second water sprite
        WaterResetLocation = GameObject.FindGameObjectWithTag("River2").transform.position;

        //Get Screen size
        ScreenHalfHeightWorldUnits = Camera.main.orthographicSize;
        ScreenHalfWidthWorldUnits = ScreenHalfHeightWorldUnits * Camera.main.aspect;

    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        soundEffectPlayer = GetComponent<AudioSource>();
        soundEffectPlayer.playOnAwake = false;
        soundEffectPlayer.loop = false;

        backgroundMusicPlayer = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
        startPosition = player.transform.position;
        backgroundMusicPlayer.playOnAwake = true;
        backgroundMusicPlayer.loop = true;


        //Reference assets
        key = GameObject.FindGameObjectWithTag("Key");

        //Reference lights
        safeSpotLight = GameObject.FindGameObjectWithTag("SafeLight").GetComponent<Light2D>();
        safeSpotLight.intensity = 0f;

        //Reference UI elements
        ui_BackgroundImage = GameObject.FindGameObjectWithTag("UI_BackgroundImage").GetComponent<Image>();
        welcomeText = GameObject.FindGameObjectWithTag("WelcomeText").GetComponent<TMP_Text>();
        startButton = GameObject.FindGameObjectWithTag("StartButton").GetComponent<Button>();
        numberOfLivesText = GameObject.FindGameObjectWithTag("NumberOfLivesText").GetComponent<TMP_Text>();
        wellDoneText = GameObject.FindGameObjectWithTag("WellDoneText").GetComponent<TMP_Text>();
        keyAssetImage = GameObject.FindGameObjectWithTag("KeyAssetImage").GetComponent<Image>();
        gameOverText = GameObject.FindGameObjectWithTag("GameOverText").GetComponent<TMP_Text>();
        playAgainButton = GameObject.FindGameObjectWithTag("PlayAgainButton").GetComponent<Button>();
        quitButton = GameObject.FindGameObjectWithTag("QuitButton").GetComponent<Button>();

        joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<Image>();
        jumpButton = GameObject.FindGameObjectWithTag("JumpButton").GetComponent<Image>();

        //Deactivate all UI elements
        ui_BackgroundImage.enabled = false;
        welcomeText.enabled = false;
        startButton.gameObject.SetActive(false);
        numberOfLivesText.enabled = false;
        keyAssetImage.enabled = false;
        wellDoneText.enabled = false;
        gameOverText.enabled = false;
        playAgainButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);

        joystick.enabled = false;
        jumpButton.enabled = false;

        //Asign audio clips from audio clip array
        startBackgroundClip = audioClips[0];
        mainBackgroundClip = audioClips[1];
        boingClip = audioClips[2];
        failClip = audioClips[3];
        gotBonusClip = audioClips[4];
        nextLevelClip = audioClips[5];
        gameOverClip = audioClips[6];


        WelcomeScreen();
    }

    void Update()
    {
        UpdateAssets();
        UpdateHealth();
        FlashingLight();
        

    }

    void FixedUpdate()
    {
        Vector2 leftOffset = new Vector2((player.transform.position.x - (player.transform.localScale.x / 2)), player.transform.position.y);
        Vector2 rightOffset = new Vector2((player.transform.position.x + (player.transform.localScale.x / 2)), player.transform.position.y);

        RaycastHit2D hitLeft = Physics2D.Raycast(leftOffset, Vector2.down, 1.0f, LayerMask.GetMask("SwingingPlatforms"));
        RaycastHit2D hitRight = Physics2D.Raycast(rightOffset, Vector2.down, 1.0f, LayerMask.GetMask("SwingingPlatforms"));

        if((hitLeft.collider != null && hitLeft.collider.tag == "Ground") || (hitRight.collider != null && hitRight.collider.tag == "Ground"))
        {
            IsGrounded = true;
        }

        else
        {
            IsGrounded = false;
        }
    }

    void WelcomeScreen()
    {
        //Set up screen
        ui_BackgroundImage.enabled = true;
        ui_BackgroundImage.color = new Color(1f, 1f, 1f, 1f);
        keyAssetImage.enabled = false;
        welcomeText.enabled = true;
        wellDoneText.enabled = false;
        startButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        backgroundMusicPlayer.clip = mainBackgroundClip;
        backgroundMusicPlayer.Play();

    }

    public void GameScreen()
    {
        //Set up screen
        ui_BackgroundImage.enabled = false; 
        welcomeText.enabled = false;
        startButton.gameObject.SetActive(false);

        numberOfLivesText.enabled = true;
        numberOfLivesText.text = "LIVES :  " + Health;

        wellDoneText.enabled = false;
        gameOverText.enabled = false;
        playAgainButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(true);

        joystick.enabled = true;
        jumpButton.enabled = true;

        //Set player position
        player.transform.position = startPosition;

        //Set health
        Health = 10;

        //Set assets
        keyAssetImage.enabled = false;

    }

    void GameOverScreen()
    {
        //Set up screen
        ui_BackgroundImage.enabled = true;
        ui_BackgroundImage.color = new Color(1f, 1f, 1f, 0.8f);
        welcomeText.enabled = false;
        startButton.gameObject.SetActive(false);
        numberOfLivesText.enabled = true; 
        wellDoneText.enabled = false;
        gameOverText.enabled = true;
        playAgainButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);

        joystick.enabled = false;
        jumpButton.enabled = false;
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void UpdateAssets()
    {
        //If player has got the key
        if(HasKey)
        {
            key.SetActive(false);
            keyAssetImage.enabled = true;
            
        }

        if(IsInWater)
        {
            key.SetActive(true);
            keyAssetImage.enabled = false;
            HasKey = false;
            
        }
        
    }

    void UpdateHealth()
    {
        //Update player points
        
        numberOfLivesText.text = "LIVES :  " + Health;
        if(Health <= 0)
        {
            GameOverScreen();
        }
    }

    public void PlaySoundsEffects(string soundEffect)
    {   

        switch (soundEffect)
        {
            case "boingClip":
            
                soundEffectPlayer.clip = boingClip;
                soundEffectPlayer.Play();
                break;

            case "failClip":

                soundEffectPlayer.clip = failClip;
                soundEffectPlayer.Play();
                break;

            case "gotBonusClip":

                soundEffectPlayer.clip = gotBonusClip;
                soundEffectPlayer.Play();
                break;
            
            case "nextLevelClip":

                soundEffectPlayer.clip = nextLevelClip;
                soundEffectPlayer.Play();
                break;

            case "gameOverClip":
                soundEffectPlayer.clip = gameOverClip;
                soundEffectPlayer.Play();
                break;
        }
    } 

    //Light effects
    void FlashingLight()
    {   
        if (timeElapsed < lerpDuration)
        {
            safeSpotLight.intensity = Mathf.Lerp(lightIntesityLow, lightIntesityHigh, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
        }
        else
        {
            safeSpotLight.intensity = 0f;
            timeElapsed = 0f;
        } 
                
    }
}




    


