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

                OsopagsSDK.Instance.Analytic.Track(
                    "game_started",
                    result => Debug.Log($"Tracked event: {result.id}"),
                    error => Debug.LogError($"Failed to track event: {error.message}")
                );
            },
            error =>
            {
                Debug.LogError($"Failed to authenticate device: {error.message}");
            }
        );
    }
}
