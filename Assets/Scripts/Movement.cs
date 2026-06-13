using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotate;
    [SerializeField] float thrustStrength = 100f;
    [SerializeField] float roatationStrength = 100f;
    [SerializeField] AudioClip mainEngine;
    
    [SerializeField] ParticleSystem mainBoosterParticle;
    [SerializeField] ParticleSystem leftBoosterParticle;
    [SerializeField] ParticleSystem rightBoosterParticle;

    Rigidbody rb;
    AudioSource audioSource;
    

    private void Start() {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable(){
        thrust.Enable();
        rotate.Enable();
    }

    private void Update()
    {
        processThrust();
        processRotation();
    }

    private void processThrust()
    {
        if(thrust.IsPressed())
        {
        StartThrusting();
        } 
        else
        {
            StopThrusting();
        }
    
    }
    void StartThrusting()
    {
        
        rb.AddRelativeForce(Vector3.up * thrustStrength * Time.fixedDeltaTime);
        if(!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        if(!mainBoosterParticle.isPlaying)
        {
            mainBoosterParticle.Play();    
        }
    }
    void StopThrusting()
    {
        audioSource.Stop();
        mainBoosterParticle.Stop();
    }
    void RotateRight()
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * Time.fixedDeltaTime * roatationStrength);
        if(!rightBoosterParticle.isPlaying)
        {
            leftBoosterParticle.Stop();
            rightBoosterParticle.Play();
        }
        rb.freezeRotation = false;
    }
    void RotateLeft()
    {
        rb.freezeRotation = true;
            transform.Rotate(Vector3.back * Time.fixedDeltaTime * roatationStrength);
            rb.freezeRotation = false;
            if(!leftBoosterParticle.isPlaying)
            {
                rightBoosterParticle.Stop();
                leftBoosterParticle.Play();
            }
    }
  void StopRotating()
  {
        rightBoosterParticle.Stop();
        leftBoosterParticle.Stop();
  }  
    
    private void processRotation()
    {
        if(rotate.ReadValue<float>() < 0)
        {
            RotateRight();
        }
        else if(rotate.ReadValue<float>() > 0)
        {
            RotateLeft();
        }
        else
        {
            StopRotating();
        }
    }

    
    
}
