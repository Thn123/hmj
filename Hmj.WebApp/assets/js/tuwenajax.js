; (function (window, undefined, $) {
    editsu = function editsu() {
        if ($.trim($("#tiles").html()) == "") {
            tilesajax();
        }
        $("#tiles li").each(function () {
            $(this).find("bdo").attr("class", "");
        });
        // $("#tiles").empty();
        document.getElementById('tishi2').style.display = "block";
        document.getElementById('tishibg').style.display = "block";

    }

    closedsu = function closedsu() {
        document.getElementById('tishi2').style.display = "none";
        document.getElementById('tishibg').style.display = "none";
    }

    function tilesajax() {
        $.post(options.getGraphicListUrl, { pageIndex: 0 },
                function (res) {
                    var graphic = res.data;
                    var html = "";
                    for (var i = 0; i < graphic.length; i++) {
                        var details = graphic[i].details;
                        var divonetuwen = "";
                        var divduotuwen = "";
                        for (var j = 0; j < details.length; j++) {
                            if (j == 0) {
                                divonetuwen = "<div class=\"onetuwen\"><div class=\"tuwenTitle\"><a href=\"#\">" + details[j].title + "</a></div><div class=\"tuwenTime\">" + details[j].createdate
                                    + "</div><div class=\"tuwencon\"><img src=" + details[j].pic + "><p>" + details[j].title + "</p></div></div>";
                            }
                            else {
                                if (divduotuwen == "") {
                                    divduotuwen = "<div class=\"duotuwen\">";
                                }
                                divduotuwen += "<p><img src=" + details[j].pic + " width=\"80\" height=\"80\"><span><a href=\"#\">" + details[j].title + "</a></span></p>"
                            }
                        }
                        if (divduotuwen != "") {
                            divduotuwen += " </div>";
                        }
                        html += "<li graphicid=" + graphic[i].id + " onclick=\"xuanzhong(" + graphic[i].id + ")\"><div class=\"tuwen\">" + divonetuwen + divduotuwen +
                            " </div><div class=\"hoverbg\"></div><bdo></bdo></li>"
                    }
                    $("#tiles").append(html);
                    setTimeout(pubuliu, 1000);  //直接执行瀑布流会有问题，等1秒
                }, "json");
    }

    xuanzhong = function xuanzhong(id) {
        $("#tiles li").each(function () {
            if ($(this).attr("graphicid") == id) {
                $(this).find("bdo").attr("class", "xuanzhong");
            }
            else {
                $(this).find("bdo").attr("class", "");
            }
        });
    }

    function pubuliu() {
        var handler = $('#tiles li');
        handler.wookmark({
            autoResize: true,
            container: $('#tanmain'),
            offset: 20,
            itemWidth: 302
        });
    }

    saveGraphic = function saveGraphic() {
        var id = $(".zidingyicon").find("input[type='hidden'][name='hfID']").val();
        var graphicId = $("#tiles li bdo[class='xuanzhong']").parent("li").attr("graphicid");
        if ($.trim(graphicId) != "") {
            $.post(options.saveMenuUrl, { id: id, type: 1, typeArg: graphicId },
                   function (res) {
                       if (res.status == 0) {
                           var menu = res.data;
                           domInitialize(menu);
                       }
                   }, "json");
        }
        else {
            alert("请选择一个图文。");
        }
    }
})(window, undefined, jQuery);

function showGraphic(graphicId) {
    $.post(options.getGraphicUrl, { id: graphicId },
                 function (res) {
                     if (res.status == 0) {
                         var html = bindTuWen(res.data);
                         $("#divClick #watch #graphic .tuwen").append(html);
                         showHideDiv(false, true, true, false, true, false, false, false, false);
                         closedsu();
                     }
                 }, "json");
}

function bindTuWen(details) {
    var html = "";
    var divonetuwen = "";
    var divduotuwen = "";
    for (var j = 0; j < details.length; j++) {
        if (j == 0) {
            divonetuwen = "<div class=\"onetuwen\"><div class=\"tuwenTitle\"><a href=\"#\">" + details[j].title + "</a></div><div class=\"tuwenTime\">" + details[j].createdate
                + "</div><div class=\"tuwencon\"><img src=" + details[j].pic + "><p>" + details[j].title + "</p></div></div>";
        }
        else {
            if (divduotuwen == "") {
                divduotuwen = "<div class=\"duotuwen\">";
            }
            divduotuwen += "<p><img src=" + details[j].pic + " width=\"80\" height=\"80\"><span><a href=\"#\">" + details[j].title + "</a></span></p>"
        }
    }
    if (divduotuwen != "") {
        divduotuwen += " </div>";
    }
    html = "<li><div class=\"tuwen\">" + divonetuwen + divduotuwen + " </div><div class=\"hoverbg\"></div><bdo></bdo></li>"
    return html;
}