using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
	public float rotationSpeed = 2f;

	public void Start()
	{
		//Losujemy prêdkoœæ obrotu z zakresu 50% do 150% pierwotnej
		rotationSpeed = Random.Range(0.5f * rotationSpeed, 1.5f * rotationSpeed);
	}

	public void FixedUpdate()
	{
		//Jeœli aktualnie nie trwa gra nie wykonuj reszty metody
		if (!GameManager.instance.inGame) return;

		//Obracamy w osi Z (do ekranu)
		transform.Rotate(new Vector3(0f, 0f, rotationSpeed));
	}
}