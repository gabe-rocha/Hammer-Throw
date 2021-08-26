using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cinemachine.PostFX;
using Cinemachine.Editor;
using TMPro;

public class Hammer : MonoBehaviour
{
    [SerializeField] private Transform cameraFollow;
    [SerializeField] private ParticleSystem dust;
    [SerializeField] private GameObject spriteDistance;
    [SerializeField] private TextMeshPro textDistance;
    [SerializeField] private GameObject hammerModel;
    

    public float defaultForce = 100f, forceTimeMultFactor = 3f;
    private Rigidbody2D rb;
    private float initialX, initialCameraFollowY;
    private SpriteRenderer spriteRenderer;

    private CinemachineVirtualCamera vCam;
    private Coroutine updateOffsetCor;


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
        spriteDistance.SetActive(false);
        hammerModel.SetActive(false);
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
        #if UNITY_EDITOR
            if(Input.GetKeyDown(KeyCode.Alpha1)){
                Throw();
            }
        #endif

        textDistance.text = GameManager.Instance.GetDistance();
        
        // transform.rotation.SetLookRotation(rb.velocity);

        Vector2 v = rb.velocity;
        var angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        hammerModel.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }
    private void FixedUpdate()
    {
        FixedCameraLookAtYPosition();
    }

    private void FixedCameraLookAtYPosition()
    {
        var pos = cameraFollow.position;
        pos.y = initialCameraFollowY;
        cameraFollow.position = pos;
    }

    private void Throw()
    {
        spriteRenderer.enabled = true;
        hammerModel.SetActive(true);
        // defaultForce += GameManager.Instance.GetRemainingTime() * forceTimeMultFactor;
        var force = GameManager.Instance.GetRemainingTime() * forceTimeMultFactor * 15f;
        Vector2 direction = new Vector2(1f, 0.33f);
        rb.AddForce(direction * force);
        
        // updateOffsetCor = StartCoroutine(UpdateCameraFollowOffset(-8, 2f));

        // StartCoroutine(UpdateCameraFollowOffset());

        EventManager.Instance.TriggerEventWithGOParam(Data.Events.OnHammerThrown, gameObject);
    }
    private IEnumerator UpdateCameraFollowOffset()
    {
        float speed = 2f;
        var cineTransposer = vCam.GetCinemachineComponent<CinemachineTransposer>();
        var newOffset = cineTransposer.m_FollowOffset;
        
        while(newOffset.x > 0){
            newOffset.x -= speed * Time.deltaTime;
            cineTransposer.m_FollowOffset = newOffset;
            yield return null;
        }
    }

    // private IEnumerator UpdateCameraFollowOffset(float newOffsetX, float speed)
    // {
    //     var cineTransposer = vCam.GetCinemachineComponent<CinemachineTransposer>();
    //     var newOffset = cineTransposer.m_FollowOffset;
    //     newOffset.x = newOffsetX;

    //     if(newOffsetX <= 0){
    //         while(cineTransposer.m_FollowOffset.x > newOffset.x){
    //             var offset = cineTransposer.m_FollowOffset;
    //             offset.x -= speed * Time.deltaTime;
    //             cineTransposer.m_FollowOffset = offset;
    //             yield return null;
    //         }
    //     }
    //     else{
    //         while(cineTransposer.m_FollowOffset.x < newOffset.x){
    //             var offset = cineTransposer.m_FollowOffset;
    //             offset.x += speed * Time.deltaTime;
    //             cineTransposer.m_FollowOffset = offset;
    //             yield return null;
    //         }
    //     }
    // }

    private void OnCorrectElementSelected(){
        rb.bodyType = RigidbodyType2D.Dynamic;
        Throw();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Ground")){
            // rb.bodyType = RigidbodyType2D.Static;

            spriteDistance.transform.parent = null;
            spriteDistance.SetActive(true);

            var vel = rb.velocity;
            vel.x /= 2f;
            vel.y = vel.y > 0.1f ? vel.y / 1.5f : 0f;
            rb.velocity = vel;
            
            // var dist = transform.position.x - initialX;
            // rb.mass = dist / 5f;

            dust.Play();
            
            StartCoroutine(UpdateCameraFollowOffsetWithBounce(0, 3f));
            // var cineTransposer = vCam.GetCinemachineComponent<CinemachineTransposer>();
            // cineTransposer.m_XDamping = 15f;
            // var newOffset = cineTransposer.m_FollowOffset;
            // newOffset.x = 0f;
            // cineTransposer.m_FollowOffset = newOffset;

            // StartCoroutine(UpdateCameraFollowOffset());

            // if(updateOffsetCor != null){
            //     StopCoroutine(updateOffsetCor);
            // }
            // var cineTransposer = vCam.GetCinemachineComponent<CinemachineTransposer>();
            // var newOffset = cineTransposer.m_FollowOffset;
            // newOffset.x = 0;
            // cineTransposer.m_FollowOffset = newOffset;

            Debug.Log($"Distance: {transform.position.x - initialX}");

            

            EventManager.Instance.TriggerEventWithGOParam(Data.Events.OnHammerHitGround, gameObject);
        }
    }

        

    private IEnumerator UpdateCameraFollowOffsetWithBounce(float newOffsetX, float speed)
    {
        if(updateOffsetCor != null){
            StopCoroutine(updateOffsetCor);
        }
        // vCam.Follow = null;
        var cineTransposer = vCam.GetCinemachineComponent<CinemachineTransposer>();
        var newOffset = cineTransposer.m_FollowOffset;
        // newOffset.x = newOffsetX;

        // int numBounces = 4;
        var bounciness = .25f;

        while(newOffset.x > newOffsetX ){
            newOffset.x -= speed * Time.deltaTime;
            cineTransposer.m_FollowOffset = newOffset;
            yield return null;
        }
        // while(newOffset.x < newOffsetX + bounciness / 2){
        //     newOffset.x += speed / 3 * Time.deltaTime;
        //     cineTransposer.m_FollowOffset = newOffset;
        //     yield return null;
        // }
        // while(newOffset.x > newOffsetX){
        //     newOffset.x -= speed / 6 * Time.deltaTime;
        //     cineTransposer.m_FollowOffset = newOffset;
        //     yield return null;
        // }
        

        // var movingLeft = transform.position.x < vCam.transform.position.x - 10;

        // int bounceCount = 0;
        // while(bounceCount < numBounces){
        //     if(movingLeft){
        //         while(cineTransposer.m_FollowOffset.x > newOffset.x - bounciness){
        //             var offset = cineTransposer.m_FollowOffset;
        //             offset.x -= speed * Time.deltaTime;
        //             cineTransposer.m_FollowOffset = offset;
        //             yield return null;
        //         }
        //     }
        //     else{
        //         while(cineTransposer.m_FollowOffset.x > newOffset.x + bounciness){
        //             var offset = cineTransposer.m_FollowOffset;
        //             offset.x -= speed * Time.deltaTime;
        //             cineTransposer.m_FollowOffset = offset;
        //             // cineTransposer.m_FollowOffset = Vector3.MoveTowards(cineTransposer.m_FollowOffset, offset, Time.deltaTime);
        //             yield return null;
        //         }
        //     }

        //     bounciness /= 2;
        //     speed /= 8;
        //     movingLeft = !movingLeft;
        //     bounceCount++;
        //     yield return null;
        // }
    }
}
