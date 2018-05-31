using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using TextSpeech;
using TMPro;
//The Game Manager Eng keeps track of which scenes to load, handles loading scenes, fading between scenes and also video playing/pausing

namespace Interactive360
{

    public class GameManagerEng : MonoBehaviour
    {
		public static GameManagerEng instance = null;

		private int numScript = -1;
        [Header("Script Management")]
        public string[] scriptsQuestions;
        public string[] scriptsAnswers;
        public Button nextScript;

		public Button m_buttonQuestion;
		public Button m_buttonAnswer;
		public Button m_buttonHome;

		public GameObject m_ObjectTextQuestion;
		public GameObject m_ObjectTextAnswer;
		public GameObject m_ObjectTextReply;

		//make sure that we only have a single instance of the game manager
		void Awake()
		{
			if (instance == null)
			{
				DontDestroyOnLoad(gameObject);
				instance = this;
			}
			else if (instance != this)
			{
				Destroy(gameObject);
			}
		}

        void Start(){
			TextToSpeech.instance.Setting("en-US", 1, 1);

			NextNumScript();
			nextScript.onClick.AddListener(() => NextNumScript());
			m_buttonHome.onClick.AddListener (() => bindHome ());

			SpeechToText.instance.Setting("en-US");
			SpeechToText.instance.onResultCallback = OnResultSpeech;
		
        }

		void bindHome(){
			SceneManager.LoadScene ("MainControl");
			SceneManager.UnloadSceneAsync ("Coffee");
			SceneManager.UnloadSceneAsync ("Beach");
		}
        void NextNumScript(){
			if (numScript < scriptsQuestions.Length - 1) 
			{
				numScript++;
				TextMeshProUGUI m_textQues = m_ObjectTextQuestion.GetComponent<TextMeshProUGUI> ();
				TextMeshProUGUI m_textAns = m_ObjectTextAnswer.GetComponent<TextMeshProUGUI> ();
				m_textQues.SetText (getCurrentQuestion());
				m_textAns.SetText (getCurrentAnswer());

				TextToSpeech.instance.StartSpeak (getCurrentQuestion());

			}
        }
		public string getCurrentQuestion(){
			return scriptsQuestions [numScript];
		}

		public string getCurrentAnswer(){
			return scriptsAnswers [numScript];
		}

		void OnResultSpeech(string _data)
		{
			#if UNITY_IOS
			TextMeshProUGUI m_textReply = m_ObjectTextReply.GetComponent<TextMeshProUGUI> ();
			m_textReply.SetText(_data);
			#endif
		}
    }
}
