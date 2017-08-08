using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechFlyFSM : AIStateMachine
{
	[Header("Components")]
	public Animator modelAnim;
	public Transform model;
	public Transform muzzlePos;
	public AudioSource audioSource;
	public ParticleSystem fireFlarePrefab;
	private ParticleSystem fireFlare;
	private CharacterHP player;
	public CharacterHP Player{get{return player;}}

	[Header("FSM States")]
	public MechFlyWait wait = new MechFlyWait();
	public MechFlyMove move = new MechFlyMove();
	public MechFlyAttack attack = new MechFlyAttack();

	//all in world pos
	public Vector3 worldCenter;
	public Vector3 moveRange;
	public Vector2 RangeX{
		get{
			return new Vector2(worldCenter.x - moveRange.x, worldCenter.x + moveRange.x);
		}
	}
	public Vector2 RangeY{
		get{
			return new Vector2(worldCenter.y - moveRange.y, worldCenter.y + moveRange.y);
		}
	}
	public Vector2 RangeZ{
		get{
			return new Vector2(worldCenter.z - moveRange.z, worldCenter.z + moveRange.z);
		}
	}

	void Start()
	{
		audioSource = GetComponent<AudioSource>();
		player = FPSCharacterController.Instance.GetComponent<CharacterHP>();

		if(fireFlarePrefab != null)
		{
			fireFlare = Instantiate(fireFlarePrefab, muzzlePos);
			fireFlare.transform.localPosition = Vector3.zero;
			fireFlare.gameObject.SetActive(false);
		}

		wait.Initialize(this);
		move.Initialize(this);
		attack.Initialize(this);

		currentState = wait;
	}

	void Update()
	{
		currentState.StateUpdate();
	}

	public void ShowFireFlare()
	{
		fireFlare.gameObject.SetActive(true);
	}
}
