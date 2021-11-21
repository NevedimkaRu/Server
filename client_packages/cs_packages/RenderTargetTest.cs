using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using RAGE;

namespace cs_packages
{
	class RenderTargetTest : Events.Script
	{
		/*public RenderTargetTest()
		{
			Events.Tick += Tick;
		}

		private void Tick(List<Events.TickNametagData> nametags)
		{

		}
		//Create object for example
		public RAGE.Elements.MapObject CreateModel(string model, Vector3 pos, Vector3 rot)
		{
			if (!RAGE.Game.Streaming.HasModelLoaded(RAGE.Util.Joaat.Hash(model)))
				RAGE.Game.Streaming.RequestModel(RAGE.Util.Joaat.Hash(model));
			while (!RAGE.Game.Streaming.HasModelLoaded(RAGE.Util.Joaat.Hash(model)))
				Task.WaitAsync();
			RAGE.Elements.MapObject obj = new RAGE.Elements.MapObject(RAGE.Util.Joaat.Hash(model), pos, rot);
			return obj;

		}
		public int CreateRenderTarget(string name, string model)
		{
			if (RAGE.Game.Ui.IsNamedRendertargetRegistered(name))
				RAGE.Game.Ui.RegisterNamedRendertarget(name, false);//Register render target
			if (RAGE.Game.Ui.IsNamedRendertargetLinked(RAGE.Util.Joaat.Hash(model)))
				RAGE.Game.Ui.LinkNamedRendertarget(RAGE.Util.Joaat.Hash(model));//Link it to all models
			if (RAGE.Game.Ui.IsNamedRendertargetRegistered(name))
				return RAGE.Game.Ui.GetNamedRendertargetRenderId(name);//Get the handle

			return -1;
		}

		public void RenderThing(int id)
		{
			RAGE.Game.Ui.SetTextRenderId(id);
			RAGE.Game.Graphics.Set2dLayer(4);

			RAGE.Game.Graphics.DrawRect(0.5f, 0.5f, 1.0f, 1.0f, 255, 0, 0, 255, 0);
			RAGE.NUI.UIResText.Draw($"RAGEMP",
					(int)(1),
					(int)(2),
					RAGE.Game.Font.HouseScript,
					0.4f,
					Color.White,
					RAGE.NUI.UIResText.Alignment.Centered, true, true, 0);
		}*/

		/*function RenderThings(id)
		{
			mp.game.ui.setTextRenderId(id); //Set render ID of render target
			mp.game.graphics.set2dLayer(4); //Only layer 4 works

			mp.game.graphics.drawRect(0.5, 0.5, 1, 1, 255, 0, 0, 255); //Draw rect is always behind text/sprites

			mp.game.graphics.drawText("~r~Rage~w~MP", [0.5, 0.35], //Draw text is always the most top element
		{
				font: 0, 
		  color:[255, 255, 255, 255], 
		  scale:[2.0, 2.0], 
		  outline: true
		});

				//Scaleforms work too although the majority have messed up scaling
				mp.game.graphics.drawScaleformMovie(scale, 0.5, 0.5, 1, 1, 255, 255, 255, 255, 0);

				//Draw sprites. The layering for this is last drawn is the most top element
				mp.game.graphics.drawSprite("mpweaponsgang0", "w_ar_advancedrifle", 0.25, 0.5, 0.25, 0.25, 0, 255, 255, 255, 255);
				mp.game.graphics.drawSprite("mpweaponsgang0", "w_ex_grenadefrag", 0.25, 0.5, 0.25, 0.25, 0, 255, 255, 255, 255);

				mp.game.ui.setTextRenderId(1); //Do not forget to reset the render ID. 1 is always the default render target the game uses
			}

			var TargetsToRender = [];
			var scale = 0;

			mp.keys.bind(69, false, () => //Button E
	{
		var pos = mp.players.local.position;
			pos.z += 1;
	
		scale = mp.game.graphics.requestScaleformMovie("cellphone_ifruit");
		while(!mp.game.graphics.hasScaleformMovieLoaded(scale))
			mp.game.wait(0);
	
		var x = CreateModel("xm_prop_x17dlc_monitor_wall_01a", pos, new mp.Vector3());
			pos.x += 10;
		var x = CreateModel("xm_prop_x17dlc_monitor_wall_01a", pos, new mp.Vector3());
			var id = CreateRenderTarget("prop_x17dlc_monitor_wall_01a", "xm_prop_x17dlc_monitor_wall_01a");
		if(id != -1)
			TargetsToRender.push(id);
		else
			mp.gui.chat.push("Could not create render target.");
	});

	/*mp.events.add("render", () =>
	{
		for(var i = 0; i<TargetsToRender.length; i++)
		{
			RenderThings(TargetsToRender[i]);
	}
	});*/
	}
}
