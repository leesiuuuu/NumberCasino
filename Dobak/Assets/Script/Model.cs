using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

[System.Serializable]
public struct ModelSprite
{
	public Sprite[] ModelSprites;
	public Sprite[] ModelArmSprites;

	public string[] logArray;
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
	public void ChangeSprite(int index)
	{
		sr.sprite = currentModelSprite.ModelSprites[index];
		armSr.sprite = currentModelSprite.ModelArmSprites[index];
		currentLogs = currentModelSprite.logArray[Random.Range(0, currentModelSprite.logArray.Length-1)];
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
