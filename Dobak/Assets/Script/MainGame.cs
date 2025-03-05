using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CasinoModel
{
	public int[] nums = new int[2]; //번호 지정 배열

	public int SeletedNum = 0; //최종적으로 선택된 번호
	public int SeletedIndex;   //선택된 번호의 인덱스

	public float percentage = 0; //동정심 게이지

	public int ChangePercentage = UnityEngine.Random.Range(0, 100); //바꿀 확률

	public Dictionary<string, Action<int>> personalitys = new Dictionary<string, Action<int>>(); //성격 엑션 저장 리스트
	public Dictionary<string, Action<int>> SeletedPersonalitys = new Dictionary<string, Action<int>>(); //모델에 들어간 성격 엑션 리스트

	public PersonalityModule pm; //성격 모듈

	public CasinoModel(PersonalityModule.Personality p)
	{
		for(int i = 0; i < 2; i++) nums[i] = UnityEngine.Random.Range(0, 100);

		SeletedIndex = UnityEngine.Random.value < 0.5f ? 1 : 0;

		pm = new PersonalityModule(p);

		//리스트에 메서드 추가
		personalitys.Add("p1", p1);
		personalitys.Add("p2", p2);
		personalitys.Add("p3", p3);
		personalitys.Add("p4", p4);
		personalitys.Add("p5", p5);

		//SeletedPersonalitys에 특정 personality를 적용
		foreach (var action in personalitys)
		{
			if (UnityEngine.Random.value > 0.8f)
			{
				SeletedPersonalitys.Add(action.Key, action.Value);
			}
		}
	}

	public void ChangeValue(int PlayerNum, int RandomRange)
	{
		int dif = PlayerNum - nums[SeletedIndex];

		if(Mathf.Abs(dif) <= RandomRange)
		{
			ChangePercentage = (int)UnityEngine.Random.Range(0f, 100f);
		}
		else if(dif > RandomRange)
		{
			ChangePercentage = (int)Mathf.Min(100f, 1.2f * dif);
		}
		else if(dif < -RandomRange)
		{
			ChangePercentage = 0;
		}
		ChangePercentage = Mathf.Clamp(ChangePercentage, 0, 100);
	}

	//성격 엑션 1. 반대맨
	private void p1(int seletedIndex)
	{
		int result = 1 - seletedIndex;
		SeletedNum = nums[result];
	}
	
	//성격 엑션 2. 큰 수를 좋아함
	private void p2(int seletedIndex)
	{
		int result = 1 - seletedIndex;
		if (nums[seletedIndex] < nums[result])
		{
			SeletedNum = nums[result];
		}
	}
	
	//성격 엑션 3. 작은 수를 좋아함
	private void p3(int seletedIndex)
	{
		int result = 1 - seletedIndex;
		if (nums[seletedIndex] >= nums[result])
		{
			SeletedNum = nums[result];
		}
	}
	
	//성격 엑션 4. 소수 러버
	private void p4(int seletedIndex)
	{
		if (nums[seletedIndex] < 2)
		{
			int n = 1 - seletedIndex;
			SeletedNum = nums[n];
			return;
		}
		else if (nums[seletedIndex] == 2) SeletedNum = nums[seletedIndex];
		else
		{
			for (int i = 2; i * i <= nums[seletedIndex]; i++)
			{
				if (nums[seletedIndex] % i == 0)
				{
					int n = 1 - seletedIndex;
					SeletedNum = nums[n];
					return;
				}
			}
			SeletedNum = nums[seletedIndex];
		}

	}
	
	//성격 엑션 5. 일반인
	private void p5(int _)
	{
		SeletedNum = nums[(UnityEngine.Random.value > 0) ? 1 : 0];
	}
}

public class MainGame : MonoBehaviour
{
	[Header("UI")]
	public TMP_Text Number;

	public Slider slider;
	public TMP_Text myNumber;

	[Header("DevelopCanvas")]
	public TMP_Text Percentage;
	public PersonalityModule.Personality module;
	public TMP_Text moduleText;
	public TMP_Text pText;

	private int randomRand = 5;

	private CasinoModel model;
	
	private int _myNumber = 0;

	private bool Changed = false;
	private int count = 0;
	private void Awake()
	{
		var value = System.Enum.GetValues(enumType: typeof(PersonalityModule.Personality));
		module = (PersonalityModule.Personality)value.GetValue(UnityEngine.Random.Range(0, value.Length));

		model = new CasinoModel(module);
		pText.text = "현재 특성 :";
		foreach(var action in model.SeletedPersonalitys)
		{
			pText.text += " " + action.Key.ToString();
		}
		moduleText.text = "현재 모듈 : " + module.ToString();
		model.percentage = 0f;
	}
	private void Start()
	{
		Init();
		model.ChangeValue(_myNumber, 5);
	}
	private void Update()
	{
		if(count >= 5)
		{
			count = 0;
			StartCoroutine(NextGame());
		}
	}
	IEnumerator NextGame()
	{
		yield return new WaitForSeconds(0.5f);
		Awake();
		Init();
		model.ChangeValue(_myNumber, 5);
	}

