using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

	public float jumpForce;
	public float liftingForce;

	public bool jumped;
	public bool doubleJumped;

	public LayerMask whatIsGround;

	private Rigidbody2D rb;
	private float timestamp;
	private BoxCollider2D boxCollider2D;

	// Na pocz¹tku
	void Start()
	{
		//Zaczynamy od pobrania Rigidbody oraz BoxCollider2D - pierwszy bêdzie s³u¿y³ do wykonywania skoku, drugi s³u¿y to do pobrania wymiarów BoxCollidera naszego gracza do metody IsGrounded
		rb = gameObject.GetComponent<Rigidbody2D>();
		boxCollider2D = GetComponent<BoxCollider2D>();
	}

	// Co klatkê
	void Update()
	{
		//Jeœli aktualnie nie trwa gra nie wykonuj reszty metody
		if (!GameManager.instance.inGame) return;

		//Najpierw sprawdzamy czy gracz jest na ziemi - za pomoc¹ metody IsGrounded() (opisanej dok³adniej dalej) jeœli tak to ustawiamy flagi jumped i doubleJumped na false, 
		//robimy jeszcze sprawdzenie czasu, aby nie zmieniæ za szybko flag i postaæ zd¹¿y³a opaœæ (Update() jest wywo³ywany co klatkê, na ró¿nych komputerach jest ró¿ny czas odœwie¿enia klatki )
		if (IsGrounded() && Time.time >= timestamp)
		{
			if (jumped || doubleJumped)
			{
				jumped = false;
				doubleJumped = false;
			}

			timestamp = Time.time + 1f;
		}


		//Pobieramy input przyciœniêcie przycisku myszy w Unity jest równoznaczne z dotkniêciem ekranu.
		if (Input.GetMouseButtonDown(0))
		{
			//Jeœli gracz chce skoczyæ (dotkn¹³ ekranu) i jeszcze nie skaka³ nadajemy mu prêdkoœæ skierowan¹ w górê równ¹ polu jumpForce i ustawiamy odpowiedni¹ flagê
			if (!jumped)
			{
				rb.velocity = (new Vector2(0f, jumpForce));
				jumped = true;
			}
			else if (!doubleJumped)
			{
				//Analogicznie je¿eli jesteœmy ju¿ po pierwszym skoku, ale nadal mo¿emy zrobiæ drugi
				rb.velocity = (new Vector2(0f, jumpForce));
				doubleJumped = true;
			}
		}

		//Jeœli gracz ca³y czas przytrzymuje palec na ekranie to co klatkê dodajemy si³ê liftingForce przemno¿on¹ przez czas od ostatniej klatki (¯eby zniwelowaæ wp³yw wahania framerate na grê). Powoduje to powolniejsze opadanie.
		if (Input.GetMouseButton(0) && rb.velocity.y <= 0)
		{
			rb.AddForce(new Vector2(0f, liftingForce * Time.deltaTime));
		}
	}

	// Metoda IsGrounded wykorzystuje Physics2D.BoxCast sprawdzaj¹c czy jakiœ Collider obiektu znajduj¹cego siê w warstwie okreœlanej jako pod³o¿e znajduje siê w prostok¹cie o wymiarach gracza (boxCollider2D.bounds.size), tylko ¿e 0.1f jednostki w dó³ (Vector2.down)
	private bool IsGrounded()
	{
		RaycastHit2D hit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 0.1f, whatIsGround);
		return hit.collider != null;
	}

	//Metoda zostanie automatycznie wywo³ana w momencie zderzenia z jakimkolwiek
	//colliderem na którym ustawione jest pole IsTrigger
	void OnTriggerEnter2D(Collider2D other)
	{
		//sprawdzamy czy zderzyliœmy siê z przeszkod¹
		if (other.CompareTag("Obstacle"))
		{
			//Wywo³ujemy metodê odpowiedzialn¹ za œmieræ gracza
			PlayerDeath();
		}
		else if (other.CompareTag("Coin"))
		{
			GameManager.instance.CollectCoint();
			Destroy(other.gameObject);

		}



	}

	void PlayerDeath()
	{
		//Zamra¿amy fizykê gracza (pozostanie on wtedy w miejscu w którym przegra³
		GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

		//Wywo³ujemy Game Over na managerze gry:
		GameManager.instance.GameOver();
	}

}
