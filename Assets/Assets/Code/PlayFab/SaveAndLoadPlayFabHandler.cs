using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndLoadPlayFabHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var cacheSettings = new ES3Settings(ES3.Location.Cache);
        ES3.Save("transform", this.transform, cacheSettings);
        ES3.DeleteFile("transform");


       
    }



    private void saveCache()
    {
        var cacheSettings = new ES3Settings(ES3.Location.Cache);
        ES3.Save("transform", this.transform, cacheSettings);
        byte[] bytes = ES3.LoadRawBytes(cacheSettings);

        loadCache(bytes);
        
    }
    

    private void loadCache(byte[] bytes)
    {
        var cacheSettings = new ES3Settings(ES3.Location.Cache);
        ES3.SaveRaw(bytes, cacheSettings);
        ES3.LoadInto("transform", this.transform, cacheSettings);
    }
}
