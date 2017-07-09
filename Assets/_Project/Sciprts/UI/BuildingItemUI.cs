using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingItemUI : MonoBehaviour {

	[SerializeField] GameObject buildingPrfab;
	[SerializeField] Transform bornParent;

	public void Build(BuyBuildingEvent buildEvent){

		if (buildEvent == null)
			return;

		if (buildingPrfab != null) {
			GameObject _go = Instantiate (buildingPrfab) as GameObject;
			_go.transform.SetParent (bornParent);
			Building _building = _go.GetComponent<Building> ();
			buildEvent.item = _building;
			_building.Init (buildEvent.buyers, buildEvent.setGrid);
		}
	}
}
