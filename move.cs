using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class move : MonoBehaviour
{
    [SerializeField]
    float speed = 1.0f;
    Rigidbody rb;

    public bool see = false;
    [SerializeField]
    GameObject hed;
    public float hed_speed = 10f;

    [SerializeField]
    float time = 0;
    float limit_time = 0;
    [SerializeField]
    Image timer_image;

    Vector3 start_pos;

    [SerializeField]
    Text goal;
    [SerializeField]
    Text gameOver;

    bool goal_flag = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Random.InitState(System.DateTime.Now.Millisecond);
        limit_time = Random.Range(2.0f,4.0f);

        start_pos = transform.position;

        goal.enabled = false;
        gameOver.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {   
        if(!goal_flag){
            my_move();

            hed_move();

            timer();
            timer_image.fillAmount = 1-time/limit_time;

            if(see && rb.velocity.magnitude > 0.1f){
                rb.velocity = Vector3.zero;
                gameOver.enabled = true;
                Invoke(nameof(Go_start),0.5f);
            }
        }
        else{
            timer_image.color = new Color(0.0f,255.0f,0.0f);
            timer_image.fillAmount = 1;
        }


    }

    void my_move(){
        float tranlation_z = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float tranlation_x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        if(rb.velocity.magnitude < 10){
            rb.AddForce(tranlation_x,0,tranlation_z);
        }
    }

    void hed_move(){
        if(see){
            if(hed.transform.rotation.y != 180){
                hed.transform.rotation = Quaternion.RotateTowards(hed.transform.rotation,Quaternion.Euler(0,180,0),hed_speed*2);
            }
        }
        else{
            if(hed.transform.rotation.y != 0){
                hed.transform.rotation = Quaternion.RotateTowards(hed.transform.rotation,Quaternion.Euler(0,0,0),hed_speed);
            }
        }
    }

    void timer(){
        if(time < limit_time){
            time += Time.deltaTime;
        }
        else{
            time = 0;
            if(see){
                see = false;
                limit_time = Random.Range(2.0f,4.0f);
            }
            else{
                see = true;
                limit_time = Random.Range(1.0f,2.0f);
            }
        }

        if(see){
            timer_image.color = new Color(255.0f,0.0f,0.0f);
        }
        else{
            timer_image.color = new Color(0.0f,255.0f,0.0f);
        }
    }

    void Go_start(){
        transform.position = start_pos;
        see = false;
        time = 0;
        limit_time = Random.Range(2.0f,4.0f);
        gameOver.enabled = false;
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "goal"){
            Debug.Log("GOAL!");
            goal.enabled = true;
            goal_flag = true;
        }
    }

}
