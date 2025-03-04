using UnityEngine;

public class PersonalityModule
{
    //성격 모듈을 지정해줌
    public enum Personality
    {
        Normal,
        Glum,
        Kind,
        Bad,
        Evil
    }

    public Personality personality;

    public PersonalityModule(Personality value)
    {
        personality = value;
    }
    
    //기존 확률을 오브젝트에 적용된 성격에 따라 추가 or 감소된 확률을 반환하기
    //value에 들어갈 값은 동정심 게이지 값인 Percentage가 들어가야 한다.
    //반환되는 값은 바꿀 확률인 ChangePercentage
    public float GeneratePercentage(float value)
    {
        switch (personality)
        {
            case Personality.Normal:
                //기존 확률에 추가 확률을 더해줌(기존 확률 / 4)
                if (value > 60) return value + value / 4;
                else return value;
            case Personality.Glum:
                //기존 확률이 50 이상이면 추가된 값을, 그렇지 않을 경우 감소된 값을 더해줌(기존 확률 / 5)
                return value > 50 ? value + value / 5 : value - value / 5;
            case Personality.Kind:
                //기존 확률을 그대로 반환해줌
                return value;
            case Personality.Bad:
                //기존 확률에 감소된 값을 더해줌(기존 확률 / 3)
                return value - value / 3;
            case Personality.Evil:
                //100%에 기존 확률은 모듈러 연산한 값을 반환함.
                float result = 100 % value;
                return result < 0 ? 0 : result;
        }
        return -1;
    }
}
