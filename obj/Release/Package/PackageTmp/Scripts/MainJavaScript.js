﻿
/*
** Unobtrusive Ajax support library for jQuery
** Copyright (C) Microsoft Corporation. All rights reserved.
*/
(function (a) { var b = "unobtrusiveAjaxClick", g = "unobtrusiveValidation"; function c(d, b) { var a = window, c = (d || "").split("."); while (a && c.length) a = a[c.shift()]; if (typeof a === "function") return a; b.push(d); return Function.constructor.apply(null, b) } function d(a) { return a === "GET" || a === "POST" } function f(b, a) { !d(a) && b.setRequestHeader("X-HTTP-Method-Override", a) } function h(c, b, e) { var d; if (e.indexOf("application/x-javascript") !== -1) return; d = (c.getAttribute("data-ajax-mode") || "").toUpperCase(); a(c.getAttribute("data-ajax-update")).each(function (f, c) { var e; switch (d) { case "BEFORE": e = c.firstChild; a("<div />").html(b).contents().each(function () { c.insertBefore(this, e) }); break; case "AFTER": a("<div />").html(b).contents().each(function () { c.appendChild(this) }); break; default: a(c).html(b) } }) } function e(b, e) { var j, k, g, i; j = b.getAttribute("data-ajax-confirm"); if (j && !window.confirm(j)) return; k = a(b.getAttribute("data-ajax-loading")); i = b.getAttribute("data-ajax-loading-duration") || 0; a.extend(e, { type: b.getAttribute("data-ajax-method") || undefined, url: b.getAttribute("data-ajax-url") || undefined, beforeSend: function (d) { var a; f(d, g); a = c(b.getAttribute("data-ajax-begin"), ["xhr"]).apply(this, arguments); a !== false && k.show(i); return a }, complete: function () { k.hide(i); c(b.getAttribute("data-ajax-complete"), ["xhr", "status"]).apply(this, arguments) }, success: function (a, e, d) { h(b, a, d.getResponseHeader("Content-Type") || "text/html"); c(b.getAttribute("data-ajax-success"), ["data", "status", "xhr"]).apply(this, arguments) }, error: c(b.getAttribute("data-ajax-failure"), ["xhr", "status", "error"]) }); e.data.push({ name: "X-Requested-With", value: "XMLHttpRequest" }); g = e.type.toUpperCase(); if (!d(g)) { e.type = "POST"; e.data.push({ name: "X-HTTP-Method-Override", value: g }) } a.ajax(e) } function i(c) { var b = a(c).data(g); return !b || !b.validate || b.validate() } a(document).on("click", "a[data-ajax=true]", function (a) { a.preventDefault(); e(this, { url: this.href, type: "GET", data: [] }) }); a(document).on("click", "form[data-ajax=true] input[type=image]", function (c) { var g = c.target.name, d = a(c.target), f = d.parents("form")[0], e = d.offset(); a(f).data(b, [{ name: g + ".x", value: Math.round(c.pageX - e.left) }, { name: g + ".y", value: Math.round(c.pageY - e.top) }]); setTimeout(function () { a(f).removeData(b) }, 0) }); a(document).on("click", "form[data-ajax=true] :submit", function (c) { var e = c.currentTarget.name, d = a(c.target).parents("form")[0]; a(d).data(b, e ? [{ name: e, value: c.currentTarget.value }] : []); setTimeout(function () { a(d).removeData(b) }, 0) }); a(document).on("submit", "form[data-ajax=true]", function (d) { var c = a(this).data(b) || []; d.preventDefault(); if (!i(this)) return; e(this, { url: this.action, type: this.method || "GET", data: c.concat(a(this).serializeArray()) }) }) })(jQuery);
//--------------------------------------------------------------------------------------------------------------------------------------------------------
//MyScripts
//--------------------------------------------------------------------------------------------------------------------------------------------------------
$(document).ready(function () {
    
        // при нажатии на кнопку scrollup
    $('.scrollup').click(function () {
        // переместиться в верхнюю часть страницы
        $("html, body").animate({
            scrollTop: 0
        }, 500);
    });
    
    // при прокрутке окна (window)
    $(window).scroll(function () {
        // если пользователь прокрутил страницу более чем на 200px
        if ($(this).scrollTop() > 200) {
            // то сделать кнопку scrollup видимой
            $('.scrollup').fadeIn();
        }
            // иначе скрыть кнопку scrollup
        else {
            $('.scrollup').fadeOut();
        }
    });

    $('[data-toggle="popover"]').popover();
        // инициализировать все элементы на страницы, имеющих атрибут data-toggle="tooltip", как компоненты tooltip
    $('[data-toggle="tooltip"]').tooltip();

    $("[data-readonly]").on('keydown paste', function (e) {
        e.preventDefault();
    });
 
});


$(document).on('click', '.modal-dialog .modalTable tbody tr', (function () {

    if ($(this).is('.chosen')) {
        $(this).removeClass('chosen');
        $(".modal-footer .btn-primary").attr('disabled', 'disabled');
    }
    else {
        $('.modal-dialog .table-striped tbody tr').removeClass('chosen');

        $(this).toggleClass('chosen');


            $(".modal-footer .btn-primary").removeAttr("disabled");
    }
}));




$(document).on('click', '#btChoiceProv', (function () {

    var id = $('.chosen input[type=hidden]').attr("value");

    var name = $("#tbProviders .chosen strong")[1].innerText;

    $("#indoorText").val(name);
    $("#hidNameProv").val(id);
    $('#tbProviders tr').removeClass('chosen');
    $("#btChoiceProv").attr("disabled", "disabled");


}));
//-----------------------------------------------------------------------------------------------
$(document).on('click', '#btChoiceOrder', (function () {
    var a = $('.chosen').children()[0].innerText;
    $("#indoorTextOrder").val(a);
    $('#tbOrderChoice tr').removeClass('chosen');
    $("#btChoiceOrder").attr("disabled", "disabled");

    

}));



//--------------------------------------------------------------------------------------------------

$(document).on('hidden.bs.modal', function (event) {
    if ($('.modal:visible').length) {
        $('body').addClass('modal-open');
    }
});

$(document).ready(function () {
    
                                                
    

        $('form input').change(function () {
            var formValid = true;
            var formGroup = $(this).parents('.form-group');
            var glyphicon = formGroup.find('.form-control-feedback');
            if (this.checkValidity()) {
                formGroup.addClass('has-success').removeClass('has-error');
                glyphicon.addClass('glyphicon-ok').removeClass('glyphicon-remove');
            } else {
                formGroup.addClass('has-error').removeClass('has-success');
                glyphicon.addClass('glyphicon-remove').removeClass('glyphicon-ok');
                formValid = false;
            }
        });

        $(document).on('click', '.modal-content button[data-dismiss="modal"]', (function () {


            $('.modal-body .table-striped tr').removeClass('chosen');
            $(".modal-footer .btn-primary").attr("disabled", "disabled");
        }));

    
});








