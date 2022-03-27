using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.SceneManagement;

class BuilderManager : MonoBehaviour
{
    public GameState gameState;

    public void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            // var objects = SceneManager.GetActiveScene().GetRootGameObjects();
            Debug.Log("Key down q");
        }
    }
}
