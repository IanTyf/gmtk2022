using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnbaseController : MonoBehaviour
{
    #region Reference

    public GameObject player;
    public GameObject Monster;
    public roll playerScript;
    public Monster_Turnbase monsterScript;

    #endregion

    #region State

    //pre round
    [SerializeField]
    private bool prePlayerExpo;

    //cur round
    [SerializeField]
    private bool curPlayerExpo;
    //next round


    #endregion

    [SerializeField]
    private bool startTest = false;
    [SerializeField]
    private float testingTime = 2;
    [SerializeField]
    private float testingTimer = 0;

    private void Awake()
    {
        
    }

    private void Start()
    {
        prePlayerExpo = false;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            startTest = true;
        }

        if (startTest)
        {
            testingTimer -= Time.deltaTime;
            if (testingTimer < 0)
            {
                newTurn();
                testingTimer = testingTime;
                startTest = false;
            }

        }


    }

    //execute when player finished one move// player speed = 0 
    public void newTurn()
    {
        /*
         * 怪物根据上一回合玩家的位置行动
         * 记录这一回合玩家的信息
         *推算下一个回合怪物的移动
         * check上回合玩家
         * monster check: has voice?
         * monster check: saw player?
         * monster state update : patrol? chase player? idle?
         * monster execute
         * 
         */
        //player.PlayerRound()

        UpdateCurPlayerData();

        Monster.GetComponent<Monster_Turnbase>().MonsterRound(prePlayerExpo,curPlayerExpo);
        if (player.GetComponent<roll>().playerMode != roll.Mode.Dead)
        {
            //怪物根据上一回合玩家的位置行动
            //上一回合被发现
            if (prePlayerExpo)
            {
                //patrol to attention
                if (Monster.GetComponent<Monster_Turnbase>().mode_Monster == Monster_Turnbase.Mode.Partrol)
                {
                    Monster.GetComponent<Monster_Turnbase>().mode_Monster = Monster_Turnbase.Mode.Attention;
                }
                //attention to chase
                else if (Monster.GetComponent<Monster_Turnbase>().mode_Monster == Monster_Turnbase.Mode.Attention)
                {
                    Monster.GetComponent<Monster_Turnbase>().mode_Monster = Monster_Turnbase.Mode.Chase;
                }
                // attention to patrol
                else
                {
                    
                }

            }
            //上一回合没被发现
            else
            {
                Monster.GetComponent<Monster_Turnbase>().mode_Monster = Monster_Turnbase.Mode.Partrol;
            }
        
            //* 记录这一回合玩家的信息
        }
    }

    void UpdateCurPlayerData()
    {
        if (player.GetComponent<roll>().playerMode == roll.Mode.MakeSound
            ||
            monsterScript.DetectPlayer())
        {
            curPlayerExpo = true;
        }
        else
        {
            curPlayerExpo = false;
        }
    }
}