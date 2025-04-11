using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

public class SendRequest : MonoBehaviour
{

    private string ipToSendTo = "";
    private string filePath = Path.Combine(Application.streamingAssetsPath, "arm_config.json");

    void Start()
    {
        //InitialSetting();
    }

    public void InitialSetting()
    {
        // Ensure StreamingAssets directory exists
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
            Debug.Log("Streaming Asset Path is created");
        }


        if (!File.Exists(filePath))
        {
            // default data used for the the initial robot turn on positioning.
            RobotArmData defaultData = new RobotArmData()
            {
                delay = 20,
                baseJ = 0,
                shoulderJ = 90,
                elbowJ = 50,
                wristVerticalJ = 170,
                wristRotationJ = 40,
                gripperJ = 10
            };

            string json = JsonUtility.ToJson(defaultData, true);
            File.WriteAllText(filePath, json);

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
            Debug.Log("Created new arm_config.json with default values");
        }
        else
        {
            Debug.Log("arm_config.json already exists");
        }
    }

    public void SaveToJSONBeforeSend(GameObject baseJ, GameObject elbowJ, GameObject wristVertJ)
    {
        // Always clear existing file first
        if (File.Exists(filePath))
        {
            File.WriteAllText(filePath, string.Empty); // Clear contents
        }

        int delay = 20;
        int baseValue = (int)baseJ.transform.localEulerAngles.y;
        int shoulderValue = 0;
        int elbowValue = (int)elbowJ.transform.localEulerAngles.z;
        int wristVertValue = (int)wristVertJ.transform.localEulerAngles.z;
        int wristRotValue = 40;
        int gripperValue = 10;

        RobotArmData armData = new RobotArmData()
        {
            delay = delay,
            baseJ = baseValue,
            shoulderJ = shoulderValue,
            elbowJ = elbowValue,
            wristVerticalJ = wristVertValue,
            wristRotationJ = wristRotValue,
            gripperJ = gripperValue

        };

        // Convert to JSON
        string json = JsonUtility.ToJson(armData, true);
       

        // Save file
        //string filePath = Path.Combine(Application.streamingAssetsPath, "robotdata.json");
        File.WriteAllText(filePath, json);

        // Refresh editor to see changes immediately
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif

        Debug.Log("Saved JSON to: " + filePath);

        /*
        {
        ​"delay" : 20,
        ​"base" : 0,
        ​"shoulder" : 90,
        ​"elbow" : 50,
        ​"wristVertical" : 170,
        ​"wristRotation" : 40,
        ​"gripper" : 10
        }
         */
        //StartCoroutine(SendGetRequestToRobot(ipToSendTo));
    }

    IEnumerator SendGetRequestToRobot(string url)
    {
        // Load the JSON file from the StreamingAssets folder
        string jsonFilePath = Path.Combine(Application.streamingAssetsPath, "robotdata.json");
        string jsonData;

        // Read the JSON file (works in Editor and standalone builds)
        if (jsonFilePath.Contains("://") || jsonFilePath.Contains(":///"))
        {
            // Handle Android/WWW-style paths
            UnityWebRequest fileReader = UnityWebRequest.Get(jsonFilePath);
            yield return fileReader.SendWebRequest();
            jsonData = fileReader.downloadHandler.text;
        }
        else
        {
            // Read directly from the file system (Editor/Windows/Mac)
            jsonData = File.ReadAllText(jsonFilePath);
        }

        // Create the POST request
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request
        yield return request.SendWebRequest();

        // Handle the response
        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Error: {request.error}");
        }
        else
        {
            Debug.Log($"Response: {request.downloadHandler.text}");
        }
    }
}
