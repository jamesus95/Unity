using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SquadAction
{
    EnemySighted, 
    EngagedInMelee, 
    UnitDestroyed,
    DestinationReached,
    UnitDied,
    TargetDestroyed,
}

public enum SquadState
{
    Idle,
    Moving,
    ForcedMove,
    Engaging,
    Melee,
}

public class Squad : MonoBehaviour, Selectable
{
    ///////////////////////////////////////////////////////////////////////////////////
    // Public Methods
    ///////////////////////////////////////////////////////////////////////////////////
    public UnitType UnitType;
    public int NumSquadMembers { get { return mSquadMembers.Count; } }
    public Vector3 SquadCenter;
    public Vector3 RallyPoint;
    
    public List<Unit> SquadMembers { get { return mSquadMembers; } }

    public Unit SquadLeader { get { return mSquadMembers [0]; } }

    public SquadState SquadState { 
        get { return mSquadState; } 
        set { mSquadState = value; }
    }

    public bool IsIdle { get { return SquadState == SquadState.Idle; } }

    public bool IsDead {
        get { return mSquadMembers == null || mSquadMembers.Count == 0; }
    }

    public bool IsEngaged { 
        get { 
            return ! (SquadState == SquadState.Idle 
                || SquadState == SquadState.Moving
                || SquadState == SquadState.ForcedMove); 
        } 
    }
    
    public bool IsIndependent = false;
    public SpriteRenderer SightCircle;
    
    public void Notify (SquadAction action, params object[] args)
    {
        switch (action) {
        case(SquadAction.EnemySighted):
            if (this.SquadState == SquadState.ForcedMove)
                return;
            AttackTarget ((Target)args [0]);
            break;
            
        case(SquadAction.DestinationReached):
            CheckSquadIdle ();
            break;
            
        case(SquadAction.UnitDestroyed):
            AssignNewTarget ((Unit)args [0]);
            break;
            
        case(SquadAction.EngagedInMelee):
            EngageInMelee ();
            break;
            
        case (SquadAction.UnitDied):
            UpdateSquadMembers ((Unit)args [0]);
            break;
            
        case (SquadAction.TargetDestroyed):
            Disengage ();
            break;
        }
    }

    private void CheckSquadIdle ()
    {
        if (SquadLeader.IsIdle)
            this.SquadState = SquadState.Idle;
    }
    
    public void ForceMove (Vector3 location)
    {
        RallyPoint = location;
        location.z = 0;

        Disengage (); 
        this.SquadState = SquadState.ForcedMove;
        foreach (Unit u in mSquadMembers)
            u.MoveTo(RallyPoint);
    }
    
    public void SetDestination (Vector3 location)
    {
        if (Vector3.Distance (RallyPoint, location) < 5f) {
            ForceMove(location);
        } else {
            RallyPoint = location;
            location.z = 0;
            
            if (IsEngaged)
                return;
            
            this.SquadState = SquadState.Moving;
            UpdateSquadCoherency ();
        }
    }
    
    public void Spawn (UnitType type, Vector3 location, int count, Allegiance allegiance)
    {
        mAllegiance = allegiance;
        
        this.transform.position = location; 
        
        mUnitPrefab = TextureResource.GetUnitPrefab(type, (allegiance == Allegiance.Rodelle));
        
        if (mSquadMembers != null) {
            foreach (Unit u in mSquadMembers)
                Destroy (u.gameObject);
        }
        
        mSquadMembers = new List<Unit> ();
        
        List<Vector3> randomPositions = this.RandomSectionLocations (count, kSquadMemberWidth * 1.5f);
        
        // Instantiates and initializes the position of each member in the squad
        // TODO fix the placement for large numbers of squad members
        for (int i = 0; i < count; ++i) {
            // instantiate the unit from the prefab
            GameObject o = (GameObject)Instantiate (mUnitPrefab);
            Unit u = (Unit)o.GetComponent (typeof(Unit));
            u.Squad = this;
            mSquadMembers.Add (u);
            
            // offset from squad center
            Vector3 memberPosition = this.transform.position;
            memberPosition += randomPositions [i];
            u.transform.position = memberPosition;
            u.Allegiance = mAllegiance;
            
        }
        
        if (SquadLeader.Allegiance == Allegiance.AI) // Reduce the sight range of armed enemy peasants
            SquadLeader.SightRange = UnitStats.GetStat(UnitType.Peasant, UnitStat.SightRange);

        float sightRadius = SquadLeader.SightRange;
        
        InitializeSightCircleSprite ();
        this.GetComponent<CircleCollider2D> ().radius = 3;
        this.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(sightRadius / 3, sightRadius / 3, 0f);
        
        this.SetDestination (this.RallyPoint);
        this.ShowSelector(false);
    }
    
