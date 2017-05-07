 $(document).ready(function () {




        $('#tbGoods tr').click(function () {

            $(this).toggleClass('tr_red');

            $('.tr_red #ID_______').attr('name', 'ID');
            $('#tbGoods tr:not(.tr_red) #ID_______').attr('name', 'ID_товара');


            if ($('#tbGoods tr .tr_red')) {
                $("#sidebtnG").fadeIn("slow");
                //$("#AdGoods_table").css("display","block");
            }

            if (!($("tr").is(".tr_red"))) {
                $("#sidebtnG").fadeOut("slow");
                //$("#AdGoods_table").css("display", "none");;
            }

        });

        //$('#AdGoods_table td a').click(function () {
        //    var a = $("#AdGoods_table tr").length;

        //    if (a == 2) {


        //        $(".test-border").css("display", "none");

        //    }
        //    else { $("#AdGoods_table").fadeIn("slow"); }


        //});

        $(document).on('click', '#AddGoods a', function () {
            //alert('нажатие!');
            $(this).closest('tr').remove();
            var a = $("#AddGoods tbody tr").length;

            if (a == 0) {
                $(".test-border").css("display", "none");
                $('#btnDone').fadeOut("slow");
            }
            else { $("#AdGoods_table").fadeIn("slow"); }
        });



        $('#sidebtnG').click(function () {

            disablePopup('popupContact', 'sidebtnG');
            var tr = $('#tbGoods tr.tr_red');
            
            $('#AdGoods_div').fadeIn('fast');
            //$('#btnDone').removeAttr("disabled");
            $('#btnDone').fadeIn('fast');
            
            $('#AddGoods > tbody:last-child').append(tr);
            $('#AddGoods tr.tr_red').removeClass('tr_red').addClass('added');
            $('#AddGoods tbody tr.added').append('<td><input type="number" min="1" max="2147483647" name="Количество" style="width:70px;" value="1" required /></td><td><a class="glyphicon glyphicon-remove" style="text-decoration:none;color:#E81C2C"></a></td>');
                $('#AddGoods tbody tr.added').removeClass('added');
                $('#AddGoods tr').unbind('click');
           
        });

        

        //$('.delbtn').click(function () {

        //    var a = $("#datepicker").attr("value");
        //    $("#hDateD").attr("value", a);

        //    var b = $("#ID________").val();
        //    $("#hClientD").attr("value", b);

        //    var c = $("#employee").val();
        //    $("#hEmployeeD").attr("value", c);
        //});
		
		var popupStatus = 0;



function disablePopup(id,btn){
	if(popupStatus==1){
		$("#backgroundPopup").fadeOut("slow");
		$('#' + id + '').fadeOut("fast");
		$('#'+btn+'').fadeOut("fast");
		popupStatus = 0;
	}
}

function centerPopup(id,btn){
	var windowWidth = document.documentElement.clientWidth;
	var windowHeight = document.documentElement.clientHeight;
	var popupHeight = $('#' + id + '').height();
	var popupWidth = $('#' + id + '').width();
	$('#'+id+'').css({
	    "position": "absolute",
	    "top": "30px",
	    "left": windowWidth / 3 - popupWidth / 2,
	  
	    
	});


	$('#'+btn+'').css({
	    
	    "position": "absolute",
	    //"left": (windowWidth / 2 - popupWidth / 2) - 50,
	    "left": "-50px",
	});
	
	$("#backgroundPopup").css({
		"height": windowHeight
	});
	
}

function loadPopup(id) {
    if (popupStatus == 0) {
        $("#backgroundPopup").css({
            "opacity": "0.7"
        });
        $("#backgroundPopup").fadeIn("slow");
        $('#'+id+'').fadeIn("slow");
        popupStatus = 1;
    }
}



	
	$("#button").click(function(){
	    centerPopup('popupContact', 'sidebtnG');
		loadPopup('popupContact');

		
		
	});

	
	
				
	$("#popupContactClose").click(function(){
	    disablePopup('popupContact', 'sidebtnG');
	});
	//$(".backgroundPopup").click(function(){
	//    disablePopup('popupContact', 'sidebtnG');
	//});

	

	


	$(document).keypress(function(e){
		if(e.keyCode==27 && popupStatus==1){
			disablePopup();
		}
	});

     //клик по боковой кнопке списка клиентов
	$('#sidebtnC').click(function () {

	    var a = $('.tr_red input[type=hidden]').attr("value");
	    $("#ID_клиента").val(a);
	    var b = $('.tr_red td:last').text();
	    $("#indoorText").val(b);
	    $('#tbClients tr').removeClass('tr_red');
	    $("#sidebtnC").fadeOut("slow");
	    disablePopup('popupClients', 'sidebtnC');

	});

     //логика выбора клиента из списка
	$('#tbClients tr').click(function () {


	    $('#tbClients tr').removeClass('tr_red');

	    $(this).toggleClass('tr_red');



	    if ($('#tbClients tr .tr_red')) {
	        $("#sidebtnC").fadeIn("slow");
	    }

	    if (!($("tr").is(".tr_red"))) {
	        $("#sidebtnC").fadeOut("slow");
	    }

	})

	$("#btnAddClient").click(function () {
	   
	    centerPopup('popupClients', 'sidebtnC');
	    loadPopup('popupClients');



	});

	$("#popupClientsClose").click(function () {
	    disablePopup('popupClients', 'sidebtnC');
	});
	

})