using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float thrustAmount = 1000.0f;
    [SerializeField] float rotationSpeed = 100.0f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem mainBooster;
    [SerializeField] ParticleSystem leftBooster;
    [SerializeField] ParticleSystem rightBooster;

    Rigidbody rb;
    AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }



    private void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            ApplyRotation(rotationSpeed);
            TriggerRightThruster();

        }
        else if (Input.GetKey(KeyCode.D))
        {
            ApplyRotation(-rotationSpeed);
            TriggerLeftThruster();
        }
        else
        {
            StopRotationParticles();
        }
    }

    private void StopRotationParticles()
    {
        rightBooster.Stop();
        leftBooster.Stop();
    }

    private void TriggerLeftThruster()
    {
        if (!leftBooster.isPlaying)
        {
            leftBooster.Play();
        }
    }

    private void TriggerRightThruster()
    {
        if (!rightBooster.isPlaying)
        {
            rightBooster.Play();
        }
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; // freezing all rotation for manual rotation
        transform.Rotate(Vector3.forward * Time.deltaTime * rotationThisFrame);
        rb.freezeRotation = false; // unfreeze after 
    }

    private void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrust();
        }
        else
        {
            StopThrust();
        }
    }

    private void StopThrust()
    {
        audioSource.Stop();
        mainBooster.Stop();
    }

    private void StartThrust()
    {
        rb.AddRelativeForce(Vector3.up * Time.deltaTime * thrustAmount);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }

        if (!mainBooster.isPlaying)
        {
            mainBooster.Play();
        }
    }
}