    public void AddUnits(UnitType type, Vector3 location, int count, Allegiance allegiance)
    {
        if (count == 0)
            return;
        
        if (mSquadMembers == null)
            mSquadMembers = new List<Unit>();
        
        mAllegiance = allegiance;
        
        this.transform.position = location; 
        
        GameObject unitPrefab = TextureResource.GetUnitPrefab(type, (allegiance == Allegiance.Rodelle));
        
        List<Vector3> randomPositions = this.RandomSectionLocations (count, kSquadMemberWidth * 1.5f);
        
        for (int i = 0; i < count; ++i) {
            // instantiate the unit from the prefab
            GameObject o = (GameObject)Instantiate (unitPrefab);
            Unit u = (Unit)o.GetComponent<Unit>();
            u.Squad = this;
            mSquadMembers.Add (u);
            u.Allegiance = mAllegiance;
            
            // offset from squad center
            Vector3 memberPosition = this.transform.position;
            memberPosition += randomPositions [i];
            u.transform.position = memberPosition;
        }
        
        float squadSpeed = UnitStats.GetStat(SquadLeader.UnitType, UnitStat.MovementSpeed);
        
        // Sets the speed of the squad to the speed of the squad leader
        for (int i = 0; i < mSquadMembers.Count; ++i) {
            // TODO refactor this process. Remove the public access to MoveSpeed and implement a better method of 
            // modifying a unit's base stats
            mSquadMembers[i].MoveSpeed = squadSpeed;
        }
        
        if (SquadLeader.Allegiance == Allegiance.AI) // Reduce the sight range of armed enemy peasants
            SquadLeader.SightRange = UnitStats.GetStat(UnitType.Peasant, UnitStat.SightRange);
        
        float sightRadius = SquadLeader.SightRange;
        
        this.GetComponent<CircleCollider2D> ().radius = 3;
        this.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(sightRadius / 3, sightRadius / 3, 0f);
        
        this.SetDestination (this.RallyPoint);
        this.ShowSelector(false);
    }
    
    ///////////////////////////////////////////////////////////////////////////////////
    // Interface: Selectable
    ///////////////////////////////////////////////////////////////////////////////////
    
    public void ShowSelector (bool status)
    {
        if (mSquadMembers.Count == 0)
            return;
            
        foreach (Unit u in mSquadMembers)
            if (u.Selector != null)
                u.Selector.enabled = status;
            
        this.GetComponent<SpriteRenderer>().enabled = status;
    }
    
    // does nothing
    public void UseTargetedAbility (Target target) {}
    
    public void Select ()
    {
        ShowSelector (true);
    }
    
    public void Deselect ()
    {
        ShowSelector (false);
    }
    
    ///////////////////////////////////////////////////////////////////////////////////
    // Private Methods
    ///////////////////////////////////////////////////////////////////////////////////
    private GameObject mUnitPrefab;
    private GameObject mEnemyPrefab;
    private Squad mTargetSquad;
    private List<Unit> mSquadMembers;
    protected Allegiance mAllegiance;
    private SquadState mSquadState;
    public int mTargetPriority;
    
    // Squad members form concentric circles around the squad center
    // the member width is used to determine the width of each band of the circles
    // ie. inner circle radius = kSquadMemberWidth * 1.5
    //     2nd circle width = kSquadMemberWidth * 1.5
    private const float kSquadMemberWidth = 3.0f;
    
    private void AttackTarget (Target target)
    {      
        if (target == null || HasLowerPriority(target))
            return;

        if (target == null)
            Debug.Break ();
        
        mTargetPriority = TargetPriority(target);
        
        if (target is Unit) {
            Unit u = (Unit)target.GetComponent (typeof(Unit));
            mTargetSquad = u.Squad;
            AttackEnemySquad (u.Squad);
        } else if (target is Tower) {
            mTargetSquad = null;
            Tower t = (Tower)target.GetComponent (typeof(Tower));
            AttackEnemyTower (t);
        }
    }
        
    private void AssignNewTarget (Unit who)
    {
        if (this.mAllegiance == Allegiance.Rodelle) {
            UnitStats.AddToExperience (who.UnitType, 1);
        }
            
        List<Unit> mEnemies = mTargetSquad.mSquadMembers;
        int numEnemies = mEnemies.Count;
        
        if (numEnemies == 0)
            Disengage ();
        else 
            who.Engage (mEnemies [Random.Range (0, numEnemies)]); // engage random enemy
    }
    
