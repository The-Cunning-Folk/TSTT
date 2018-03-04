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
	private int newActiveChoice;

	void Awake () {
		StartStory();
	}

	void StartStory () {
		story = new Story (inkJSONAsset.text);
		activeChoice = 0;
		RefreshView();
	}

	void Update(){
		int nChoices = story.currentChoices.Count;
		if(newActiveChoice != activeChoice && nChoices > 0){
			if(newActiveChoice >= 0){
				activeChoice = newActiveChoice%nChoices;
			}
			else if(newActiveChoice < 0){
				activeChoice = newActiveChoice%nChoices!=0 ? 
				nChoices + newActiveChoice%nChoices:
				0;
			}
			newActiveChoice = activeChoice;
			RefreshChoices();
		}
	}

	public void SetActiveChoice(int choice){
		newActiveChoice = choice;
	}

	public void AlterActiveChoice(int amount){
		newActiveChoice = activeChoice + amount;
	}

	void ConstructChoicesList() {
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

	void RefreshChoices(){
		RemoveChoices ();
		ConstructChoicesList();
	}

	void RefreshView () {
		activeChoice = 0;
		RemoveChildren ();

		while (story.canContinue) {
			string text = story.Continue ().Trim();
			CreateContentView(text);
		}
		ConstructChoicesList();


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
