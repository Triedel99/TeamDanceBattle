﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the outcome of a dance off between 2 dancers, determines the strength of the victory form -1 to 1
/// 
/// TODO:
///     Handle GameEvents.OnFightRequested, resolve based on stats and respond with GameEvents.FightCompleted
///         This will require a winner and defeated in the fight to be determined.
///         This may be where characters are set as selected when they are in a dance off and when they leave the dance off
///         This may also be where you use the BattleLog to output the status of fights
///     This may also be where characters suffer mojo (hp) loss when they are defeated
/// </summary>
public class FightManager : MonoBehaviour
{
    public Color drawCol = Color.gray;

    public float fightAnimTime = 2;

    private void OnEnable()
    {
        GameEvents.OnFightRequested += Fight;
    }

    private void OnDisable()
    {
        GameEvents.OnFightRequested -= Fight;
    }

    public void Fight(FightEventData data)
    {
        StartCoroutine(Attack(data.lhs, data.rhs));
    }

    IEnumerator Attack(Character lhs, Character rhs)
    {
        lhs.isSelected = true;
        rhs.isSelected = true;
        lhs.GetComponent<AnimationController>().Dance();
        rhs.GetComponent<AnimationController>().Dance();

        yield return new WaitForSeconds(fightAnimTime);

        //calulating the luck values for lhs and rhs
        int lhsLuck = Random.Range(0, lhs.luck + 1);
        int rhsLuck = Random.Range(0, rhs.luck + 1);

        //Debug.Log(lhsLuck);
        //Debug.Log(rhsLuck);

        //Calulating the lhs and rhs total battle number
        int lhsResult = lhs.rhythm + lhs.style * lhsLuck;
        int rhsResult = rhs.rhythm + rhs.style * rhsLuck;

        //Debug.Log(lhsResult);
        //Debug.Log(rhsResult);

        float outcome = 1;
        Character winner = lhs, defeated = rhs;

        //Lhs wins
        if (lhsResult > rhsResult)
        {
            outcome = 1;
            winner = lhs;
            defeated = rhs;
            Debug.Log("lhs wins");
        }

        //Rhs wins
        else
        {
            outcome = -1;
            winner = rhs;
            defeated = lhs;
            Debug.Log("rhs wins");
        }

        Debug.Log(outcome);

        
        
        
        Debug.LogWarning("Attack called, needs to use character stats to determine winner with win strength from 1 to -1. This can most likely be ported from previous brief work.");


        Debug.LogWarning("Attack called, may want to use the BattleLog to report the dancers and the outcome of their dance off.");

        var results = new FightResultData(winner, defeated, outcome);

        lhs.isSelected = false;
        rhs.isSelected = false;
        GameEvents.FightCompleted(results);

        yield return null;
    }
}