    private void UpdateSquadMembers (Unit who)
    {
        if (this.gameObject == null)
            return;
            
        mSquadMembers.Remove (who);
        
        if (mSquadMembers.Count <= 0) {
            Destroy (this.gameObject);
        }
    }
    
    private void AttackEnemySquad (Squad enemySquad)
    {
        if (enemySquad.IsDead)
            return;
            
        this.SquadState = SquadState.Engaging;
        
        Unit squadUnit = this.SquadLeader;
        // Surround the enemy!
        List<Vector3> positions = SurroundingPositions (
            enemySquad.SquadCenter,
            this.SquadCenter, 
            mSquadMembers.Count, 
            kSquadMemberWidth * 2.0f, 
            squadUnit.Range); // TODO add target radius for large targets
        
        List<Unit> mEnemies = enemySquad.mSquadMembers;
        int numEnemies = mEnemies.Count;
        
        // engage enemies 1 to 1
        for (int i = 0; i < mSquadMembers.Count; ++i) {
            mSquadMembers [i].Engage (mEnemies [i % numEnemies], positions [i]);
        }
        
        enemySquad.Notify (SquadAction.EnemySighted, this.SquadLeader);
    }
    
    private void AttackEnemyTower (Tower tower)
    {
        this.SquadState = SquadState.Engaging;
        
        Unit squadUnit = this.SquadLeader;
        // Surround the enemy!
        List<Vector3> positions = SurroundingPositions (
            tower.Position,
            this.SquadCenter, 
            mSquadMembers.Count, 
            kSquadMemberWidth * 2.0f, 
            squadUnit.Range); // TODO add target radius for large targets
            
        // engage in a circle around the tower
        for (int i = 0; i < mSquadMembers.Count; ++i) {
            mSquadMembers [i].Engage (tower, positions [i]);
        }
    }
    
    // re-enable the search for new enemies
    private void Disengage ()
    {
        mTargetSquad = null;
        mTargetPriority = 0;
        this.SquadState = SquadState.Moving;
        for (int i = 0; i < mSquadMembers.Count; ++i) {
            mSquadMembers [i].Disengage ();
        }
        UpdateSquadCoherency ();
    }
    
    public void EngageInMelee ()
    {
        this.SquadState = SquadState.Melee;
        for (int i = 0; i < mSquadMembers.Count; ++i)
            mSquadMembers [i].SwitchToMeleeWeapon ();
    }
    
    private void UpdateSquadCoherency ()
    {
        // Randomize the squad's new position around the central location
        List<Vector3> randomPositions = 
            RandomSectionLocations (mSquadMembers.Count, kSquadMemberWidth * 1.5f);
        
        // Move each squad member to their new location
        for (int i = 0; i < mSquadMembers.Count; ++i) {
            Vector3 memberPosition = RallyPoint;
            memberPosition += randomPositions [i];
            mSquadMembers [i].MoveTo (memberPosition);
        }
    }
    
    /**
    Creates random directions for squad members to form a concentric circle around the target location
    
    @param numSections: how many sections to divide the circle into
    @param unitWidth: the width of each unit
    
    TODO fix the placement for large numbers of squad members
    **/
    private List<Vector3> RandomSectionLocations (int numSections, float circleWidth)
    {
        if (numSections <= 0) // return empty list
            return new List<Vector3> ();
            
        List<Vector3> randomLocations = new List<Vector3> ();
        
        List<float> randomAngles = new List<float> ();
        float anglesPerSection = 360f / (numSections - 1);
        for (int i = 0; i < numSections; ++i) {
            float randomAngle = Random.Range (anglesPerSection * .3f, anglesPerSection * 0.7f);
            randomAngle += i * anglesPerSection;
            randomAngles.Add (randomAngle * Mathf.Deg2Rad);
        }
        
        randomLocations.Add (Vector3.zero); // place first squad in center
        // assigns a random position in the section
        for (int i = 1; i < numSections; ++i) {
            float angle = randomAngles [i];
            Vector3 randomDir = new Vector3 (Mathf.Cos (angle), Mathf.Sin (angle), 0f);
            randomDir.Normalize ();
            //randomDir *= circleWidth;
            randomDir *= Random.Range (circleWidth, circleWidth * 2f);
            randomLocations.Add (randomDir);
        }
        
        return randomLocations;
    }
    
