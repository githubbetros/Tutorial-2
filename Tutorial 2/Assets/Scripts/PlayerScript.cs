using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    // Start is called before the first frame update

    private Rigidbody2D r2d2;
    public AudioClip victory;
    public AudioClip defaultMusic;
    public float speed;
    public Text score;
    private int scoreValue = 0;
    public Text win;
    public Text lives;
    public Text lose;
    private int livesValue;
    private bool groundValue;
    private bool gameOver;
    private bool facingRight = true;
    public AudioSource musicSource;
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        musicSource.clip = defaultMusic;
        musicSource.Play();
        musicSource.loop = true;
        r2d2 = GetComponent<Rigidbody2D>();
        score.text = scoreValue.ToString();
        win.text = "";
        lose.text = "";
        livesValue = 3;
        lives.text = "Lives: " + livesValue;
        groundValue = false;
        gameOver = false;
    }

    void Update()
    {
        if(Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("State", 1);
        }

        if(Input.GetKeyUp(KeyCode.D))
        {
            anim.SetInteger("State", 0);
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            anim.SetInteger("State", 1);
        }

        if(Input.GetKeyUp(KeyCode.A))
        {
            anim.SetInteger("State", 0);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        r2d2.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);
        }

        if(scoreValue == 8 && !gameOver)
        {
            win.text = "You Win! Game created by Alex Betros!";
            gameOver = true;
            musicSource.clip = victory;
            musicSource.Play();
            musicSource.loop = false;
        }

        if(collision.collider.tag == "Enemy")
        {
            livesValue -= 1;
            lives.text = "Lives: " + livesValue.ToString();
            Destroy(collision.collider.gameObject);
        }

        if(livesValue == 0)
        {
            Destroy(gameObject);
            lose.text = "You Lose!";
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground")
        {
            if(Input.GetKey(KeyCode.W))
            {
                r2d2.AddForce(new Vector2(0, 4), ForceMode2D.Impulse);
                anim.SetInteger("State", 2);
            }
        }

        if(scoreValue == 4 && !groundValue)
        {
            groundValue = true;
            transform.position = new Vector2(127, 0);
            livesValue = 3;
            lives.text = "Lives: " + livesValue;
        }
    }
}
