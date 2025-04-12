using OVRSimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

public class SendRequest : MonoBehaviour
{

    private string ipToSendTo = "";
    private string filePath;

    void Start()
    {
        InitialSetting();
    }

    private void InitialSetting()
    {
        // Ensure StreamingAssets directory exists
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
            Debug.Log("Streaming Asset Path is created");
        }
        else
        {
            Debug.Log("Streaming Asset Path already exists");
        }

        filePath = Path.Combine(Application.streamingAssetsPath, "arm_config.json");
        if (!File.Exists(filePath))
        {
            //default data used for the the initial robot turn on positioning.

           RobotArmData defaultData = new RobotArmData()
           {
               delay = 20,
               @base = 0,
               shoulder = 90,
               elbow = 50,
               wristVertical = 170,
               wristRotation = 40,
               gripper = 10
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
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
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
        int elbowValue = (int)normalizeNegatives(elbowJ.transform.localEulerAngles.z);
        int wristVertValue = (int)normalizeNegatives(wristVertJ.transform.localEulerAngles.z);

        int shoulderValue = 0;
        int wristRotValue = 40;
        int gripperValue = 10;

        RobotArmData armData = new RobotArmData()
        {
            delay = delay,
            @base = baseValue,
            shoulder = shoulderValue,
            elbow = elbowValue,
            wristVertical = wristVertValue,
            wristRotation = wristRotValue,
            gripper = gripperValue
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
        //StartCoroutine(SendGetRequestToRobot(ipToSendTo, json));
    }

    private IEnumerator SendGetRequestToRobot(string url, string thejson)
    {
        yield return new WaitForSecondsRealtime(1f);
        // Create the POST request
        /*
         * as long as both the sender and receiver use the same encoding 
         * (UTF-8 is the most common and default in many cases), the conversion is 
         * lossless. The server will take the byte stream, decode it into a string, 
         * and then parse that string into JSON.
         */
        Debug.Log("NOW SENDING");
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(thejson);

            request.uploadHandler = new UploadHandlerRaw(jsonBytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // Send the request
            yield return request.SendWebRequest();

            // Handle response
            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
                Debug.Log("Response: " + request.downloadHandler.text);
            }
            else
            {
                Debug.Log("Success! Response: " + request.downloadHandler.text);
            }
        }
    }

    //private IEnumerator SendGetRequestToRobot(string url, string thejson)
    //{
    //    // Load the JSON file from the StreamingAssets folder
    //    yield return new WaitForSecondsRealtime(1f);
    //    //string jsonFilePath = Path.Combine(Application.streamingAssetsPath, "arm_config.json");
    //    string jsonData;
    //    // Read the JSON file (works in Editor and standalone builds)
    //    if (filePath.Contains("://") || filePath.Contains(":///"))
    //    {
    //        // Handle Android/WWW-style paths
    //        UnityWebRequest fileReader = UnityWebRequest.Get(filePath);
    //        yield return fileReader.SendWebRequest();
    //        jsonData = fileReader.downloadHandler.text;
    //    }
    //    else
    //    {
    //        // Read directly from the file system (Editor/Windows/Mac)
    //        jsonData = File.ReadAllText(filePath);
    //    }

    //    // Create the POST request
    //    UnityWebRequest request = new UnityWebRequest(url, "POST");
    //    byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
    //    request.uploadHandler = new UploadHandlerRaw(jsonToSend);
    //    request.downloadHandler = new DownloadHandlerBuffer();
    //    request.SetRequestHeader("Content-Type", "application/json");

    //    // Send the request
    //    yield return request.SendWebRequest();

    //    // Handle the response
    //    if (request.result == UnityWebRequest.Result.ConnectionError ||
    //        request.result == UnityWebRequest.Result.ProtocolError)
    //    {
    //        Debug.LogError($"Error: {request.error}");
    //    }
    //    else
    //    {
    //        Debug.Log($"Response: {request.downloadHandler.text}");
    //    }
    //}

    private float normalizeNegatives(float value)
    {
        value %= 360;
        if (value > 180)
        {
            value -= 360;
        }
        return value;
    }
}
