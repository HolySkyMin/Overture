using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Song;
using Ingame;

namespace Idol
{
    [System.Serializable]
    public class IdolPickGroup
    {
        public int Capacity;
        public int Count;
        public int[] IdolIndices;
        public int[] Restriction;
        public Dictionary<IdolPersonality, int> PersonaDic;

        public override string ToString()
        {
            string res = "[ ";
            for (int i = 0; i < IdolIndices.Length; i++)
                if (IdolIndices[i] != -1)
                    res += IngameManager.Instance.Data.Idols[IdolIndices[i]].Name + ", ";
            res += "]";
            return res;
        }

        public IdolPickGroup(int capacity)
        {
            Capacity = capacity;
            Count = 0;
            IdolIndices = new int[capacity];
            for (int i = 0; i < capacity; i++)
                IdolIndices[i] = -1;
            Restriction = new int[7];
            PersonaDic = new Dictionary<IdolPersonality, int>();
        }

        public bool SetIdol(int index, IdolData idol, bool isWorkLesson, out string rejectReason)
        {
            if (index < Capacity && index >= 0)
            {
                if(idol.CheckRestriction(Restriction) == false)
                {
                    rejectReason = "해당 아이돌은 조건에 맞지 않습니다.";
                    return false;
                }

                if (isWorkLesson)
                    idol.IsWorkLessonPicked = true;
                IdolIndices[index] = idol.Index;
                if (!PersonaDic.ContainsKey(idol.Personality))
                    PersonaDic.Add(idol.Personality, 0);
                PersonaDic[idol.Personality]++;
                Count++;
                rejectReason = "";
                return true;
            }
            else
            {
                rejectReason = "슬롯이 다 찼습니다.";
                return false;
            }
        }

        public void RemoveIdol(int index, bool isWorkLesson)
        {
            if (index < Capacity && index >= 0)
            {
                var idol = IngameManager.Instance.Data.Idols[IdolIndices[index]];
                if (isWorkLesson)
                    idol.IsWorkLessonPicked = false;
                PersonaDic[idol.Personality]--;
                if (PersonaDic[idol.Personality] < 1)
                    PersonaDic.Remove(idol.Personality);
                IdolIndices[index] = -1;
                Count--;
            }
        }

        public (float, float[]) CalculateAppeal(SongData song)
        {
            var resIdol = new float[Capacity];

            for(int i = 0; i < Capacity; i++)
            {
                if (IdolIndices[i] != -1)
                {
                    var idol = IngameManager.Instance.Data.Idols[IdolIndices[i]];
                    resIdol[i] = song.CalculateAppeal(idol.Vocal, idol.Dance, idol.Visual);
                }
            }
            var finalRes = ApplyPersonality(ref resIdol);
            return (finalRes, resIdol);
        }

        public (float, float[]) CalculateAppeal(WorkData work)
        {
            var resIdol = new float[Capacity];

            for(int i = 0; i < Capacity; i++)
            {
                if(IdolIndices[i] != -1)
                {
                    var idol = IngameManager.Instance.Data.Idols[IdolIndices[i]];
                    if (work.CheckAbility[0])
                        resIdol[i] += idol.Vocal * 10;
                    if (work.CheckAbility[1])
                        resIdol[i] += idol.Dance * 10;
                    if (work.CheckAbility[2])
                        resIdol[i] += idol.Visual * 10;
                    if (work.CheckAbility[3])
                        resIdol[i] += idol.Variety * 10;
                }
            }
            var finalRes = ApplyPersonality(ref resIdol);
            return (finalRes, resIdol);
        }

