 
	$(document).ready(function(){
		var $default2 = $("#default2");
		$(window).scroll(function(){
		if ( $(this).scrollTop() > 100 && $default2.hasClass("navbar-light bg-light default2") ){
			$default2.removeClass("navbar-light bg-light default2").addClass("navbar-dark bg-dark fixed-top");
		} else if($(this).scrollTop() <= 100 && $default2.hasClass("navbar-dark bg-dark fixed-top")) {
			$default2.removeClass("navbar-dark bg-dark fixed-top").addClass("navbar-light bg-light default2");
		}
		});//scroll
	});
	
	$(document).ready(function(){
		var $default3 = $("#nav-size");  
		$(window).scroll(function(){
		if ( $(this).scrollTop() > 100 && $default3.hasClass("container-fuil") ){
			$default3.removeClass("container-fuil").addClass("container");
		} else if($(this).scrollTop() <= 100 && $default3.hasClass("container")) {
			$default3.removeClass("container").addClass("container-fuil");
		}
		});//scroll
	});
	/* 
		<----- NavBar Scroll -----
	*/
	function checkScroll() {
		var startY = $('.navbar').height() * 0;
	//The Point Where The NavBar Changed in px
		if($(window).scrollTop() > startY) {
			$('.navbar').addClass("scrolled");	
		} else {
			$('.navbar').removeClass("scrolled");
		}
	}
	if($('.navbar').length > 0) {
		$(window).on("scroll load resize" , 
		function () {
			checkScroll();
		});
	}
	/*
	 <----- End NavBar Scroll -----
	  */
	/* Scroll Top Page */
	$('#scrollTop').on('click', function () {
		$('html, body').animate({ scrollTop: 0}, 2000);
	});
	$( document ).ready(function() {
		$('#scrollTop').removeClass('active');
	});

	$(window).on('scroll', function () {
		if ($(window).scrollTop() >= 1000) {
			$('#scrollTop').addClass('active');
		} else {
			$('#scrollTop').removeClass('active');
		}
	});
	/* End Scroll Top Page */
	/** shopping market */
	$(document).ready(function() {
		var cartCountValue = 0;
		var cartCount = $('.btnmarket .count');
		$(cartCount).text(cartCountValue)
	
		$('.card-btn').on('click', function() {
			var cartBtn = this;
			var cartCountPosition = $(cartCount).offset();
			var btnPosition = $(this).offset();
			var leftPos = 
			cartCountPosition.left < btnPosition.left
				? btnPosition.left - (btnPosition.left - cartCountPosition.left)
				: cartCount.left;
			var topPos = 
			cartCountPosition.top < btnPosition.top
				? cartCountPosition.top
				: cartCountPosition.top;
			$(cartBtn)
				.append("<span class='count'></sapn>");
				
				$(cartBtn).find(".count").each(function(i,count){
					$(count).offset({
					  left: leftPos,
					  top: topPos
					})
					.animate(
					  {
						opacity: 0
					  },
					  200,
					  function() {
						$(this).remove();
						cartCountValue++;
						$(cartCount).text(cartCountValue);
					  }
					);
				  }); 
				});
			  
				function getRndInteger(min, max) {
				  return Math.floor(Math.random() * (max - min + 1)) + min;
				}
		
	});
	

