
//tozhuce.cshtml也写了这个方法
//设置cookie
function setCookie(c_name, value, expiredays) {
    var exdate = new Date()
    exdate.setDate(exdate.getDate() + expiredays)
    document.cookie = c_name + "=" + escape(value) +
        ((expiredays == null) ? "" : ";expires=" + exdate.toGMTString())
}

//取回cookie
function getCookie(c_name) {
    if (document.cookie.length > 0) {
        c_start = document.cookie.indexOf(c_name + "=")
        if (c_start != -1) {
            c_start = c_start + c_name.length + 1
            c_end = document.cookie.indexOf(";", c_start)
            if (c_end == -1) c_end = document.cookie.length
            return unescape(document.cookie.substring(c_start, c_end))
        }
    }
    return ""
}

/*性别选择*/
var showGenderDom = document.querySelector('#genderId');
var xingbiedata = [{ 'id': '0', 'value': '女' }, { 'id': '1', 'value': '男' }];
if (showGenderDom !== null)
    showGenderDom.addEventListener('click', function () {
        var genderId = showGenderDom.dataset['id'];
        var genderValue = showGenderDom.dataset['value'];
        //参数1：级联等级，支持1,2,3,4,5 必选项
        new IosSelect(1, [xingbiedata],
            {
                title: '性别选择',
                itemHeight: 50,
                itemShowCount: 3,
                oneLevelId: genderId,
                callback: function (selectOneObj) {
                    showGenderDom.dataset['id'] = selectOneObj.id;
                    showGenderDom.dataset['value'] = selectOneObj.value;
                    $('#genderId').val(selectOneObj.value);
                    //注册页面使用
                    if (typeof (fill_gender) !== "undefined") {
                        fill_gender = true;
                        if (fill_lastName && fill_firstName && fill_birthday && fill_gender) {
                            $("#btnSubmit").addClass("active");
                        }
                    }
                }
            });
    });


// 日历选择 
var selectDateDom = $('.selectDate');
var selectDateDomS = $(".selectDaBir");
var selectDateDomErBir = $(".selectErBir");
var showDateDom = $('.showDate');
var showDateDomS = $('.showDaBir');
var showDateDomErBir = $('.showErBir');
// 初始化时间
var now = new Date();
var nowYear = now.getFullYear();
var nowMonth = now.getMonth() + 1;
var nowDay = now.getDay();
var nowDate = now.getDate();
// 数据初始化
function formatYear(nowYear) {
    var arr = [];
    for (var i = 1900; i <= nowYear; i++) {
        arr.push({
            id: i + '',
            value: i + '年'
        });
    }
    return arr;
}
function formatMonth(year) {
    var arr = [];
    var maxMonth = year == nowYear ? nowMonth : 12;
    for (var i = 1; i <= maxMonth; i++) {
        if (i < 10) {
            arr.push({
                id: '0' + i + '',
                value: i + '月'
            });
        } else {
            arr.push({
                id: i + '',
                value: i + '月'
            });
        }
    }
    return arr;
}


function formatDate(year, month, count) {
    var arr = [];
    var maxDay = (year == nowYear && month == nowMonth) ? nowDate : count;
    for (var i = 1; i <= maxDay; i++) {
        if (i < 10) {
            arr.push({
                id: '0' + i + '',
                value: i + '日'
            });
        } else {
            arr.push({
                id: i + '',
                value: i + '日'
            });
        }
    }
    return arr;
}
var yearData = function (callback) {
    // settimeout只是模拟异步请求，真实情况可以去掉
    // setTimeout(function() {
    callback(formatYear(nowYear));
    // }, 2000)
};
var monthData = function (year, callback) {
    // settimeout只是模拟异步请求，真实情况可以去掉
    // setTimeout(function() {
    callback(formatMonth(year));
    // }, 2000);
};
var dateData = function (year, month, callback) {
    // settimeout只是模拟异步请求，真实情况可以去掉
    // setTimeout(function() {
    if (/^(01|03|05|07|08|10|12)$/.test(month)) {
        callback(formatDate(year, month, 31));
    }
    else if (/^(04|06|09|11)$/.test(month)) {
        callback(formatDate(year, month, 30));
    }
    else if (/^02$/.test(month)) {
        if (year % 4 === 0 && year % 100 !== 0 || year % 400 === 0) {
            callback(formatDate(year, month, 29));
        }
        else {
            callback(formatDate(year, month, 28));
        }
    }
    else {
        throw new Error('month is illegal');
    }
    // }, 2000);
    // ajax请求可以这样写
    /*
     $.ajax({
     type: 'get',
     url: '/example',
     success: function(data) {
     callback(data);
     }
     });
     */
};


if (Number(nowMonth) < 10) {
    nowMonth = "0" + nowMonth;
}
if (Number(nowDay) < 10) {
    nowDay = "0" + nowDay;
}

//生日
var myBir = localStorage.getItem('birthday');
if (myBir) {
    //var birthday = myBir.split('-');
    var birthday = myBir.split('.');
    showDateDom.attr('data-year', birthday[0]);
    showDateDom.attr('data-month', birthday[1]);
    showDateDom.attr('data-date', birthday[2]);
}
else {
    showDateDom.attr('data-year', nowYear);
    showDateDom.attr('data-month', nowMonth);
    showDateDom.attr('data-date', nowDay);
}

