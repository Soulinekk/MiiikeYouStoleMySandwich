using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour {

    public static GM Instance;
    public static int cashAmount;

    void Awake() {

        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            DestroyImmediate(gameObject);
        }

    }

	void Start () {
		
	}
	
	void Update () {
		
	}
}
