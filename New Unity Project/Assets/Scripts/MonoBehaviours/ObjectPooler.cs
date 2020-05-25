using UnityEngine;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour {

    // Instance for other scripts
    public static ObjectPooler objectPooler;
    // Names of projectiles to instantiate at the game start
    public List<string> objectNames;
    public List<GameObject> pooledObjects;
    // All projectiles
    public List<GameObject> objectsToPool;
    private bool objectPoolerSet = false;

    private void Awake() {
        objectPooler = this;
        SetObjectPooler();
    }
    // Instantiate the object pool of enemies
    public void SetObjectPooler() {
        pooledObjects = new List<GameObject>();
        objectNames = new List<string>();
        for(int i = 0; i < objectsToPool.Count; i++) {
            GameObject clone = Instantiate(objectsToPool[i]);
            clone.SetActive(false);
            pooledObjects.Add(clone);
        }
        for(int i = 0; i < objectsToPool.Count; i++) {
            objectNames.Add(objectsToPool[i].GetComponent<GetPooled>().name);
        }
        objectPoolerSet = true;
    }
    // Set the projectiles pool
    public void AddToObjectPooler(List<GameObject> ojectsToPool) {
        objectNames = new List<string>();
        objectsToPool.AddRange(ojectsToPool);
        for(int i = 0; i < objectsToPool.Count; i++) {
            objectNames.Add(objectsToPool[i].GetComponent<GetPooled>().name);
        }
        objectPoolerSet = true;
    }
    // Return a currently available pooled object, expand if necessary and allowed    
    public GameObject GetPooledObject(string name) {
        if(objectPoolerSet) {
            for(int i = 0; i < pooledObjects.Count; i++) {
                if(!pooledObjects[i].activeInHierarchy && pooledObjects[i].GetComponent<GetPooled>().gameObject.name.Contains(name)) {
                    if(pooledObjects[i].GetComponent<GetPooled>()) {
                        // int index = pooledObjects[i].gameObject.GetComponent<Enemy>().enemy.index;,
                        return pooledObjects[i];
                    }
                    /*
                    else if(pooledObjects[i].gameObject.GetComponent<ProjectileMovement>() != null) {
                        return pooledObjects[i];
                    }
                    */
                    else
                        return null;
                }
            }
            // Expand the pool if needed
            for(int i = 0; i < objectsToPool.Count; i++) {
                if(objectsToPool[i].GetComponent<GetPooled>().name == name) {
                    GameObject clone = Instantiate(objectsToPool[i]);
                    clone.SetActive(false);
                    pooledObjects.Add(clone);
                    return clone;
                }
            }
            return null;
        }
        return null;
    }
}
