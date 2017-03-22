using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenario : MonoBehaviour {

    private Generator gen;
	void Start () {
        gen = GetComponent<Generator>();
        gen.BuildEras();
        
        gen.createNewEra();
        gen.createNewActor(Actor.NewName(4, 10, Actor.defaultConstants, Actor.defaultVowels));
        gen.createNewActor(Actor.NewName(4, 10, Actor.defaultConstants, Actor.defaultVowels));
        gen.createNewActor(Actor.NewName(4, 10, Actor.defaultConstants, Actor.defaultVowels));
        gen.createNewActor(Actor.NewName(4, 10, Actor.defaultConstants, Actor.defaultVowels));
    }
}
