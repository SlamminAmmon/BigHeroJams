using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed=0;
    [SerializeField] private float gravity = 0;
    private Rigidbody2D player;
    [SerializeField] private BoxCollider2D playerCollider;
    public BoxCollider2D sideWalkCollider;
    public Transform cam;
    public int downCount = 0;
    public int camOffset =0;
    public float falling;
    public float laneChangeTime;
    private int jCount = 0;
    private float initialG = 0;

    private void Awake() {
        //Get the rigidbody of player
        player = GetComponent<Rigidbody2D>();

        playerCollider = GetComponent<BoxCollider2D>();
        initialG = gravity;




    }

    private void Update() {

        //Int that stors the the horizantal input
        //*******Unused in current itteration*******
        float horizontalInput = Input.GetAxis("Horizontal");
        player.velocity = new Vector2(speed, player.velocity.y);
    

        //flips the player sprite to appear backward
        //*******Unused in current itteration*******
        /**if (horizontalInput > 0.01f) {
            transform.localScale = new Vector3.one;
        } else if (horizontalInput < -0.01f) {
            transform.localScale = new Vector3(-1, 1, 1);
        }**/


        //Get the camera to follow player on the X axis
        cam.position = new Vector3((player.position.x)+camOffset, cam.position.y, cam.position.z);

        //Jump
        if (Input.GetKey(KeyCode.Space) && jCount==0) {
            downCount = 0;
            player.velocity = new Vector2(player.velocity.x , gravity);
            jCount++;
            sideWalkCollider.isTrigger = false;
        }

        //Allow the player to drop down to bike lane
        if (Input.GetKey("s")) {
            StartCoroutine("Fall");
            downCount=1;
            //gravity=gravity * 2;
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "sidewalk")
        {
            jCount = 0;
            sideWalkCollider = collision.gameObject.GetComponent<BoxCollider2D>();
            gravity = initialG;
        }


        //Turns off collider for Side walk obsticals while in bike lane
        if (collision.gameObject.tag == "sideWalkObstacle")
        {
            if (downCount==1) {
                collision.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            }
        }

        //Turns off collider for Bike lane obsticals while on side walk
        if (collision.gameObject.tag == "bikeLaneObstacle")
        {
            if (downCount == 0)
            {
                collision.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            }
        }


        //Death Peramiters
        if (collision.gameObject.tag == "sideWalkObstacle")
        {
            if (downCount == 0 && jCount == 0)
            {
                //Death 
                SceneManager.LoadScene("Death");
            }
        }

        if (collision.gameObject.tag == "bikeLaneObstacle")
        {
            if (downCount == 1)
            {
                //Death
                SceneManager.LoadScene("Death");
            }
        }

        if (collision.gameObject.tag == "DeathZone")
        {
            //Death
            SceneManager.LoadScene("Death");
        }

        //Win
        if (collision.gameObject.tag == "WinZone")
        {
            //Death
            SceneManager.LoadScene("Win");
        }



    }

    private IEnumerator Fall() {
        if (downCount == 0)
        {
            sideWalkCollider.isTrigger = true;
        }
        //playerCollider.isTrigger = true;
        yield return null; //new WaitForSeconds(laneChangeTime);
        //sideWalkCollider.isTrigger = false;


        //playerCollider.isTrigger = false;

    }



}
