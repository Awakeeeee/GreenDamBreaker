using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The valuable ideas have been mark as "Core point" in comment
[RequireComponent(typeof(CharacterController))]
public class FPSCharacterController : PersistentSingletonBase<FPSCharacterController>
{
	public GameObject lookingAtObject;

	public enum CharacterState
	{
		Idle,
		Walking,
		Running,
		Crouching,
		Jumping
	}
	public CharacterState characterState;

	public Camera cam;
	[SerializeField] MouseTracker mouseTracker = new MouseTracker();
	[SerializeField] bool useHeadBob;
	[SerializeField] BobCurveController headBob = new BobCurveController();
	[SerializeField] bool useFOVEffect;
	public FOVManipulater fovManipulater = new FOVManipulater();
	[SerializeField] bool isWalking;
	[SerializeField] float walkSpeed;
	[SerializeField] float runSpeed;
	[SerializeField] float jumpForce;
	[SerializeField] float gravityMultiplier;

	[Space(10)]
	[Header("Move Cadence Adjustment")]
	[SerializeField] float footStepLength = 5f;		//a virtual step length used as if a timer to loop step sound & body bob effect
	[SerializeField] [Range(0, 2f)] float walkCadenceAdjustment = 0.5f;	//walk speed can be very fast and the sound/bob effect rate is related to walk speed. However, problem is very fast sound/bob rate can be unplesant, so have this variable to adjust it
	[SerializeField] [Range(0, 2f)] float runCadenceAdjustment = 0.3f;

	[Space(10)]
	[Header("Audio")]
	[SerializeField] AudioClip[] footSteps;
	[SerializeField] AudioClip jumpSFX;
	[SerializeField] AudioClip landSFX;

	CollisionFlags moveInfo;	//somehing return by CharacterController.Move, saving relevant info
	CharacterController controller;	//Kinematic Unity built-in character controller, its functionality is abit like the PhysicsController in CorgiEngine
	Vector2 input = Vector2.zero;
	Vector3 frameMovement = Vector3.zero;
	AudioSource soundPlayer;

	float passingDistance;	//the distance that has been passed
	float nextStepPlace;	//a distance value that is reckon to be the next step, thus should play step sound and body bob

	bool callJump;
	//bool isJumping;	//deprecated, use enum state machine
	bool previouslyGrounded;

	float defaultWalkSpeed;
	float defaultRunSpeed;

	void Start()
	{
		controller = GetComponent<CharacterController>();
		mouseTracker.Setup(this.transform, cam.transform);
		headBob.Setup(cam, footStepLength);
		fovManipulater.Setup(cam);
		soundPlayer = GetComponent<AudioSource>();

		characterState = CharacterState.Idle;

		defaultWalkSpeed = walkSpeed;
		defaultRunSpeed = runSpeed;
	}

	//Update checks player state, and set the flags
	//Update() is called later than FixedUpdate()
	void Update()
	{
		mouseTracker.Track(this.transform, cam.transform);

		//if player input attemps to jump this frame
		//Core point : the if doesnot check isJumping state, in this case you press jump when jumping, character will jump once it's grounded. It's like the pre-calculation.
		if(!callJump)
		{
			callJump = Input.GetButtonDown("Jump");
		}

		//if player just got grounded this frame
		if(!previouslyGrounded && controller.isGrounded)
		{
			//TODO shake
			PlayAudioClip(landSFX);
			nextStepPlace += footStepLength; //if not add one step, landing sound and a foot step sound will play at the same time(if character traveled more than one step dist in air)
			frameMovement.y = 0f;	//TODO what is this
			//isJumping = false;
			characterState = CharacterState.Idle;
		}

		//if player start jumping this frame
		if(previouslyGrounded && /*!isJumping*/characterState != CharacterState.Jumping && !controller.isGrounded)
		{
			frameMovement.y = 0f;
		}

		previouslyGrounded = controller.isGrounded;
	}

