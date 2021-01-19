const NativeUI = require("js/nativeui.js");
const Menu = NativeUI.Menu;
const UIMenuItem = NativeUI.UIMenuItem;
const Point = NativeUI.Point;

const categoryTitles = 
[
	"Spoiler",
	"Front Bumper",
	"Rear Bumper",
	"Side Skirts",
	"Exhaust",
	"Rollcage",
	"Grille",
	"Bonnet",
	"Fenders and Arches",
	"Fenders",
	"Roof",
	"Engine",
	"Brakes",
	"Transmission",
	"Horn",
	"Suspension",
	"Armor",
	"",
	"",
	"",
	"",
	"",
	"Headlights",
	"",
	"",
	"Plate Holders",
	"Vanity Plates",
	"Interior Trim",
	"Ornaments",
	"Interior Dash",
	"Dials",
	"Door Speakers",
	"Leather Seats",
	"Steering Wheels",
	"Column Shifters",
	"Plaques",
	"ICE",
	"Speakers",
	"Hydraulics",
	"Engine Block",
	"Air Filters",
	"Strut Braces",
	"Arch Covers",
	"Aerials",
	"Exterior Trim",
	"Tank",
	"Windows",
	"",
	"Livery"
];

// main menu
let mainMenu = null;

let curCategory = -1;
let transition = false;

let menus = {};

function createTuningMenu(vehicle)
{
	curCategory = -1;
	let categoryMenus = [];

	if(mainMenu)
	{
		mainMenu.Visible = false;
	}
	
	if(menus.hasOwnProperty(vehicle.model))
	{
		mainMenu = menus[vehicle.model];
		return;
	}
	
	mainMenu = new Menu("Vehicle Tuning", "", new Point(1250, 150));
	mainMenu.Visible = false;
	
	menus[vehicle.model] = mainMenu;
	
	mainMenu.vehicleModel = vehicle.model;

	mainMenu.ItemSelect.on((item, index) => {
		mainMenu.Visible = false;
		curCategory = index;
		categoryMenus[index].Visible = true;
		transition = true;
	});

	

	// categories
	for (let i = 0; i < categoryTitles.length; i++) 
	{
		let numMods = vehicle.getNumMods(i);
		
		if(numMods > 0 && categoryTitles[i].length > 0)
		{
			mainMenu.AddItem(new UIMenuItem(categoryTitles[i], ""));

			let categoryMenu = new Menu(categoryTitles[i], "", new Point(1250, 150));
			categoryMenu.Visible = false;

			categoryMenu.ItemSelect.on((item, index) => {
				if (!transition) mp.events.callRemote("vtuning_set", item.modType, item.modIndex);
				transition = false;
			});

			categoryMenu.MenuClose.on(() => {
				curCategory = -1;
				mainMenu.Visible = true;
			});
			
			categoryMenu.modType = i;

			for(let modIndex = 0; modIndex < numMods; modIndex++)
			{
				let vehicleName = mp.game.ui.getLabelText(vehicle.getModTextLabel(i, modIndex));
				let vehicleItem = new UIMenuItem(vehicleName == "NULL" ? `${categoryTitles[i]} #${modIndex}` : vehicleName, "");
				vehicleItem.modType = i;
				vehicleItem.modIndex = modIndex;
				categoryMenu.AddItem(vehicleItem);
			}

			categoryMenus.push(categoryMenu);
		}
	}
}


mp.events.add("toggle_veh_tuning_menu", () =>
{
   if (curCategory > -1) 
	{
        categoryMenus[curCategory].Visible = !categoryMenus[curCategory].Visible;
    }
	else 
	{
		if(!mainMenu || !mainMenu.Visible)
		{		
			let vehicle = mp.players.local.vehicle;
			
			if(vehicle)
			{				
				if(!mainMenu
					|| mainMenu.vehicleModel !== vehicle.model)
				{
					createTuningMenu(vehicle);
				}
				
				mainMenu.Visible = true;
			}
			else
			{
				mp.game.graphics.notify("~R~You should be in a vehicle to access this menu.");
			}
		}	
		else
		{
			mainMenu.Visible = false;
		}
    }	
});

// f5 key - toggle menu visibility
mp.keys.bind(0x74, false, () => 
{
	mp.events.call("toggle_veh_tuning_menu");
});