using System;
using DG.Tweening;
using Unity.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Touch mainTouch;

    [SerializeField] private Health health;
    
    [SerializeField] private Vector2 movementVector = Vector2.zero;
    [SerializeField] public float speedMultiplier;
    [SerializeField] private float turnSpeed;
    [SerializeField] private Ease easeType;
    [SerializeField] [ReadOnly] private bool canSwitch = true;

    private void Start()
    {
        xRightWall = Camera.main.ViewportToWorldPoint(Vector3.one).x * 1.1f;
        xLeftWall = Camera.main.ViewportToWorldPoint(Vector3.zero).x * 1.1f;
    }

    void Update()
    {
        if (Input.touchCount > 0) UpdateTouch();
        UpdateMovement();
    }

    #region Touch
    void UpdateTouch()
    {
        mainTouch = Input.GetTouch(0);
        if (mainTouch.phase == TouchPhase.Began && !HelperFunctions.IsOverUI() && canSwitch)
        {
            SwitchDirections();
        }
    }
    #endregion
    
    #region Movement

    public Transform measurementObject;
    private float xRightWall, xLeftWall;
    
    void UpdateMovement()
    {
        transform.position += speedMultiplier * Time.deltaTime * (Vector3) movementVector;
        measurementObject.position += speedMultiplier * Time.deltaTime * Vector3.down;
        
        if ((transform.position.x > xRightWall || transform.position.x < xLeftWall) && health.health > 0)
        {
            Damage();
            SwitchDirections();
        }
    }
    void SwitchDirections()
    {
        SoundManager.Instance.PlaySound("switchDir");
        canSwitch = false;
        movementVector.x *= -1;
        float offsetZ;
        
        if (transform.localRotation.z > 0) offsetZ = -35;
        else offsetZ = 35;
        
        transform.DORotate(new Vector3(0f, 0f, offsetZ), turnSpeed).SetEase(easeType).OnComplete(() => canSwitch = true);
    }
    #endregion
    
    #region Death

    public void ResetGame()
    {
        transform.position = new Vector3(0, -2.35f, 0);
        speedMultiplier = 3f;
        health.RestartGame();
        measurementObject.transform.position = transform.position;
    }
    
    void Damage()
    {
        StartCoroutine(HelperFunctions.ScreenShake(Camera.main, 0.5f, 0.15f, true));
        health.OnDamage();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Respawn"))
        {
            Damage();
        }
    }

    #endregion
}