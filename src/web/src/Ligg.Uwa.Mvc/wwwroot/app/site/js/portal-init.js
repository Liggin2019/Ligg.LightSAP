
$(document).ready(function () {
    $('.menu-item-b').on('click', function menuItem() {
        var dataUrl = $(this).data('url');
        if (dataUrl == undefined || $.trim(dataUrl).length == 0) {
            return;
        }
        if (!dataUrl.toLowerCase().startsWith('/custpage/show')) setHeaderBackground(false);
        else {
            var dataIsTranHeader = $(this).data('tag'); 
            if (dataIsTranHeader=='True') setHeaderBackground(true);
            else setHeaderBackground(false);
        }
        renderElement(dataUrl,'body-main')
    });
}),

    !function () {
        'use strict';
        function i(i) {
            var o = $(this),
                t = o.parent(),
                e = t.children('ul.nav-list');

            i.stopPropagation && i.stopPropagation(),
                r && r.stop(),
                o.hasClass('show-list') ? (o.removeClass('show-list'), t.removeClass('bgc-blue'), e.css({
                    display: 'block',
                    opacity: 1
                }), r = e.animate({
                    opacity: '0'
                }, 500, function () {
                    e.css({
                        display: 'none'
                    })
                })) : (o.addClass('show-list'), t.addClass('bgc-blue'), e.css({
                    display: 'block',
                    opacity: 0
                }), r = e.animate({
                    opacity: '1'
                }, 500))
        }
        function m() {

        }
        function o(i) {
            var o = $(this),
                t = o.children('.child-list-container');
            parseInt(t.attr('show-list')) ? (o.removeClass('show-list'), n(this)) : (o.addClass('show-list'), c(this))
        }
        function t(i) {
            i.stopPropagation(),
                c(this)
        }
        function e(i) {
            i.stopPropagation(),
                n(this)
        }
        function n(i) {
            var o = $(i).children('.child-list-container'),
                t = a(i);
            parseInt(o.attr('show-list')) && (o.attr('show-list', 0), t && t.stop(), t = o.animate({
                opacity: 0
            }, 200, function () {
                parseInt(o.attr('show-list')) || o.css('display', 'none')
            }), l(i, t))
        }
        function c(i) {
            var o = $(i).children('.child-list-container'),
                t = a(i);
            parseInt(o.attr('show-list')) || (o.attr('show-list', 1), t && t.stop(), o.css('display', 'block'), t = o.animate({
                opacity: 1
            }, 200), l(i, t))
        }
        function a(i) {
            return window['animeObj' + i.getAttribute('list-index')]
        }
        function l(i, o) {
            window['animeObj' + i.getAttribute('list-index')] = o
        }
        for (var d = function (i) { return document.querySelector(i) }
            ('div.toogle-button-holder'), s = function (i) {
                return document.querySelectorAll(i)
            }
                ('.nav-list li.has-child'), r = null, A = 0; A < s.length; A++)
            (function () { return document.getElementsByTagName('body')[0].clientWidth <= 768 })() ? (!function (i, o, t) { i.addEventHandler ? i.addEventListener(o, t, !1) : i.attachEvent ? i.attachEvent('on' + o, t) : i['on' + o] = t }(d, 'click', i), $(s[A]).on('click', o)) : ($('.nav-list>li.has-child').on('mouseover', t), $('.nav-list>li.has-child').on('mouseleave', e)
            );



        location.href.indexOf('searchengines') >= 0 && $('.s-online-experience').show()
    }()