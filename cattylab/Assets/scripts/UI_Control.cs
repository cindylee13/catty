using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Control : MonoBehaviour {
	
	public cattyLabDictionaty CLD;
	private List<string> _pendingMessage;
	private bool _isMessengerActive = false;
	public playerStateControl overallData;
	public List<Text> _moneyText;
	public List<Text> _craftingTimeText;
	public List<Text> _exploreTimeText;
	public UI_ListCtrl _CraftingList;
	public UI_OccupyListControl _OccupyList;
	public MainUI_GroupCounterControl _GCC;
	public List<int> _catOccupied, _itemOccupied;
	private List<ListItemData> _CraftlidList;
	public Button _StartCraftingBtn;
	public Animator messengerAnim;
	public Text messengerText;
	private bool _ready = false;
	public GameObject _groupAvailable, _groupUnavailable, _groupPanel;
	// Use this for initialization
	IEnumerator Start () {
		if(!CLD.IsReady){
			yield return 0;
		}
		_pendingMessage = new List<string>();
		_CraftlidList = new List<ListItemData>();
		_catOccupied = new List<int>();
		_itemOccupied = new List<int>();
		_StartCraftingBtn.interactable = false;
		_StartCraftingBtn.onClick.AddListener(SendRecipe);
		ResetCraftingOccupy();
		_ready = true;
	}

	void OnApplicationQuit(){
		overallData.SaveGameData();
	}

	IEnumerator GameSavor(){
		while(true){
			yield return new WaitForSeconds(150);
			overallData.SaveGameData();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(!_ready) return;
		if(!_isMessengerActive && _pendingMessage.Count > 0){
			StartCoroutine(ShowPendingMessage());
		}
	}

	void SendRecipe(){
		recipeData s_recipe = CLD.FindRecipeResultData(_catOccupied.ToArray(), _itemOccupied.ToArray());
		overallData.SendMessage("SubmitRecipe", s_recipe.ent_id);
		ResetCraftingOccupy();
		StartCoroutine(craftingCountDownClock());

	}

	void ChangeMoneyText(){
		long money = overallData.money;
		foreach(Text tx in _moneyText){
			tx.text = "$" + money;
		}
	}

	

	void RefreshCraftingList(){
		//create Item data list
		_CraftlidList.Clear();
		Debug.Log(overallData);
		if(overallData.Ownedcats == null) return;
		foreach(cat c in overallData.Ownedcats){
			ListItemData lid = new ListItemData();
			lid.EntityID = c.ent_id;
			lid.EntityType = "cat";
			lid.MiscData = "X" + (c.avaliable - MatchingEntityInList(_catOccupied, c.ent_id));
			_CraftlidList.Add(lid);
		}
		foreach(item i in overallData.OwnedItems){
			ListItemData lid = new ListItemData();
			lid.EntityID = i.ent_id;
			lid.EntityType = "item";
			lid.MiscData = "X" + (i.count - MatchingEntityInList(_itemOccupied, i.ent_id));
			_CraftlidList.Add(lid);
		}
		_CraftingList.listItemData = _CraftlidList;
		CheckRecipe();
	}

	void RefreshOccupyList(){
		List<ListItemData> occupylidList = new List<ListItemData>();
		foreach(int cid in _catOccupied){
			ListItemData lid = new ListItemData();
			lid.EntityID = cid;
			lid.EntityType = "cat";
			occupylidList.Add(lid);
		} 
		foreach(int iid in _itemOccupied){
			ListItemData lid = new ListItemData();
			lid.EntityID = iid;
			lid.EntityType = "item";
			occupylidList.Add(lid);
		}
		_OccupyList.listItemData = occupylidList;
		CheckRecipe();
	}

	void ResetCraftingOccupy(){
		_CraftlidList.Clear();
		_catOccupied.Clear();
		_itemOccupied.Clear();
		RefreshCraftingList();
		RefreshOccupyList();
	}

	public void CraftingItemClicked(int orderInList){
		
		Debug.Log("Yee Haw "+orderInList);
		if(_catOccupied.Count + _itemOccupied.Count >= 8) return;
		if(_CraftlidList[orderInList].EntityType == "cat"){
			_catOccupied.Add(_CraftlidList[orderInList].EntityID);
		}else{
			_itemOccupied.Add(_CraftlidList[orderInList].EntityID);
		}
		RefreshCraftingList();
		RefreshOccupyList();
	}

	public void RemoveInOccupy(int idInOccupy, string type){
		if(type == "cat"){
			try{
				_catOccupied.RemoveAt(IndexInList(_catOccupied, idInOccupy));
			}catch{

			}
		}else{
			try{
				_itemOccupied.RemoveAt(IndexInList(_itemOccupied, idInOccupy));
			}catch{

			}
		}
		RefreshCraftingList();
		RefreshOccupyList();
	}

	private int IndexInList(List<int> a, int b){
		for(int i = 0; i< a.Count;i++){
			if(a[i] == b) return i;
		}
		return -1;
	}

	private void CheckRecipe(){
		_StartCraftingBtn.interactable = CLD.FindRecipeResultData(_catOccupied, _itemOccupied) != null;
	}

	private int MatchingEntityInList(List<int> a, int id){
		int matching = 0;
		foreach(int data in a){
			if(data==id) matching++;
		}
		return matching;
	}

	IEnumerator craftingCountDownClock(){
		while(overallData.isCrafting){
			foreach(Text t in _craftingTimeText){
				t.text = IntToTime((int)(overallData.craftETC - ConvertToUnixTimestamp(System.DateTime.Now)));
			}
			yield return new WaitForSeconds(1f);
		}
		foreach(Text t in _craftingTimeText){
				t.text = "---";
			}
	}
	double ConvertToUnixTimestamp(System.DateTime date){
		System.DateTime st = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
 	    System.TimeSpan diff = date - st;
	    return System.Math.Floor(diff.TotalSeconds);
	}

	string IntToTime(int seconds){
		string output = "";
		if(saveDivide(seconds,3600) > 0)
			output += saveDivide(seconds, 3600).ToString().PadLeft(2,'0') + ":";
		if(saveDivide(seconds,60) > 0){
			output += (saveDivide(seconds, 60) % 60).ToString().PadLeft(2,'0') + ":";
		}
		output += (seconds % 60).ToString().PadLeft(2,'0');
		
		return output;
	}

	int saveDivide(int t, int o){
		int result = -1;
		if(o == 0 || o > t){
			return 0;
		}
		while(t > o * (++result));
		return result;
	}

	public List<cat> GetCats(){
		return overallData.Ownedcats;
	}

	public bool SendGroup(List<int> crew, int levelID){
		return overallData.SendGroup(crew.ToArray(), levelID);
	}

	void ChangeUIGroup(){
		foreach(Transform child in _groupPanel.transform){
			Destroy(child.gameObject);
		}
		for(int i = 0;i<overallData.maxGroupCount;i++){
			if(i<overallData.maxGroupCount-overallData.groupCount){
				Instantiate(_groupAvailable,Vector3.zero,new Quaternion(),_groupPanel.transform);
			}else{
				Instantiate(_groupUnavailable,Vector3.zero,new Quaternion(),_groupPanel.transform);
			}
		}
		ChangeGroupCount();
	}

	void ChangeGroupCount(){
		StartCoroutine(StartExploreClock());
	}

	IEnumerator StartExploreClock(){
		double time = 0;
		while(overallData.groupCount > 0){
			for(int i = 0;i< overallData.groupCount;i++){
				if(overallData.GetGroupData(i).ETC > time){
					time = overallData.GetGroupData(i).ETC;
				}
			}
			foreach(Text t in _exploreTimeText){
				t.text = IntToTime((int)(time - ConvertToUnixTimestamp(System.DateTime.Now)));
			}
			yield return new WaitForSeconds(1f);
		}
		foreach(Text t in _exploreTimeText){
				t.text = "";
			}
	}

	public void EventMessage(string text){
		_pendingMessage.Add(text);
	}

	public int maxGroupPplCount{
		get{
			return overallData.maxGroupPplCount;
		}
	}

	public int groupCount{
		get{
			return overallData.groupCount;
		}
	}

	public int maxGroupCount{
		get{
			return overallData.maxGroupCount;
		}
	}

	IEnumerator ShowPendingMessage(){
		_isMessengerActive = true;
		messengerText.text = _pendingMessage[0];
		messengerAnim.Play("messenger_show");
		_pendingMessage.RemoveAt(0);
		yield return new WaitForSecondsRealtime(0.7f);
		_isMessengerActive = false;
	}

}
