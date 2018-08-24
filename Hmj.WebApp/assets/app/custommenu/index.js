; (function (window, undefined, $) {
    $(document).ready(function () {
        $("#hRootID").val();
        pageDefault();
        showHideDiv(false, false, false, false, false, false, false, false, false, false, false, false);
    });
    var rootID = $("#hRootID").val();

    function pageDefault() {
        //同步读取左边菜单
        $.ajax({
            type: "post",
            async: false,
            url: options.treeNodesUrl,
            success: function (menus) {
                console.log(menus);
                //先生成一级菜单的DOM
                //menus = $.parseJSON(menus);
                if (typeof (menus[0].ID) == "undefined") { menus = $.parseJSON(menus); }
                for (var i = 0; i < menus.length; i++) {
                    if (menus[i].Depth == 1) {
                        $("#menu").append(getH3AndUl(menus[i].ID, menus[i].Name));
                    }
                    $("#menu h3:eq(0)").css("border-top", "none");
                }
                //再生成二级菜单的DOM
                for (var i = 0; i < menus.length; i++) {
                    if (menus[i].Depth == 2) {
                        $("#menu h3[menuid='" + menus[i].ParentID + "']").next("ul").append(getLi(menus[i].ID, menus[i].Name));
                    }
                }
            }
        });
    }

    addup = function addup(level, id) {
        if (!id && level == 1) {
            if ($("#menu h3").length > 2) {
                alert("您最多只能添加3个菜单");
            } else {
                addMenu("微网站", rootID, level);
            }
        }

        else if (id && level == 2) {
            if ($("#menu h3[menuid='" + id + "']").next("ul").find("li").length > 4) {
                alert("您最多只能添加5个菜单");
            } else {
                addMenu("子菜单", id, level);
            }
        }
    }

    //AJAX请求与数据库交互添加一个菜单
    var newMenuCount = 1;
    function addMenu(name, parentID, level) {
        //showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });
        $.post(options.addChildUrl, { Name: name + (newMenuCount++), ParentID: parentID },
            function (menu) {
                if (level == 1) {
                    $("#menu").append(getH3AndUl(menu.ID, menu.Name));
                    $("#menu h3:eq(0)").css("border-top", "none");
                }
                else if (level == 2) {
                    $("#menu h3[menuid='" + parentID + "']").next("ul").append(getLi(menu.ID, menu.Name));
                }
                //hideLoadingMsg();
            }, "json");
    };

    deleteaway = function deleteaway(level, id) {
        $.post(options.deleteMenuUrl, { id: id },
            function (ret) {
                if (ret.status == 0) {
                    if (id && level == 1) {
                        //删 h3 和 ul
                        $("#menu h3[menuid='" + id + "']").next("ul").remove();
                        $("#menu h3[menuid='" + id + "']").remove();
                    }
                    else if (id && level == 2) {
                        //删 li
                        $("#menu ul li[menuid='" + id + "']").remove();
                    }
                }
            }, "json");
    };

    edit = function edit(id) {
        $("#tishi").css("display", "block");
        $("#tishibg").css("display", "block");
        $.ajax({
            type: "post",
            async: false,
            url: options.showMenuInfoUrl,
            data: { id: id },
            success: function (ret) {
                if (ret.status == 0) {
                    $("#frmChangeName").find("input[type='text'][name='Name']").val(ret.data.Name);
                    $("#frmChangeName").find("input[type='hidden'][name='ID']").val(ret.data.ID);
                }
            }
        });
    }

    close = function close() {
        $("#tishi").css("display", "none");
        $("#tishibg").css("display", "none");
    }

    tuoye = function tuoye() {
        $(".zidingyiTItle").find("bdo").hide();
        $(".zidingyiTItle").find("i").fadeIn();
        sortall();
    }

    notuoye = function notuoye() {
        $(".zidingyiTItle").find("bdo").fadeIn();
        $(".zidingyiTItle").find("i").hide();
        baocun();

        $("#menu").empty();
        pageDefault();
    }

    complete = function complete() {
        var list0 = [];
        $("#menu ul").each(function () {
            var list1 = [];
            $(this).find("li").each(function () {
                var li = $(this);
                var menu = { "id": parseInt(li.attr("menuid")), "name": li.text() };
                list1.push(menu);
            });
            list0.push({ children: list1 });
        });

        var obj = { menuJSON: $.toJSON({ menus: list0 }) };

        $.ajax({
            type: "post",
            async: false,
            url: options.saveMenuOrderUrl,
            data: obj,
            success: function (ret) {
                notuoye();
            }
        });
    }

    setAction = function setAction(id) {
        var quanli = $("#menu li");
        for (var c = 0; c < quanli.length; c++) {
            $(quanli[c]).removeClass("curzi");
        }
        $("#menu li[menuid='" + id + "']").addClass("curzi");

        $.post(options.showMenuInfoUrl, { id: id },
            function (res) {
                if (res.status == 0) {
                    var menu = res.data;
                    domInitialize(menu);

                    if (menu.Type == null) {     //没有设置过 默认界面
                        showHideDiv(true, false, false, false, false, false, false, false, false, false, false, false);
                    }
                    else {
                        if (menu.Type == 0) {
                            showHideDiv(false, true, true, true, false, false, false, false, false, false, false, false);
                        }
                        else if (menu.Type == 1) {
                            //domInitialize会在AJAX执行后showHideDiv
                        }
                        else if (menu.Type == 3) {
                            showHideDiv(false, false, false, false, false, false, true, true, false, false, false, false);
                        }
                        else if (menu.Type == 4) {
                            showHideDiv(false, false, false, false, false, false, false, false, false, true, true, false);
                        }
                        else { }
                    }
                }
            }, "json");
    }


    setNewMini = function setNewMini() {
        $("#divMini #linksMini p").text("");
        $("#divMini #nolinksMini input[type='text'][name='txtMini']").val("");
        showHideDiv(false, false, false, false, false, false, false, false, false, true, false, true);
    }

    editMini = function editMini() {
        var pText = $("#divMini #linksMini p").text();
        var orderNum = $("#divMini #linksMini span").text();
        $("#divMini #nolinksMini input[type='text'][name='txtMini']").val(pText);
        $("#divMini #nolinksMini input[type='text'][name='txtOrderNum']").val(orderNum);
        showHideDiv(false, false, false, false, false, false, false, false, false, true, false, true);
    };

    saveMini = function saveMini() {
        var id = $(".zidingyicon").find("input[type='hidden'][name='hfID']").val();
        var minitxt = $("#divMini").find("input[type='text'][name='txtMini']").val();
        var OrderNum = $("#divMini").find("input[type='text'][name='txtOrderNum']").val(); 
        //if (isURL(url)) {
        $.post(options.saveMenuUrl, { id: id, type: 4, typeArg: minitxt, OrderNum: OrderNum},
            function (res) {
                if (res.status == 0) {
                    var menu = res.data;
                    domInitialize(menu);
                    showHideDiv(false, false, false, false, false, false, false, false, false, true, true, false);
                }
            }, "json");
        //}
        //else {
        //    alert("请输入一个有效url。");
        //}
    }

    returnMini = function returnMini() {
        if ($(".zidingyicon").find("input[type='hidden'][name='hfType']").val() == "") {
            showHideDiv(true, false, false, false, false, false, false, false, false, false, false, false);
        }
        else {
            showHideDiv(false, false, false, false, false, false, false, false, false, true, true, false);
        }
    }




    setNewClick = function setNewClick() {
        $("#divClick #watch #text p").text("");
        $("#divClick #edit").find("textarea[name='txtContent']").val("");
        gbcount(500);
        showHideDiv(false, true, false, false, false, true, false, false, false, false, false, false);
    }

    editClick = function editClick() {
        var pText = $("#divClick #watch #text p").text();
        var orderNum = $("#divClick #watch #text span").text();
        $("#divClick").find("textarea[name='txtContent']").val(pText);
        $("#divClick").find("input[name='txtOrderNum']").val(orderNum);
        gbcount(500);
        showHideDiv(false, true, false, false, false, true, false, false, false, false, false, false);
    }

    saveClick = function saveClick() {
        var id = $(".zidingyicon").find("input[type='hidden'][name='hfID']").val();
        var content = $.trim($("#divClick").find("textarea[name='txtContent']").val());
        var OrderNum = $.trim($("#divClick").find("input[name='txtOrderNum']").val());
        if (content != "") {
            $.post(options.saveMenuUrl, { id: id, type: 0, typeArg: content, OrderNum: OrderNum },
                function (res) {
                    if (res.status == 0) {
                        var menu = res.data;
                        domInitialize(menu);
                        showHideDiv(false, true, true, true, false, false, false, false, false, false, false, false);
                    }
                }, "json");
        }
        else {
            alert("请输入内容。");
        }
    }

    returnClick = function returnClick() {
        var type = $(".zidingyicon").find("input[type='hidden'][name='hfType']").val();
        if (type == "") {
            showHideDiv(true, false, false, false, false, false, false, false, false, false, false, false);
        }
        else {
            if (type == "0") {
                showHideDiv(false, true, true, true, false, false, false, false, false, false, false, false);
            }
            else if (type == "1") {
                showHideDiv(false, true, true, false, true, false, false, false, false, false, false, false);
            }
        }
    }

    setNewView = function setNewView() {
        $("#divView #links p").text("");
        $("#divView #nolinks input[type='text'][name='txtUrl']").val("");
        showHideDiv(false, false, false, false, false, false, true, false, true, false, false, false);
    }

    editView = function editView() {
        var pText = $("#divView #links p").text();
        var orderNum = $("#divView #links span").text();
        $("#divView #nolinks input[type='text'][name='txtUrl']").val(pText);
        $("#divView #nolinks input[type='text'][name='txtOrderNum']").val(orderNum);
        showHideDiv(false, false, false, false, false, false, true, false, true, false, false, false);
    };

    saveView = function saveView() {
        var id = $(".zidingyicon").find("input[type='hidden'][name='hfID']").val();
        var url = $("#divView").find("input[type='text'][name='txtUrl']").val();
        var OrderNum = $("#divView").find("input[type='text'][name='txtOrderNum']").val();
        //if (isURL(url)) {
        $.post(options.saveMenuUrl, { id: id, type: 3, typeArg: url, OrderNum: OrderNum },
            function (res) {
                if (res.status == 0) {
                    var menu = res.data;
                    domInitialize(menu);
                    showHideDiv(false, false, false, false, false, false, true, true, false, false, false, false);
                }
            }, "json");
        //}
        //else {
        //    alert("请输入一个有效url。");
        //}
    }

    returnView = function returnView() {
        if ($(".zidingyicon").find("input[type='hidden'][name='hfType']").val() == "") {
            showHideDiv(true, false, false, false, false, false, false, false, false, false, false, false);
        }
        else {
            showHideDiv(false, false, false, false, false, false, true, true, false, false, false, false);
        }
    }

    function getByteLen(val) {
        var len = 0;
        for (var i = 0; i < val.length; i++) {
            if (val[i].match(/[^\x00-\xff]/ig) != null) //全角
                len += 2;
            else
                len += 1;
        }
        return len;
    }

    gbcount = function gbcount(num) {
        var txtlen = getByteLen(document.getElementById('area2').value);
        var curlen = num - txtlen;
        if (curlen >= 0) {
            $("#area2").next(".bianjitext").html("还可以输入" + curlen + "个字符，中文占2个");
        } else {
            $("#area2").next(".bianjitext").html("已超出<strong>" + (-curlen) + "</strong>字符");
        }
    }

    function isURL(url) {
        var strRegex = "^((https|http|ftp|rtsp|mms)://)?[a-z0-9A-Z]{3}\.[a-z0-9A-Z][a-z0-9A-Z]{0,61}?[a-z0-9A-Z]\.com|net|cn|cc (:s[0-9]{1-4})?/$";
        var re = new RegExp(strRegex);
        if (re.test(url)) {
            return true;
        }
        else {
            return false;
        }
    }

    //拖拽模式
    function sortall() {
        var quanli = $("#menu li");
        for (var b = 0; b < quanli.length; b++) {
            $(quanli[b]).find("dd").show();
        }
        $('#menu').removeClass("zidingTT2");
        $('#menu ul').sortable().bind('sortupdate');
    }

    //非拖拽模式
    function baocun() {
        var quanli = $("#menu li");
        for (var c = 0; c < quanli.length; c++) {
            $(quanli[c]).find("dd").hide();
        }
        $('#menu').addClass("zidingTT2");
    }

    changeName = function changeName() {
        $("#frmChangeName").submit();
    };

    $('#frmChangeName').validator({
        rules: {},
        fields: { Name: "required" },
        valid: function (form) {
            FormSubmit(form, function (res) {
                if (res.status == 0) {
                    //左边更新DOM元素
                    if (res.data.Depth == 1) {
                        // $("#menu h3[menuid='" + res.data.ID + "']").text(res.data.Name);
                        $("#menu h3[menuid='" + res.data.ID + "']").html(getH3ExceptTag(res.data.ID, res.data.Name));
                    }
                    else if (res.data.Depth == 2) {
                        $("#menu li[menuid='" + res.data.ID + "']").html(getLiExceptTag(res.data.ID, res.data.Name));
                    }
                }
                close();
            })
        }
    });



    function getH3AndUl(id, name) {
        //生成 h3 和 ul
        return '<h3 menuid=' + id + ' onclick="setAction(' + id + ')"><span><a href="javascript:addup(2,' + id + ');" class="jiahao2"></a> <a href="javascript:edit(' + id +
            ');" class="editicon"></a> <bdo><a href="javascript:deleteaway(1,' + id + ');" class="delicon"></a></bdo></span>' + name + '</h3><ul></ul>';
    }

    function getH3ExceptTag(id, name) {
        return '<span><a href="javascript:addup(2,' + id + ');" class="jiahao2"></a> <a href="javascript:edit(' + id +
            ');" class="editicon"></a> <bdo><a href="javascript:deleteaway(1,' + id + ');" class="delicon"></a></bdo></span>' + name;
    }

    function getLi(id, name) {
        //生成 li
        return '<li menuid=' + id + ' onclick="setAction(' + id + ')"><span><a href="javascript:edit(' + id + ');" class="editicon"></a> <bdo><a href="javascript:deleteaway(2,' + id +
            ')" class="delicon"></a></bdo></span><dd><a href="javascript:void(0)" class="tuoicon"></a></dd>' + name + '</li>';
    }

    function getLiExceptTag(id, name) {
        return '<span><a href="javascript:edit(' + id + ');" class="editicon"></a> <bdo><a href="javascript:deleteaway(2,' + id +
            ')" class="delicon"></a></bdo></span><dd><a href="javascript:void(0)" class="tuoicon"></a></dd>' + name;
    }

    publish = function publish() {
        //if (confirm("公众平台限制了每天发布的次数，确认如此做吗？")) {
        //$.post(options.publishToWechatUrl,
        // null,
        // function (ret) {
        //     if (ret.status == 0) {
        //         alert("发布成功,创建自定义菜单后,由于微信客户端缓存，需要24小时微信客户端才会展现出来。");
        //     }
        //     else {
        //         alert("发布失败，errcode:" + ret.status + ",errmsg:" + ret.message);
        //     }
        // },
        // "json");

        $.ajax({
            type: "Post",
            url: options.publishToWechatUrl,
            dataType: "json",
            success: function (ret) {
                console.log(ret);
                if (ret.status == 0) {
                    alert("发布成功,创建自定义菜单后,由于微信客户端缓存，需要24小时微信客户端才会展现出来。");
                }
                else {
                    alert("发布失败，errcode:" + ret.status + ",errmsg:" + ret.message);
                }
            },
            error: function (d, c, e) {
                alert(d + " " + c + " " + e);
            }
        });
        //}
    }
})(window, undefined, jQuery);

