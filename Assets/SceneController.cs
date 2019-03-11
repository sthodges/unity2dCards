using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
	[SerializeField] private MemoryCard originalCard;
	[SerializeField] private Sprite[] images;
	[SerializeField] private TextMesh scoreLabel;

	private MemoryCard _firstCard;
	private MemoryCard _secondCard;

	public const int gridRows = 2;
	public const int gridCols = 4;
	public const float offsetX = 2.0f;
	public const float offsetY = 2.5f;
	private int _score = 0;


	public void Reset(){

		SceneManager.LoadScene ("Table");
	}



	// Use this for initialization
	void Start () {
		int[] ids = { 0, 0, 1, 1, 2, 2, 3, 3 };
		ids = ShuffleArray (ids);
		Vector3 startPos = originalCard.transform.position;
		for (int i = 0; i < gridCols; i++) {
			for (int j = 0; j < gridRows; j++) {
				MemoryCard card;
				if (i == 0 && j == 0) {
					card = originalCard;
				} else {
					card = Instantiate (originalCard) as MemoryCard;
				}
			
				int index = j * gridCols + i;
				int id = ids [index];
				card.SetCard(id, images[id]);
				float posX = (offsetX * i) + startPos.x;
				float posY = -(offsetY * j) + startPos.y;
				card.transform.position = new Vector3 (posX, posY, startPos.z);
			
			
			}// j
		} // i


	}

	public bool canReveal(){
		return (_secondCard == null);	
	}

	public void CardRevealed(MemoryCard card){
		if (_firstCard == null) {
			_firstCard = card;

		} else {
			_secondCard = card;
			StartCoroutine (checkMatch ());
		}
	}

	// Update is called once per frame
	void Update () {
		
	}

	private int []ShuffleArray( int [] a){
		for (int i = 0; i < a.Length; i++) {
			int temp = a [i];
			int r = Random.Range (0, i + 1); // 0 .. i
			a[i] = a[r];
			a [r] = temp;

		}
		return a;
	}


	private IEnumerator checkMatch(){
		if (_firstCard.id == _secondCard.id) {
			_score++;
			scoreLabel.text = "Score: " + _score;
			_firstCard = null;
			_secondCard = null;

		} else {
			yield return new WaitForSeconds (0.5f);
			_firstCard.Unreveal ();
			_secondCard.Unreveal ();
			_firstCard = null;
			_secondCard = null;

		}


	}
}
