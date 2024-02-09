using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game_controller_script : MonoBehaviour
{
    [Header("Assets Game")]
    [SerializeField] GameObject carrotPrefab;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject floor;
    
    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip audioIntro;
    [SerializeField] AudioClip audioGame;

    [Header("Text")]
    [SerializeField] GameObject textPress;
    [SerializeField] GameObject textHighScore;

    [Header("Scores")]
    [SerializeField] int highScorePrefs;

    private GameObject carrot;
    private GameObject player;

    private Vector3 v3;
    private Quaternion q;
    private bool play;

    private int vSize;

    void Start()
    {

        floor.transform.localScale = new Vector3(1, 1, 1);
        v3 = new Vector3(0, 0.1f, 0);
        q.eulerAngles = new Vector3(0, 90, 0);

        Camera.main.GetComponent<Camera>().orthographicSize = 1;

        /***************
         *  HighScore  *
         ***************/

        // Peticion de highscore en base de datos local
        highScorePrefs = PlayerPrefs.GetInt("HighScore");
        // Publica el maximo puntaje en pantalla
        textHighScore.GetComponent<TextMesh>().text = "" + highScorePrefs;

        player = playerPrefab;
        
        play = false;

        vSize = 1;

    }


    void Update()
    {
        if (play)
        {
            ReUbication();
            play = player.GetComponent<player_controller_script>().GetPlay();
            highScorePrefs = PlayerPrefs.GetInt("HighScore");
        } else
        {
            if (audioSource.clip == audioGame)
            {
                audioSource.clip = audioIntro;
                audioSource.Play();
            }
        }

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonUp(0)) & !play)
        {

            audioSource.clip = audioGame;
            audioSource.Play();

            floor.transform.localScale = new Vector3(15, 1, 15);

            floor.transform.rotation = q;
            player.transform.rotation = q;

            textHighScore.transform.position = new Vector3(-9, 3, 1.12f);
            textHighScore.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

            textPress.GetComponent<TextMesh>().fontSize = 0;

            play = true;

            player.GetComponent<player_controller_script>().SetPlay();
            player.GetComponent<player_controller_script>().Init(textHighScore);
    
            carrotPrefab.transform.position = new Vector3(UnityEngine.Random.Range(-7, 7), 0, UnityEngine.Random.Range(-7, 7));
            carrot = Instantiate(carrotPrefab);
        }
        // ANIMATIONS
        else if (!play)
        {
            textHighScore.transform.position = new Vector3(-1.2f, 0.65f, 0.15f);
            textHighScore.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            //  Text Press to Start
            if (textPress.GetComponent<TextMesh>().fontSize < 24)
                vSize = 1;
            else if (textPress.GetComponent<TextMesh>().fontSize > 32)
                vSize = -1;

            //  Score
            textPress.GetComponent<TextMesh>().fontSize += vSize;
            textHighScore.GetComponent<TextMesh>().text = "" + highScorePrefs;

            //  pieces Rotation
            floor.transform.localScale = new Vector3(1, 1, 1);
            floor.transform.Rotate(v3);
            player.transform.Rotate(v3);
            Destroy(carrot);
        }

    }

    private void ReUbication()
    {


        if (Camera.main.GetComponent<Camera>().orthographicSize < 7.5)
            Camera.main.GetComponent<Camera>().orthographicSize += 0.1f;

    }
}
