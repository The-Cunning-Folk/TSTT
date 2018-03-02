using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Ink.Runtime;

public class DialogController : MonoBehaviour {

	[SerializeField]
	private TextAsset inkJSONAsset;
	private Story story;

	[SerializeField]
	private Canvas canvas;

	// UI Prefabs
	[SerializeField]
	private Text textPrefab;
	[SerializeField]
	private Button buttonPrefab;

	private int  activeChoice;

	void Awake () {
		StartStory();
	}

	void StartStory () {
		story = new Story (inkJSONAsset.text);
		activeChoice = 0;
		RefreshView();
	}

	void Update(){
		int tmpChoice = activeChoice;
		if (Input.GetKey("left") && activeChoice >= 1)
            activeChoice -= 1;
        else if (Input.GetKey("right") && activeChoice < story.currentChoices.Count - 1)
        	activeChoice += 1;
        if(tmpChoice != activeChoice)
        	RefreshChoices();
	}

	void RefreshChoices(){
		RemoveChoices ();

		if(story.currentChoices.Count > 0) {
			Choice choice = story.currentChoices [activeChoice];
			Button button = CreateChoiceView (choice.text.Trim ());
			button.onClick.AddListener (delegate {
				OnClickChoiceButton (choice);
			});
		}
		 else {
			Button choice = CreateChoiceView("End of story.\nRestart?");
			choice.onClick.AddListener(delegate{
				StartStory();
			});
		}
	}

	void RefreshView () {
		activeChoice = 0;
		RemoveChildren ();

		while (story.canContinue) {
			string text = story.Continue ().Trim();
			CreateContentView(text);
		}
		if(story.currentChoices.Count > 0) {
			Choice choice = story.currentChoices [activeChoice];
			Button button = CreateChoiceView (choice.text.Trim ());
			button.onClick.AddListener (delegate {
				OnClickChoiceButton (choice);
			});
		}
		 else {
			Button choice = CreateChoiceView("End of story.\nRestart?");
			choice.onClick.AddListener(delegate{
				StartStory();
			});
		}

	}

	void OnClickChoiceButton (Choice choice) {
		story.ChooseChoiceIndex (choice.index);
		RefreshView();
	}

	void CreateContentView (string text) {
		Text storyText = Instantiate (textPrefab) as Text;
		storyText.text = text;
		storyText.transform.SetParent (canvas.transform, false);
	}

	Button CreateChoiceView (string text) {
		Button choice = Instantiate (buttonPrefab) as Button;
		choice.transform.SetParent (canvas.transform, false);

		Text choiceText = choice.GetComponentInChildren<Text> ();
		choiceText.text = text;

		HorizontalLayoutGroup layoutGroup = choice.GetComponent <HorizontalLayoutGroup> ();
		layoutGroup.childForceExpandHeight = false;

		return choice;
	}

	void RemoveChoices() {
		int childCount = canvas.transform.childCount;
		for (int i = childCount - 1; i >= 0; --i) {
			if(canvas.transform.GetChild (i).gameObject.CompareTag("DialogOption"))
				GameObject.Destroy (canvas.transform.GetChild (i).gameObject);
		}
	}

	void RemoveChildren () {
		int childCount = canvas.transform.childCount;
		for (int i = childCount - 1; i >= 0; --i) {
			GameObject.Destroy (canvas.transform.GetChild (i).gameObject);
		}
	}
}
