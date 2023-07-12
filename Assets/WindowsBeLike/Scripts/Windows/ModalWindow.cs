/// <summary>
/// Represents a modal window in the WindowsBeLike interface.
/// </summary>
using System;
using TMPro;

namespace WindowsBeLike
{
    /// <summary>
    /// Represents a modal window in the WindowsBeLike interface.
    /// </summary>
    public class ModalWindow : Window
    {
        Action OnPositiveResposeCallbackList;
        Action OnNegativeResposeCallbackList;

        public string Question = "This is a serious question. Are you really sure you want to do this?";
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

        /// <summary>
        /// Called when the positive response button is clicked.
        /// </summary>
        public void OnClickPositive()
        {
            OnPositiveResposeCallbackList?.Invoke();
            Reset();
        }

        /// <summary>
        /// Called when the negative response button is clicked.
        /// </summary>
        public void OnClickNegative()
        {
            OnNegativeResposeCallbackList?.Invoke();
            Reset();
        }

        /// <summary>
        /// Registers a callback to be invoked when the positive response button is clicked.
        /// </summary>
        /// <param name="callback">The callback action.</param>
        public void RegisterOnClickYesCallback(Action callback)
        {
            OnPositiveResposeCallbackList += callback;
        }

        /// <summary>
        /// Registers a callback to be invoked when the negative response button is clicked.
        /// </summary>
        /// <param name="callback">The callback action.</param>
        public void RegisterOnClickNoCallback(Action callback)
        {
            OnNegativeResposeCallbackList += callback;
        }

        /// <summary>
        /// Unregisters a callback from the positive response button click event.
        /// </summary>
        /// <param name="callback">The callback action.</param>
        public void UnregisterOnClickYesCallback(Action callback)
        {
            OnPositiveResposeCallbackList -= callback;
        }

        /// <summary>
        /// Unregisters a callback from the negative response button click event.
        /// </summary>
        /// <param name="callback">The callback action.</param>
        public void UnregisterOnClickNoCallback(Action callback)
        {
            OnNegativeResposeCallbackList -= callback;
        }

        /// <summary>
        /// Resets the modal window's properties to their default values.
        /// </summary>
        private void Reset()
        {
            Question = "Question Not Set!";
            Note = "Note Not Set!";
            PositiveResponce = "Yes";
            NegativeResponce = "No";
            gameObject.SetActive(false);
        }
    }
}
