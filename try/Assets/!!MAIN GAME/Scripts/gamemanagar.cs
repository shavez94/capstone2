using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class vehicle
{
    public int node;
    public string name;
    public vehicle(int node, string name)
    {
        this.node = node;
        this.name = name;
    }
}

public class gamemanagar : MonoBehaviour {

    public GameObject camer1;
    [SerializeField]
    public List<vehicle> presentvehicales;

    [SerializeField]
    public TextMeshPro[] postions;

    [SerializeField]
    public carcontoller carcontoller;

    public GameObject[] levels;
    private GameObject activeplatform;
    private GameObject nextplatform;
    public GameObject AI;
    private List<GameObject> temporaryList;
    [SerializeField]
    public GameObject[] presentGameojectVehicals; // array of ai car driver
    public GameObject[] fullarray; // all the vehical inclduing player

    public GameObject levelfailed_panel;
    public GameObject levelfinished_panel;
    public GameObject game_panel;
    public GameObject pausecamera;
    public GameObject watereffect;
    public GameObject[] checkpoint_beam;

    public Material green_beam;
    public Material orange_beam;


    [SerializeField]
    public Slider fuel;
    [SerializeField]
    public Slider health;

    [SerializeField]
    public List<GameObject> checkpoint = new List<GameObject>();
    [SerializeField]
    public TextMeshProUGUI playerpos;
    [SerializeField]
    public GameObject player;
    private Rigidbody rb;
    [SerializeField]
    public inputmanager inputmanager;

    private int activelevel;
    int min_check, maxcheck;
    private Quaternion Quaternion;

    private Quaternion Quaternion_A1;
    private Quaternion Quaternion_A2;
    private Quaternion Quaternion_A3;
    private Vector3 Postion_A1;
    private Vector3 Postion_A2;
    private Vector3 Postion_A3;


    private bool brakes;
    private bool left = false;
    private bool right = false;
    private string pos;

    public float rotationspeed;
    public bool aioff;

    //FPS

    public float timer, refersh, avgframe;
    public string display = "{0} FPS";
    public TextMeshProUGUI fps;
    private void Awake()

    {
        presentvehicales = new List<vehicle>();
        foreach (GameObject R in presentGameojectVehicals)
            presentvehicales.Add(new vehicle(R.GetComponent<aiinput>().currentnode, R.GetComponent<aicontroller>().carname));

        presentvehicales.Add(new vehicle(player.gameObject.GetComponent<inputmanager>().currentnode, player.gameObject.GetComponent<carcontoller>().carname));
        temporaryList = new List<GameObject>();
        foreach (GameObject R in presentGameojectVehicals)
            temporaryList.Add(R);
        temporaryList.Add(player.gameObject);
        fullarray = temporaryList.ToArray();
        rb = player.GetComponent<Rigidbody>();
        carcontoller = carcontoller.GetComponent<carcontoller>();

        fuel.maxValue = PlayerPrefs.GetFloat("fuel", 20);
        fuel.value = fuel.maxValue;
        fuel.minValue = 0;
        health.maxValue = PlayerPrefs.GetFloat("health", 20);
        health.value = health.maxValue;
        health.minValue = 0;
        PlayerPrefs.SetInt("timeleft", 0);
    }

    public void Start()
    {
        activelevel = PlayerPrefs.GetInt("levels", 0);
        setpostion();
        color_beam();
    }

