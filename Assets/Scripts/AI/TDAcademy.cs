using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using UnityEngine.SceneManagement;

public class TDAcademy : Academy
{
    public GameObject gameMaster;
    public GameObject gameMasterOpponent;
    public GameObject cores;
    public GameObject opponentCores;

    public new void AcademyReset()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("Attempting Reset");
        AttackEntity[] entities = FindObjectsOfType<AttackEntity>();
        foreach (AttackEntity entity in entities)
        {
            entity.Kill();
        }
        gameMaster.GetComponent<StatManager>().Restart();
        gameMaster.GetComponent<WaveSpawner>().Restart();
        gameMasterOpponent.GetComponent<WaveSpawner>().Restart();
        cores.GetComponentInChildren<Core>().Restart();
        opponentCores.GetComponentInChildren<Core>().Restart();
    }
}
