using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class CheatHandler : MonoBehaviour
{
    public void checkCheater(int stages)
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "validateStage",
            FunctionParameter = new
            {
                stages = stages
            }
        };
        PlayFabClientAPI.ExecuteCloudScript(request, OnExecuteSuccess, OnError);
    }


    void OnExecuteSuccess(ExecuteCloudScriptResult result)
    {
        Debug.Log("Cheater checked");
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error while logging in/creating account!");
        Debug.Log(error.GenerateErrorReport());
    }

}

