using System;
using System.Collections;
using System.Collections.Generic;
using Scenes.MainGameWorld.Scripts;
using UnityEngine;

public class LightGlobalSun : MonoBehaviour
{
    // Global Components
    private GameObject _worldEventManagerGameObject;
    private WorldEventManager _worldEventManager;
    
    private Transform _transform;

    private void Awake()
    {
        _worldEventManagerGameObject = GameObject.Find("WorldEventManager");
        _worldEventManager = _worldEventManagerGameObject.GetComponent<WorldEventManager>();
        
        _transform = GetComponent<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        _transform.Rotate(Vector3.right * 0.001f);
    }
}
