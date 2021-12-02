using System.Collections.Generic;
using UnityEngine;

public class BulletObjectPool : MonoBehaviour {
	public List<GameObject> pooledBullets;
	public GameObject bulletToPool;
	public int amountToPool;
	
	private void Start() {
		pooledBullets = new List<GameObject>();
		GameObject tmp;
		for (int i = 0; i < amountToPool; i++) {
			tmp = Instantiate(bulletToPool);
			tmp.SetActive(false);
			pooledBullets.Add(tmp);
		}
	}
	
	public GameObject GetFreeBullet() {
		for (int i = 0; i < amountToPool; i++) {
			if (!pooledBullets[i].activeInHierarchy) {
				return pooledBullets[i];
			}
		}
		return null;
	}
}
