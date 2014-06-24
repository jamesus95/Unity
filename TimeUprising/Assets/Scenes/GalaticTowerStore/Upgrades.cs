using UnityEngine;
using System.Collections;
using System.IO;
using System;

public enum BonusSubject : int {
	Melee = 0,
	Ranged = 1,
	Special = 2,
	HealTower = 3,
	AOETower = 4,
	BuffTower = 5,
}

public enum BonusType : int{
	Health = 0,
	CoolDown = 1,
	SpawnSize = 2,
	TowerModifier = 3,
}


public class Upgrades : MonoBehaviour
{
    public static void ResetTowerUpgrades ()
    {
        if(File.Exists(kTowerUpgradesPath))
            File.Delete(kTowerUpgradesPath);
    }

	const float kLetterDisplayTime = 0.5f;

    //string[] subjectName = ;
	int mSubjectMax = Enum.GetNames(typeof(BonusSubject)).Length;
	int mBonusMax = Enum.GetNames(typeof(BonusType)).Length;
    float mBonusMagnitude = 0.2f;
	
    private static string kTowerUpgradesPath = SaveLoad.kSaveFileDirectory + "Bonus.save"; //path of the txt file
	StreamReader mFile;
	string line; //used to read line from mfile and arrays
	
	
	int[,] mBonusArray;

	
	// Use this for initialization
	void Awake ()
	{
		mBonusArray = new int[mSubjectMax, mBonusMax];
		
		if(File.Exists(kTowerUpgradesPath)){
			LoadBonuses();		
		}
		else{
		//	PopulateArray();
			WriteBonuses();
		}
		
	}
	

	void LoadBonuses()
	{
		mFile = new StreamReader(kTowerUpgradesPath);	
		System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en");
		char[] delim = new char[1];
		delim[0] = ',';
		while (!mFile.EndOfStream)
		{
			int curSubject = -1;
			int curBonus = -1;
			line = mFile.ReadLine();

			string []numbers = line.Split(delim[0]);

			if(numbers.Length < 3)
				continue;


			int ability = ci.CompareInfo.IndexOf(numbers[0], "Heal", System.Globalization.CompareOptions.IgnoreCase);
			int melee = ci.CompareInfo.IndexOf(numbers[0], "Melee", System.Globalization.CompareOptions.IgnoreCase);
			int ranged = ci.CompareInfo.IndexOf(numbers[0], "Ranged", System.Globalization.CompareOptions.IgnoreCase);
            int magic = ci.CompareInfo.IndexOf(numbers[0], "Special", System.Globalization.CompareOptions.IgnoreCase);
            int AOE = ci.CompareInfo.IndexOf(numbers[0], "AOETower", System.Globalization.CompareOptions.IgnoreCase);
            int Buff = ci.CompareInfo.IndexOf(numbers[0], "BuffTower", System.Globalization.CompareOptions.IgnoreCase);

			int health = ci.CompareInfo.IndexOf(numbers[1], "Health", System.Globalization.CompareOptions.IgnoreCase);
			int coolD = ci.CompareInfo.IndexOf(numbers[1], "CoolDown", System.Globalization.CompareOptions.IgnoreCase);
			int spawnSize = ci.CompareInfo.IndexOf(numbers[1], "SpawnSize", System.Globalization.CompareOptions.IgnoreCase);
			int ampAbiilty = ci.CompareInfo.IndexOf(numbers[1], "TowerModifier", System.Globalization.CompareOptions.IgnoreCase);
			

			if(ability >= 0)
				curSubject = (int)BonusSubject.HealTower;
			else if(melee >= 0)
				curSubject = (int)BonusSubject.Melee;
			else if(ranged >= 0)
				curSubject = (int)BonusSubject.Ranged;
            else if(magic >= 0)
                curSubject = (int)BonusSubject.Special;
            else if(AOE >= 0)
                curSubject = (int)BonusSubject.AOETower;
            else if(Buff >= 0)
                curSubject = (int)BonusSubject.BuffTower;

			if(health >= 0)
				curBonus = (int)BonusType.Health;
			else if(coolD >= 0)
				curBonus = (int)BonusType.CoolDown;
			else if(ampAbiilty >= 0)
				curBonus = (int)BonusType.TowerModifier;
			else if(spawnSize >= 0)
				curBonus = (int)BonusType.SpawnSize;
		

			if (curSubject != -1 && curBonus != -1){
				mBonusArray[curSubject, curBonus] = int.Parse(numbers[2]);				
			}
		}
		mFile.Close();
	}	
	public float GetBonus(BonusSubject subject, BonusType bonus){
		float bonusLevel = mBonusArray[(int)subject ,(int) bonus];
		
        //spawn size needs to return a whole number
        if(bonus == BonusType.SpawnSize)
            return bonusLevel;

        //no bonuses purchases
        if(bonusLevel <= 0)
			return 1.0f;
        //max bonuses cap
		if(bonusLevel >= 5)
		   return 2.0f;

		//each level represents 20%
		return 1 + bonusLevel * mBonusMagnitude;
	}
	public void SetBonus(BonusSubject subject, BonusType bonus, int value){
		mBonusArray[(int)subject ,(int) bonus] = value;
	}
	public void WriteBonuses(){
		StreamWriter writer = new StreamWriter(kTowerUpgradesPath);

			foreach(string sub in Enum.GetNames(typeof(BonusSubject))){
				foreach(string bon in Enum.GetNames(typeof(BonusType))){
					writer.WriteLine( sub + "," + bon + "," +
                	mBonusArray[(int)Enum.Parse(typeof(BonusSubject),sub) , 
				            (int)Enum.Parse(typeof(BonusType),bon)].ToString());
				}
		}
		writer.Close();
	}
	void PopulateArray(){
		for(int i = 0; i < mSubjectMax; i ++)
			for(int j = 0; j < mBonusMax; j++)
				mBonusArray[i ,j] = 0;
	}
	public float GetUnitUpgrades(UnitType unit, BonusType sType){
		BonusSubject mySubject = (BonusSubject)(-1);
		BonusType myType = sType;

		switch(unit){
		case UnitType.Swordsman:
			mySubject = BonusSubject.Melee;
			break;
		case UnitType.Archer:
			mySubject = BonusSubject.Ranged;
			break;
		case UnitType.Mage:
			mySubject = BonusSubject.Special;
			break;

		}
		if(mySubject != (BonusSubject)(-1))
			return GetBonus(mySubject, myType);
		else
			return 1.0f;
	}
	public void SetStoreArray(ref int[,] tempArray){
		for(int i = 0; i < mSubjectMax; i ++)
			for(int j= 0; j < mBonusMax; j++){
				tempArray[i,j] = mBonusArray[i,j]; 
		}
	}
	public void SetUpgradeArray(int[,] tempArray){
		for(int i = 0; i < mSubjectMax; i ++)
			for(int j = 0; j < mBonusMax; j++){
				mBonusArray[i,j] = tempArray[i,j]; 
		}
	}

}