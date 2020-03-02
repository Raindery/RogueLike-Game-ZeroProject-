using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MovingObject
{
    public int wallDamage = 1;
    public int pointsPerSoda = 20;
    public int pointsPerFood = 10;
    public float restartLevelDelay = 1f;
    public Text foodText;

    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip drinkSound1;
    public AudioClip drinkSound2;
    public AudioClip gameOverSound;


    private Animator playerAnimator;
    private int food;


    
    protected override void Start()
    {
        

        playerAnimator = GetComponent<Animator>();

        food = GameManager.instance.playerFoodCount;

        foodText.text = $"Food: {food}";

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
        if(collision.CompareTag("Exit"))
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (collision.CompareTag("Food"))
        {
            food += pointsPerFood;

            foodText.text = $"+ {pointsPerFood} Food: {food}";

            collision.gameObject.SetActive(false);

            SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);
        }
        else if(collision.CompareTag("Soda"))
        {
            food += pointsPerSoda;

            foodText.text = $"+ {pointsPerSoda} Food: {food}";

            collision.gameObject.SetActive(false);

            SoundManager.instance.RandomizeSfx(drinkSound1, drinkSound2);
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
       
    }

    public void LoseFood(int loss)
    {
        playerAnimator.SetTrigger("playerHit");
        food -= loss;

        foodText.text = $"- {loss} Food: {food}";

        CheckGameOver();
    }


    // Every Time On Player Move
    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        food--;

        foodText.text = $"Food: {food}";

        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;

        if(Move(xDir, yDir, out hit))
        {
            SoundManager.instance.RandomizeSfx(moveSound1, moveSound2);
        }

        CheckGameOver();

        GameManager.instance.playersTurn = false;
    }

    private void CheckGameOver()
    {
        if (food <= 0)
        {
            SoundManager.instance.PlaySingle(gameOverSound);
            SoundManager.instance.musicSource.Stop();
            GameManager.instance.GameOver();
            
        }
            
    }
}
