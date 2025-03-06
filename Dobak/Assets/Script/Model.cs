using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ModelSprite
{
	public Sprite[] ModelSprites;
	public Sprite[] ModelArmSprites;
}

public class Model : MonoBehaviour
{
	public ModelSprite[] modelSprites;

	private ModelSprite currentModelSprite;

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
	}

	private void ChangeCurrentSprite(ModelSprite sprites)
	{
		currentModelSprite = sprites;
	}

}