    /**
    Creates vectors to points on a circle around the target
      
    @param x: target to surround
    @param y: the direction to surround from
    @param n: number of units surrounding target
    @param w: width of each unit
    @param r: radius of the circle
    */
    private List<Vector3> SurroundingPositions (Vector3 x, Vector3 y, int n, float w, float r)
    {
        List<Vector3> surroundingPositions = new List<Vector3> ();
        
        Vector3 toTarget = y - x;
        toTarget.Normalize ();
        float sign = Vector3.Cross (Vector3.right, toTarget).normalized.z;
        float theta = Mathf.Acos (Vector3.Dot (Vector3.right, toTarget));
        theta *= Mathf.Rad2Deg * sign;
        
        float angleBetweenUnits = 2f * Mathf.Atan (0.5f * w / r) * Mathf.Rad2Deg;
        
        List<float> angles = new List<float> ();
        
        float toggle = 1;
        for (int i = 0; i < n; ++i) {
            toggle *= -1; // flip back and forth
            theta += i * toggle * angleBetweenUnits;
            
            angles.Add (theta * Mathf.Deg2Rad);
        }
        
        for (int i = 0; i < n; ++i) {
            Vector3 newPosition = x;
            newPosition.x = r * Mathf.Cos (angles [i]);
            newPosition.y = r * Mathf.Sin (angles [i]);
            
            surroundingPositions.Add (newPosition);
        }
        
        return surroundingPositions;
    }
    
    private bool HasLowerPriority (Target target)
    {
        int targetPriority = TargetPriority (target);
            
        if (mTargetPriority == 3 && targetPriority == 3) {
            Unit unit = (Unit) target;
            if (unit.Squad == mTargetSquad)
                return true;
            
            if (mTargetSquad.IsDead || this.IsDead)
                return false;
            
            if (Vector3.Distance(mTargetSquad.SquadLeader.Position, this.SquadLeader.Position) <
                Vector3.Distance(unit.Position, this.SquadLeader.Position))
                return true;
            return false;
        }
        
        return targetPriority <= mTargetPriority;
    }
    
    private int TargetPriority (Target target)
    {
        if (target is Tower)
            return 1;
        
        if (target is Unit && ((Unit)target).UnitType == UnitType.King)
            return 2;
        
        if (target is Unit)
            return 3;
            
        return 0;
    }
    
    private void UpdateRangeCircle()
    {
        float sightRadius;
        switch (this.SquadState) {
        case (SquadState.Moving):
            sightRadius = SquadLeader.SightRange * 0.85f;
            break;
        case (SquadState.Idle):
            sightRadius = SquadLeader.SightRange;
            break;
        default:
            sightRadius = 0;
            break;
        }
        
        this.GetComponent<CircleCollider2D> ().radius = 3;
        this.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(sightRadius / 3, sightRadius / 3, 0f);
    }
    
    private void InitializeSightCircleSprite ()
    {
        Sprite[] rangeCircleTextures = Resources.LoadAll<Sprite>("Squads/Textures/RangeCircles255x255");
        SpriteRenderer sr = this.GetComponent<SpriteRenderer> ();
        if (this.UnitType == UnitType.Swordsman)
            sr.sprite = rangeCircleTextures[0];
        if (this.UnitType == UnitType.Archer)
            sr.sprite = rangeCircleTextures[1];
        if (this.UnitType == UnitType.Mage)
            sr.sprite = rangeCircleTextures[2];
    }
    
    ///////////////////////////////////////////////////////////////////////////////////
    // Unity Overrides
    ///////////////////////////////////////////////////////////////////////////////////
    
    void OnTriggerStay2D (Collider2D other)
    {
        this.OnTriggerEnter2D (other);
    }
    
    // Check for enemies in sight range
    void OnTriggerEnter2D (Collider2D other)
    {            
        Target target = other.gameObject.GetComponent<Target> ();
        
        if (target != null && target.Allegiance != mAllegiance) {
            Notify (SquadAction.EnemySighted, target);
        }
    }
        
    void Update ()
    {
        if (mSquadMembers == null)
            return;
        
        // Follow sight of squad leader
        this.SquadCenter = mSquadMembers[0].Position; 
        this.transform.position = SquadCenter;
        
        UpdateRangeCircle();
    }
    
    // Use this for initialization
    void Awake ()
    {    
        this.SquadState = SquadState.Idle;
        if (IsIndependent) {
            this.Spawn (this.UnitType, this.transform.position, 1, Allegiance.Rodelle);
            this.SetDestination (this.transform.position);
        }
    }
}
