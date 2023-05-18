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

	// Na pocz�tku
	void Start()
	{
		//Zaczynamy od pobrania Rigidbody oraz BoxCollider2D - pierwszy b�dzie s�u�y� do wykonywania skoku, drugi s�u�y to do pobrania wymiar�w BoxCollidera naszego gracza do metody IsGrounded
		rb = gameObject.GetComponent<Rigidbody2D>();
		boxCollider2D = GetComponent<BoxCollider2D>();
	}

	// Co klatk�
	void Update()
	{
		//Je�li aktualnie nie trwa gra nie wykonuj reszty metody
		if (!GameManager.instance.inGame) return;

		//Najpierw sprawdzamy czy gracz jest na ziemi - za pomoc� metody IsGrounded() (opisanej dok�adniej dalej) je�li tak to ustawiamy flagi jumped i doubleJumped na false, 
		//robimy jeszcze sprawdzenie czasu, aby nie zmieni� za szybko flag i posta� zd��y�a opa�� (Update() jest wywo�ywany co klatk�, na r�nych komputerach jest r�ny czas od�wie�enia klatki )
		if (IsGrounded() && Time.time >= timestamp)
		{
			if (jumped || doubleJumped)
			{
				jumped = false;
				doubleJumped = false;
			}

			timestamp = Time.time + 1f;
		}


		//Pobieramy input przyci�ni�cie przycisku myszy w Unity jest r�wnoznaczne z dotkni�ciem ekranu.
		if (Input.GetMouseButtonDown(0))
		{
			//Je�li gracz chce skoczy� (dotkn�� ekranu) i jeszcze nie skaka� nadajemy mu pr�dko�� skierowan� w g�r� r�wn� polu jumpForce i ustawiamy odpowiedni� flag�
			if (!jumped)
			{
				rb.velocity = (new Vector2(0f, jumpForce));
				jumped = true;
			}
			else if (!doubleJumped)
			{
				//Analogicznie je�eli jeste�my ju� po pierwszym skoku, ale nadal mo�emy zrobi� drugi
				rb.velocity = (new Vector2(0f, jumpForce));
				doubleJumped = true;
			}
		}

		//Je�li gracz ca�y czas przytrzymuje palec na ekranie to co klatk� dodajemy si�� liftingForce przemno�on� przez czas od ostatniej klatki (�eby zniwelowa� wp�yw wahania framerate na gr�). Powoduje to powolniejsze opadanie.
		if (Input.GetMouseButton(0) && rb.velocity.y <= 0)
		{
			rb.AddForce(new Vector2(0f, liftingForce * Time.deltaTime));
		}
	}

	// Metoda IsGrounded wykorzystuje Physics2D.BoxCast sprawdzaj�c czy jaki� Collider obiektu znajduj�cego si� w warstwie okre�lanej jako pod�o�e znajduje si� w prostok�cie o wymiarach gracza (boxCollider2D.bounds.size), tylko �e 0.1f jednostki w d� (Vector2.down)
	private bool IsGrounded()
	{
		RaycastHit2D hit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 0.1f, whatIsGround);
		return hit.collider != null;
	}

	//Metoda zostanie automatycznie wywo�ana w momencie zderzenia z jakimkolwiek
	//colliderem na kt�rym ustawione jest pole IsTrigger
	void OnTriggerEnter2D(Collider2D other)
	{
		//sprawdzamy czy zderzyli�my si� z przeszkod�
		if (other.CompareTag("Obstacle"))
		{
			//Wywo�ujemy metod� odpowiedzialn� za �mier� gracza
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
		//Zamra�amy fizyk� gracza (pozostanie on wtedy w miejscu w kt�rym przegra�
		GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

		//Wywo�ujemy Game Over na managerze gry:
		GameManager.instance.GameOver();
	}

}
