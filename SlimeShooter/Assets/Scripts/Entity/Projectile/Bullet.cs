using UnityEngine;

public class Bullet : MonoBehaviour {
	public float xDirection = 1.0f;
	[SerializeField] private float setBulletTimer = 15.0f;
	[SerializeField] private float setSpeed = 25.0f;
	[SerializeField] private LayerMask floor;
	[SerializeField] private float speed, bulletTimer;
	private Rigidbody rb;
	private Vector3 previousPosition;
	private void OnEnable() {
		speed = setSpeed;
		bulletTimer = setBulletTimer;
		
		rb = gameObject.GetComponent<Rigidbody>();
		
		rb.velocity = new Vector3(speed * xDirection, rb.velocity.y, rb.velocity.z);
		
		previousPosition = gameObject.transform.position;
	}
	private void FixedUpdate() {
		if (bulletTimer > 0) {
			bulletTimer -= Time.fixedDeltaTime;
		} else {
			gameObject.SetActive(false);
		}
		
		RaycastHit[] detected = Physics.RaycastAll(previousPosition, (transform.position - previousPosition).normalized, (transform.position - previousPosition).magnitude, floor);
		
		for(int i = 0; i < detected.Length; i++) {
			Debug.Log(detected[i].collider.gameObject.name);
		}
		Debug.DrawLine(gameObject.transform.position, previousPosition);
		
		previousPosition = gameObject.transform.position;
	}
}