selectDateDom.bind('click', function () {
    var oneLevelId = showDateDom.attr('data-year');
    var twoLevelId = showDateDom.attr('data-month');
    var threeLevelId = showDateDom.attr('data-date');
    var iosSelect = new IosSelect(3,
        [yearData, monthData, dateData],
        {
            title: '生日选择',
            itemHeight: 35,
            oneLevelId: oneLevelId,
            twoLevelId: twoLevelId,
            threeLevelId: threeLevelId,
            showLoading: true,
            callback: function (selectOneObj, selectTwoObj, selectThreeObj) {
                showDateDom.attr('data-year', selectOneObj.id);
                showDateDom.attr('data-month', selectTwoObj.id);
                showDateDom.attr('data-date', selectThreeObj.id);
                showDateDom.addClass("gray");
                var newBirthday = selectOneObj.id + "." + selectTwoObj.id + '.' + selectThreeObj.id;
                localStorage.setItem('birthday', newBirthday);
                //var newBirthday = parseFloat(selectOneObj.value) + '-' + parseFloat(selectTwoObj.value) + '-' + parseFloat(selectThreeObj.value);
                showDateDom.html(newBirthday).attr('new_value', newBirthday).css({ "color": "#999" });
                showDateDom.addClass("gray");
                if (typeof (fill_birthday) !== "undefined") {
                    fill_birthday = true;
                    if (fill_lastName && fill_firstName && fill_birthday && fill_gender) {
                        $("#btnSubmit").addClass("active");
                    }
                }
            }
        });
});

//大宝生日
var dabao = localStorage.getItem('dabaoDay');
if (dabao) {
    var firstBir = dabao.split('-');
    showDateDomS.attr('data-year', firstBir[0]);
    showDateDomS.attr('data-month', firstBir[1]);
    showDateDomS.attr('data-date', firstBir[2]);
}
else {
    showDateDomS.attr('data-year', nowYear);
    showDateDomS.attr('data-month', nowMonth);
    showDateDomS.attr('data-date', nowDay);
}
selectDateDomS.bind('click', function () {
    var oneLevelId = showDateDomS.attr('data-year');
    var twoLevelId = showDateDomS.attr('data-month');
    var threeLevelId = showDateDomS.attr('data-date');
    var iosSelect = new IosSelect(3,
        [yearData, monthData, dateData],
        {
            title: '生日选择',
            itemHeight: 35,
            oneLevelId: oneLevelId,
            twoLevelId: twoLevelId,
            threeLevelId: threeLevelId,
            showLoading: true,
            callback: function (selectOneObj, selectTwoObj, selectThreeObj) {
                showDateDomS.attr('data-year', selectOneObj.id);
                showDateDomS.attr('data-month', selectTwoObj.id);
                showDateDomS.attr('data-date', selectThreeObj.id);
                showDateDomS.html(selectOneObj.id + "." + selectTwoObj.id + '.' + selectThreeObj.id);
                var dabao = selectOneObj.id + "-" + selectTwoObj.id + '-' + selectThreeObj.id;
                localStorage.setItem('dabaoDay', dabao);
                //showDateDomS.html(parseFloat(selectOneObj.value) + '-' + parseFloat(selectTwoObj.value) + '-' + parseFloat(selectThreeObj.value));
                sessionStorage.setItem('dabao', showDateDomS.html());
                showDateDomS.addClass("gray");
            }
        });
});

//二宝生日
var erbao = localStorage.getItem('erbaoDay');
if (erbao) {
    var secondBir = erbao.split('-');
    showDateDomErBir.attr('data-year', secondBir[0]);
    showDateDomErBir.attr('data-month', secondBir[1]);
    showDateDomErBir.attr('data-date', secondBir[2]);
}
else {
    showDateDomErBir.attr('data-year', nowYear);
    showDateDomErBir.attr('data-month', nowMonth);
    showDateDomErBir.attr('data-date', nowDay);
}
selectDateDomErBir.bind('click', function () {
    var oneLevelId = showDateDomErBir.attr('data-year');
    var twoLevelId = showDateDomErBir.attr('data-month');
    var threeLevelId = showDateDomErBir.attr('data-date');
    var iosSelect = new IosSelect(3,
        [yearData, monthData, dateData],
        {
            title: '生日选择',
            itemHeight: 35,
            oneLevelId: oneLevelId,
            twoLevelId: twoLevelId,
            threeLevelId: threeLevelId,
            showLoading: true,
            callback: function (selectOneObj, selectTwoObj, selectThreeObj) {
                showDateDomErBir.attr('data-year', selectOneObj.id);
                showDateDomErBir.attr('data-month', selectTwoObj.id);
                showDateDomErBir.attr('data-date', selectThreeObj.id);
                showDateDomErBir.html(selectOneObj.id + "." + selectTwoObj.id + '.' + selectThreeObj.id);
                var erbao = selectOneObj.id + "-" + selectTwoObj.id + '-' + selectThreeObj.id;
                localStorage.setItem('erbaoDay', erbao);
                //showDateDomErBir.html(parseFloat(selectOneObj.value) + '-' + parseFloat(selectTwoObj.value) + '-' + parseFloat(selectThreeObj.value));
                sessionStorage.setItem('erbao', showDateDomErBir.html());
                showDateDomErBir.addClass("gray");
            }
        });
});
