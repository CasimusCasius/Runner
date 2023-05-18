using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	//Tworzymy statyczn¹ zmienn¹ przechowuj¹c¹ jedyny obiekt klasy GameManager (wg. wzorca Singletonu)
	//Pozwoli to na odwo³¹nie siê do GameManagera w dowolnym miejscu projektu poprzez GameManager.instance
	public static GameManager instance;

	//Pole na element UI Text s³u¿¹cy do wyœwietlania wyniku
	public Text scoreText;

	//Pole zawieraj¹ce prêdkoœæ œwiata do którego bêdzie móg³ odwo³aæ siê dowolny obiekt w grze
	//Domyœlnie ustawiamy 0.2f, ale wartoœæ mo¿na dopasowaæ w edytorze
	public float worldScrollingSpeed = 0.2f;

	//pole w którym bêdziemy pamiêtaæ czy aktualnie trwa gra
	public bool inGame;

	//pole do podpiêcia przycisku reset
	public GameObject resetButton;

	//Pole na wynik
	private float score;

	// Use this for initialization
	void Start()
	{
		//Podczas uruchomienia przypisujemy aktualn¹ instancjê do statycznego pola instance
		//!!! Nale¿y uwa¿aæ, ¿eby zawsze na scenie by³ dok³adnie jeden GameManager !!!
		if (instance == null) { instance = this; }

		//Inicjalizujemy grê
		InitializeGame();

	}

	void InitializeGame()
	{
		//Ustawiamy pole mówi¹ce, ¿e jesteœmy  w trakcie gry
		inGame = true;
	}

	void FixedUpdate()
	{
		//Jeœli aktualnie nie trwa gra nie wykonuj reszty metody
		if (!GameManager.instance.inGame) return;

		//Co tick silnika fizyki dopisujemy do wyniku przebyt¹ odleg³oœæ i wywo³ujemy metodê wyœwietlaj¹c¹ wynik na ekranie
		score += worldScrollingSpeed;
		UpdateOnScreenScore();
	}

	void UpdateOnScreenScore()
	{
		//Wyœwietlamy na elemencie nasz wynik bez czêœci dziesiêtnej
		scoreText.text = score.ToString("0");
	}

	public void GameOver()
	{
		//gra siê skoñczy³a, wiêc:
		inGame = false;
		//Wyœwietlamy przycisk Restart
		resetButton.SetActive(true);
	}

	public void RestartGame()
	{
		//Ponownie ³adujemy scenê o indeksie 0 (czyli jedyn¹ w naszej grze)
		//Spowoduje to reset gry
		SceneManager.LoadScene(0);
	}
}
