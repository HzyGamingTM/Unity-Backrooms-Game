using Kino;
using UnityEngine;

public class Player : MonoBehaviour {
    public static Player shared = null;
	public static Vector3 velocity;
	private float yVelocity;

	public Quaternion rotation, cameraRotation;
	public Camera playerCamera;
    public float cameraRotZ;
    public Glitch glitchManager;

    public CapsuleCollider theCollider;
    public CharacterController controller;
    public AudioSource audioSource;

	public Transform playerTransform, cameraTransform;

	public Transform groundCheck;
	public LayerMask groundMask;

    public float speed, stamina;
    public float walkSpeed = 2f, sprintSpeed = 4f;
	private const float gravity = -9.81f, jumpHeight = 3f;

	private float xrot, yrot, mouseSens = 3f;
	private float MIN_FOV = 60f, MAX_FOV = 70f;

    public bool shouldClip = true;

	void Start() {
        shared = this;
		playerTransform = GetComponent<Transform>();
        glitchManager = new Glitch(playerCamera.GetComponent<AnalogGlitch>(), playerCamera.GetComponent<DigitalGlitch>());
        stamina = 7f;
        PlayerStats.Reset();

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        // Camera Movement
        yrot += Input.GetAxis("Mouse X") * mouseSens;
        xrot -= Input.GetAxis("Mouse Y") * mouseSens;
        xrot = Mathf.Clamp(xrot, -89.99f, 89.99f);
        
        rotation = Quaternion.Euler(0, yrot, 0);
        cameraRotation = Quaternion.Euler(xrot, yrot, cameraRotZ);
        
        playerTransform.localRotation = rotation;
        cameraTransform.rotation = cameraRotation;

        // Movement
        velocity = speed * (
            Input.GetAxis("Horizontal") * playerTransform.right +
            Input.GetAxis("Vertical") * playerTransform.forward
        );
        velocity.y += yVelocity;

        // Footsteps
        if (velocity.x != 0 || velocity.z != 0) {
            audioSource.enabled = true;
        } else audioSource.enabled = false;
        
        if (shouldClip) controller.Move(Time.deltaTime * velocity);
        else playerTransform.position += Time.deltaTime * velocity;

        // Gravity
        if (Physics.CheckSphere(groundCheck.position, 0.1f, groundMask) && shouldClip) {
            if (Input.GetKeyDown(KeyCode.Space) && stamina >= 2) {
                yVelocity = jumpHeight;
                stamina -= 1.5f;
            }
            else yVelocity = 0f;
        } else yVelocity += gravity * Time.deltaTime;
        
        // Camera Effects
        HandleSprint();
        glitchManager.Update();
    }

    float HandleSprint_fov;
    float sprintCooldown;
	void HandleSprint() {
        
        if (Input.GetKeyUp(KeyCode.LeftShift)) sprintCooldown = 0f;
        if (sprintCooldown < 3) sprintCooldown += Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftShift) && sprintCooldown > 0.25f) {
            if (stamina <= 0) {
                speed = walkSpeed;
				WalkFov();
			} else {
                stamina -= Time.deltaTime;
                speed = sprintSpeed;
                HeadBob.amplitude = 0.12f;
                HeadBob.frequency = 0.8f;
                SprintFov();
            }
        } else {
			if (stamina <= 7) stamina += 1.3f * Time.deltaTime;
            HeadBob.amplitude = 0.10f;
            HeadBob.frequency = 0.6f;
            speed = walkSpeed;
            WalkFov();
        }
        
        if (stamina < 2) LevelManager.Blur();
        else LevelManager.Unblur();
    }
    void WalkFov() {
        HandleSprint_fov -= 4 * Time.deltaTime;
        if (HandleSprint_fov < 0f) HandleSprint_fov = 0f;
        playerCamera.fieldOfView = Mathf.Lerp(  
            MIN_FOV, MAX_FOV, HandleSprint_fov * HandleSprint_fov
        );
    }
	void SprintFov() {
        HandleSprint_fov += 4 * Time.deltaTime;
        if (HandleSprint_fov > 1f) HandleSprint_fov = 1f;
        playerCamera.fieldOfView = Mathf.Lerp(
            MIN_FOV, MAX_FOV, HandleSprint_fov * HandleSprint_fov
        );
    }
}


public class Glitch {
    public bool glitch;

    float glitchTime = 0f;
    AnalogGlitch analogGlitch;
    DigitalGlitch digitalGlitch;

    public Glitch(AnalogGlitch a, DigitalGlitch d) {
        analogGlitch = a;
        digitalGlitch = d;
    }

    public void Update() {
        if (glitch && glitchTime < 3) {
            glitchTime += Time.deltaTime;
            analogGlitch.enabled = true;
            digitalGlitch.enabled = true;
        } else {
            glitch = false;
            glitchTime = 0f;
            analogGlitch.enabled = false;
            digitalGlitch.enabled = false; ;
        }
    }
}

public static class PlayerStats {
    public static float health;
    public static float thirst;
    public static float hunger;
    public static float sanity;

    public static void Reset() {
        health = 100f; thirst = 100f; hunger = 100f; sanity = 100f;
    }
}