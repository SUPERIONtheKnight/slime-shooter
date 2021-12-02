using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
	public short xDirection = 1;
	private float speed = 500.0f, jumped = 0.0f, jumpForce = 2000.0f, lowerBound = -25, getFireWeaponCooldown;
	private bool fireWeapon = false, onFloor, onFloor1, onFloor2, onFloor3, onFloor4;
	public Vector3 floorDetectPosition1, floorDetectPosition2, floorDetectPosition3, floorDetectPosition4;
	private Vector3 movement;
	private Rigidbody rb;
	private PlayerInput controls;
	[SerializeField] private LayerMask floor;
	[SerializeField] private float fireWeaponCooldown = 0.5f;
	
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
	}
	private void FixedUpdate() {
		Movement();
		Jump();
		Shoot();
		OutOfBounds();
	}
	private void Movement() { // Player Movement.
		rb.velocity = new Vector3 (movement.x * speed * Time.fixedDeltaTime, rb.velocity.y, 0);
		floorDetectPosition1 = transform.TransformPoint(-0.5f, 0.0f, 0.0f);
		floorDetectPosition2 = transform.TransformPoint(0.5f, -0.5f, 0.0f);
		floorDetectPosition3 = new Vector3(floorDetectPosition1.x, floorDetectPosition2.y, floorDetectPosition1.z);
		floorDetectPosition4 = new Vector3(floorDetectPosition2.x, floorDetectPosition1.y, floorDetectPosition1.z);

		if (movement.x < 0) {
			xDirection = -1;
		} else if (movement.x > 0) {
			xDirection = 1;
		}
	}
	private void Jump() { // Makes the player jump.
		onFloor1 = Physics.Linecast(floorDetectPosition1, floorDetectPosition3, floor);
		onFloor2 = Physics.Linecast(floorDetectPosition1, floorDetectPosition4, floor);
		onFloor3 = Physics.Linecast(floorDetectPosition2, floorDetectPosition3, floor);
		onFloor4 = Physics.Linecast(floorDetectPosition2, floorDetectPosition4, floor);
		
		if (onFloor1 || onFloor2 || onFloor3 || onFloor4) {onFloor = true;}
		else {onFloor = false;}
		
		if (jumped != 0.0f && onFloor && rb.velocity.y == 0f) {
			rb.AddForce(0, jumpForce, 0);
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
			GameObject bullet = GetComponent<BulletObjectPool>().GetFreeBullet();
			if (bullet) {
				bullet.transform.position = transform.position;
				bullet.SetActive(true);
				bullet.GetComponent<Bullet>().speed *= GetComponent<PlayerController>().xDirection;
			}
		}
	}
	private void OutOfBounds() { // Resets player position when below world boundary.
		if (gameObject.transform.position.y < lowerBound) {
			gameObject.transform.position = Vector3.zero;
		}
	}
}