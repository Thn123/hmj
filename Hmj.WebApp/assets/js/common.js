/**
 * 异步提交表单
 * @param {UI Element}   form     表单元素
 * @param {Function} callback 异步提交的回调函数
 */
function FormSubmit(form, callback) {
    showLoadingMsg("正在努力地请求服务端,请稍后...");
    $.ajax({
        url: form.action,
        type: form.method,
        data: $(form).serialize(),
        success: function (result) {
            hideLoadingMsg();
            callback(result);
        },
        error: function (result) {
            hideLoadingMsg();
            //alert("提交表单失败："+result);
            _showInfoMessage("操作失败。", 'error');
        }
    });
}
var popUpWin;
function PopupWindow(URLStr, width, height, newWin, scrollbars) {
    var popUpWin = 0;
    if (typeof (newWin) == "undefined") {
        newWin = false;
    }
    if (typeof (scrollbars) == "undefined") {
        scrollbars = 0;
    }
    if (typeof (width) == "undefined") {
        width = 800;
    }
    if (typeof (height) == "undefined") {
        height = 600;
    }
    var left = 0;
    var top = 0;
    if (screen.width >= width) {
        left = Math.floor((screen.width - width) / 2);
    }
    if (screen.height >= height) {
        top = Math.floor((screen.height - height) / 2);
    }
    if (newWin) {
        open(URLStr, '', 'toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=' + scrollbars + ',resizable=yes,copyhistory=yes,width=' + width + ',height=' + height + ',left=' + left + ', top=' + top + ',screenX=' + left + ',screenY=' + top + '');
        return;
    }

    if (popUpWin) {
        if (!popUpWin.closed) popUpWin.close();
    }
    popUpWin = open(URLStr, 'popUpWin', 'toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=' + scrollbars + ',resizable=yes,copyhistory=yes,width=' + width + ',height=' + height + ',left=' + left + ', top=' + top + ',screenX=' + left + ',screenY=' + top + '');
    popUpWin.focus();
}
function StrFormat(temp, dataarry) {
    return temp.replace(/\{([\d]+)\}/g, function (s1, s2) { var s = dataarry[s2]; if (typeof (s) != "undefined") { if (s instanceof (Date)) { return s.getTimezoneOffset() } else { return encodeURIComponent(s) } } else { return "" } });
}
function StrFormatNoEncode(temp, dataarry) {
    return temp.replace(/\{([\d]+)\}/g, function (s1, s2) { var s = dataarry[s2]; if (typeof (s) != "undefined") { if (s instanceof (Date)) { return s.getTimezoneOffset() } else { return (s); } } else { return ""; } });
}
function guidGenerator() {
    var S4 = function () {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    };
    return (S4() + S4() + S4() + S4() + S4() + S4() + S4() + S4());
}
function idGenerator() {
    var S4 = function () {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    };
    return (S4() + S4() + S4() + S4());
}
/**
 * 获取元素的相对于页面的居中位置
 * @param  {jQuery Element} el 需要定位的元素
 * @return {object}    返回元素居中定位的{left,top}
 */
function getCenterPos(el) {
    return {
        left: Math.max((document.documentElement.clientWidth - el.width()) / 2 + $(document).scrollLeft(), 0) + 'px',
        top: Math.max((document.documentElement.clientHeight - el.height()) / 2 + $(document).scrollTop(), 0) + 'px'
    }
}
function StrToDate(date) {
    var reg = /^(\d{1,4})(-|\/|.)(\d{1,2})\2(\d{1,2})(\040+(\d{1,2}):(\d{1,2}):(\d{1,2}))?$/;
    var arr = date.match(reg);
    if (arr == null) {
        return NaN;
    }
    else {
        return new Date(arr[1], parseInt(arr[3], 10) - 1, arr[4], arr[6] || 0, arr[7] || 0, arr[8] || 0);
    }
}
function showLoadingMsg(msg, position, isautohide, timeout) {
    var loadpanel = $("#loadpanel");
    if (loadpanel.length == 0) {
        loadpanel = $("<div id=\"loadpanel\" class=\"loadingpanel\"/>").appendTo("body");
    }
    loadpanel.html("<img src='../../assets/img/icon/indicator_remembermilk_orange.gif' style='vertical-align:middle;margin-right:10px;' alt=''/><span>" + msg + "</span>");
    if (!position) {
        position = { right: 20, top: 3 };
    }
    loadpanel.css(position).show();
    if (isautohide) {
        showLoadTipTimerId = setTimeout(hideLoadingMsg, timeout);
    }
}
function hideLoadingMsg() {
    var loadpanel = $("#loadpanel");
    if (loadpanel.length > 0) {
        loadpanel.hide();
    }
}
var showErrorTipTimerId;
var showLoadTipTimerId;
var msg;
function showErrorTip(msg, position, isautohide, timeout) {

    var errorpanel = $("#errorpanel");
    if (errorpanel.length == 0) {
        errorpanel = $("<div id=\"errorpanel\" class=\"errorpanel\"/>").appendTo("body");
    }
    if (errorpanel.css("display") != "none") {
        errorpanel.find(">dt").append("<dl>" + msg + "</dl>");
        if (showErrorTipTimerId) {
            window.clearTimeout(showErrorTipTimerId);
        }
    }
    else {
        errorpanel.html("<dt><dl>" + msg + "</dl></dt>");
        if (!position) {
            position = { right: 20, top: 3 };
        }
        errorpanel.css(position).fadeIn();
    }
    if (isautohide) {
        showErrorTipTimerId = setTimeout(hideErrortip, timeout);
    }

}
function hideErrortip() {
    var errorpanel = $("#errorpanel");
    if (errorpanel.length > 0) {
        errorpanel.fadeOut();
    }
}
Date.prototype.Format = function (format) {
    var o = {
        "M+": this.getMonth() + 1,
        "d+": this.getDate(),
        "h+": this.getHours(),
        "H+": this.getHours(),
        "m+": this.getMinutes(),
        "s+": this.getSeconds(),
        "q+": Math.floor((this.getMonth() + 3) / 3),
        "w": "0123456".indexOf(this.getDay()),
        "S": this.getMilliseconds()
    };
    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format))
            format = format.replace(RegExp.$1,
      RegExp.$1.length == 1 ? o[k] :
        ("00" + o[k]).substr(("" + o[k]).length));
    }
    return format;
};
function DateAdd(interval, number, idate) {
    number = parseInt(number);
    var date;
    if (typeof (idate) == "string") {
        date = idate.split(/\D/);
        eval("var date = new Date(" + date.join(",") + ")");
    }

    if (typeof (idate) == "object") {
        date = new Date(idate.toString());
    }
    switch (interval) {
        case "y": date.setFullYear(date.getFullYear() + number); break;
        case "m": date.setMonth(date.getMonth() + number); break;
        case "d": date.setDate(date.getDate() + number); break;
        case "w": date.setDate(date.getDate() + 7 * number); break;
        case "h": date.setHours(date.getHours() + number); break;
        case "n": date.setMinutes(date.getMinutes() + number); break;
        case "s": date.setSeconds(date.getSeconds() + number); break;
        case "l": date.setMilliseconds(date.getMilliseconds() + number); break;
    }
    return date;
};