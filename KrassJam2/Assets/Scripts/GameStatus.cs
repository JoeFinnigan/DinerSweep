using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameStatus {

	/*
	 * ACTIVE - game still running
	 * COMPLETED_DEBT - player ran into negative digits (-100?)
	 * COMPLETED_TURNCOUNT - turn limit was reached
	 * COMPLETED_ZEROPERCENT - player reached 0% on progress bar
	 * COMPLETED_HUNDREDPERCENT - player reached 100% on progress bar
	*/
	public enum Status {ACTIVE, COMPLETED_DEBT, COMPLETED_TURNCOUNT, COMPLETED_ZEROPERCENT, COMPLETED_HUNDREDPERCENT}

	public Status status;
}
