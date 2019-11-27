using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class Brain : MonoBehaviour
{
    public int DNALength = 1;
    public float timeAlive;
    public float distanceTravelled;
    Vector3 startPosition;
    public DNA dna;

    private ThirdPersonCharacter _m_Character;
    private Vector3 _m_Move;
    private bool _m_Jump;
    bool alive = true;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "dead")
        {
            this.alive = false;
        }
    }

    public void Init()
    {
        // initialise DNA
        // 0 forward
        // 1 back
        // 2 left
        // 3 right
        // 4 jump
        // 5 crouch
        this.dna = new DNA(this.DNALength, 6);
        this._m_Character = GetComponent<ThirdPersonCharacter>();
        this.timeAlive = 0;
        this.alive = true;
        this.startPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        // read DNA
        float h = 0;
        float v = 0;
        bool crouch = false;

        if(this.dna.GetGene(0) == 0) { v = 1; }
        else if(this.dna.GetGene(0) == 1) { v = -1; }
        else if (this.dna.GetGene(0) == 2) { h = -1; }
        else if (this.dna.GetGene(0) == 3) { h = 1; }
        else if (this.dna.GetGene(0) == 4) { this._m_Jump = true; }
        else if (this.dna.GetGene(0) == 5) { crouch = true; }

        this._m_Move = v * Vector3.forward + h * Vector3.right;
        this._m_Character.Move(this._m_Move, crouch, this._m_Jump);
        this._m_Jump = false;
        if(this.alive)
        {
            this.timeAlive += Time.deltaTime;
            this.distanceTravelled = Vector3.Distance(this.transform.position, this.startPosition);
        }
    }
}
