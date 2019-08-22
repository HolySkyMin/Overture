using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ScriptedAnimation;

namespace Ingame
{
    public class ResultManager : MonoBehaviour
    {
        [Header("Master Result Panel")]
        public ScriptAnimation TheaterResult;
        public ScriptAnimation WorkResult, LessonResult, CDResult1, CDResult2, ConcertResult1, ConcertResult2, IdolPay;
        [Header("Result Itself Panel")]
        public Text GroupName;
        public Text DateTimeText;
        [Header("Theater Panel")]
        public Text TheaterAppeal;
        public Text TheaterMoney, TheaterTotalFan;
        [Header("Work Panel")]
        public Text WorkMoney;
        public Text WorkHonor, WorkFan;
        [Header("Lesson Panel")]
        public Text LessonMoney;
        public Text LessonVocal, LessonDance, LessonVisual, LessonVariety;
        [Header("CD Panel")]
        public Text CDTurnLeft;
        public Text CDAppeal, CDMoney, CDTotalFan;
        [Header("Concert Panel")]
        public Text ConcertTurnLeft;
        public Text ConcertMoney, ConcertReqSeat, ConcertFillSeat, ConcertScale, ConcertBool;
        [Header("Idol Pay Panel")]
        public Text IdolCount;
        public Text IdolSpendMoney;

        private bool nextClicked = false;

        public IEnumerator ShowResult()
        {
            GroupName.text = IngameManager.Instance.Data.GroupName;
            DateTimeText.text = $"{IngameManager.Instance.Data.Month / 12 + 1}년 {IngameManager.Instance.Data.Month % 12 + 1}월";
            yield return ShowResultInternal();
            IngameManager.Instance.StartNewTurn();
            gameObject.SetActive(false);
        }

        IEnumerator ShowResultInternal()
        {
            var theaterData = IngameManager.Instance.Theater.ApplySendResultData();
            TheaterAppeal.text = theaterData.Item1.ToString("N0");
            TheaterMoney.text = theaterData.Item2.ToString();
            TheaterTotalFan.text = theaterData.Item3.ToString();
            yield return TheaterResult.Appear_C();
            yield return new WaitUntil(() => nextClicked);
            nextClicked = false;
            yield return TheaterResult.Disappear_C();

            var workData = IngameManager.Instance.Work.ApplySendResultData();
            WorkMoney.text = workData.Item1.ToString();
            WorkHonor.text = workData.Item2.ToString();
            WorkFan.text = workData.Item3.ToString();
            yield return WorkResult.Appear_C();
            yield return new WaitUntil(() => nextClicked);
            nextClicked = false;
            yield return WorkResult.Disappear_C();

            var lessonData = IngameManager.Instance.Lesson.ApplySendResultData();
            LessonVocal.text = lessonData.Item1[0].ToString();
            LessonDance.text = lessonData.Item1[1].ToString();
            LessonVisual.text = lessonData.Item1[2].ToString();
            LessonVariety.text = lessonData.Item1[3].ToString();
            LessonMoney.text = lessonData.Item2.ToString();
            yield return LessonResult.Appear_C();
            yield return new WaitUntil(() => nextClicked);
            nextClicked = false;
            yield return LessonResult.Disappear_C();

            var CDData = IngameManager.Instance.CD.ApplySendResultData();
            if(CDData.Item1 == true)
            {
                if (CDData.Item5 > 0)
                {
                    CDTurnLeft.text = CDData.Item5.ToString();
                    yield return CDResult1.Appear_C();
                    yield return new WaitUntil(() => nextClicked);
                    nextClicked = false;
                    yield return CDResult1.Disappear_C();
                }
                else
                {
                    CDAppeal.text = CDData.Item2.ToString();
                    CDMoney.text = CDData.Item3.ToString();
                    CDTotalFan.text = CDData.Item4.ToString();
                    yield return CDResult2.Appear_C();
                    yield return new WaitUntil(() => nextClicked);
                    nextClicked = false;
                    yield return CDResult2.Disappear_C();
                }
            }

            var ConcertData = IngameManager.Instance.Concert.ApplySendResultData();
            if(ConcertData.Item1 == true)
            {
                if(ConcertData.Item2 > 0)
                {
                    ConcertTurnLeft.text = ConcertData.Item2.ToString();
                    yield return ConcertResult1.Appear_C();
                    yield return new WaitUntil(() => nextClicked);
                    nextClicked = false;
                    yield return ConcertResult1.Disappear_C();
                }
                else
                {
                    ConcertMoney.text = ConcertData.Item3[0].ToString();
                    ConcertReqSeat.text = ConcertData.Item3[1].ToString();
                    ConcertFillSeat.text = ConcertData.Item3[2].ToString();
                    ConcertScale.text = ConcertData.Item4;
                    ConcertBool.text = ConcertData.Item5 ? "성공!" : "실패...";
                    yield return ConcertResult2.Appear_C();
                    yield return new WaitUntil(() => nextClicked);
                    nextClicked = false;
                    yield return ConcertResult2.Disappear_C();
                }
            }

            int spendMoney = 0;
            foreach (var idol in IngameManager.Instance.Data.Idols)
                spendMoney += idol.Value.Cost;
            IngameManager.Instance.Data.Money -= spendMoney;
            IdolCount.text = IngameManager.Instance.Data.Idols.Count.ToString();
            IdolSpendMoney.text = spendMoney.ToString();
            yield return IdolPay.Appear_C();
            yield return new WaitUntil(() => nextClicked);
            nextClicked = false;
            yield return IdolPay.Disappear_C();

            if (IngameManager.Instance.Data.Money < 0)
                SceneChanger.Instance.ChangeScene("GameOver");

            if (ConcertData.Item4 == "돔 투어" && ConcertData.Item5 == true)
                SceneChanger.Instance.ChangeScene("GameClear");
        }

        public void NextBtnClick()
        {
            nextClicked = true;
        }
    }
}