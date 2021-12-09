using UnityEngine;

public class Enemy : MonoBehaviour {
	[SerializeField] private float setBulletCooldown = 1.0f;
	public float speed = 2.0f, guardBoundary = 4.0f, maxFollowRange = 10.0f;
	private float bulletCooldown;
	public short xDirection = 1;
	private bool followPlayer = false;
	private Vector3 guardPosition;
	private Rigidbody enemy;
	private Enemy otherEnemy;
	private PlayerController player;

	private void OnEnable() {
		player = FindObjectOfType<PlayerController>();
		enemy = GetComponent<Rigidbody>();
		otherEnemy = FindObjectOfType<Enemy>();

		Physics.IgnoreCollision(GetComponent<BoxCollider>(), player.GetComponent<BoxCollider>());
		Physics.IgnoreCollision(GetComponent<BoxCollider>(), otherEnemy.GetComponent<BoxCollider>());
		guardPosition = gameObject.transform.position;
	}
	private void Start() {
		bulletCooldown = setBulletCooldown;
	}
	private void FixedUpdate() {
		if (followPlayer) {
			FindPlayer();
			Shoot();
		} else {
			Eyesight();
			PatrolMovement();
		}
	}
	private void Eyesight() {
		float distanceToPlayer = enemy.transform.position.x - player.transform.position.x;
		if (distanceToPlayer < 0) distanceToPlayer *= -1;
		
		if (enemy.transform.position.x <= player.transform.position.x && distanceToPlayer <= maxFollowRange && xDirection > 0 ||
			enemy.transform.position.x >= player.transform.position.x && distanceToPlayer <= maxFollowRange && xDirection < 0) {
			followPlayer = true;
		}
	}
	private void PatrolMovement() {
		if (enemy.transform.position.x > guardPosition.x + guardBoundary && xDirection > 0) {
			xDirection *= -1;
		} else if (enemy.transform.position.x < guardPosition.x - guardBoundary && xDirection < 0) {
			xDirection *= -1;
		}
		enemy.velocity = new Vector3(xDirection * speed, enemy.velocity.y, enemy.velocity.z);
	}
	private void FindPlayer() {
		float distanceToPlayer = enemy.transform.position.x - player.transform.position.x;
		
		if (distanceToPlayer > 0.1) {
			xDirection = -1;
			enemy.velocity = new Vector3(xDirection * speed, enemy.velocity.y, enemy.velocity.z);
		} else if (distanceToPlayer < -0.1) {
			xDirection = 1;
			enemy.velocity = new Vector3(xDirection * speed, enemy.velocity.y, enemy.velocity.z);
		}
		
		if (distanceToPlayer < 0) distanceToPlayer *= -1;
		if (distanceToPlayer >= maxFollowRange) followPlayer = false;
	}
	private void Shoot() {
		//bool shoot = false;
		//if (bulletCooldown > 0.0f) {
		//	bulletCooldown -= Time.fixedDeltaTime;
		//} else {
		//	shoot = true;
		//	bulletCooldown = setBulletCooldown;
		//}
		//if (shoot == true) {
		//	GameObject bullet = GetComponent<BulletObjectPool>().GetFreeBullet();
		//	if (bullet) {
		//		bullet.transform.position = transform.position;
		//		bullet.SetActive(true);
		//		bullet.GetComponent<Bullet>().speed *= GetComponent<Enemy>().xDirection;
		//	}
		//}
	}
}