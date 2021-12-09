using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
	private float xDirection = 1, speed = 500.0f, jumped = 0.0f, jumpForce = 2000.0f, lowerBound = -25, getFireWeaponCooldown;
	private bool fireWeapon = false, onFloor;
	private Vector3 movement;
	private Rigidbody rb;
	private PlayerInput controls;
	[SerializeField] private LayerMask floor;
	[SerializeField] private float fireWeaponCooldown = 0.5f;
	
	private RaycastHit hit;
	
	private void OnEnable() {
		controls.Player.Enable();
	}
	private void OnDisable() {
		controls.Player.Disable();
	}
	private void Awake() {
		controls = new PlayerInput();
		
		controls.Player.Jump.performed += context => jumped = context.ReadValue<float>();
		
		controls.Player.Move.performed += context => movement = context.ReadValue<Vector2>();
		controls.Player.Move.canceled += context => movement = Vector2.zero;
		
		controls.Player.Fire.performed += context => fireWeapon = true;
		controls.Player.Fire.canceled += context => fireWeapon = false;
	}
	private void Start() {
		rb = GetComponent<Rigidbody>();
		
		getFireWeaponCooldown = fireWeaponCooldown;
		
		xDirection = 1;
	}
	private void FixedUpdate() {
		Movement();
		Jump();
		Shoot();
		OutOfBounds();
	}
	private void Movement() {
		rb.velocity = new Vector3 (movement.x * speed * Time.fixedDeltaTime, rb.velocity.y, 0);

		if (movement.x != 0) {
			xDirection = movement.normalized.x;
		}
	}
	private void Jump() {
		onFloor = Physics.BoxCast(transform.position, new Vector3(0.5f, 0.0f, 0.5f), Vector3.down, out hit, transform.rotation, 1.0f, floor);
		Debug.Log(onFloor);
		
		if (jumped != 0.0f && onFloor && rb.velocity.y == 0f) {
			rb.AddForce(0, jumpForce, 0);
		}
	}
	
	void OnDrawGizmos()
    {
        //Check if there has been a hit yet
        if (onFloor)
        {
        	Gizmos.color = Color.green;
            //Draw a Ray forward from GameObject toward the hit
            Gizmos.DrawRay(transform.position, Vector3.down * hit.distance);
            //Draw a cube that extends to where the hit exists
            Gizmos.DrawWireCube(transform.position + Vector3.down * hit.distance, new Vector3(0.5f, 0.0f, 0.5f));
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
        	Gizmos.color = Color.red;
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(transform.position, Vector3.down * 1.0f);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(transform.position + Vector3.down * 1.0f, new Vector3(0.5f, 0.0f, 0.5f));
        }
    }
	
	private void Shoot() {
		bool shoot = false;
		if (getFireWeaponCooldown > 0.0f) {
			getFireWeaponCooldown -= Time.fixedDeltaTime;
		} else if (getFireWeaponCooldown <= 0.0f && fireWeapon) {
			shoot = true;
			getFireWeaponCooldown = fireWeaponCooldown;
		}
		if (shoot == true) {
			GameObject bullet = GetComponent<GameObjectPool>().GetFreeObject();
			if (bullet) {
				bullet.transform.position = transform.position;
				bullet.GetComponent<Bullet>().xDirection = xDirection;
				bullet.SetActive(true);
			}
		}
	}
	private void OutOfBounds() { // Resets player position when below world boundary.
		if (gameObject.transform.position.y < lowerBound) {
			gameObject.transform.position = Vector3.zero;
		}
	}
}