using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_controller_script : MonoBehaviour
{

    [SerializeField] GameObject carrot;
    [SerializeField] GameObject babbyPrefab;
    private GameObject babby;
    private GameObject textScore;

    [Header("SlideMove")]
    private Vector3 p;
    private Vector3 mousePosition;

    private byte timeJump;
    public byte twoPreMove; 
    public byte preMove;
    public byte dirMove;
    private byte inputMove;
    private byte limitt;

    private byte CountBunny;
    private byte CountCarrot;
    public int HighScore;

    private bool contact;
    private bool play;

    void Start()
    {
        CountBunny = 0;
        CountCarrot = 0;

        contact = false;
        play = false;

        timeJump = 0;
        twoPreMove = 2;
        preMove = 2;
        dirMove = 2;
        inputMove = 2;
        limitt = 30;
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (play)
        {

            InputMove();
            InputSlideMove();
            if (timeJump >= 5 && timeJump < 15)
            {
                Move();
                Rotation();
            }
            Jump();
            UpDateTime();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Babby") && other.transform != babby.transform)
        {
            ResetPlayer();
        }
    }

    private void Move()
    {
        Vector3 lP = transform.localPosition;
        if (dirMove == 0)
        {
            lP.x -= 0.1f;
            transform.localPosition = lP;
        }
        if (dirMove == 1)
        {
            lP.z += 0.1f;
            transform.localPosition = lP;
        }
        if (dirMove == 2)
        {
            lP.x += 0.1f;
            transform.localPosition = lP;
        }
        if (dirMove == 3)
        {
            lP.z -= 0.1f;
            transform.localPosition = lP;
        }
    }
    private void Rotation()
    {
        if (preMove == dirMove+1 || ((dirMove - 3) == preMove))
            transform.Rotate(new Vector3(0, -9f, 0));

        if (preMove == dirMove-1 || ((dirMove + 3) == preMove))
            transform.Rotate(new Vector3(0, 9f, 0));

    }
    private void InputMove()
    {
        if (Input.GetAxis("Horizontal") < 0 && dirMove != 2)
            inputMove = 0;
        
        if (Input.GetAxis("Horizontal") > 0 && dirMove != 0)
            inputMove = 2;

        if (Input.GetAxis("Vertical") < 0 && dirMove != 1)
            inputMove = 3;

        if (Input.GetAxis("Vertical") > 0 && dirMove != 3)
            inputMove = 1;
    }
    private void InputSlideMove()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            p = Input.mousePosition - mousePosition;

            if (Mathf.Abs(p.x) > Mathf.Abs(p.y))
            {
                if (p.x > 100 && dirMove != 0)
                {
                    inputMove = 2;
                } else if (p.x <-100 && dirMove != 2)
                {
                    inputMove = 0;
                }
            } else
            {
                if (p.y > 100 && dirMove != 3)
                {
                    inputMove = 1;
                }
                else if (p.y < -100 && dirMove != 1)
                {
                    inputMove = 3;
                }
            }
        }

    }

    private void Jump()
    {
        Vector3 lS = transform.localScale;
        Vector3 lP = transform.localPosition;
        if (timeJump < 3)
        {
            lS.y -= 0.1f;
            transform.localScale = lS;
        }
        else if (timeJump < 5)
        {
            lS.y += 0.15f;
            transform.localScale = lS;
        }
        else if (timeJump < 10)
        {
            lP.y += 0.15f;
            transform.localPosition = lP;

            lS.y = 1f;
            transform.localScale = lS;
        }
        else if (timeJump < 15)
        {
            lP.y -= 0.15f;
            transform.localPosition = lP;
        }
    }
    private void UpDateTime()
    {
        if (timeJump <= limitt)
        {
            p = transform.position;
            if (p.x < -7 || p.x > 7 || p.z < -7 || p.z > 7)
                ResetPlayer();

            
            timeJump++;
        }
        else
        {

            if (CountBunny < CountCarrot)
            {
                if (CountBunny == 0)
                    FirstBabby();
                else    
                    NewBunny();
                
                CountBunny++;
            }
              
            if(CountBunny > 0)
                babby.GetComponent<babby_script>().SetMove(dirMove, limitt);

            preMove = dirMove;
            dirMove = inputMove;
            timeJump = 0;
            
            if (CountCarrot > HighScore)
                PlayerPrefs.SetInt("HighScore", CountCarrot);

            textScore.GetComponent<TextMesh>().text = "" + CountCarrot;
        }

    }
    public void NewBunny()
    {
        if (contact && limitt >= 15)
        {
            babby.GetComponent<babby_script>().NewBunny(babbyPrefab, limitt, timeJump);
            contact = false;
        }
    }
    public void Point()
    {
        if (limitt > 15)
            limitt--;

        CountCarrot++;
    }
    public void SetCollision(bool c)
    {
        contact = c;
    }
    public void SetPlay()
    {
        if (play)
            play = false;
        else
            play = true;
    }
    public bool GetPlay()
    {
        return play;
    }
    private void FirstBabby ()
    {
        babbyPrefab.transform.SetPositionAndRotation(transform.position, transform.rotation);

        babby = Instantiate(babbyPrefab);
        babby.GetComponent<babby_script>().SetMove(10, limitt);

        contact = false;
    }

    private void ResetPlayer()
    {

        if (CountBunny > 0)
        {
            babby.GetComponent<babby_script>().SetMove(10, limitt);
            babby.GetComponent<babby_script>().DestroyFamilyBunny();
        }

        CountBunny = 0;
        CountCarrot = 0;

        play = false;

        timeJump = 0;
        preMove = 2;
        dirMove = 2;
        inputMove = 2;
        limitt = 30;

        Quaternion q = new()
        {
            eulerAngles = new Vector3(0, 90, 0)
        };
        transform.rotation = q;

        Camera.main.transform.position = new Vector3(3, 6, -5);
        q = Camera.main.transform.rotation;
        q.eulerAngles = new Vector3(45, -30, 0);

        Camera.main.transform.rotation = q;
        Camera.main.GetComponent<Camera>().orthographicSize = 1;

        transform.position = new Vector3(0, 0, 0);
    }
    public void Init(GameObject textHighScore)
    {
        textScore = textHighScore;
        HighScore = PlayerPrefs.GetInt("HighScore");
        babbyPrefab.transform.SetPositionAndRotation(new Vector3(0, 0, 0), transform.rotation);
    }

    public int GetCarrot()
    {
        return CountCarrot;
    }
}
