using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;
    [SerializeField] float thrustStrength = 100f;
    [SerializeField] float rotationStrength = 50f;
    [SerializeField] AudioClip mainEngineSFX;
    [SerializeField] ParticleSystem mainEngineParticels;
    [SerializeField] ParticleSystem rightThrustParticels;
    [SerializeField] ParticleSystem leftThrustParticels;

    Rigidbody rb;
    AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }


    void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }

    void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust()
    {
        if (thrust.IsPressed() == true)
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    private void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * thrustStrength * Time.fixedDeltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngineSFX);
        }
        if (!mainEngineParticels.isPlaying)
        {
            mainEngineParticels.Play();
        }
    }

    private void StopThrusting()
    {
        audioSource.Stop();
        mainEngineParticels.Stop();
    }    

    void ProcessRotation()
    {
        float rotationInput = rotation.ReadValue<float>();
        if (rotationInput < 0)
        {
            RotateRight();
        }
        else if (rotationInput > 0)
        {
            RotateLeft();
        }
        else
        {
            StopRotating();
        }
    }

    private void RotateRight()
    {
        ApplyRotation(rotationStrength);
        if (!rightThrustParticels.isPlaying)
        {
            leftThrustParticels.Stop();
            rightThrustParticels.Play();
        }
    }

     private void RotateLeft()
    {
        ApplyRotation(-rotationStrength);
        if (!leftThrustParticels.isPlaying)
        {
            rightThrustParticels.Stop();
            leftThrustParticels.Play();
        }
    }

    private void StopRotating()
    {
        rightThrustParticels.Stop();
        leftThrustParticels.Stop();
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.fixedDeltaTime);
        rb.freezeRotation = false;
    }
}
