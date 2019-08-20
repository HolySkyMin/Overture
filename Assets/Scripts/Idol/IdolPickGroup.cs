﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Song;
using Ingame;
using System;
using System.Linq;

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
            float res = 0;
            var resIdol = new float[Capacity];

            for(int i = 0; i < Capacity; i++)
            {
                if (IdolIndices[i] != -1)
                {
                    var idol = IngameManager.Instance.Data.Idols[IdolIndices[i]];
                    resIdol[i] = song.CalculateAppeal(idol.Vocal, idol.Dance, idol.Visual);
                    res += resIdol[i];
                }
            }
            var finalRes = ApplyPersonality(res, resIdol);
            return (finalRes, resIdol);
        }

        public float CalculateAppeal(WorkData work)
        {

            return 0;
        }

        public float ApplyPersonality(float allAppeal, float[] idolAppeal)
        {
            // 아이돌 성격을 따온 배열을 만들어 계산하려고 함
            var personaList = new List<IdolPersonality>();
            for(int i = 0; i < IdolIndices.Length; i++)
            {
                if (IdolIndices[i] != -1)
                    personaList.Add(IngameManager.Instance.Data.Idols[IdolIndices[i]].Personality);
                else
                    personaList.Add(IdolPersonality.Unknown);
            }

            float[] bonus = new float[idolAppeal.Length];
            bool applyBold = false;
            for (int i=0; i<idolAppeal.Length;i++)
            {
                var personality = personaList[i];
                if(personality == IdolPersonality.Naive)
                {
                    if(i!=0)
                    {
                        if(personaList[i - 1] != IdolPersonality.Naive)
                            personaList[i - 1] = IdolPersonality.Unknown;
                    }
                    if (i!=idolAppeal.Length -1)
                    {
                        if (personaList[i + 1] != IdolPersonality.Naive)
                            personaList[i + 1] = IdolPersonality.Unknown;
                    }
                }
            }
            int[] counts = new int[Enum.GetNames(typeof(IdolPersonality)).Length];
            for(int i=0; i<idolAppeal.Length;i++)
                counts[(int)personaList[i]]++;
            counts[(int)IdolPersonality.Unknown] = 0;

            //count personality 종류
            int percnt = 0;
            for(int i=0; i< counts.Length;i++)
            {
                if (counts[i] > 0)
                    percnt++;
            }
            for (int i=0;i<idolAppeal.Length;i++)
            {
                switch (personaList[i])
                {
                    case IdolPersonality.Unknown:
                        break;
                    case IdolPersonality.Bashful:       //만약 아이돌이 자기 자신뿐이라면 총 어필 +10% 같이 참여하는 아이돌당 자신의 어필 - 5 %
                        if(idolAppeal.Length ==1)
                        {
                            bonus[i] = 1.1f;
                        } else
                        {
                            bonus[i] = Math.Max(1.0f - 0.05f * idolAppeal.Length, 0.3f);
                        }
                        break;
                    case IdolPersonality.Jolly:         //같이 참여하는 명랑함 아이돌당 자신의 어필 +5%
                        bonus[i] = 1f + counts[(int)IdolPersonality.Jolly] * 0.05f;
                        break;
                    case IdolPersonality.Naive:         //양 옆 슬롯 아이돌의 성격 보정 효과를 제거
                        // pre-processed
                        break;
                    case IdolPersonality.Quirky:        //자신의 어필 랜덤으로 -5% ~ 5%
                        bonus[i] = 1.0f + 0.01f * new System.Random().Next(-5, 6);
                        break;
                    case IdolPersonality.Bold:          //총 어필 +10% (여러 명이 있어도 한 번만 적용)
                        applyBold = true;
                        break;
                    case IdolPersonality.Mild:          //같이 참여하는 아이돌의 성격 하나당 자신의 어필 +2%
                        bonus[i] = 1.0f + 0.02f * percnt;
                        break;
                    case IdolPersonality.Relaxed:       //만약 무사태평함 아이돌이 자기 자신뿐이라면 자신의 어필 +10%
                        if(counts[(int)IdolPersonality.Relaxed]==1)
                        {
                            bonus[i] = 1.1f;
                        }
                        break;
                    default:
                        break;
                }
            }
            for(int i=0;i<idolAppeal.Length;i++)
            {
                idolAppeal[i] *= bonus[i];
            }
            if (applyBold)
            {
                for (int i = 0; i < idolAppeal.Length; i++)
                {
                    idolAppeal[i] *= 1.1f;
                }
            }
            return idolAppeal.Sum();
        }
    }
}