$(document).ready(function() {
		$('#loginbtn').on('click',function(){
			mp.trigger('requestPlayerLogin', $('.login').val(),$('.pass').val());
			/*console.log($('.pass').val());
			console.log($('.login').val());*/
			
		});
		
		$('#gotoregbtn').on('click',function(){
			$('.formlogin').slideUp();
			$('.formreg').slideDown();
		});
		
		$('#gotologinbtn').on('click',function(){
			$('.formlogin').slideDown();
			$('.formreg').slideUp();
		});
		
		$('#regbtn').on('click',function(){
			mp.trigger('requestPlayerRegister', $('.loginreg').val(),$('.passreg').val());
			/*console.log($('.pass').val());
			console.log($('.login').val());*/
			
		});
});