	public void Selete()
	{
		bool Persona23 = false;
		int result = 0;

		if (model.percentage > 0) result = (int)model.pm.GeneratePercentage(model.percentage);

		model.ChangePercentage += result;

		Debug.Log($"동정심에 의해 변경된 확률 : {model.ChangePercentage}%");

		bool isChange = UnityEngine.Random.Range(0, 100) > model.ChangePercentage ? true : false;
		if (isChange)
		{
			int n = 1 - model.SeletedIndex;
			model.SeletedNum = model.nums[n];
		}
		else
		{
			model.SeletedNum = model.nums[model.SeletedIndex];
		}

		//저장된 성격 엑션 실행
		foreach (var action in model.SeletedPersonalitys)
		{
			if (action.Value.Equals("p2") && !Persona23)
			{
				Persona23 = true;
			}

			if (action.Value.Equals("p3") && Persona23) continue;

			action.Value.Invoke(model.SeletedIndex);
		}
		
		if (_myNumber > model.SeletedNum)
		{
			Debug.Log($"승리! -- 당신의 숫자 : {_myNumber} 상대가 선택한 숫자 : {model.SeletedNum}");
			Init();
		}
		else
		{
			Debug.Log($"패배! -- 당신의 숫자 : {_myNumber} 상대가 선택한 숫자 : {model.SeletedNum}");
			Init();
		}
		count++;
		
	}

	private void Init()
	{
		InitNumber();
		model.ChangeValue(_myNumber, 5);
		randomRand = 5;
		UpdateUI();
	}
	private void UpdateUI()
	{
		slider.value = model.percentage / 100;

		Number.text = "선택한 숫자 : " + model.nums[model.SeletedIndex];
		myNumber.text = "내 숫자 : " + _myNumber;

		Percentage.text = "바꿀 확률 : " + model.ChangePercentage + "%";
	}
	private void InitNumber()
	{
		_myNumber = UnityEngine.Random.Range(0, 100);

		for (int i = 0; i < 2; i++) model.nums[i] = UnityEngine.Random.Range(0, 100);

		model.ChangePercentage = UnityEngine.Random.Range(0, 100);
	}

	public void ChangeMyNum()
	{
		if(randomRand > 0)
		{
			_myNumber = UnityEngine.Random.Range(0, 100);
			myNumber.text = "내 숫자 : " + _myNumber;
			--randomRand;

			model.ChangeValue(_myNumber, 5);
			UpdateUI();
		}
	}
	//흥정하기
	public void Bargain()
	{
		//Percentage 증가 or 급격 감소
		switch (model.pm.personality)
		{
			case PersonalityModule.Personality.Normal:
				if (UnityEngine.Random.value > 0.5f)
					model.percentage += 2.4f;
				else
					model.percentage -= 3f;
				break;
			case PersonalityModule.Personality.Glum:
				if(UnityEngine.Random.value > 0.3f)
				{
					model.percentage -= 4f;
				}
				else
				{
					model.percentage += 4f;
				}
				break;
			case PersonalityModule.Personality.Kind:
				model.percentage += 3f; break;
			case PersonalityModule.Personality.Bad:
				if (UnityEngine.Random.value > 0.2)
					model.percentage -= 3.5f;
				else
					model.percentage += 4f;
				break;
			case PersonalityModule.Personality.Evil:
				model.percentage -= 2.5f; break;
		}
		model.percentage = Mathf.Clamp(model.percentage, 0, 100);
		slider.value = model.percentage / 100;
	}
	//도발하기
	public void Provocation()
	{
		//Percentage 감소 or 증가
		switch (model.pm.personality)
		{
			case PersonalityModule.Personality.Normal:
				if (UnityEngine.Random.value > 0.5f)
					model.percentage -= 3.5f;
				else
					model.percentage += 6f;
				break;
			case PersonalityModule.Personality.Glum:
				if (UnityEngine.Random.value > 0.3f)
				{
					model.percentage -= 5f;
				}
				else
				{
					model.percentage += 10f;
				}
				break;
			case PersonalityModule.Personality.Kind:
				model.percentage -= 3f; break;
			case PersonalityModule.Personality.Bad:
				if (UnityEngine.Random.value > 0.1f)
					model.percentage -= 6f;
				else
					model.percentage += 20f;
				break;
			case PersonalityModule.Personality.Evil:
				model.percentage -= 5f; break;
		}
		model.percentage = Mathf.Clamp(model.percentage, 0, 100);
		slider.value = model.percentage / 100;
	}
	//빌붙기
	public void Begging()
	{
		//Percentage 급격 감소 or 급격 증가
		switch (model.pm.personality)
		{
			case PersonalityModule.Personality.Normal:
				if (UnityEngine.Random.value > 0.5f)
					model.percentage -= 10f;
				else
					model.percentage += 20f;
				break;
			case PersonalityModule.Personality.Glum:
				if (UnityEngine.Random.value > 0.3f)
				{
					model.percentage += 10f;
				}
				else
				{
					model.percentage -= 8f;
				}
				break;
			case PersonalityModule.Personality.Kind:
				model.percentage += 10f; break;
			case PersonalityModule.Personality.Bad:
				if (UnityEngine.Random.value > 0.07f)
					model.percentage -= 20f;
				else
					model.percentage += 40f;
				break;
			case PersonalityModule.Personality.Evil:
				model.percentage -= 25f; break;
		}
		model.percentage = Mathf.Clamp(model.percentage, 0, 100);
		slider.value = model.percentage / 100;
	}
}