//boolDefault
//boolClick
//         boolWatch
//                  boolWatchText
//                  boolWatchGraphic
//boolView
//        boolLinks
//        boolNolinks
function showHideDiv(boolDefault, boolClick, boolWatch, boolWatchText, boolWatchGraphic, boolEdit, boolView, boolLinks, boolNolinks, boolMini, boolLinksMini, boolNolinksMini) {



    $("#divDefault").hide();
    $("#divClick").hide();
    $("#divClick #watch").hide();
    $("#divClick #watch #text").hide();
    $("#divClick #watch #graphic").hide();
    $("#divClick #edit").hide();
    $("#divView").hide();
    $("#divView #links").hide();
    $("#divView #nolinks").hide();

    $("#divMini").hide();
    $("#divMini #linksMini").hide();
    $("#divMini #nolinksMini").hide();

    console.log(boolMini);
    console.log(boolLinksMini);
    console.log(boolNolinksMini);

    if (boolDefault) {
        $("#divDefault").fadeIn();
    }

    if (boolClick) {
        $("#divClick").fadeIn();
        if (boolWatch) {
            $("#divClick #watch").fadeIn();

            if (boolWatchText) {
                $("#divClick #watch #text").fadeIn();
            }
            if (boolWatchGraphic) {
                $("#divClick #watch #graphic").fadeIn();
            }
        }
        if (boolEdit) {
            $("#divClick #edit").fadeIn();
        }
    }

    if (boolView) {
        $("#divView").fadeIn();
        if (boolLinks) {
            $("#divView #links").fadeIn();
        }
        if (boolNolinks) {
            $("#divView #nolinks").fadeIn();
        }
    }

    if (boolMini) {
        $("#divMini").fadeIn();
        if (boolLinksMini) {
            $("#divMini #linksMini").fadeIn();
        }
        if (boolNolinksMini) {
            $("#divMini #nolinksMini").fadeIn();
        }
    }
}

function domInitialize(menu) {
    $(".zidingyicon").find("input[type='hidden'][name='hfID']").val(menu.ID);
    $(".zidingyicon").find("input[type='hidden'][name='hfType']").val(menu.Type);
    $("#divClick #watch #text p").text("");
    $("#divClick #edit").find("textarea[name='txtContent']").val("");
    $("#divClick #watch #graphic .tuwen").empty();
    $("#divView #links p").text("");
    $("#divView #nolinks input[type='text'][name='txtUrl']").val("");

    if (!menu.OrderNum||menu.OrderNum == "undefined")
        menu.OrderNum = "";

    if (menu.Type == "0") {
        $("#divClick #watch #text p").text(menu.Content);
        $("#divClick #watch #text span").text(menu.OrderNum);
        gbcount(500);
    }

    else if (menu.Type == "1") {
        showGraphic(menu.Graphic_ID);
    }
    else if (menu.Type == "3") {
        $("#divView #links p").text(menu.Url);
        $("#divView #links span").text(menu.OrderNum);
    }
    else if (menu.Type == "4") {
        $("#divMini #linksMini p").text(menu.Content);
        $("#divMini #linksMini span").text(menu.OrderNum);
    }
    else {

    }
}