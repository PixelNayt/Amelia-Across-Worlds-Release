using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MumeiEnemyAI : MonoBehaviour
{
    //Components to use
    CharacterController enemy;

    public Transform amelia; //player
    public Transform mumei; //enemy
    public Transform lookAtPlayer;

    public float mumeiSpeed = 20f;

    public GameObject gameOverScreen;
    public GameObject timer;

    void Start()
    {
        Time.timeScale = 1;
    }

    //Chasing Mechnanic
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, amelia.transform.position, mumeiSpeed * Time.deltaTime);
        transform.LookAt(lookAtPlayer);
    }

    /*
    public bool IsGrounded()
    {
        if (enemy.isGrounded)
            return true;

        //Get the position of the bottom of our object
        Vector3 bottom = enemy.transform.position - new Vector3(0, enemy.height / 2, 0);

        //Cast a ray downwards with a range of -1, if it hits something we reposition the enemy lower
        RaycastHit hit;
        if (Physics.Raycast(bottom, new Vector3(0, -1, 0), out hit, 0.2f))
        {
            enemy.Move(new Vector3(0, -hit.distance, 0)); 
            return true;
        }

        return false;
    } */

    //ENEMY COLLIDES WITH PLAYER 
    void onCollisionEnter (Collision enemyCollides)
    {
        if (enemyCollides.gameObject.name == "SmolAmeModelSeafoamboi")
        {
            timer.SetActive(false);
            gameOverScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
