var car = null;

$(document).ready(function () {
	$('.exit').on('click',function(){
		mp.trigger('onCloseCarMenu');
			
    });
    $('.testButton').on('click', function () {
        car = this.children.item('data-id').getAttribute('data-id');
    });
    $('.spawncar').on('click', function () {
        mp.trigger('onSpawnCar', car);
			
	});	
});

var carID = null;

function getCarInfo(carid, name) {
	var row = $(".spisok li:last-child").clone(true);
    $(row).html("<div class = 'carbutton' data-id = '" + carid + "' oninput='carButton()' > " + name + "</div>");
	$(row).css({"display":"block"});
	$('.spisok').append(row);
	row = $(".spisok li:last-child").clone(true);
}