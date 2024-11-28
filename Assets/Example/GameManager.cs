using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Osopags.Core;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        OsopagsSDK.Instance.IAM.AuthDevice(
            result =>
            {
                Debug.Log($"Device authenticated: {result.deviceToken}");
            },
            error =>
            {
                Debug.LogError($"Failed to authenticate device: {error.message}");
            }
        );
    }
}
