/// <reference path="jquery-1.4.1-vsdoc.js" />

if (typeof We7 == 'undefined' || !We7) {
    var We7 = {};
}

/***********************************************
** Usage: var o={a:"a"};var c={c:"c"}; We7.apply(o,c); 
          (results: o={a:"a",c:"c"})
** Description:provider a methed to merge two object
               (also defauts)
***********************************************/
We7.apply = function(o, c, defaults) {
    if (defaults) {
        We7.apply(o, defaults);
    }

    if (o && c && typeof c === 'object') {
        for (var p in c) {
            o[p] = c[p];
        }
    }
}

/***************************************************
* add new method to We7:
*     include:extend,namespace,and some define object
*
*
*
*****************************************************/
We7.apply(We7, {
    extend: function() {

        var io = function(o) {
            for (var m in o) {
                this[m] = o[m];
            }
        };
        var oc = Object.prototype.constructor;

        return function(sb, sp, overrides) {

            if (typeof sp == 'object') {
                overrides = sp;
                sp = sb;
                sb = overrides.constructor != oc ? overrides.constructor : function() { sp.apply(this, arguments); };
            }
            var F = function() { },
                    sbp,
                    spp = sp.prototype;

            F.prototype = spp;
            sbp = sb.prototype = new F();
            sbp.constructor = sb;
            sb.superclass = spp;
            if (spp.constructor == oc) {
                spp.constructor = sp;
            }
            sb.override = function(o) {
                We7.override(sb, o);
            };
            sbp.superclass = sbp.supr = (function() {
                return spp;
            });
            sbp.override = io;
            We7.override(sb, overrides);
            sb.extend = function(o) { return We7.extend(sb, o); };
            return sb;
        };
    } (),
    override: function(origclass, overrides) {
        if (overrides) {
            var p = origclass.prototype;
            We7.apply(p, overrides);
            if ($.isIE && overrides.hasOwnProperty('toString')) {
                p.toString = overrides.toString;
            }
        }
    },
    namespace: function() {
        var a = arguments, o = null, i, j, d;
        for (i = 0; i < a.length; i = i + 1) {
            d = ("" + a[i]).split(".");
            o = We7;


            for (j = (d[0] == "We7") ? 1 : 0; j < d.length; j = j + 1) {
                o[d[j]] = o[d[j]] || {};
                o = o[d[j]];
            }
        }

        return o;
    },
    _generateIdCounter: 1,
    generateId: function() {

        return 'genId-' + this._generateIdCounter++;
    },
    isEmpty: function(v, allowBlank) {
        return v === null || v === undefined || ((We7.isArray(v) && !v.length)) || (!allowBlank ? v === '' : false);
    },


    isArray: function(v) {
        return Object.prototype.toString.apply(v) === '[object Array]';
    },


    isDate: function(v) {

        return toString.apply(v) === '[object Date]';
    },


    isObject: function(v) {
        return !!v && Object.prototype.toString.call(v) === '[object Object]';
    },


    isPrimitive: function(v) {
        return We7.isString(v) || We7.isNumber(v) || We7.isBoolean(v);
    },


    isFunction: function(v) {
        return !!v && !v.nodeName && v.constructor != String &&
        v.constructor != RegExp && v.constructor != Array &&
        /function/i.test(v + "");
    },


    isNumber: function(v) {
        return typeof v === 'number' && isFinite(v);
    },


    isString: function(v) {
        return typeof v === 'string';
    },


    isBoolean: function(v) {
        return typeof v === 'boolean';
    },


    isElement: function(v) {
        return v ? !!v.tagName : false;
    },


    isDefined: function(v) {
        return typeof v !== 'undefined';
    },
    isUndefined: function(v) {
        return typeof v === 'undefined';
    },
    addEvent: function(element, type, handler, capture) {

        capture = capture || false;
        if (element.addEventListener) {
            element.addEventListener(type, handler, capture);
            return true;
        }
        else if (element.attachEvent) {
            var r = element.attachEvent('on' + type, handler);
            return r;
        }
        else {
            element['on' + type] = handler;
        }
    },
    removeEvent: function(element, type, handler) {

        if (element.removeEventListener) {
            element.removeEventListener(type, handler, false);
            return true;
        }
        else if (element.detachEvent) {
            var r = element.detachEvent('on' + type, handler);
            return r;
        }
        else {
            element['on' + type] = handler;
        }
    }
});

We7.namespace("Controls");