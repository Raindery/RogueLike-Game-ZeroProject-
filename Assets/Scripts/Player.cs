using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MovingObject
{
    public int wallDamage = 1;
    public int pointsPerSoda = 20;
    public int pointsPerFood = 10;
    public float restartLevelDelay = 1f;

    private Animator playerAnimator;
    private int food;
    
    protected override void Start()
    {
        

        playerAnimator = GetComponent<Animator>();

        food = GameManager.instance.playerFoodCount;

        base.Start();
    }

    private void OnDisable()
    {
        GameManager.instance.playerFoodCount = food;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.playersTurn) return;

        int horizontal;
        int vertical;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
            vertical = 0;

        if (horizontal != 0 || vertical != 0)
            AttemptMove<Wall>(horizontal, vertical);

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (collision.tag == "Food")
        {
            food += pointsPerFood;
            collision.gameObject.SetActive(false);
        }
        else if(collision.tag == "Soda")
        {
            food += pointsPerSoda;
            collision.gameObject.SetActive(false);
        }

    }

    protected override void OnCantMove<T>(T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        playerAnimator.SetTrigger("playerChop");
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
        // Application.LoadLevel(Application.loadedLevel);
    }

    private void LoserFood(int loss)
    {
        playerAnimator.SetTrigger("playerHit");
        food -= loss;
        CheckGameOver();
    }


    // Every Time On Player Move
    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        food--;
        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;

        CheckGameOver();

        GameManager.instance.playersTurn = false;
    }

    private void CheckGameOver()
    {
        if (food <= 0)
            GameManager.instance.GameOver();
    }
}
