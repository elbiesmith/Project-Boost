using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float sceneLoadSpeed = 1f;
    [SerializeField] AudioClip crashAudio;
    [SerializeField] AudioClip finishAudio;

    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem finishParticles;

    Movement moveScript;
    AudioSource audioSource;

    bool isTransitioning = false;
    bool enableCollisions = true;

    private void Start()
    {
        moveScript = GetComponent<Movement>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        DebugKeys();
    }

    private void DebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            enableCollisions = !enableCollisions;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || !enableCollisions) { return; }


        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("You bumped a friendly");
                break;
            case "Finish":
                StartFinishSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }

    }

    private void StartFinishSequence()
    {
        isTransitioning = true;
        moveScript.enabled = false;
        audioSource.Stop();
        audioSource.PlayOneShot(finishAudio);
        finishParticles.Play();

        Invoke("LoadNextLevel", sceneLoadSpeed);
    }

    private void StartCrashSequence()
    {
        isTransitioning = true;
        moveScript.enabled = false;
        audioSource.Stop();
        audioSource.PlayOneShot(crashAudio);
        crashParticles.Play();

        Invoke("ReloadLevel", sceneLoadSpeed);
    }

    private void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        SceneManager.LoadScene(nextSceneIndex);
        moveScript.enabled = true;
        audioSource.Stop();
        isTransitioning = false;
    }

    private void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        moveScript.enabled = true;
        audioSource.Stop();
        isTransitioning = false;

    }
}
