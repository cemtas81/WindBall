using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using TMPro;
using UnityEngine.UI;
public enum Swipe { None, Up, Down, Left, Right };
public class MovePlayer : MonoBehaviour
{
    private Rigidbody rb;
    public float speed;
    public float JumpForce;
    public bool canJump;
    public float MaxSpeed;
    public DynamicJoystick joy;
    public GameObject sign;
    public CinemachineVirtualCamera cine;
    //public TMPro.TextMeshPro score;
    public TextMeshProUGUI score;
    public TextMeshProUGUI textLevel;    
    public int sayi;
    private bool onBridge;
    //public bool On;
    private Animation ani;
    private ParticleSystem particle;
    public ParticleSystem part;
    [SerializeField] GameObject bridge1;
    [SerializeField] GameObject bridge2;
    [SerializeField] GameObject bridge3;  
    [SerializeField] GameObject bridge4;    
    [SerializeField] GameObject bridge5;    
    [SerializeField] GameObject bridge6;    
    [SerializeField] GameObject bridge7;    
    [SerializeField] GameObject bridge8;
    public float minSwipeLength = 190f;
    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;
    public AudioSource scorin;
    public AudioSource crush;
    public AudioSource fall;
    public GameObject PausePanel;
    public GameObject soundon;
    public GameObject soundoff;
    public GameObject Sound;
    public static Swipe swipeDirection;
    public GameObject lev1;
    public GameObject lev2;
    public GameObject lev3;
    public GameObject lev4;
    public GameObject lev5;
    public GameObject ses1;
    public GameObject ses2;
    public int levo;
    private int Bonus;
    private int total;
    public GameObject next;
    public Text bonus;
    public Text totalscore;  
    void Start()
    {
        levo = PlayerPrefs.GetInt("Level");
        rb=GetComponent<Rigidbody>();
        ani=score.GetComponent<Animation>();
        particle=GetComponentInChildren<ParticleSystem>();
        if (PlayerPrefs.GetInt("SoundOff")==1)
        {
            Sound.SetActive(false);
            soundoff.SetActive(true);  
        }
        else if (PlayerPrefs.GetInt("SoundOff")==0)
        {
            Sound.SetActive(true);
            soundoff.SetActive(false);
        }
        if (PlayerPrefs.GetInt("Level") == 0)
        {
            lev1.SetActive(true);
            ses1.SetActive(true);
            textLevel.text = "Stage:" + (PlayerPrefs.GetInt("Level")+1).ToString();
        }
        else if (PlayerPrefs.GetInt("Level") == 1)
        {

            lev2.SetActive(true);

            ses2.SetActive(true);
            textLevel.text = "Stage:" + (PlayerPrefs.GetInt("Level") + 1).ToString();
        }
        else if (PlayerPrefs.GetInt("Level") == 2)
        {
            lev3.SetActive(true);
            ses1.SetActive(true);
            textLevel.text = "Stage:" + (PlayerPrefs.GetInt("Level") + 1).ToString();
        }
        else if (PlayerPrefs.GetInt("Level") == 3)
        {
            lev4.SetActive(true);
            ses2.SetActive(true);
            textLevel.text = "Stage:" + (PlayerPrefs.GetInt("Level") + 1).ToString();
        }
        else if (PlayerPrefs.GetInt("Level") >= 4)
        {
            lev5.SetActive(true);
            ses1.SetActive(true);
            textLevel.text = "Stage:" + (PlayerPrefs.GetInt("Level") + 1).ToString();
        }


    }
    public void Next()
    {
        PlayerPrefs.SetInt("Score", total);
        PlayerPrefs.Save();
        SceneManager.LoadSceneAsync(0);
       
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
    public void Again()
    {
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") - 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
    public void Quit()
    {
        //PlayerPrefs.SetInt("Level", 0);
        //PlayerPrefs.SetInt("Score", 0);
        Application.Quit();
        Debug.Log("Quit");
    }
    public void Pause()
    {
        if (Time.timeScale==0)
        {
            Time.timeScale=1;
            PausePanel.SetActive(false);
        }
        else if (Time.timeScale==1)
        {
            Time.timeScale = 0;
            PausePanel.SetActive(true);
        }
            
    }
    public void SoundOff()
    {
        soundoff.SetActive(false);
        soundon.SetActive(true);
        PlayerPrefs.SetInt("SoundOff", 0);
        Sound.SetActive(true);
        PlayerPrefs.Save();
    }
    public void SoundOn()
    {
        soundoff.SetActive(true);
        soundon.SetActive(false);
        PlayerPrefs.SetInt("SoundOff", 1);
        Sound.SetActive(false);
        PlayerPrefs.Save();
    }
    void Update()
    {
       
        if (sayi >= 0)
        {
            score.text = sayi.ToString() + "X";
        }
        DetectSwipe();
        //if (joy.Vertical >= 0.3 && canJump == true)
        //{
        //    rb.AddForce(JumpForce / 2 * Vector3.up, ForceMode.Impulse);

        //    canJump = false;
        //}


    }
    public void DetectSwipe()
    {
        if (Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began)
            {
                firstPressPos = new Vector2(t.position.x, t.position.y);
            }

            if (t.phase == TouchPhase.Ended)
            {
                secondPressPos = new Vector2(t.position.x, t.position.y);
                currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

              
                if (currentSwipe.magnitude < minSwipeLength)
                {
                    swipeDirection = Swipe.None;
                    return;
                }

                currentSwipe.Normalize();

               
                if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f) 
                {
                    swipeDirection = Swipe.Up;
                    if (canJump == true && onBridge == false)
                    {
                        rb.AddForce(JumpForce * Vector3.up, ForceMode.Impulse);
                       
                        canJump = false;
                    }

                }
                else if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f) 
                {
                    swipeDirection = Swipe.Down;
                    
                } 
                else if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) 
                {
                    swipeDirection = Swipe.Left;
                   
                } 
                else if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) 
                {
                    swipeDirection = Swipe.Right;
                }
            }
        }
        else
        {
            swipeDirection = Swipe.None;
        }
    }
   
    private void FixedUpdate()
    {

        var localVel = transform.InverseTransformDirection(rb.velocity);

        if (joy.Vertical<=-0.5f&&localVel.z>=0&& onBridge == false)
        {
            rb.AddForce(-2f * speed * Time.deltaTime * Vector3.forward);
            
        }
       
        if (rb.velocity.magnitude > MaxSpeed&& onBridge == false)
        {
            rb.velocity = rb.velocity.normalized * MaxSpeed;
          
        }
        if (onBridge==false)
        {
            rb.AddForce(joy.Horizontal * speed * 1.7f * Time.deltaTime, 0, 1f * speed * Time.deltaTime);
        }
       
     
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            canJump = true;
          
        }
        if (collision.collider.CompareTag("Obstacle"))
        {
            if (Sound.activeInHierarchy ==true)
            {
                crush.PlayDelayed(0);
            }
           
            part.Play();
            Destroy(collision.collider.transform.parent.gameObject, 0.1f);
           
            if (sayi>0)
            {
                sayi -= 1;
                ani.Play();
            }
      
        }
        if (collision.collider.CompareTag("Finish"))
        {
            sayi = Bonus;
            onBridge = true;
            rb.freezeRotation = true;
            rb.velocity = Vector3.zero;
            joy.enabled = false;
            cine.enabled = false;
            if (lev1.activeInHierarchy == true)
            {
                PlayerPrefs.SetInt("Level", 1);
            }
            else if (lev2.activeInHierarchy == true)
            {
                PlayerPrefs.SetInt("Level", 2);
            }
            else if (lev3.activeInHierarchy == true)
            {
                PlayerPrefs.SetInt("Level", 3);
            }
            else if (lev4.activeInHierarchy == true)
            {
                PlayerPrefs.SetInt("Level", 4);
            }
            else if (lev5.activeInHierarchy == true)
            {
                PlayerPrefs.SetInt("Level", 0);
            }
            StartCoroutine(NextLevel());
            bonus.text = (Bonus * 100).ToString();
            total = (Bonus * 100) + PlayerPrefs.GetInt("Score");
            totalscore.text = total.ToString();


        }
    }
    IEnumerator NextLevel()
    {
        
        PlayerPrefs.Save();
        yield return new WaitForSeconds(3);
        next.SetActive(true);
        
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Finish"))
        {
            rb.AddForce(0, 10, 0, ForceMode.Impulse);
        }
        if (collision.collider.CompareTag("Ground"))
        {
            canJump = true;
            MaxSpeed = 20;
        }
    }
    
   
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            canJump = false;
            MaxSpeed = 15;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bonus"))
        {
            if (Sound.activeInHierarchy ==true)
            {
                scorin.PlayDelayed(0);
            }
           
            Destroy(other.transform.parent.gameObject);
            sayi += 1;
            ani.Play();
        }
        if (other.CompareTag("Death"))
        {
            cine.enabled = false;
            
            StartCoroutine(LoadNew());
            
        }
        if (other.CompareTag("Respawn"))
        {
            if (Sound.activeInHierarchy ==true)
            {
                fall.PlayDelayed(0);
            }
           
            particle.Play();
        }
        if (other.CompareTag("Bridge"))
        {
            if (lev1.activeInHierarchy == true && sayi >= 4)
            {
                Bonus = sayi - 4;    
                sayi = 4;

            }
            else if (lev2.activeInHierarchy == true && sayi >= 5)
            {
                Bonus = sayi - 5;
                sayi = 5;
            }
            else if (lev3.activeInHierarchy == true && sayi >= 6)
            {
                Bonus = sayi - 6;  
                sayi = 6;
            }
            else if (lev4.activeInHierarchy == true && sayi >= 7)
            {
                Bonus = sayi - 7;
                sayi = 7;
            }
            else if (lev5.activeInHierarchy == true && sayi >= 8)
            {
                Bonus = sayi - 8;

                sayi = 8;
            }
            MaxSpeed = 20;
            StartCoroutine(Bridge());
            sign.SetActive(false);
            onBridge = true;
            rb.freezeRotation = true;
            rb.velocity = Vector3.zero;
            transform.position=new Vector3(0,0.5f,transform.position.z);
            rb.AddForce(0, 10, 10, ForceMode.Impulse);
          
        }
        if (other.CompareTag("Eksi"))
        {
            sayi -= 1;
            other.gameObject.SetActive(false);
            
            ani.Play();
            if (Sound.activeInHierarchy == true)
            {
                scorin.PlayDelayed(0);
            }
        }
    }
    IEnumerator Bridge()
    {
    
        if (sayi == 1)
        {
            yield return new WaitForSeconds(0.4f);
            bridge1.SetActive(true);
          
        }
        else if (sayi == 2)
        {
            yield return new WaitForSeconds(0.4f);
            bridge1.SetActive(true);
           
            yield return new WaitForSeconds(0.4f);
            bridge2.SetActive(true);
       
        }
        else if (sayi == 3)
        {
            yield return new WaitForSeconds(0.4f);
            bridge1.SetActive(true);
           
            yield return new WaitForSeconds(0.4f);
            bridge2.SetActive(true);
           
            yield return new WaitForSeconds(0.4f);
            bridge3.SetActive(true);
      
        }
      
        else if (sayi==4)
        {
           
                yield return new WaitForSeconds(0.4f);
                bridge1.SetActive(true);

                yield return new WaitForSeconds(0.4f);
                bridge2.SetActive(true);

                yield return new WaitForSeconds(0.4f);
                bridge3.SetActive(true);

                yield return new WaitForSeconds(0.4f);
                bridge4.SetActive(true);
      
        }
       
        else if (sayi==5)
        {
           
                yield return new WaitForSeconds(0.4f);
                bridge1.SetActive(true);

                yield return new WaitForSeconds(0.4f);
                bridge2.SetActive(true);

                yield return new WaitForSeconds(0.4f);
                bridge3.SetActive(true);

                yield return new WaitForSeconds(0.4f);
                bridge4.SetActive(true);
                yield return new WaitForSeconds(0.4f);
                bridge5.SetActive(true);
           
        }
        else if (sayi==6)
        {
          
                yield return new WaitForSeconds(0.4f);
                bridge1.SetActive(true);

                yield return new WaitForSeconds(0.4f);
                bridge2.SetActive(true);

                yield return new WaitForSeconds(0.4f);
                bridge3.SetActive(true);

                yield return new WaitForSeconds(0.4f);
                bridge4.SetActive(true);

                yield return new WaitForSeconds(0.4f);
                bridge5.SetActive(true);
                yield return new WaitForSeconds(0.4f);
                bridge6.SetActive(true);
        
        }
        else if (sayi==7)
        {
           
                yield return new WaitForSeconds(0.4f);
                bridge1.SetActive(true);

                yield return new WaitForSeconds(0.4f);
                bridge2.SetActive(true);

                yield return new WaitForSeconds(0.4f);
                bridge3.SetActive(true);

                yield return new WaitForSeconds(0.4f);
                bridge4.SetActive(true);

                yield return new WaitForSeconds(0.4f);
                bridge5.SetActive(true);
                yield return new WaitForSeconds(0.4f);
                bridge6.SetActive(true);
                yield return new WaitForSeconds(0.4f);
                bridge7.SetActive(true);
       
        }
        else if (sayi==8)
        {
          
                yield return new WaitForSeconds(0.4f);
                bridge1.SetActive(true);

                yield return new WaitForSeconds(0.4f);
                bridge2.SetActive(true);

                yield return new WaitForSeconds(0.4f);
                bridge3.SetActive(true);

                yield return new WaitForSeconds(0.4f);
                bridge4.SetActive(true);

                yield return new WaitForSeconds(0.4f);
                bridge5.SetActive(true);
                yield return new WaitForSeconds(0.4f);
                bridge6.SetActive(true);
                yield return new WaitForSeconds(0.4f);
                bridge7.SetActive(true);
                yield return new WaitForSeconds(0.4f);
                bridge8.SetActive(true);
        
        }

        else

            yield return null;
    }
    IEnumerator LoadNew()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(0);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bridge"))
        {
            onBridge = false;
            rb.freezeRotation = false;
            
        }
    }

}
