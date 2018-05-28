using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.Sockets;
using System.Net;
using System;
using UnityEngine.XR;
using UnityEngine.SceneManagement;


public class CustomNetwork : MonoBehaviour
{

    GameObject Player1;
    GameObject Player2;
    GameObject Player1ScoreBox;
    GameObject Player2ScoreBox;
    GameObject PlayerWaitingMessage;
    GameObject Key;
    IPAddress ServerIPAddress;
    public Camera Player1Camera;
    public Camera Player2Camera;
    public GameObject SpawnedObject;
    string ServerIP = "255.255.255.255";
    int ServerPort = 30663;
    IPEndPoint ServerEndPoint;
    IPEndPoint LocalEndPoint;
    public static CustomNetwork instance = null;
    int LocalPort = 0;
    UdpClient client;
    bool flag = false;
    bool test = false;
    string LocalPlayerID = "0";
    bool Player1State = false;
    bool Player2State = false;
    float TimeScaleValue = 0.0f;
    float rx1 = 0.0f; // position of player 1
    float ry1 = 0.0f;
    float rz1 = 0.0f;
    float rx2 = 0.0f; // position of player 2
    float ry2 = 0.0f;
    float rz2 = 0.0f;
    float qx1 = 0.0f; // rotation of player 1 (in quaternion form)
    float qy1 = 0.0f;
    float qz1 = 0.0f;
    float qw1 = 0.0f;
    float qx2 = 0.0f; // rotation of player 2 (in quaternion form)
    float qy2 = 0.0f;
    float qz2 = 0.0f;
    float qw2 = 0.0f;
    float ox = 0.0f; // player 1's object coordinates
    float oy = 0.0f;
    float oz = 0.0f;
    string objectName = ""; // player 1's object name
    void Awake()
    {
        ServerIP = PlayerPrefs.GetString("gameserver_ip");
        XRSettings.enabled = false;

        //Check if instance already exists
        if (instance == null)
            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
    }



    // Use this for initialization
    public void Start()
    {
        ServerIPAddress = IPAddress.Parse(ServerIP);
        ServerEndPoint = new IPEndPoint(ServerIPAddress, ServerPort);
        LocalEndPoint = new IPEndPoint(IPAddress.Any, LocalPort);
        client = new UdpClient(LocalEndPoint);
        DisablePlayers();
        RequestPlayerFromServer();
        ReceiveMsg();
    }

    public void DisablePlayers()
    {
        Player1 = GameObject.Find("Player1");
        Player2 = GameObject.Find("Player2");
        PlayerWaitingMessage = GameObject.Find("PlayerWaitingMessage");
        Player1ScoreBox = GameObject.Find("Player1ScoreBox");
        Player2ScoreBox = GameObject.Find("Player2ScoreBox");
        Player1.SetActive(false);
        Player2.SetActive(false);
    }

    public void RequestPlayerFromServer()
    {
        SendMsg("request,0");
    }

    public void CreateRemotePlayer()
    {
        if (LocalPlayerID.Equals("1"))
        {
            Player2State = true;
            flag = true;
        }
        else if (LocalPlayerID.Equals("2"))
        {
            Player1State = true;
            flag = true;
        }
    }

    public void ReceiveCallBack(IAsyncResult ar)
    {
        UdpClient client = (UdpClient)((ar.AsyncState));
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, LocalPort);

        System.Byte[] receivedBytes = client.EndReceive(ar, ref endPoint);
        string receivedString = System.Text.Encoding.ASCII.GetString(receivedBytes);

