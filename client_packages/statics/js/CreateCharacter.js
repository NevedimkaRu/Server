$(document).ready(function() {
		/*$('.ranger').on('change',function(){
				let dataObject = {};
				let dataKey = event.currentTarget.id;
				let dataValue = event.currentTarget.value;
				
				dataObject[dataKey] = parseInt(dataValue);

				mp.trigger('SetSkin', JSON.stringify(dataObject));
		});*/	
		
		$('#exit').on('click',function(){
			mp.trigger('closeSkinMenu');
			
		})
});

function updateFaceFeature(){
				let dataObject = {};
				let dataKey = event.currentTarget.id;
				let dataValue = event.currentTarget.value;
				
				dataObject[dataKey] = parseInt(dataValue);
		//console.log(dataObject);
				mp.trigger('SetSkin', JSON.stringify(dataObject));
		}
		
		function updateFaceFeature1(){
				let dataObject = {};
				let dataKey = event.currentTarget.id;
				let dataValue = event.currentTarget.value;
				
				dataObject[dataKey] = parseFloat(dataValue);
		//console.log(dataObject);
				mp.trigger('SetSkin', JSON.stringify(dataObject));
		}