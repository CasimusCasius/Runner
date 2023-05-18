using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	//Tworzymy statyczn� zmienn� przechowuj�c� jedyny obiekt klasy GameManager (wg. wzorca Singletonu)
	//Pozwoli to na odwo��nie si� do GameManagera w dowolnym miejscu projektu poprzez GameManager.instance
	public static GameManager instance;

	//Pole na element UI Text s�u��cy do wy�wietlania wyniku
	public Text scoreText;

	//Pole zawieraj�ce pr�dko�� �wiata do kt�rego b�dzie m�g� odwo�a� si� dowolny obiekt w grze
	//Domy�lnie ustawiamy 0.2f, ale warto�� mo�na dopasowa� w edytorze
	public float worldScrollingSpeed = 0.2f;

	//pole w kt�rym b�dziemy pami�ta� czy aktualnie trwa gra
	public bool inGame;

	//pole do podpi�cia przycisku reset
	public GameObject resetButton;

	//Pole na wynik
	private float score;

	// Use this for initialization
	void Start()
	{
		//Podczas uruchomienia przypisujemy aktualn� instancj� do statycznego pola instance
		//!!! Nale�y uwa�a�, �eby zawsze na scenie by� dok�adnie jeden GameManager !!!
		if (instance == null) { instance = this; }

		//Inicjalizujemy gr�
		InitializeGame();

	}

	void InitializeGame()
	{
		//Ustawiamy pole m�wi�ce, �e jeste�my  w trakcie gry
		inGame = true;
	}

	void FixedUpdate()
	{
		//Je�li aktualnie nie trwa gra nie wykonuj reszty metody
		if (!GameManager.instance.inGame) return;

		//Co tick silnika fizyki dopisujemy do wyniku przebyt� odleg�o�� i wywo�ujemy metod� wy�wietlaj�c� wynik na ekranie
		score += worldScrollingSpeed;
		UpdateOnScreenScore();
	}

	void UpdateOnScreenScore()
	{
		//Wy�wietlamy na elemencie nasz wynik bez cz�ci dziesi�tnej
		scoreText.text = score.ToString("0");
	}

	public void GameOver()
	{
		//gra si� sko�czy�a, wi�c:
		inGame = false;
		//Wy�wietlamy przycisk Restart
		resetButton.SetActive(true);
	}

	public void RestartGame()
	{
		//Ponownie �adujemy scen� o indeksie 0 (czyli jedyn� w naszej grze)
		//Spowoduje to reset gry
		SceneManager.LoadScene(0);
	}
}