    private void color_beam()
    {
 
        MeshRenderer MeshRenderer;
        switch (activelevel)
        {
            case 0:
                min_check = 0;
                maxcheck = 1;
                break;
            case 1:
                min_check = 2;
                maxcheck = 5;
                break;
            case 2:
                min_check = 6;
                maxcheck = 2;
                break;
            case 3:
                min_check = 0;
                maxcheck = 2;
                break;
            case 4:
                min_check = 0;
                maxcheck = 2;
                break;
            case 5:
                min_check = 0;
                maxcheck = 2;
                break;
            case 6:
                min_check = 0;
                maxcheck = 2;
                break;
            case 7:
                min_check = 0;
                maxcheck = 2;
                break;
            case 8:
                min_check = 0;
                maxcheck = 2;
                break;
            case 9:
                min_check = 0;
                maxcheck = 2;
                break;
            case 10:
                min_check = 0;
                maxcheck = 2;
                break;
            case 11:
                min_check = 0;
                maxcheck = 2;
                break;
            case 12:
                min_check = 0;
                maxcheck = 2;
                break;

        }
        for (int i = 0; i < min_check+1; i++)
        {
            MeshRenderer = checkpoint_beam[i].GetComponent<MeshRenderer>();
            MeshRenderer.material = orange_beam;
        }
        for (int i = min_check; i < maxcheck+1; i++)
        {
            MeshRenderer = checkpoint_beam[i].GetComponent<MeshRenderer>();
            MeshRenderer.material = green_beam;
        }

    }
    public void FixedUpdate()
    {
        SortArray();
        if (brakes==true)
        {
            if (rb.velocity.magnitude > 10)
                inputmanager.handbrake = true;
            else
            {
                inputmanager.vertical = -1;
                inputmanager.handbrake = false;
            }
        }
        if (left == true)
        {
            inputmanager.horizontal = Mathf.Clamp(inputmanager.horizontal - rotationspeed * Time.deltaTime, -1, 0);
        }
        else if (right == true)
        {
            inputmanager.horizontal = Mathf.Clamp(inputmanager.horizontal + rotationspeed * Time.deltaTime, 0, 1);
        }

    }
    public void Update()
    {
        if (rb.velocity.magnitude>10 || rb.velocity.magnitude <-10)
        {
            fuel.value -= Time.deltaTime;
        }
        if (fuel.value == 0)
        {
            game_panel.SetActive(false);
            levelfailed_panel.SetActive(true);
        }
        if(carcontoller.inwater)
        {
            health.value -= Time.deltaTime;
            watereffect.SetActive(true);
        }
        else
        {
            watereffect.SetActive(false);
            health.value += Time.deltaTime;
        }
        if (health.value == 0)
        {
            game_panel.SetActive(false);
            levelfailed_panel.SetActive(true);
        }

        float timelapse = Time.smoothDeltaTime;
        timer = timer <= 0 ? refersh : timer -= timelapse;
        if (timer <= 0) avgframe = (int)(1f / timelapse);
        fps.text = string.Format(display, avgframe.ToString());
    }

    public void setpostion()
    {
        Vector3 P_pos = new Vector3(levels[activelevel].transform.position.x-1, levels[activelevel].transform.position.y-2, levels[activelevel].transform.position.z);
        Vector3 AI_1 = new Vector3(levels[activelevel].transform.position.x+2, levels[activelevel].transform.position.y, levels[activelevel].transform.position.z);
        Vector3 AI_2 = new Vector3(levels[activelevel].transform.position.x-3, levels[activelevel].transform.position.y, levels[activelevel].transform.position.z-3);
        Vector3 AI_3 = new Vector3(levels[activelevel].transform.position.x+4, levels[activelevel].transform.position.y, levels[activelevel].transform.position.z -3);
        player.transform.position = P_pos;
        presentGameojectVehicals[0].transform.position = AI_1;
        presentGameojectVehicals[1].transform.position = AI_2;
        presentGameojectVehicals[2].transform.position = AI_3;


    }
    private void SortArray()
    {

        for (int i = 0; i < fullarray.Length; i++)
        {
            if (i > 2)
            {
                presentvehicales[i].name = fullarray[i].GetComponent<carcontoller>().carname;
                presentvehicales[i].node = fullarray[i].GetComponent<inputmanager>().currentnode;
            }
            else
            {
                presentvehicales[i].name = fullarray[i].GetComponent<aicontroller>().carname;
                presentvehicales[i].node = fullarray[i].GetComponent<aiinput>().currentnode;
            }
        }
        for (int i = 0; i < presentvehicales.Count; i++)
        {
            for (int j = i + 1; j < presentvehicales.Count; j++)
            {
                if (presentvehicales[j].node < presentvehicales[i].node)
                {
                    vehicle qq = presentvehicales[i];
                    presentvehicales[i] = presentvehicales[j];
                    presentvehicales[j] = qq;
                }
            }
        }
        presentvehicales.Reverse();
        for (int i = 0; i < presentvehicales.Count; i++)
        {
            if (player.gameObject.GetComponent<carcontoller>().carname == presentvehicales[i].name)
            {
                playerpos.text = ((i + 1) + "/" + presentvehicales.Count).ToString();
                switch (i)
                {
                    case 0:
                        pos = "1st";
                        break;
                    case 1:
                        pos = "2nd";
                        break;
                    case 2:
                        pos = "3rd";
                        break;
                    case 3:
                        pos = "4th";
                        break;

                }
                postions[0].text = pos;
            }
            else if ("green" == presentvehicales[i].name)
            {
                switch (i)
                {
                    case 0:
                        pos = "1st";
                        break;
                    case 1:
                        pos = "2nd";
                        break;
                    case 2:
                        pos = "3rd";
                        break;
                    case 3:
                        pos = "4th";
                        break;

                }
                postions[1].text = pos;
            }
            else if ("blue" == presentvehicales[i].name)
            {
                switch (i)
                {
                    case 0:
                        pos = "1st";
                        break;
                    case 1:
                        pos = "2nd";
                        break;
                    case 2:
                        pos = "3rd";
                        break;
                    case 3:
                        pos = "4th";
                        break;

                }
                postions[2].text = pos;
            }
            else if ("light" == presentvehicales[i].name)
            {
                switch (i)
                {
                    case 0:
                        pos = "1st";
                        break;
                    case 1:
                        pos = "2nd";
                        break;
                    case 2:
                        pos = "3rd";
                        break;
                    case 3:
                        pos = "4th";
                        break;

                }
                postions[3].text = pos;
            }
        }

    }