        public float ApplyPersonality(ref float[] idolAppeal)
        {
            // 이제부터의 계산이 아이돌 데이터에 직접적인 영향을 끼치면 안 되므로, 계산을 위한 별도의 배열(컬렉션)들을 만들어 준다.
            // 이 때 빈 슬롯은 Unknown 처리되기 때문에, 이후 성격 계산에서 Unknown은 성격으로 치지 않을 것.
            var personaList = new List<IdolPersonality>();
            var personaDicCopy = new Dictionary<IdolPersonality, int>();
            for(int i = 0; i < Capacity; i++)
            {
                if (IdolIndices[i] != -1)
                    personaList.Add(IngameManager.Instance.Data.Idols[IdolIndices[i]].Personality);
                else
                    personaList.Add(IdolPersonality.Unknown);
            }
            foreach (var item in PersonaDic)
                personaDicCopy.Add(item.Key, item.Value);
            
            // 우선 '천진난만함'(Naive) 성격을 체크. 있으면 양 옆의 슬롯 성격을 (마찬가지로 천진난만함일 때를 제외하고) Unknown으로 바꾼다.
            for (int i = 0; i < Capacity; i++)
            {
                if (personaList[i] == IdolPersonality.Naive)
                {
                    if (i > 0 && personaList[i - 1] != IdolPersonality.Naive && personaList[i - 1] != IdolPersonality.Unknown)
                    {
                        personaDicCopy[personaList[i - 1]]--;
                        if (personaDicCopy[personaList[i - 1]] < 0)
                            personaDicCopy.Remove(personaList[i - 1]);
                        personaList[i - 1] = IdolPersonality.Unknown;
                    }
                    if (i < idolAppeal.Length - 1 && personaList[i + 1] != IdolPersonality.Naive && personaList[i + 1] != IdolPersonality.Unknown)
                    {
                        personaDicCopy[personaList[i + 1]]--;
                        if (personaDicCopy[personaList[i + 1]] < 0)
                            personaDicCopy.Remove(personaList[i + 1]);
                        personaList[i + 1] = IdolPersonality.Unknown;
                    }
                }
            }

            // 그 다음 '기운참'(Healthy) 성격을 체크, 있으면 양 옆 슬롯의 보너스 계수를 2배 증가시킨다.
            int[] bonusCoeff = new int[idolAppeal.Length];
            for (int i = 0; i < bonusCoeff.Length; i++)
                bonusCoeff[i] = 1;
            for(int i = 0; i < Capacity; i++)
            {
                if(personaList[i] == IdolPersonality.Healthy)
                {
                    if (i > 0)
                        bonusCoeff[i - 1] *= 2;
                    if (i < idolAppeal.Length - 1)
                        bonusCoeff[i + 1] *= 2;
                }
            }

            // 나머지 성격들을 체크 및 적용시킨다.
            float[] idolBonus = new float[idolAppeal.Length];
            for (int i = 0; i < idolBonus.Length; i++)
                idolBonus[i] = 1;
            float totalBonus = 1;
            bool applyBold = false, applyFocus = false;
            int boldMaxBonusCoeff = 0, focusMaxBonusCoeff = 0;
            for (int i = 0; i < Capacity; i++)
            {
                switch (personaList[i])
                {
                    case IdolPersonality.Bashful:       //만약 아이돌이 자기 자신뿐이라면 총 어필 +10%, 같이 참여하는 아이돌당 자신의 어필 -5%. 최대 어필 감소량 -30%
                        if (Count == 1)
                            totalBonus += 0.1f * bonusCoeff[i];
                        else
                            idolBonus[i] = Mathf.Max(1.0f - 0.05f * (Count - 1), 0.7f);
                        break;
                    case IdolPersonality.Jolly:         //같이 참여하는 명랑함 아이돌당 자신의 어필 +5%
                        idolBonus[i] += (personaDicCopy[IdolPersonality.Jolly] - 1) * 0.05f * bonusCoeff[i];
                        break;
                    case IdolPersonality.Quirky:        //자신의 어필 랜덤으로 -5% ~ 5%
                        idolBonus[i] += 0.01f * bonusCoeff[i] * Random.Range(-5f, 5f);
                        break;
                    case IdolPersonality.Bold:          //총 어필 +10% (여러 명이 있어도 한 번만 적용)
                        applyBold = true;
                        if (boldMaxBonusCoeff < bonusCoeff[i])
                            boldMaxBonusCoeff = bonusCoeff[i];
                        break;
                    case IdolPersonality.Mild:          //같이 참여하는 아이돌의 성격 하나당 자신의 어필 +2%
                        int cnt = personaDicCopy.Keys.Count;
                        if (personaDicCopy[IdolPersonality.Mild] == 1)
                            cnt--;
                        idolBonus[i] += 0.02f * bonusCoeff[i] * cnt;
                        break;
                    case IdolPersonality.Relaxed:       //만약 무사태평함 아이돌이 자기 자신뿐이라면 자신의 어필 +10%
                        if (personaDicCopy[IdolPersonality.Relaxed] == 1)
                            idolBonus[i] += 0.1f * bonusCoeff[i];
                        break;
                    case IdolPersonality.Focus:         //같이 참여하는 모든 아이돌의 성격이 집중함이면 총 어필 +5%
                        if (personaDicCopy[IdolPersonality.Focus] == Count)
                        {
                            applyFocus = true;
                            if (focusMaxBonusCoeff < bonusCoeff[i])
                                focusMaxBonusCoeff = bonusCoeff[i];
                        }
                        break;
                    case IdolPersonality.Sad:           //의젓함 아이돌이 같이 참여하지 않을 시 자신의 어필 -10%
                        if (!personaDicCopy.ContainsKey(IdolPersonality.Mild))
                            idolBonus[i] -= 0.1f * bonusCoeff[i];
                        break;
                    case IdolPersonality.Ambitious:     //자신의 어필이 가장 높을 시 자신의 어필 +10%
                        bool flag = false;
                        foreach (var appeal in idolAppeal)
                            if (appeal > idolAppeal[i])
                                flag = true;
                        if (!flag)
                            idolBonus[i] += 0.1f * bonusCoeff[i];
                        break;
                    default:
                        break;
                }
            }
            if (applyBold)
                totalBonus += 0.1f * boldMaxBonusCoeff;
            if (applyFocus)
                totalBonus += 0.05f * focusMaxBonusCoeff;

            // 어필을 계산한다.
            float totalAppeal = 0;
            for (int i = 0; i < idolAppeal.Length; i++)
            {
                idolAppeal[i] *= idolBonus[i];
                totalAppeal += idolAppeal[i];
            }
            totalAppeal *= totalBonus;
            return totalAppeal;
        }
    }
}