using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

[System.Serializable]
public class ModelSprite
{
	public Sprite[] ModelSprites;
	public Sprite[] ModelArmSprites;

	public logArray[] logArray_Bar;
	public logArray[] logArray_Pro;
	public logArray[] logArray_Beg;
}

[System.Serializable]
public class logArray
{
	public string[] LogArray_col;
}

public class Model : MonoBehaviour
{
	public ModelSprite[] modelSprites;
	public TMP_Text Log;

	private ModelSprite currentModelSprite;
	public string currentLogs { get; private set; }

	private SpriteRenderer sr;
	private SpriteRenderer armSr;

	private void Awake()
	{
		sr = GetComponent<SpriteRenderer>();
		armSr = transform.GetChild(0).GetComponent<SpriteRenderer>();
	}
	
	public void ChangeModelSprite(PersonalityModule.Personality personality)
	{
		switch (personality)
		{
			case PersonalityModule.Personality.Normal:
				ChangeCurrentSprite(modelSprites[0]);
				break;
		}
	}
	public void ChangeSprite(int index, MainGame.State state)
	{
		sr.sprite = currentModelSprite.ModelSprites[index];
		armSr.sprite = currentModelSprite.ModelArmSprites[index];
		switch (state)
		{
			case MainGame.State.Bergain:
				currentLogs = currentModelSprite.logArray_Bar[index].LogArray_col[Random.Range(0, currentModelSprite.logArray_Bar[index].LogArray_col.Length)]; break;

			case MainGame.State.Provocation:
				currentLogs = currentModelSprite.logArray_Pro[index].LogArray_col[Random.Range(0, currentModelSprite.logArray_Pro[index].LogArray_col.Length)]; break;

			case MainGame.State.Begging:
				currentLogs = currentModelSprite.logArray_Beg[index].LogArray_col[Random.Range(0, currentModelSprite.logArray_Beg[index].LogArray_col.Length)]; break;
		}
	}
	public IEnumerator TextLogAppear(string text)
	{
		Log.text = "";
		for (int i = 0; i < text.Length; i++)
		{
			Log.text += text[i];
			yield return new WaitForSeconds(0.05f);
		}
		yield break;
	}

	private void ChangeCurrentSprite(ModelSprite sprites)
	{
		currentModelSprite = sprites;
	}
}