    public void checkpoint_Reached(GameObject gameObject)
    {
        checkpoint.Add(gameObject);
        Quaternion = player.transform.rotation;
        Quaternion_A1 = presentGameojectVehicals[0].transform.rotation;
        Quaternion_A2 = presentGameojectVehicals[1].transform.rotation;
        Quaternion_A3 = presentGameojectVehicals[2].transform.rotation;
        Postion_A1 = presentGameojectVehicals[0].transform.position;
        Postion_A2 = presentGameojectVehicals[0].transform.position;
        Postion_A3 = presentGameojectVehicals[0].transform.position;
        refill_fuel();
    }


    public void levelreached(Transform gameObject)
    {
        
        if(checkpoint.Count<1)
        {
            return;
        }
        for (int i = 0; i < levels.Length; i++)
        {
            if(gameObject == levels[i].transform.parent)
            {

                if(activelevel<i)
                {
                     PlayerPrefs.SetInt("levels", i);
                    activelevel = i;
                    Quaternion = player.transform.rotation;
                }
            }
        }
        rb.isKinematic = true;
        Invoke("levelfinished",1.5f);
    }
    public void levelfinished()
    {

        GameObject.Find("Camera").GetComponent<VehicleCameraControl>().enabled = false;
        checkpoint.Clear();
        Debug.Log("list clean");
        game_panel.SetActive(false);
        levelfinished_panel.SetActive(true);

    }
    public void refill_fuel()
    {
        fuel.value = fuel.maxValue;
    }

    public void spawn()
    {
        int last = checkpoint.Count;
        if(last>0)
        {
            Vector3 vector3 = new Vector3(checkpoint[last - 1].transform.position.x, checkpoint[last - 1].transform.position.y + 2, checkpoint[last - 1].transform.position.z);
            player.transform.position = vector3;
        }
        else
        {
            player.transform.position = levels[activelevel].transform.position;
        }
        player.transform.rotation = Quaternion;
        presentGameojectVehicals[0].transform.position= Postion_A1;
        presentGameojectVehicals[1].transform.position = Postion_A2;
        presentGameojectVehicals[2].transform.position = Postion_A3;
        presentGameojectVehicals[0].transform.rotation=Quaternion_A1;
        presentGameojectVehicals[1].transform.rotation = Quaternion_A2;
        presentGameojectVehicals[2].transform.rotation = Quaternion_A3;
        rb.velocity = new Vector3(0,0,0);
    }
    public void changescene(int n)
    { 
        SceneManager.LoadScene(n);
    }
    public void gamepause()
    {
        Time.timeScale = 0;
    }
    public void retry()
    {
        PlayerPrefs.DeleteAll();
        Time.timeScale = 1;
        if(checkpoint.Count>0)
        {
            spawn();
        }
        else

        {
            setpostion();
            rb.velocity = new Vector3(0, 0, 0);
        }
    }
    public void gameresume()
    {
        Time.timeScale = 1;
    }
    public void nextlevel()
    {
        Time.timeScale = 1;
       
    }

    public void accelrationdown()
    {
        inputmanager.vertical = 1;
    }

    public void accelrationup()
    {
        inputmanager.vertical = 0;
    }

    public void breakdown()
    {
        brakes = true;
    }
    public void breakup()
    {
        brakes = false;
        inputmanager.handbrake = false;
        inputmanager.vertical = 0;
    }

    public void leftup()
    {
        inputmanager.horizontal = 0;
        left = false;
    }
    public void leftdown()
    {
        left = true;
    }
    public void rightup()
    {
        inputmanager.horizontal = 0;
        right = false;
    }
    public void rightdown()
    {
        right = true;
    }
    public void menu()
    {
            SceneManager.LoadScene(1);
    }

    public void insta()
    {
        Application.OpenURL("https://www.instagram.com/redcherrygames/");
    }

    public void facebook()
    {
        Application.OpenURL("https://www.facebook.com/redcherrygaming");
    }
    public void gamestart(int n)
    {
        PlayerPrefs.SetInt("levels", n-1);
        camer1.SetActive(true);
        if (aioff == false) 
        AI.SetActive(true);
        activeplatform = levels[n].transform.parent.gameObject;
        activeplatform.SetActive(true);
            nextplatform = levels[n-1].transform.parent.gameObject;
            nextplatform.SetActive(true);
        
    }

}
