using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cinemachine.PostFX;
using Cinemachine.Editor;

public class Hammer : MonoBehaviour
{
    [SerializeField] private Transform cameraFollow;
    [SerializeField] private ParticleSystem dust;

    public float defaultForce = 100f, forceTimeMultFactor = 3f;
    private Rigidbody2D rb;
    private float initialX, initialCameraFollowY;
    private SpriteRenderer spriteRenderer;

    private CinemachineVirtualCamera vCam;


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
        initialCameraFollowY = cameraFollow.position.y;

        // var pos = cameraFollow.position;
        // // pos.x = initialX + 3f; //TODO Magic number
        // pos.y = initialCameraFollowY;
        // cameraFollow.position = pos;

        vCam = (CinemachineVirtualCamera)Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera;
        vCam.Follow = cameraFollow;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            Throw();
        }

    }
    private void FixedUpdate()
    {
        UpdateCameraLookAt();
    }

    private void UpdateCameraLookAt()
    {
        var pos = cameraFollow.position;
        pos.y = initialCameraFollowY;
        cameraFollow.position = pos;
    }

    private void Throw()
    {
        spriteRenderer.enabled = true;
        defaultForce += GameManager.Instance.GetRemainingTime() * forceTimeMultFactor;
        Vector2 direction = new Vector2(1f, 0.33f);
        rb.AddForce(direction * defaultForce);
        
        StartCoroutine(UpdateCameraFollowOffset(4, 0.5f));

        EventManager.Instance.TriggerEventWithGOParam(Data.Events.OnHammerThrown, gameObject);
    }

    private IEnumerator UpdateCameraFollowOffset(float newOffsetX, float speed)
    {
        var cineTransposer = vCam.GetCinemachineComponent<CinemachineTransposer>();
        var newOffset = cineTransposer.m_FollowOffset;
        newOffset.x = newOffsetX;

        while(cineTransposer.m_FollowOffset.x > newOffset.x){
            var offset = cineTransposer.m_FollowOffset;
            offset.x -= speed * Time.deltaTime;
            cineTransposer.m_FollowOffset = offset;
            yield return null;
        }
    }

    private void OnCorrectElementSelected(){
        rb.bodyType = RigidbodyType2D.Dynamic;
        Throw();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Ground")){
            rb.bodyType = RigidbodyType2D.Static;

            dust.Play();
            
            StartCoroutine(UpdateCameraFollowOffset(-3, 2f));
            Debug.Log($"Distance: {transform.position.x - initialX}");

            EventManager.Instance.TriggerEventWithGOParam(Data.Events.OnHammerHitGround, gameObject);
        }
    }
}