	//Work out the actual movement vector3 and use CharacterController to move 
	void FixedUpdate()
	{
		float speed;	//either walk speed or run speed depending on input
		GetInput(out speed);

		//Core point : input has xy two values. Map them to move direction in game
		Vector3 inputToMoveDir = transform.forward * input.y + transform.right * input.x;

		RaycastHit hitInfo;
		if(Physics.SphereCast(this.transform.position, controller.radius, Vector3.down, out hitInfo, controller.height / 2f, Physics.AllLayers))
		{
			//Core point : this also handles slope situation, if on slope, the move direction is along the slope
			inputToMoveDir = Vector3.ProjectOnPlane(inputToMoveDir, hitInfo.normal).normalized;
		}

		frameMovement.x = inputToMoveDir.x * speed;
		frameMovement.z = inputToMoveDir.z * speed;

		if(controller.isGrounded)
		{
			frameMovement.y = Physics.gravity.y;	//CharacterController is kinematic, so gravity has to be manually added
			if(callJump)
			{
				PlayAudioClip(jumpSFX);
				frameMovement.y = jumpForce;
				callJump = false;
				//isJumping = true;
				characterState = CharacterState.Jumping;
			}
		}else
		{
			frameMovement += Physics.gravity * gravityMultiplier * Time.deltaTime;
		}

		moveInfo = controller.Move(frameMovement * Time.deltaTime);
	}

	void GetInput(out float speed)
	{
		float inputX = Input.GetAxis("Horizontal");
		float inputY = Input.GetAxis("Vertical");

		bool wasWalking = isWalking;
		isWalking = !Input.GetKey(KeyCode.LeftShift);	//hold shift to run
		speed = isWalking ? walkSpeed : runSpeed;

		//save the input to member variable inputMovement
		input = new Vector2(inputX, inputY);
		if(input.sqrMagnitude > 1f)
		{
			input.Normalize();
		}

		//change state based on input
		if((inputX != 0f || inputY != 0f) && characterState != CharacterState.Jumping && characterState != CharacterState.Crouching)
		{
			if(isWalking)
			{
				characterState = CharacterState.Walking;
			}else{
				characterState = CharacterState.Running;
			}
		}else if(inputX == 0f && inputY == 0f && characterState != CharacterState.Jumping && characterState != CharacterState.Crouching)
		{
			characterState = CharacterState.Idle;
		}

		//if player start to run this frame
		if(wasWalking != isWalking && controller.velocity.sqrMagnitude > 0f && useFOVEffect)
		{
			StopAllCoroutines();
			if(isWalking)
			{
				StartCoroutine(fovManipulater.DecreaseFOV());
			}else{
				StartCoroutine(fovManipulater.IncreaseFOV());
			}
		}

		//TODO: it also make sense to have head bob when crouching, but crouching method has conflict with camera position
		if(characterState == CharacterState.Walking || characterState == CharacterState.Running)
		{
			FootStepCycle(speed);
			HeadBobEffect(speed);
		}
	}

	void PlayAudioClip(AudioClip clip)
	{
		soundPlayer.clip = clip;
		soundPlayer.Play();
	}

	//Core point : imagine this is a timer between two step, which makes a cadence of walking(the cadence of foot step sound and head bob)
	void FootStepCycle(float speed)
	{
		if(controller.velocity.sqrMagnitude > 0 && (input.x != 0f || input.y != 0f))
		{
			//TODO why is the two speed add together?
			passingDistance += (controller.velocity.magnitude + speed) * (isWalking ? walkCadenceAdjustment : runCadenceAdjustment) * Time.deltaTime;
		}

		if(passingDistance <= nextStepPlace)
		{
			return;
		}

		nextStepPlace += footStepLength;
		PlayFootStepAudio();
	}

	void HeadBobEffect(float speed)
	{
		if(!useHeadBob)
			return;

		if(!controller.isGrounded || controller.velocity.sqrMagnitude <= 0f)
			return;

		Vector3 newPos = Vector3.zero;

		newPos = headBob.Bob((controller.velocity.magnitude + speed) * (isWalking ? walkCadenceAdjustment : runCadenceAdjustment));

		cam.transform.localPosition = newPos;
	}

	void PlayFootStepAudio()
	{
		if(!controller.isGrounded)
			return;

		int x = Random.Range(0, footSteps.Length);
		soundPlayer.PlayOneShot(footSteps[x]);
	}

	public void SetMoveSpeed(float walk, float run)
	{
		walkSpeed = walk;
		runSpeed = run;
	}

	public void ResetMoveSpeed()
	{
		walkSpeed = defaultWalkSpeed;;
		runSpeed = defaultRunSpeed;
	}
}
