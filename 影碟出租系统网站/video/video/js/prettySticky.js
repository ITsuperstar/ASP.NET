/*!
 * prettySticky - v1 - 2014-10-26
 * https://github.com/moyamiller/prettySticky
 * Copyright (c) 2014 Moya Miller
 */

$(function() {
    $(window).scroll(function() {
        var scroll = $(window).scrollTop() + 90;
        var currentArea = $("section").filter(function() {
        	return scroll <= $(this).offset().top + $(this).height();
        });
        $(".nav a").removeClass("selected");
        $(".nav a[href=#" + currentArea.attr("id") + "]").addClass("selected");

        if ($(window).scrollTop() > 100) {
            $('nav').addClass("navScroll");
            $('img.logo').addClass("logoScroll");
            $('div.menu').addClass("menuScroll");
        } else if ($(window).scrollTop() < 100 ) {
            $('nav').removeClass("navScroll");
            $('img.logo').removeClass("logoScroll");
            $('div.menu').removeClass("menuScroll");
        }
    });
});

