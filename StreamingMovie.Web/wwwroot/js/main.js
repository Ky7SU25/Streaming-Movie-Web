/*  ---------------------------------------------------
    Theme Name: Anime
    Description: Anime video tamplate
    Author: Colorib
    Author URI: https://colorib.com/
    Version: 1.0
    Created: Colorib
---------------------------------------------------------  */

'use strict';

(function ($) {

    /*------------------
        Preloader
    --------------------*/
    $(window).on('load', function () {
        $(".loader").fadeOut();
        $("#preloder").delay(200).fadeOut("slow");

        /*------------------
            FIlter
        --------------------*/
        $('.filter__controls li').on('click', function () {
            $('.filter__controls li').removeClass('active');
            $(this).addClass('active');
        });
        if ($('.filter__gallery').length > 0) {
            var containerEl = document.querySelector('.filter__gallery');
            var mixer = mixitup(containerEl);
        }
    });

    /*------------------
        Background Set
    --------------------*/
    $('.set-bg').each(function () {
        var bg = $(this).data('setbg');
        $(this).css('background-image', 'url(' + bg + ')');
    });

    // Search model
    $('.search-switch').on('click', function () {
        $('.search-model').fadeIn(400);
    });

    $('.search-close-switch').on('click', function () {
        $('.search-model').fadeOut(400, function () {
            $('#search-input').val('');
        });
    });

    /*------------------
        Navigation
    --------------------*/
    $(".mobile-menu").slicknav({
        prependTo: '#mobile-menu-wrap',
        allowParentLinks: true
    });

    /*------------------
        Hero Slider
    --------------------*/
    var hero_s = $(".hero__slider");
    hero_s.owlCarousel({
        loop: true,
        margin: 0,
        items: 1,
        dots: true,
        nav: true,
        navText: ["<span class='arrow_carrot-left'></span>", "<span class='arrow_carrot-right'></span>"],
        animateOut: 'fadeOut',
        animateIn: 'fadeIn',
        smartSpeed: 1200,
        autoHeight: false,
        autoplay: true,
        mouseDrag: false
    });

    /*------------------
        Video Player
    --------------------*/
    const player = new Plyr('#player', {
        controls: ['play-large', 'play', 'progress', 'current-time', 'mute', 'captions', 'settings', 'fullscreen'],
        seekTime: 25
    });

    /*------------------
        Niceselect
    --------------------*/
    $('select').niceSelect();

    /*------------------
        Scroll To Top
    --------------------*/
    $("#scrollToTopButton").click(function () {
        $("html, body").animate({ scrollTop: 0 }, "slow");
        return false;
    });

})(jQuery);

// Sticky Header
window.addEventListener('scroll', function () {
    const header = document.querySelector('.header');
    if (!header) return;

    if (window.scrollY > 0) {
        header.classList.add('scrolled');
    } else {
        header.classList.remove('scrolled');
    }
});

// Filter Toggle
window.toggleComponent = function (id) {
    var el = document.getElementById(id);
    if (!el) return;

    var isHidden = window.getComputedStyle(el).display === "none";
    el.style.display = isHidden ? "block" : "none";
};

// Ajax no url
function setupAjaxPagination(triggerClass, renderTargetId) {
    let isLoading = false;

    $(document).on('click', triggerClass, function (e) {
        e.preventDefault();

        if (isLoading) return;

        const url = $(this).attr('href');
        isLoading = true;

        $.ajax({
            url: url,
            method: 'GET',
            headers: { 'X-Requested-With': 'XMLHttpRequest' },
            success: function (response) {
                $(renderTargetId).html(response);
            },
            complete: function () {
                isLoading = false;
            },
            error: function () {
                alert("Failed to load content.");
            }
        });
    });
}
