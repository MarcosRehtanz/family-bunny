using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class babby_script : MonoBehaviour
{
    private GameObject nextBunny;

    private byte timeJump;
    private byte preMove;
    public byte dirMove;
    private byte inputMove;
    public byte limitt;

    void Start()
    {
        nextBunny = new GameObject();

        timeJump = 0;
        preMove = 0;
        dirMove = 10;
        inputMove = 10;
        limitt = 30;
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {

        if (timeJump >= 5 && timeJump < 15)
        {
            Move();
            Rotation();
        }
        Jump();
        UpDateTime();
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
        if (preMove == dirMove + 1 || ((dirMove - 3) == preMove))
            transform.Rotate(new Vector3(0, -9f, 0));
        if (preMove == dirMove - 1 || ((dirMove + 3) == preMove))
            transform.Rotate(new Vector3(0, 9f, 0));
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
            timeJump++;
        }
        else
        {
            if (nextBunny.transform.CompareTag("Babby"))
                nextBunny.GetComponent<babby_script>().SetMove(dirMove, limitt);

            preMove = dirMove;
            dirMove = inputMove;
            timeJump = 0;
        }
    }

    public void NewBunny(GameObject go, byte limitt, byte timeJump)
    {
        if (nextBunny.transform.CompareTag("Babby"))
        {
            this.limitt = limitt;
            this.timeJump = timeJump;
            nextBunny.GetComponent<babby_script>().NewBunny(go, limitt, timeJump);
        }
        else
        {
            go.transform.SetPositionAndRotation(transform.position, transform.rotation);
            
            Destroy(nextBunny);
            nextBunny = Instantiate(go);
        }
    }

    public void SetMove(byte nextMove, byte limitt)
    {
        inputMove = nextMove;
        this.limitt = limitt;
    }

    public void DestroyFamilyBunny()
    {
        if (nextBunny.transform.CompareTag("Babby"))
        {
            nextBunny.GetComponent<babby_script>().DestroyFamilyBunny();
            Destroy(gameObject);
        } else
        {
            Destroy(nextBunny);
            Destroy(gameObject);
        }
    }
}
