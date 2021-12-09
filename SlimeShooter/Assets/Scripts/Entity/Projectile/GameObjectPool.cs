    using System.Collections.Generic;
    using UnityEngine;
       
    public class GameObjectPool : MonoBehaviour {
        [SerializeField] private GameObject prefab;
        [SerializeField] private int amountToPool;
        [SerializeField] private List<GameObject> pooledObjects;
       
        private void Start() {
            pooledObjects = new List<GameObject>();
     
            for (int i = 0; i < amountToPool; i++) {
                GameObject instance = Instantiate(prefab);
                instance.SetActive(false);
                pooledObjects.Add(instance);
            }
        }
       
        public GameObject GetFreeObject() {
            for (int i = 0; i < pooledObjects.Count; i++) {
                if (!pooledObjects[i].activeInHierarchy) {
                    return pooledObjects[i];
                }
            }
            return null;
        }
    }
