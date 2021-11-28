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
	private float _basementSoundVolumeBase;
	
	public AudioSource outsideAmbient;
	public AudioSource basementAmbient;
	public AudioClip clickLampSound;
	public AudioClip doorOpen;
	public AudioClip doorClose;
	
	// Start is called before the first frame update
    void Start()
    {
        //Assignation de son propre animator en tant que variable pour pouvoir y accéder plus simplement
        _animator = GetComponent<Animator>();
		_outsideSoundVolumeBase = outsideAmbient.volume;
		_basementSoundVolumeBase = _outsideSoundVolumeBase;
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
		playerTransform.gameObject.GetComponent<AudioSource>().PlayOneShot(doorOpen);
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
			basementAmbient.volume = Math.Min((1.0f - (dis + 3.8f) / 7.6f) * _basementSoundVolumeBase, _basementSoundVolumeBase);
			character.isInBasement = dis < 0;
			character.isInHouse = dis > 0;
		}
	}

	//déclence l'animation de fermeture des portes
    //Y intégrer le jeu d'un son ? Le lancement d'une corroutine ?
    private void OnTriggerExit(Collider other)
    {
		AudioSource source = playerTransform.gameObject.GetComponent<AudioSource>();
        _animator.SetBool(In, false);
		if (isBasementDoor) {
			Character character = playerTransform.gameObject.GetComponent<Character>();
			if (character.isInHouse && character.isLampOn) {
				character.isLampOn = false;
				source.PlayOneShot(clickLampSound);
				character.GetComponent<Light>().enabled = false;
			}
			if (character.isInBasement && !character.isLampOn) {
				character.isLampOn = true;
				source.PlayOneShot(clickLampSound);
				character.GetComponent<Light>().enabled = true;
			}
		}
		source.PlayOneShot(doorClose);
	}

    //Créer une fonction publique à appeler lors d'un animation event ?

}
