using System;
using TMPro;

namespace WindowsBeLike
{
    public class ModalWindow : Window
    {
        Action OnPositiveResposeCallbackList;
        Action OnNegativeResposeCallbackList;

        public string Question = "This is a serious question. Are you realy sure you want to do this?";
        public string Note = "Tell me something about this action and what it can do";
        public string PositiveResponce = "Yes";
        public string NegativeResponce = "No";

        public TextMeshProUGUI QuestionText;
        public TextMeshProUGUI AnswerPosText;
        public TextMeshProUGUI AnswerNegText;
        public TextMeshProUGUI NoteText;

        public override void OnEnable()
        {
            base.OnEnable();
            QuestionText.text = Question;
            AnswerPosText.text = PositiveResponce;
            AnswerNegText.text = NegativeResponce;
            NoteText.text = Note;
        }

        public void OnClickPositive()
        {
            OnPositiveResposeCallbackList?.Invoke();
            Reset();
        }

        public void OnClickNegative()
        {
            OnNegativeResposeCallbackList?.Invoke();
            Reset();
        }

        public void RegisterOnClickYesCallback(Action callback)
        {
            OnPositiveResposeCallbackList += callback;
        }

        public void RegisterOnClickNoCallback(Action callback)
        {
            OnNegativeResposeCallbackList += callback;
        }

        public void UnregisterOnClickYesCallback(Action callback)
        {
            OnPositiveResposeCallbackList -= callback;
        }

        public void UnregisterOnClickNoCallback(Action callback)
        {
            OnNegativeResposeCallbackList -= callback;
        }


        void Reset()
        {

            Question = "Question Not Set!";
            Note = "Note Not Set!";
            PositiveResponce = "Yes";
            NegativeResponce = "No";
            gameObject.SetActive(false);
        }
    }

}