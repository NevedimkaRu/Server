let isShake = false;
let timerCounter;
let oldScore = 0;

$(document).ready(function() {
});

function driftScore(score, multiplier) {
	$('#counter').removeClass('falling');
	if(multiplier == '1') {
		$('#score').html(score);
		$('#multiple').html('');
	}
	else {
		$('#score').html(score);
		$('#multiple').html('&nbsp;&nbsp;x' + multiplier);
	}
	if(Number(multiplier) > 1) {
		if(Number(score) > oldScore) {
			setShake()
		}
	}
	oldScore = score;
	//clearInterval(timerCounter);
	//timerCounter = setTimeout(function() {$('#counter').removeClass('hover'); isShake = false;}, 4000)
}

function driftError()
{
	oldScore = 0;
	$('#counter').removeClass('hover');
	isShake = false;
	$('#counter').addClass('falling');
	let interval = setInterval(function() {
		$('#score').html('');
		$('#multiple').html('');
		clearInterval(interval)
	}, 700)
}

function setShake(){
	if(!isShake){
		$('#counter').addClass('hover');
		timerCounter = setTimeout(function() {$('#counter').removeClass('hover'); isShake = false;}, 1000);
		isShake = true
	}
}