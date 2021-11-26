using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
	private Animator _animator;
	private static readonly int In = Animator.StringToHash("In");
	public Transform playerTransform;
	public bool isBasementDoor;

	private float _outsideSoundVolumeBase;
	
	public AudioSource outsideAmbient;
	public AudioClip clickLampSound; 
	
	// Start is called before the first frame update
    void Start()
    {
        //Assignation de son propre animator en tant que variable pour pouvoir y accéder plus simplement
        _animator = GetComponent<Animator>();
		_outsideSoundVolumeBase = outsideAmbient.volume;
	}

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //déclence l'animation d'ouverture des portes
    //Y intégrer le jeu d'un son ? Le lancement d'une corroutine ?
    private void OnTriggerEnter(Collider other)
    {
        _animator.SetBool(In, true);
	}
	private void OnTriggerStay(Collider other) {
		Character character = playerTransform.gameObject.GetComponent<Character>();
		if (!isBasementDoor) {
			var dis = transform.position.z - playerTransform.position.z;
			outsideAmbient.volume = (1.0f - (dis + 3.8f) / 7.6f) * _outsideSoundVolumeBase;
			character.isInHouse = dis > 0;
		}
		else {
			var dis = transform.position.x - playerTransform.position.x;
			outsideAmbient.volume = Math.Min((1.0f - (dis + 3.8f) / 7.6f) * _outsideSoundVolumeBase, _outsideSoundVolumeBase);
			character.isInBasement = dis < 0;
			character.isInHouse = dis > 0;
		}
	}

	//déclence l'animation de fermeture des portes
    //Y intégrer le jeu d'un son ? Le lancement d'une corroutine ?
    private void OnTriggerExit(Collider other)
    {
        _animator.SetBool(In, false);
		if (isBasementDoor) {
			Character character = playerTransform.gameObject.GetComponent<Character>();
			AudioSource source = playerTransform.gameObject.GetComponent<AudioSource>();
			if (character.isInHouse && character.isLampOn) {
				character.isLampOn = false;
				float vol = source.volume;
				source.volume = 2f;
				source.PlayOneShot(clickLampSound);
				source.volume = vol;
				character.GetComponent<Light>().enabled = false;
			}
			if (character.isInBasement && !character.isLampOn) {
				character.isLampOn = true;
				float vol = source.volume;
				source.volume = 2f;
				source.PlayOneShot(clickLampSound);
				source.volume = vol;
				character.GetComponent<Light>().enabled = true;
			}
		}
	}

    //Créer une fonction publique à appeler lors d'un animation event ?

}
