﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulationManager : MonoBehaviour
{
    public GameObject botPrefab;
    public int populationSize = 50;
    List<GameObject> population = new List<GameObject>();
    public static float elapsed = 0;
    public float trialTime = 5;
    int generation = 1;

    GUIStyle guiStyle = new GUIStyle();
    private void OnGUI()
    {
        guiStyle.fontSize = 25;
        guiStyle.normal.textColor = Color.white;
        GUI.BeginGroup(new Rect(10, 10, 250, 150));
        GUI.Box(new Rect(0, 0, 140, 140), "Stats", guiStyle);
        GUI.Label(new Rect(10, 25, 200, 30), "Gen: " + this.generation, guiStyle);
        GUI.Label(new Rect(10, 50, 200, 30), string.Format("Time: {0:0.00}", elapsed), guiStyle);
        GUI.Label(new Rect(10, 75, 200, 30), "Population: " + this.population.Count, guiStyle);
        GUI.EndGroup();
    }

    // Use this for initialization
    private void Start()
    {
        for(int i = 0; i < this.populationSize; i++)
        {
            Vector3 startingPos = new Vector3(this.transform.position.x + Random.Range(-2, 2),
                                                this.transform.position.y,
                                                this.transform.position.z + Random.Range(-2, 2));

            GameObject b = Instantiate(this.botPrefab, startingPos, this.transform.rotation);
            b.GetComponent<Brain>().Init();
            this.population.Add(b);
        }
    }

    GameObject Breed(GameObject parent1, GameObject parent2)
    {
        Vector3 startingPos = new Vector3(this.transform.position.x + Random.Range(-2, 2),
                                                this.transform.position.y,
                                                this.transform.position.z + Random.Range(-2, 2));

        GameObject offspring = Instantiate(this.botPrefab, startingPos, this.transform.rotation);
        Brain b = offspring.GetComponent<Brain>();
        b.Init();
        if (Random.Range(0, 100) == 1) // mutate 1 in 100
        {
            b.dna.Mutate();
        }
        else
        {
            b.dna.Combine(parent1.GetComponent<Brain>().dna, parent2.GetComponent<Brain>().dna);
        }
        return offspring;
    }

    void BreedNewPopulation()
    {
        // time alive as fitness criteria
        // List<GameObject> sortedList = this.population.OrderBy(o => o.GetComponent<Brain>().timeAlive).ToList();
        // distance travelled as fitness criteria
        List<GameObject> sortedList = this.population.OrderBy(o => o.GetComponent<Brain>().distanceTravelled).ToList();

        this.population.Clear();
        // breed upper half of sorted list
        for (int i = (int)(sortedList.Count / 2.0f) - 1; i < sortedList.Count - 1; i++)
        {
            this.population.Add(Breed(sortedList[i], sortedList[i + 1]));
            this.population.Add(Breed(sortedList[i + 1], sortedList[i]));
        }

        // destroy all parents and previous population
        for (int i = 0; i < sortedList.Count; i++)
        {
            Destroy(sortedList[i]);
        }
        this.generation++;
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        if(elapsed >= this.trialTime)
        {
            BreedNewPopulation();
            elapsed = 0;
        }
    }
}
