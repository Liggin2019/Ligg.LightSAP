
function lazyloadInit() {
    function i(i) {
        var o = $(i);
        return o.hasClass(t) ? !1 : void o.animate({
            opacity: 1,
            top: 0
        }, 500)
    }

    function o(i) {
        $('.lazy').each(function (o, t) {
            var e = $(t);
            e.lazyload(i)
        })
    }

    var t = 'no-slide-top-animate',
        e = 'data:image/gif;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVQImWNgYGBgAAAABQABh6FO1AAAAABJRU5ErkJggg==',
        n = {
            vertical_only: !0,
            no_fake_img_loader: !0,
            placeholder_data_img: e,
            appear: function (o) {
                i(o)
            }
        };

    o($(window).width() < 500 ? n : n),
        $('.bg-lazy').lazyload({
            vertical_only: !0,
            threshold: 1000,
            no_fake_img_loader: !0
        })
}




$(document).ready(function () {
    setTimeout(lazyloadInit, 100);
});


!function (e) {
    'function' == typeof define && define.amd ? define(['jquery'], e) : e(window.jQuery || window.Zepto)
}
    (function (e, t) {
        function n() {
        }
        function i(e, t) {
            var n;
            return n = t._$container == p ? ('innerHeight' in d ? d.innerHeight : p.height()) + p.scrollTop() : t._$container.offset().top + t._$container.height(),
                n <= e.offset().top - t.threshold
        }
        function r(t, n) {
            var i;
            return i = n._$container == p ? p.width() + (e.fn.scrollLeft ? p.scrollLeft() : d.pageXOffset) : n._$container.offset().left + n._$container.width(),
                i <= t.offset().left - n.threshold
        }
        function o(e, t) {
            var n;
            return n = t._$container == p ? p.scrollTop() : t._$container.offset().top,
                n >= e.offset().top + t.threshold + e.height()
        }
        function s(t, n) {
            var i;
            return i = n._$container == p ? e.fn.scrollLeft ? p.scrollLeft() : d.pageXOffset : n._$container.offset().left,
                i >= t.offset().left + n.threshold + t.width()
        }
        function a(e, t) {
            var n = 0;
            e.each(function (a, l) {
                function u() {
                    c.trigger('_lazyload_appear'),
                        n = 0
                }
                var c = e.eq(a);
                if (!(c.width() <= 0 && c.height() <= 0 || 'none' === c.css('display'))) if (t.vertical_only) if (o(c, t));
                else if (i(c, t)) {
                    if (++n > t.failure_limit) return !1
                } else u();
                else if (o(c, t) || s(c, t));
                else if (i(c, t) || r(c, t)) {
                    if (++n > t.failure_limit) return !1
                } else u()
            })
        }
        function l(e) {
            return e.filter(function (t, n) {
                return !e.eq(t)._lazyload_loadStarted
            })
        }  // throttle : https://github.com/component/throttle , MIT License

        function u(e, t) {
            function n() {
                s = 0,
                    a = + new Date,
                    o = e.apply(i, r),
                    i = null,
                    r = null
            }
            var i,
                r,
                o,
                s,
                a = 0;
            return function () {
                i = this,
                    r = arguments;
                var e = new Date - a;
                return s || (e >= t ? n() : s = setTimeout(n, t - e)),
                    o
            }
        }
        var c,
            d = window,
            p = e(d),
            f = {
                threshold: 0,
                failure_limit: 0,
                event: 'scroll',
                effect: 'show',
                effect_params: null,
                container: d,
                data_attribute: 'original',
                data_srcset_attribute: 'original-srcset',
                skip_invisible: !0,
                appear: n,
                load: n,
                vertical_only: !1,
                check_appear_throttle_time: 300,
                url_rewriter_fn: n,
                no_fake_img_loader: !1,
                placeholder_data_img: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAANSURBVBhXYzh8+PB/AAffA0nNPuCLAAAAAElFTkSuQmCC',
                placeholder_real_img: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAANSURBVBhXYzh8+PB/AAffA0nNPuCLAAAAAElFTkSuQmCCAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=='//'http://ditu.baidu.cn/yyfm/lazyload/0.0.1/img/placeholder.png'
            };
        c = function () {
            var e = Object.prototype.toString;
            return function (t) {
                return e.call(t).replace('[object ', '').replace(']', '')
            }
        }(),
            e.fn.hasOwnProperty('lazyload') || (e.fn.lazyload = function (t) {
                var i,
                    r,
                    o,
                    s = this;
                return e.isPlainObject(t) || (t = {
                }),
                    e.each(f, function (n, i) {
                        var r = c(t[n]);
                        - 1 != e.inArray(n, [
                            'threshold',
                            'failure_limit',
                            'check_appear_throttle_time'
                        ]) ? 'String' == r ? t[n] = parseInt(t[n], 10) : 'Number' != r && (t[n] = i) : 'container' == n ? (t.hasOwnProperty(n) ? t[n] == d || t[n] == document ? t._$container = p : t._$container = e(t[n]) : t._$container = p, delete t.container) : !f.hasOwnProperty(n) || t.hasOwnProperty(n) && r == c(f[n]) || (t[n] = i)
                    }),
                    i = 'scroll' == t.event,
                    o = 0 == t.check_appear_throttle_time ? a : u(a, t.check_appear_throttle_time),
                    r = i || 'scrollstart' == t.event || 'scrollstop' == t.event,
                    s.each(function (i, o) {
                        var a = this,
                            u = s.eq(i),
                            c = u.attr('src'),
                            d = u.attr('data-' + t.data_attribute),
                            p = t.url_rewriter_fn == n ? d : t.url_rewriter_fn.call(a, u, d),
                            f = u.attr('data-' + t.data_srcset_attribute),
                            h = u.is('img');
                        return 1 == u._lazyload_loadStarted || c == p ? (u._lazyload_loadStarted = !0, void (s = l(s))) : (u._lazyload_loadStarted = !1, h && !c && u.one('error', function () {
                            u.attr('src', t.placeholder_real_img)
                        }).attr('src', t.placeholder_data_img), u.one('_lazyload_appear', function () {
                            function i() {
                                r && u.hide(),
                                    h ? (f && u.attr('srcset', f), p && u.attr('src', p)) : u.css('background-image', 'url("' + p + '")'),
                                    r && u[t.effect].apply(u, o ? t.effect_params : [
                                    ]),
                                    s = l(s)
                            }
                            var r,
                                o = e.isArray(t.effect_params);
                            u._lazyload_loadStarted || (r = 'show' != t.effect && e.fn[t.effect] && (!t.effect_params || o && 0 == t.effect_params.length), t.appear != n && t.appear.call(a, u, s.length, t), u._lazyload_loadStarted = !0, t.no_fake_img_loader || f ? (t.load != n && u.one('load', function () {
                                t.load.call(a, u, s.length, t)
                            }), i()) : e('<img />').one('load', function () {
                                i(),
                                    t.load != n && t.load.call(a, u, s.length, t)
                            }).attr('src', p))
                        }), void (r || u.on(t.event, function () {
                            u._lazyload_loadStarted || u.trigger('_lazyload_appear')
                        })))
                    }),
                    r && t._$container.on(t.event, function () {
                        o(s, t)
                    }),
                    p.on('resize load', function () {
                        o(s, t)
                    }),
                    e(function () {
                        o(s, t)
                    }),
                    this
            })
    });