        Debug.Log("Received: " + receivedString);
        ReceiveMsg();
        ProcessMsg(receivedString);
    }

    public void ReceiveMsg()
    {
        Debug.Log("Listening for messages");
        client.BeginReceive(new System.AsyncCallback(ReceiveCallBack), client);
    }

    public void ProcessMsg(string msg)
    {
        Debug.Log("Processing messages");
        string[] substrings = msg.Split(',');
        Debug.Log("Received value " + substrings[0]);
        if (substrings[0].Equals("createplayer"))
        {
            Debug.Log("Create player command received from server");
            if (substrings[1].Equals("1"))
            {
                Player1State = true;
                LocalPlayerID = "1";
                Debug.Log("Player 1 has joined the server");
            }
            else
            {
                Player2State = true;
                LocalPlayerID = "2";
                Debug.Log("Player 2 has joined the server");
            }
        }
        else if (substrings[0].Equals("request"))
        {
            CreateRemotePlayer();
            Debug.Log("Remote player created");
        }
        else if (substrings[0].Equals("position1+rotation1"))
        {
            float x = (float)System.Convert.ToDouble(substrings[1]);
            float y = (float)System.Convert.ToDouble(substrings[2]);
            float z = (float)System.Convert.ToDouble(substrings[3]);
            float x1 = (float)System.Convert.ToDouble(substrings[4]);
            float y1 = (float)System.Convert.ToDouble(substrings[5]);
            float z1 = (float)System.Convert.ToDouble(substrings[6]);
            float w1 = (float)System.Convert.ToDouble(substrings[7]);
            double packetSentTime = System.Convert.ToDouble(substrings[8]);
            double packetArrivalTime = System.Convert.ToDouble(DateTime.Now.ToString("HHmmss.fff")); // Hours (24), minutes, seconds, miliseconds
            double timeDiff = packetArrivalTime - packetSentTime;
            Debug.Log("Packet p1 received after: " + timeDiff + " seconds");
            if (!flag)
            {
                CreateRemotePlayer();
            }
            rx1 = x; // position of player 1 
            ry1 = y;
            rz1 = z;
            qx1 = x1; // rotation of player 1's camera (in quaternion form)
            qy1 = y1;
            qz1 = z1;
            qw1 = w1;
        }
        else if (substrings[0].Equals("position2+rotation2"))
        {
            float x = (float)System.Convert.ToDouble(substrings[1]);
            float y = (float)System.Convert.ToDouble(substrings[2]);
            float z = (float)System.Convert.ToDouble(substrings[3]);
            float x1 = (float)System.Convert.ToDouble(substrings[4]);
            float y1 = (float)System.Convert.ToDouble(substrings[5]);
            float z1 = (float)System.Convert.ToDouble(substrings[6]);
            float w1 = (float)System.Convert.ToDouble(substrings[7]);
            double packetSentTime = System.Convert.ToDouble(substrings[8]);
            double packetArrivalTime = System.Convert.ToDouble(DateTime.Now.ToString("HHmmss.fff")); // Hours (24), minutes, seconds, miliseconds
            double timeDiff = packetArrivalTime - packetSentTime;
            Debug.Log("Packet p2 received after: " + timeDiff + " seconds");
            if (!flag)
            {
                CreateRemotePlayer();
            }
            rx2 = x; // position of player 2 
            ry2 = y;
            rz2 = z;
            qx2 = x1; // rotation of player 2's camera (in quaternion form)
            qy2 = y1;
            qz2 = z1;
            qw2 = w1;
        }
        else if (substrings[0].Equals("score"))
        {
            if (LocalPlayerID.Equals("1"))
            {
                Player1Score.player1ScoreInstance.SetScore(substrings[1]);
                Player1OpponentScore.player1OpponentScoreInstance.SetScore(substrings[2]);
            }
            if (LocalPlayerID.Equals("2"))
            {
                Player2Score.player2ScoreInstance.SetScore(substrings[2]);
                Player2OpponentScore.player2OpponentScoreInstance.SetScore(substrings[1]);
            }
        }
        else if (substrings[0].Equals("objectDestroyed1"))
        {
            if (LocalPlayerID.Equals("2"))
            {
                // find object by its name
                GameObject tempObject = GameObject.Find(substrings[1]);

                // destroy the object if it has not been destroyed yet
                if (tempObject != null)
                {
                    DestroyObject(tempObject);
                    Debug.Log("Object " + substrings[1] + " has been destroyed by " + substrings[2]);
                }
            }
        }
        else if (substrings[0].Equals("objectDestroyed2"))
        {
            if (LocalPlayerID.Equals("1"))
            {
                // find object by its name
                GameObject tempObject = GameObject.Find(substrings[1]);

                // destroy the object if it has not been destroyed yet
                if (tempObject != null)
                {
                    DestroyObject(tempObject);
                    Debug.Log("Object " + substrings[1] + " has been destroyed by " + substrings[2]);
                }
            }
        }
        else if (substrings[0].Equals("objectLocation"))
        {
            Debug.Log("Object location received");
            // get the name for the object that was generated by the server
            string name = System.Convert.ToString(substrings[1]);

            // get the coordinates for the object that was generated by the server
            float x = (float)System.Convert.ToDouble(substrings[2]);
            float y = (float)System.Convert.ToDouble(substrings[3]);
            float z = (float)System.Convert.ToDouble(substrings[4]);

            // assigned the name and coordinates to the global variables
            objectName = name;
            ox = x;
            oy = y;
            oz = z;
            test = true;
        }
        else
        {
            Debug.Log("Not equal");
        }
    }

    public void SendDisconnectedMsg()
    {
        if (LocalPlayerID.Equals("1"))
        {
            SendMsg("disconnect_one,0");
            Player1State = false;
            Player1.SetActive(false);
        }
        else
        {
            SendMsg("disconnect_two,0");
            Player2State = false;
            Player2.SetActive(false);
        }
    }

    public void SendObjectDestroyedMsg(string ObjectID, string PlayerID)
    {
        if (LocalPlayerID.Equals("1"))
        {
            if (PlayerID == "Player1")
            {
                SendMsg("object_destroyed1," + ObjectID.Trim() + "," + PlayerID.Trim());
            }
        }
        else if (LocalPlayerID.Equals("2"))
        {
            if (PlayerID == "Player2")
            {
                SendMsg("object_destroyed2," + ObjectID.Trim() + "," + PlayerID.Trim());
            }
        }
    }

    public void SendMsg(string msg)
    {
        byte[] send_buffer = System.Text.Encoding.ASCII.GetBytes(msg);
        client.Send(send_buffer, send_buffer.Length, ServerEndPoint);
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = TimeScaleValue;
        if (Player1State)
        {
            Player1.SetActive(true);
        }
        if (Player2State)
        {
            Player2.SetActive(true);
        }
        if (flag)
        {
            // if both players have joined the game
            if (Player2State == true && Player1State == true)
            {
                // disable "waiting for your opponent to connect" message
                PlayerWaitingMessage.SetActive(false);

                // enable VR mode
                XRSettings.enabled = true;

                // set time scale to 1 = runtime
                TimeScaleValue = 1.0f;
            }
            if (test)
            {
                if (GameObject.Find(objectName) == null)
                {
                    // spawn object(s) at x,y,z
                    Key = Instantiate(SpawnedObject, new Vector3(ox, oy, oz), transform.rotation);
                    Key.tag = "SpawnedObject";
                    Key.name = objectName;
                }
            }
            if (LocalPlayerID.Equals("1"))
            {
                // sync the position of player 2 on player 1's screen
                Player2.transform.position = new Vector3(rx2, ry2, rz2);

                // sync the rotation of player2's camera on player 1's screen
                Player2Camera.transform.rotation = new Quaternion(qx2, qy2, qz2, qw2);
            }
            else if (LocalPlayerID.Equals("2"))
            {
                // sync the position of player 1 on player 2's screen
                Player1.transform.position = new Vector3(rx1, ry1, rz1);

                // sync the rotation of player1's camera on player 2's screen
                Player1Camera.transform.rotation = new Quaternion(qx1, qy1, qz1, qw1);
            }
        }
        if (LocalPlayerID.Equals("1"))
        {
            // turn off scoreboard of player 2 on player 1's screen
            Player2ScoreBox.SetActive(false);

            // turn on player 1's camera and turn off player 2's camera
            Player1Camera.enabled = true;
            Player2Camera.enabled = false;

            // get value of player 1's position (in string format)
            string position = Player1.transform.position.ToString();

            // get quaternion value of player 1 camera's rotation (in string format)
            string rotation = Player1Camera.transform.rotation.ToString();

            // get the time that the packet was sent
            string sendTime = DateTime.Now.ToString("HHmmss.fff");

            // send the position and rotation value of player 1 to the server
            SendMsg("position1+rotation1," + position.Replace("(", "").Replace(")", "").Trim() + "," + rotation.Replace("(", "").Replace(")", "").Trim() + "," + sendTime);
        }
        else if (LocalPlayerID.Equals("2"))
        {
            // turn off scoreboard of player 1 on player 2's screen
            Player1ScoreBox.SetActive(false);

            // turn on player 2's camera and turn off player 1's camera
            Player1Camera.enabled = false;
            Player2Camera.enabled = true;

            // get value of player 2's position (in string format)
            string position = Player2.transform.position.ToString();

            // get quaternion value of player 2 camera's rotation (in string format)
            string rotation = Player2Camera.transform.rotation.ToString();

            // get the time that the packet was sent
            string sendTime = DateTime.Now.ToString("HHmmss.fff");

            // send the position and rotation value of player 2 to the server
            SendMsg("position2+rotation2," + position.Replace("(", "").Replace(")", "").Trim() + "," + rotation.Replace("(", "").Replace(")", "").Trim() + "," + sendTime);
        }
        // send request for scores to the server
        SendMsg("request_scores,0");
    }

    public void OnApplicationQuit()
    {
        Debug.Log("Quitting Application");
        SendDisconnectedMsg();
    }
}
