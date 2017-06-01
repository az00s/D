
$(document).ready(function () {

var popupStatus = 0;

function loadPopup(){
	if(popupStatus==0){
		$("#backgroundPopup").css({
			"opacity": "0.7"
		});
		$("#backgroundPopup").fadeIn("slow");
		$("#popupContact").fadeIn("slow");
		popupStatus = 1;
	}
}

function disablePopup(){
	if(popupStatus==1){
		$("#backgroundPopup").fadeOut("slow");
		$("#popupContact").fadeOut("slow");
		$(".sideButton").fadeOut("slow");
		popupStatus = 0;
	}
}

function centerPopup(){
	var windowWidth = document.documentElement.clientWidth;
	var windowHeight = document.documentElement.clientHeight;
	var popupHeight = $("#popupContact").height();
	var popupWidth = $("#popupContact").width();
	$("#popupContact").css({
	    "position": "absolute",
	    "top": "50px",
	    "left": windowWidth / 5,
	    
	});

	$(".sideButton").css({
	    
	    "position": "absolute",
	    "left": windowWidth / 5,
	});
	
	$("#backgroundPopup").css({
		"height": windowHeight
	});
	
}



	
	$("#button").click(function(){
		centerPopup();
		loadPopup();

		
		
	});
				
	$("#popupContactClose").click(function(){
		disablePopup();
	});
	$("#backgroundPopup").click(function(){
		disablePopup();
	});

	$('#tbGoods tr').click(function () {


	    $('#tbGoods tr').removeClass('tr_red');

	    $(this).toggleClass('tr_red');



	    if ($('#tbGoods tr .tr_red')) {
	        $(".sideButton").fadeIn("slow");
	    }

	    if (!($("tr").is(".tr_red"))) {
	        $(".sideButton").fadeOut("slow");
	    }

	})

	$('.sideButton').click(function () {
	    var a = $('.tr_red input[type=hidden]').attr("value");
	    $("#indoorText").val(a);
	    $('#tbGoods tr').removeClass('tr_red');
	    $(".sideButton").fadeOut("slow");
	    disablePopup();

	});


	$(document).keypress(function(e){
		if(e.keyCode==27 && popupStatus==1){
			disablePopup();
		}
	});

})