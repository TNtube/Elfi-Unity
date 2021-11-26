using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
	private Animator _animator;
	private static readonly int In = Animator.StringToHash("In");
	public Transform playerTransform;
	public bool isBasementDoor;

	// Start is called before the first frame update
    void Start()
    {
        //Assignation de son propre animator en tant que variable pour pouvoir y accéder plus simplement
        _animator = GetComponent<Animator>();
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

    //déclence l'animation de fermeture des portes
    //Y intégrer le jeu d'un son ? Le lancement d'une corroutine ?
    private void OnTriggerExit(Collider other)
    {
        _animator.SetBool(In, false);
		var dot = Vector3.Dot(transform.forward, playerTransform.forward);
		playerTransform.gameObject.GetComponent<Character>().isInHouse = isBasementDoor ? dot < 0 : dot > 0;
		playerTransform.gameObject.GetComponent<Character>().isInBasement = isBasementDoor && dot > 0;
	}

    //Créer une fonction publique à appeler lors d'un animation event ?

}
