using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Hammer : MonoBehaviour
{
    [SerializeField] private Transform cameraFollow;

    public float defaultForce = 100f, forceTimeMultFactor = 3f;
    private Rigidbody2D rb;
    private float initialX, initialY;
    private SpriteRenderer spriteRenderer;


    private void OnEnable(){
        EventManager.Instance.StartListening(Data.Events.OnCorrectElementSelected, OnCorrectElementSelected);
    }
    private void OnDisable(){
        EventManager.Instance.StopListening(Data.Events.OnCorrectElementSelected, OnCorrectElementSelected);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }
    void Start()
    {
        initialX = transform.position.x;
        initialY = transform.position.y;
        var camera = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera;
        camera.Follow = cameraFollow;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            Throw();
        }

        UpdateCameraLookAt();
    }

    private void UpdateCameraLookAt()
    {
        var pos = cameraFollow.position;
        pos.y = initialY;
        cameraFollow.position = pos;
    }

    private void Throw()
    {
        spriteRenderer.enabled = true;
        defaultForce += GameManager.Instance.GetRemainingTime() * forceTimeMultFactor;
        rb.AddForce(Vector2.one * defaultForce);
    }
    
    private void OnCorrectElementSelected(){
        rb.bodyType = RigidbodyType2D.Dynamic;
        Throw();
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Ground")){
            rb.velocity = Vector2.zero;
            Debug.Log($"Distance: {transform.position.x - initialX}");
        }
    }
}
