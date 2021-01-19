    /*mp.events.addCommand('engine', (player, fullText) => {

    mp.players.local.vehicle.setHandling("fTractionCurveLateral ", parseFloat(fullText));
    
    });*/

    /*mp.events.addCommand("me", (player, message) => {
        mp.players.broadcast(`* ${player.name}: ${message}`);
    });*/
    const controlsIds = {
        F7: 0x76,
        W: 32,
        S: 33,
        A: 34,
        D: 35, 
        Space: 321,
        LCtrl: 326,
        LMB: 24,
        RMB: 25
    };

    mp.events.addCommand("curv", function (amount) {
        amount = Number(amount);
        
        mp.players.local.vehicle.setHandling("fTractionCurveLateral", amount);
        
        mp.gui.chat.push(`${amount}`);
    });
     
    mp.events.addCommand("df", function (amount) {
        
        mp.game.player.vehicle.setHandling("fDriveInertia ", amount);
    });

    mp.events.addCommand("dfs", function (amount) {
        
        mp.gui.chat.push(`${mp.game.player.vehicle.getHandling("fDriveInertia ")}`);
    });

    mp.events.addCommand("4", function (amount) {
        mp.gui.chat.push(`${mp.players.local.vehicle.getHandling("fTractionCurveLateral")}`);
    });
    
    mp.events.addCommand("2", function (amount) {
        amount = Number(amount);
        
        mp.players.local.vehicle.setHandling("fSteeringLock", amount);
        mp.gui.chat.push(`${amount}`);
    });

    mp.events.addCommand("3", function () {
        mp.players.local.vehicle.setHandling("fMass", 3500);
        mp.players.local.vehicle.setHandling("fInitialDragCoeff", 4.77);
    });

    
    
    mp.keys.bind(0x71, true, function() {
        mp.players.local.vehicle.setHandling("fTractionCurveLateral ", 0);
    });