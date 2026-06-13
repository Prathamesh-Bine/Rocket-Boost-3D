using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip sucessSFX;
    [SerializeField] AudioClip crashSFX;

    [SerializeField] ParticleSystem sucessParticle;
    [SerializeField] ParticleSystem crashParticle;


    AudioSource audioSource;

    bool isControllabled = true;
    bool isCollidable = true;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    private void Update()
    {
        RespondToDebugKeys();
    }


    void RespondToDebugKeys()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            NextLevel();
        }
        else if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            isCollidable = !isCollidable;
        }
    }



    private void OnCollisionEnter(Collision other)
    {
        if (!isControllabled || !isCollidable)
        {
            return;
        }


        switch (other.gameObject.tag)
        {

            case "Friendly":

                Debug.Log("Friendly collision detected.");
                break;


            case "Finish":

                StartSucessSequence();
                break;


            default:

                StartCrashSequence();
                break;
        }

    }



    void StartCrashSequence()
    {
        isControllabled = false;

        audioSource.Stop();
        audioSource.PlayOneShot(crashSFX);

        crashParticle.Play();

        GetComponent<Movement>().enabled = false;


        Invoke("ReloadLevel", levelLoadDelay);
    }



    void StartSucessSequence()
    {
        isControllabled = false;


        audioSource.Stop();
        audioSource.PlayOneShot(sucessSFX);

        sucessParticle.Play();


        GetComponent<Movement>().enabled = false;


        // NEW CODE
        UnlockNewLevel();


        Invoke("NextLevel", levelLoadDelay);

    }



    void UnlockNewLevel()
    {

        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;


        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);


        if(currentLevelIndex >= unlockedLevel)
        {

            PlayerPrefs.SetInt(
                "UnlockedLevel",
                currentLevelIndex + 1
            );


            PlayerPrefs.Save();


            Debug.Log("Unlocked Level: " + (currentLevelIndex + 1));

        }

    }



    private void ReloadLevel()
    {

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentSceneIndex);

    }



    private void NextLevel()
    {

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;


        currentSceneIndex++;


        if(currentSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            currentSceneIndex = 0;
        }


        SceneManager.LoadScene(currentSceneIndex);

    }

}