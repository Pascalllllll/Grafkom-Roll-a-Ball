using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private int count;

    private float movementX;
    private float movementY;

    public float speed = 10f;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    public TextMeshProUGUI timerText;
    public float timeRemaining = 20f;
    private bool gameEnded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);
    }

    void Update()
    {
        if (!gameEnded)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                gameEnded = true;

                winTextObject.SetActive(true);
                winTextObject.GetComponent<TextMeshProUGUI>().text = "Time's Up!";

                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                Destroy(GameObject.FindGameObjectWithTag("Enemy"));
            }

            timerText.text = "Time: " + Mathf.CeilToInt(timeRemaining);
        }
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();

        if (count >= 12)
        {
            gameEnded = true;

            winTextObject.SetActive(true);
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
        }
    }

    void FixedUpdate()
    {
        if (gameEnded)
        {
            return;
        }

        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        rb.AddForce(movement * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!gameEnded && other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!gameEnded && collision.gameObject.CompareTag("Enemy"))
        {
            gameEnded = true;

            // Destroy the current object
            Destroy(gameObject);

            // Update the winText to display "You Lose!"
            winTextObject.gameObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
        }
    }
}