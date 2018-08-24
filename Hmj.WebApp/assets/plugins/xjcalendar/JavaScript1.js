; (function (window, undefined, $) {
    /*
    <div style=\"width:${width};top:${top};left:${left};\" title=\"${tip}\" class=\"chip chip${i} ${drag}\">
	<div class=\"dhdV\" style=\"display:none\">${data}</div>
	<div style=\"border-bottom-color:${bdcolor}\" class='ct'>&nbsp;</div>
	<dl style=\"border-color:${bdcolor};background-color:${bgcolor1};height: ${height}px;\">
		<dt style=\"background-color:${bgcolor2}\">${icon}${title} </dt>
		<dd>
			<div>${content}</div>
			<div class='chip-tool'>${content2}</div>
		</dd>
		<div class='resizer' style='display:${redisplay}'>
			<div class='rszr_icon'>&nbsp;</div>
		</div>
	</dl>
	<div style=\"border-color:${bdcolor};background-color:${bgcolor1};\" class='cb1'>&nbsp;</div>
	<div style=\"border-color:${bdcolor};\" class='cb2'>&nbsp;</div>
  </div>
    */
    var __ITEMTTEMP__ = "<div style=\"width:${width};top:${top};left:${left};\" title=\"${tip}\" class=\"chip chip${i} ${drag}\"><div class=\"dhdV\" style=\"display:none\">${data}</div><div style=\"border-bottom-color:${bdcolor}\" class='ct'>&nbsp;</div><dl style=\"border-color:${bdcolor};background-color:${bgcolor1};height: ${height}px;\"><dt style=\"background-color:${bgcolor2}\">${icon}${title}</dt><dd><div>${content}</div><div class='chip-tool'>${content2}</div></dd><div class='resizer' style='display:${redisplay}'><div class='rszr_icon'>&nbsp;</div></div></dl><div style=\"border-color:${bdcolor};background-color:${bgcolor1};\" class='cb1'>&nbsp;</div><div style=\"border-color:${bdcolor};\" class='cb2'>&nbsp;</div></div>";
    var __DRAGDATA__;
    var __DRAGEVENT__;
    var __STARTHOUR__ = 11;
    function xjCalendar(id, options) {
        this.options = $.extend({
            url: "", //请求数据的Url    
            view: 1, // 1 多技师视图 ， 单技师多日视图
            eventItems: [], //日程数据，可通过此参数设置初始化数据
            method: "POST", //异步提交数据的方式，默认为POST建议不要修改。
            colModels: [],//横向栏目[{id:"jishi1",display:"王师傅",shm:"",ehm:""},{id:"jishi2",display:"李师傅"}]
            rowRange: { start: 11, end: 22 },// 纵向时间范围 默认为上午11点到晚上22点
            showDay: new Date(), //显示日期，默认为当天          
            onDragAdd: false, //快速添加的拦截函数，
            onFilterData: false,//
            onItemShow: false, //显示信息
            onBindEvent: false,//绑定特殊的事件
            onChipToolClick: false,
            autoLoad: false, //自动加载，如果eventItems参数没有配置，可启用该参数，默认第一次展现时
            readonly: false, //是否只读，某些情况下，可设置整个
            theme: 14,//风格
            width: 850,
            height: 650,
            extParam: [], //额外参数，在所以异步请求中，都会附加的额外参数，可配置其他扩展的查询条件
            enableDrag: true //是否可拖拽             
        }, options);
        this.elid = id;
        this.el = $("#" + id);
        this.init();
    }
    xjCalendar.prototype = {
        /**
         * 初始化         
         */
        init: function () {

            var width = 81 * (this.options.colModels.length) + 17;
            if (width < this.options.width) {
                this.options.width = width; //页面足够大的情况
            }

            this.el.addClass("xjcalendar").css({ width: (this.options.width + 60) + "px", height: (this.options.height + 30) + "px" });//+492
            __STARTHOUR__ = this.options.rowRange.start;

            if (this.options.url && this.options.autoLoad) {
                this.populate(); //访问数据
            }
            else {
                //否则直接开始输出HTML
                this.render();
            }
        },
        populate: function () {

            var self = this;
            var option = self.options;
            //向服务端发起请求
            if (option.isloading) {
                return true;
            }

            if (option.url) {
                option.isloading = true;
                var url = option.url + "?m=load";
                if (option.onBeforeRequestData && $.isFunction(option.onBeforeRequestData)) {
                    option.onBeforeRequestData(1);
                }
                var param = [
                   { name: "showdate", value: option.showDay.Format("yyyy-MM-dd") },
                   { name: "viewtype", value: option.view },
                ];
                if (option.extParam) {
                    for (var pi = 0; pi < option.extParam.length; pi++) {
                        param[param.length] = option.extParam[pi];
                    }
                }

                $.ajax({
                    type: option.method, //
                    url: url,
                    data: param,
                    dataType: "text",  // fixed jquery 1.4 not support Ms Date Json Format /Date(@Tickets)/
                    //dataType: "json",
                    //dataFilter: function(data, type) { return data.replace(/"\\\/(Date\([0-9-]+\))\\\/"/gi, "new $1"); },
                    success://function(data) {
                        function (datastr) {
                            datastr = datastr.replace(/\/(Date\(([0-9]+)([\+-][0-9]{4})?\))\//gi, 'new Date($2)');
                            var res = (new Function("return " + datastr))();
                            //debugger;
                            if (res != null && res.status != 0) {
                                if (option.onRequestDataError) {
                                    option.onRequestDataError(1, res.message);
                                }
                            }
                            else {
                                self.process(res.data);
                            }
                            if (option.onAfterRequestData && $.isFunction(option.onAfterRequestData)) {
                                option.onAfterRequestData(1);
                            }
                            option.isloading = false;
                        },
                    error: function (data) {
                        try {
                            if (option.onRequestDataError) {
                                option.onRequestDataError(1, data);
                            } else {
                                alert("意外错误！");
                            }
                            if (option.onAfterRequestData && $.isFunction(option.onAfterRequestData)) {
                                option.onAfterRequestData(1);
                            }
                            option.isloading = false;
                        } catch (e) { }
                    }
                });
            }
            else {
                alert("url没有配置");
            }


        },
        process: function (data) { // 处理返回数据
            if (this.options.onFilterData) {
                this.options.eventItems = this.options.onFilterData(data);
            }
            else {
                this.options.eventItems = data;
            }
            this.render();
        },
        render: function () {
            var html = [];
            html.push("<div class=\"xjcal-fixed\"><table>");
            html.push("<tr><td><div class=\"fix-tb-top\">&nbsp;</div></td></tr>");
            html.push("</table></div>");
            //纵向时间维度         
            html.push("<div id='xjcal-hours-left-p' class=\"xjcal-hours-left\" style='height:", this.options.height - 1, "px;'><table class=\"xjcal-hours-left-tb\">");//+530
            __RenderHoursTb__(this.options.rowRange.start, this.options.rowRange.end, html);
            html.push("</table></div>");
            //html.push("</div>");
            //html.push("</div>");

            //纵向横向维度
            html.push("<div id='xjcal-col-top-p' class=\"xjcal-col-top\" style='width:", this.options.width, "px;'><table class=\"xjcal-col-top-tb\">");
            html.push("<tr>");
            for (var j = 0, l = this.options.colModels.length; j < l; j++) {
                html.push("<td style='width:", 80, "px'><div class=\"data-main-top\">", this.options.colModels[j].display, "</div></td>");
            };
            html.push("<td style='width:17px'><div class=\"data-main-top\">&nbsp;</div></td>");
            html.push("</tr>");

            html.push("</table></div>");
            //生成数据表格
            html.push("<div id='xjcal-data-main-p' class=\"xjcal-data-main\" style='width:", this.options.width, "px;height:", this.options.height, "px'><table class=\"xjcal-data-main-tb\">");//+600
            __RenderMainTable__(this.options.rowRange.start, this.options.rowRange.end, this.options.colModels, this.options.eventItems, html, this.options);
            html.push("</table></div>");
            //html.push("</div>");
            //html.push("</div>");

            this.el.html(html.join(""));
            html = null;
            this.bind_event();

            //滚动条当前时间位置
            var mhv = $("#xjcal-data-main-p");
            var currentday = new Date();
            var h = currentday.getHours();
            var m = currentday.getMinutes();
            var th = __GetPosition__(h, m);
            var ch = mhv[0].clientHeight;
            var sh = th - 0.5 * ch;
            var ph = mhv[0].scrollHeight;
            if (sh < 0)
                sh = 0;
            if (sh > ph - ch) {
                var ehour = this.options.rowRange.end;
                var shour = this.options.rowRange.start;
                sh = ph - ch - 10 * (ehour - shour - h);
            }
            mhv[0].scrollTop = sh;


        },
        /**
        * 绑定事件  
        */
        bind_event: function () {
            var panel = this.el;

            var self = this;
            /* 屏蔽拖拽功能*/
            $("div.chip", panel).each(function (i) {
                var chip = $(this);
                chip.click(function (e) { __ItemShow__.call(this, self.options); return false; });
                chip.mousedown(__ReturnFalse__);
                /*
                if (chip.hasClass("drag")) {
                    chip.mousedown(function (e) { __DragStart__.call(this, "1", e); return false; });
                    //resize         
                    chip.find("div.resizer").mousedown(function (e) {
                        __DragStart__.call($(this).parent().parent(), "2", e); return false;
                    });
                }
                else {
                    chip.mousedown(__ReturnFalse__);
                }
                */
                if (self.options.onBindEvent) {
                    self.options.onBindEvent.call(chip, __GetData__(chip));
                }
            });
            $("a.chip-user", panel).each(function (i) {
                var chip = $(this).parent().parent().parent();
                $(this).click(function (e) {
                    __CHIPTOOLClICK__.call(chip, "1", self.options);
                })
            });

            $("div.chip-tool a", panel).each(function (i) {
                var chip = $(this).parent().parent().parent().parent();
                $(this).click(function (e) {
                    var type = $(this).attr("abbr");
                    __CHIPTOOLClICK__.call(chip, type, self.options);
                })
            });
            //$("#tgspanningwrapper div.tg-dualmarker").each(function (e) {
            //    $(this).hover(function (e) { $(this).addClass("marker_over") }, function (e) { $(this).removeClass("marker_over") });
            //});
            //给当日的添加
            if (this.options.readonly == false) {
                $("td.tg-col", panel).each(function (i) {
                    $(this).mousedown(function (e) { __DragStart__.call(this, "3", e, self.options); return false; });
                });
            }

            //滚动条事件
            $("#xjcal-data-main-p").scroll(function (e) {
                var left = $(this).scrollLeft();
                var top = $(this).scrollTop();

                $("#xjcal-col-top-p").scrollLeft(left);
                $("#xjcal-hours-left-p").scrollTop(top);
            });
        },
        setoption: function (p) {
            $.extend(this.options, p);
        },
        reload: function (p) {
            if (p) {
                //if (p.colModels) {//重新设置宽度
                //    var width = 160 * (p.colModels.length) + 100+80;
                //    this.el.css("width", width + "px");
                //}
                this.setoption(p);
            }
            this.populate();
        }

    }
    $(document).mousemove(__DragMove__).mouseup(__DragEnd__);
    //拖拽 相关
    function __RealseDragEvent__() {
        if (__DRAGEVENT__) {
            __DRAGEVENT__();
            __DRAGEVENT__ = null;
        }
    }
    // 拖拽了
    function __DragStart__(type, e, options) {
        //type 1 移动位置  2 改变大小  3 新建新预约
        var obj = $(this);
        var source = e.srcElement || e.target;
        __RealseDragEvent__(); //释放上一次的拖拽事件元素
        switch (type) {
            case "1": //移动位置 
                var h = obj.height();
                var data = __GetData__(obj);
                __DRAGDATA__ = {
                    type: 1, target: obj, sx: e.pageX, sy: e.pageY,
                    h: h,
                    data: data
                };
                break;
            case "2": //resize;
                var h = obj.height();
                var data = __GetData__(obj);
                __DRAGDATA__ = { type: 2, target: obj, sx: e.pageX, sy: e.pageY, h: h, data: data };
                break;
            case "3": //新建新预约             
                __DRAGDATA__ = { options: options, type: 3, target: obj, sx: e.pageX, sy: e.pageY };
                break;
        }
    }
    //移动中
    function __DragMove__(e) {
        if (__DRAGDATA__) {

            var d = __DRAGDATA__;
            switch (d.type) {
                case 1: //移动位置 
                    var data = d.data;
                    if (data != null) {
                        var sy = d.sy;
                        var y = e.pageY;
                        var diffy = y - sy;
                        if (diffy > 5 || diffy < -5 || d.cpwrap) {
                            var gh, ny, tempdata;
                            if (!d.cpwrap) {
                                var shm = data.OST.split(":");
                                var ehm = data.OET.split(":");
                                gh = {
                                    sh: parseInt(shm[0], 10),
                                    sm: parseInt(shm[1], 10),
                                    eh: parseInt(ehm[0], 10),
                                    em: parseInt(ehm[1], 10),
                                    h: d.h
                                };
                                d.target.hide(); //隐藏原来的元素
                                //console.log(d.target.css("display"));
                                var ny = __GetPosition__(gh.sh, gh.sm); //获取当前的位置
                                d.top = ny;
                                tempdata = __BuildTempItem__(data, gh.h);
                                var cpwrap = $("<div class='ca-evpi drag-chip-wrapper' style='top:" + ny + "px'/>").html(tempdata);
                                var evid = d.target.parent().attr("id").replace("tgCol", "#tgOver");

                                $(evid).append(cpwrap);
                                d.cpwrap = cpwrap;
                                d.ny = ny;
                            }
                            else {
                                ny = d.top + diffy;
                                var pny = ny % 17;
                                if (pny != 0) {
                                    ny = ny - pny;
                                }
                                if (d.ny != ny) {
                                    //log.info("ny=" + ny);
                                    gh = _GetPHM_(ny, ny + d.h);
                                    //log.info("sh=" + gh.sh + ",sm=" + gh.sm);
                                    tempdata = __BuildTempItem__(data, gh.h);
                                    d.cpwrap.css("top", ny + "px").html(tempdata);
                                }
                                d.ny = ny;
                            }
                        }
                    }
                    break;
                case 2: //resize; 原 type 5               
                    break;
                case 3: //新建新预约
                    //console.log("拖拽移动中....");
                    var sy = d.sy;
                    var y = e.pageY;
                    var diffy = y - sy; //计算移动的距离
                    if (diffy > 9 || diffy < -9 || d.cpwrap) { //8+1 代表移动大于半个单元格 ，cpwrap是 17 是一个单元格的高度

                        var dy = diffy % 17;
                        if (dy != 0) { //最小新建单位是一个单元格
                            diffy = dy > 0 ? diffy + 17 - dy : diffy - 17 - dy;
                            y = d.sy + diffy;
                            if (diffy < 0) {
                                sy = sy + 17;
                            }
                        }
                        if (!d.tp) {
                            d.tp = $(d.target).offset().top; //获取容器的top 位置
                        }
                        var gh = __GetHM__(sy, y, d.tp); //根据位置计算新预约的时间段                       
                        var ny = __GetPosition__(gh.sh, gh.sm);
                        var tempdata;
                        if (!d.cpwrap) {
                            tempdata = __BuildTempItem__({}, gh.h);
                            var cpwrap = $("<div class='ca-evpi drag-chip-wrapper' style='top:" + ny + "px'/>").html(tempdata);
                            $(d.target).find("div.itemOverLayerWrapper").append(cpwrap);
                            d.cpwrap = cpwrap;
                        }
                        else {
                            if (d.cgh.sh != gh.sh || d.cgh.eh != gh.eh || d.cgh.sm != gh.sm || d.cgh.em != gh.em) {
                                tempdata = __BuildTempItem__({}, gh.h);
                                d.cpwrap.css("top", ny + "px").html(tempdata);
                            }
                        }
                        d.cgh = gh;
                    }
                    break;
            }
        }
    }
    //移动结束
    function __DragEnd__() {
        if (!__DRAGDATA__) {
            return;
        }
        var d = __DRAGDATA__;
        switch (d.type) {
            case 1: //移动位置    

                break;
            case 2: //resize; 原 type 5               
                break;
            case 3: //新建新预约               
                var wrapid = new Date().getTime();
                if (!d.tp) {
                    d.tp = $(d.target).offset().top; //获取容器的top 位置
                }
                var colid = $(d.target).attr("abbr");
                var tempdata;
                if (!d.cpwrap) {
                    var gh = __GetHM__(d.sy, d.sy + 70, d.tp); //根据位置计算新预约的时间段                       
                    var ny = __GetPosition__(gh.sh, gh.sm);
                    tempdata = __BuildTempItem__({}, gh.h);
                    var cpwrap = $("<div class='ca-evpi drag-chip-wrapper' style='top:" + ny + "px'/>").html(tempdata);
                    $(d.target).find("div.itemOverLayerWrapper").append(cpwrap);
                    d.cpwrap = cpwrap;
                    d.cgh = gh
                }


                if (d.options && d.options.onDragAdd)//调用配置的新增界面
                {
                    d.options.onDragAdd({
                        OrderDate: d.options.showDay.Format("yyyy-MM-dd"),
                        OST: __pZero__(d.cgh.sh) + ":" + __pZero__(d.cgh.sm),
                        OET: __pZero__(d.cgh.eh) + ":" + __pZero__(d.cgh.em),
                        ColId: colid
                    });
                }
                d.cpwrap.remove();
                break;
        }
        d = __DRAGDATA__ = null;
    }
    //拖拽 结束

    function __CHIPTOOLClICK__(type, options) {
        if (!options.onChipToolClick) {
            return;
        }
        var item = __GetData__(this);
        options.onChipToolClick.call(this, type, item);
    }
    function __ItemShow__(options) {
        if (!options.onItemShow) {
            return;
        }
        var item = __GetData__(this);
        options.onItemShow.call(this, item);
    }
    function __GetData__(element) {
        var hdData = $(element).find("div.dhdV").text();
        var arrHdData = hdData.split("&");
        var item = {};
        for (var i = arrHdData.length - 1; i >= 0; i--) {
            var ap = arrHdData[i].split("=");
            if (ap.length == 2) {
                item[ap[0]] = ap[1];
            }
        }
        return item;
    }
    function _GetPHM_(ts1, ts2) {
        var t1 = ts1 / 72;
        var t2 = parseInt(t1);
        var t3 = __GetQuarterTime__(t1, t2);
        var t4 = ts2 / 72;
        var t5 = parseInt(t4);
        var t6 = __GetQuarterTime__(t4, t5);
        return { sh: t2 + __STARTHOUR__, sm: t3, eh: t5 + __STARTHOUR__, em: t6, h: ts2 - ts1 };
    }
    function __GetHM__(y1, y2, pt) {
        var sy1 = Math.min(y1, y2);
        var sy2 = Math.max(y1, y2);  // 鼠标的位置 
        var t1 = (sy1 - pt) / 72;  //计算当前的小时
        var t2 = parseInt(t1);
        var t3 = __GetQuarterTime__(t1, t2);
        var t4 = (sy2 - pt) / 72; //计算当前的小时
        var t5 = parseInt(t4);
        var t6 = __GetQuarterTime__(t4, t5);

        var ret = { sh: t2 + __STARTHOUR__, sm: t3, eh: t5 + __STARTHOUR__, em: t6 };
        ret.h = __GetPosition__(ret.eh, ret.em) - __GetPosition__(ret.sh, ret.sm);
        return ret;
    }
    function __GetQuarterTime__(t1, t2) {
        var diff = t1 - t2;
        if (diff > 0 && diff < 0.25) {
            return 0;
        }
        else if (diff >= 0.25 && diff < 0.5) {
            return 15;
        }
        else if (diff >= 0.5 && diff < 0.75) {
            return 30;
        }
        else {
            return 45;
        }
    }
    function __GetPosition__(h, m) {
        h = h - __STARTHOUR__;
        return h * 72 + parseInt(m / 60 * 72) - 2;
    }
    function __TP__(temp, dataarry) {
        return temp.replace(/\$\{([\w]+)\}/g, function (s1, s2) { var s = dataarry[s2]; if (typeof (s) != "undefined") { return s; } else { return s1; } });
    }

    // 工具类结束

    function __ReturnFalse__() { return false; }
    function __RenderMainTable__(start, end, colModels, items, html, options) {
        html.push("<tr>");
        for (var j = 0, l = colModels.length; j < l; j++) {
            html.push("<td style='width:", 80, "px'><div class=\"data-main-top\"></div></td>");
        };
        html.push("</tr>");
        html.push("<tr>");

        for (var j = 0, l = colModels.length; j < l; j++) {
            html.push("<td style=\"border:none;\">");

            if (colModels[j].shm != "" && colModels[j].ehm != "") {
                var arrshm = colModels[j].shm.split(":");
                var arrehm = colModels[j].ehm.split(":");
                var sh = parseInt(arrshm[0]);
                var sm = parseInt(arrshm[1]);
                var eh = parseInt(arrehm[0]);
                var em = parseInt(arrehm[1]);
                if (sh < start) {
                    sh = start;
                    sm = 0;
                }
                if (eh > end) {
                    eh = end;
                }
                var sP = __GetPosition__(sh, sm);
                var eP = __GetPosition__(eh, em);
                var h = eP - sP;
                //html.push("<div class=\"tgItemBG\" style=\"top:", sP+3, "px;height:", h, "px;\"></div>");
            }

            html.push("<div id=\"tgspanningwrapper\" class=\"tg-spanningwrapper\"><div class=\"tg-hourmarkers\">");
            for (var i = 0; i < (end - start + 1) * 2; i++) {
                html.push("<div class=\"tg-dualmarker\" style='font-size:17px'></div>");

            }
            html.push("</div></div>");

            html.push("</td>");
        }
        html.push("</tr>");

        var groupItems = [];

        //返回所有全天日程的个数,拆分日程分成全天跨天日程和当天日程;          
        __Prepare__(colModels, items, groupItems);


        //console.log("p1:" + mHg);
        var dataheight = (end - start + 1) * 68 - 2;
        html.push("<tr>");
        for (var j = 0, l = colModels.length; j < l; j++) {
            html.push("<td class='tg-col' abbr='", colModels[j].id, "' style=\"height:", dataheight, "px\"><div id='tgCol_", colModels[j].id, "' class=\"itemWrapper\" style=\"height:", dataheight, "px;margin-bottom:-", dataheight, "px\">");
            if (groupItems[j].length > 0) {
                __RenderItem__(groupItems[j], html, options.theme);
            }
            html.push("</div>");

            html.push("<div id='tgOver_", colModels[j].id, "' class=\"itemOverLayerWrapper\">");
            if (colModels[j].timeline) {
                var d = new Date();
                var dh = d.getHours();
                if (dh > start && dh < end) {
                    var mHg = __GetPosition__(d.getHours(), d.getMinutes());
                    var mhh = mHg + 0;
                    html.push("<div id=\"tgnowmarker\" class=\"tg-hourmarker tg-nowmarker\" style=\"left:0px;top:", mhh, "px\"></div>");
                }

            }
            html.push("</div></td>");
        };
        html.push("</tr>");

        //html.push("<tr>");
        //for (var j = 0, l = colModels.length; j < l; j++) {
        //    html.push("<td><div class=\"data-main-top\">", colModels[j].display, "</div></td>");
        //};
        //html.push("</tr>");
    }
    function __RenderItem__(items, html, theme) {
        for (var i = 0; i < items.length; i++) {
            var c;
            if (typeof (items[i].item.Theme) != undefined && items[i].item.Theme >= 0) {
                c = __ThemeControl__(items[i].item.Theme); //自带主题
            }
            else {
                c = __ThemeControl__(theme); //默认主题
            }
            var tt = __BuildItem__(c, items[i], i);
            html.push(tt);
        }
    }

    function __GetTip__(item) {
        // $.browser.mozilla
        return ""; //先没有title
        var br = "\r\n";
        var ret = [];
        ret.push(item.item.UserName, "-", item.item.MobilePhone, br);
        ret.push(item.item.ItemTypeName, br);
        ret.push(item.item.MasseurName, br);
        ret.push(item.item.BedName, br);
        return ret.join("");
    }
    function __BuildData__(item) {
        var data = [];
        for (var a in item) {
            if (a == "OrderStartTime") {
                data.push("OrderDate=" + item[a].Format("yyyy-MM-dd"));
                data.push("OST=" + item[a].Format("hh:mm"));
            }
            else if (a == "OrderEndTime") {
                data.push("OET=" + item[a].Format("hh:mm"));
            }
            else {
                data.push(a + "=" + item[a]);
            }

        }
        return data.join("&");
    }
    function __TipShowRemark__(remark) {
        if (remark) {
            if (remark.length > 7) {
                return remark.substr(0, 7) + "...";
            }
            else {
                return remark;
            }
        }
        return "";
    }
    function __BuildItem__(theme, item, index) {

        var p = { bdcolor: theme[0], bgcolor2: theme[0], bgcolor1: theme[2] };

        var content = [];
        //content.push("<div>预约项目:", item.item.ItemName, "</div><div>预约床位:", item.item.BedName, "</div><div>", item.item.Source, "</div>");
        //if (item.item.Remark) {
        //    content.push("<div>备注:", __TipShowRemark__(item.item.Remark), "</div>");
        //}

        //content.push("<div>", item.item.BedName, "</div>");
        content.push("<div>", item.item.UserFullName, "</div>");
        if (item.item.CusReqMasseur) {
            content.push("<div>指定技师</div>");
        }
        p.content = content.join(""); //


        p.title = "<a href='javascript:void(0);' title='点击切换的客户360视图' class='chip-user'>" + item.item.UserFullName + "</a>"; // "人名";
        p.tip = __GetTip__(item);//"提示信息";
        p.drag = ""; //drag判断是否有权限
        p.redisplay = "none";
        p.data = __BuildData__(item.item);
        var icons = [];
        if (item.item.UserCardType == 1) {
            icons.push("<i class='icon-user' style='margin-right:3px'></i>");
        }
        p.icon = icons.join("");

        var tools = [];
        //0 新登记   1 已通知  2 已开单  3 已结账    
        switch (item.item.Status) {
            case 0:
                break;
        }
        if (item.item.Status == 1 || item.item.Status == 0) {
            tools.push("<a title='开单' abbr='2' href='javascript:void(0)'><i class='icon-shopping-cart'></i><span style='margin-left:1px'>开</span></a>");
            if (item.item.Status == 0) {
                tools.push("<a title='电话确认' abbr='4' href='javascript:void(0)'><i class='icon-bullhorn' ></i></a>");
            }

            tools.push("<a title='修改预约'  abbr='5'  href='javascript:void(0)'><i class='icon-edit' ></i></a>");
            tools.push("<a title='取消预约'  abbr='6' href='javascript:void(0)'><i class='icon-trash' ></i></a>");
        }
        else if (item.item.Status == 2 || item.item.Status == 3) {
            if (item.item.Status == 2) {
                tools.push("<a title='修改预约'  abbr='5'  href='javascript:void(0)'><i class='icon-edit' ></i></a>");
            }
            tools.push("<a title='查看业务单' abbr='3' href='javascript:void(0)'><i class='icon-list-alt'></i><span style='margin-left:1px'>单</span></a>");
        }


        p.content2 = tools.join("");//item.item.MobilePhone; //"电话号码";
        var sP = __GetPosition__(item.st.hour, item.st.minute);
        var eP = __GetPosition__(item.et.hour, item.et.minute);
        p.top = sP + "px";
        p.left = (item.left * 100) + "%";
        p.width = (item.aQ * 100) + "%";
        p.height = (eP - sP - 4);
        p.i = index;

        var newtemp = __TP__(__ITEMTTEMP__, p);
        p = null;
        return newtemp;
    }
    function __BuildTempItem__(data, h) {
        var theme = __ThemeControl__(14); //TODO； 如果获取默认主题？
        var newtemp = __TP__(__ITEMTTEMP__, {
            bdcolor: theme[0],
            bgcolor2: theme[0],
            bgcolor1: theme[2],
            data: "",
            tip: "",
            content: "",
            content2: "<待选>",
            title: "客户",
            icon: "",
            top: "0",
            left: "0",
            width: "100%",
            height: h - 4,
            i: "-1",
            drag: "drag-chip",
            redisplay: "none"
        });
        return newtemp;
    }
    function __ThemeControl__(d) {
        function zc(c, i) {
            var d = "666666888888aaaaaabbbbbbdddddda32929cc3333d96666e69999f0c2c2b1365fdd4477e67399eea2bbf5c7d67a367a994499b373b3cca2cce1c7e15229a36633cc8c66d9b399e6d1c2f029527a336699668cb399b3ccc2d1e12952a33366cc668cd999b3e6c2d1f01b887a22aa9959bfb391d5ccbde6e128754e32926265ad8999c9b1c2dfd00d78131096184cb05288cb8cb8e0ba52880066aa008cbf40b3d580d1e6b388880eaaaa11bfbf4dd5d588e6e6b8ab8b00d6ae00e0c240ebd780f3e7b3be6d00ee8800f2a640f7c480fadcb3b1440edd5511e6804deeaa88f5ccb8865a5aa87070be9494d4b8b8e5d4d47057708c6d8ca992a9c6b6c6ddd3dd4e5d6c6274878997a5b1bac3d0d6db5a69867083a894a2beb8c1d4d4dae54a716c5c8d8785aaa5aec6c3cedddb6e6e41898951a7a77dc4c4a8dcdccb8d6f47b08b59c4a883d8c5ace7dcce";
            return "#" + d.substring(c * 30 + i * 6, c * 30 + (i + 1) * 6);
        }
        return [zc(d, 0), zc(d, 1), zc(d, 2), zc(d, 3)];
    }
    function __pZero__(n) {
        return n < 10 ? "0" + n : "" + n;
    }
    function __Prepare__(colModels, items, gItems) {
        //debugger;     
        var formatItems = [];
        var cmLength = colModels.length;
        var ilength = items.length;
        for (var i = 0; i < ilength ; i++) {
            if (typeof (items[i].OrderStartTime) == "string") {
                items[i].OrderStartTime = __DateFromJsonFormat__(items[i].OrderStartTime);
            }
            if (typeof (items[i].OrderEndTime) == "string") {
                items[i].OrderEndTime = __DateFromJsonFormat__(items[i].OrderEndTime);
            }
            var s = {};
            s.key = items[i].colIndex;
            s.item = items[i];
            s.ost = items[i].OrderStartTime;
            s.oet = items[i].OrderEndTime;
            s.st = {};
            s.st.hour = s.ost.getHours();
            s.st.minute = s.ost.getMinutes();
            s.st.p = s.st.hour * 60 + s.st.minute;
            s.et = {};
            s.et.hour = s.oet.getHours();
            s.et.minute = s.oet.getMinutes();
            s.et.p = s.et.hour * 60 + s.et.minute;
            formatItems.push(s);
        }
        for (var i = 0; i < cmLength; i++) {
            var col = colModels[i];
            gItems[i] = [];
            for (var j = 0; j < formatItems.length; j++) {
                if (formatItems[j].key == col.id) { // 和当前列匹配 
                    gItems[i].push(formatItems[j]);
                }
            }
        }
        // 计算 元素的显示位置
        for (var i = 0; i < cmLength; i++) {
            var de = gItems[i];
            if (de.length > 0) { //存在数据
                var x = []; //数组1
                var y = []; // 数组2
                var D = [];
                var dl = de.length;
                var Ia;
                for (var j = 0; j < dl; ++j) {
                    var ge = de[j];
                    for (var La = ge.st.p, Ia = 0; y[Ia] > La;) Ia++;
                    ge.PO = Ia; ge.ne = []; //PO是指前面有多少个日程
                    y[Ia] = ge.et.p || 1440;
                    x[Ia] = ge;
                    if (!D[Ia]) {
                        D[Ia] = [];
                    }
                    D[Ia].push(ge);
                    if (Ia != 0) {
                        ge.pe = [x[Ia - 1]]; //前面日程
                        x[Ia - 1].ne.push(ge); //后面日程
                    }
                    for (Ia = Ia + 1; y[Ia] <= La;) Ia++;
                    if (x[Ia]) {
                        var k = x[Ia];
                        ge.ne.push(k);
                        k.pe.push(ge);
                    }
                    ge.width = 1 / (ge.PO + 1);
                    ge.left = 1 - ge.width;
                }
                var k = Array.prototype.concat.apply([], D);
                x = y = D = null;
                var t = k.length;
                for (var y = t; y--;) {
                    var H = 1;
                    var La = 0;
                    var x = k[y];
                    for (var D = x.ne.length; D--;) {
                        var Ia = x.ne[D];
                        La = Math.max(La, Ia.VL);
                        H = Math.min(H, Ia.left)
                    }
                    x.VL = La + 1;
                    x.width = H / (x.PO + 1);
                    x.left = H - x.width;
                }
                for (var y = 0; y < t; y++) {
                    var x = k[y];
                    x.left = 0;
                    if (x.pe) for (var D = x.pe.length; D--;) {
                        var H = x.pe[D];
                        x.left = Math.max(x.left, H.left + H.width);
                    }
                    var p = (1 - x.left) / x.VL;
                    x.width = Math.max(x.width, p);
                    x.aQ = Math.min(1 - x.left, x.width + 0.7 * p); //width的偏移
                }
                de = null;
                gItems[i] = k;
            }
        }
    }

    function __DateFromJsonFormat__(jsonDate) {
        return (new Function("return " + jsonDate.replace(/\/(Date\(([0-9]+)([\+-][0-9]{4})?\))\//gi, 'new Date($2)')))();
    }
    function __RenderHoursTb__(start, end, html) {

        //console.log("p2:" + ny);
        html.push("<tr><td class=\"tg-times\">");
        var d = new Date();
        var h = d.getHours();
        if (h > start && h < end + 1) {
            var ny = __GetPosition__(d.getHours(), d.getMinutes());
            html.push("<div id=\"tgnowptr\" class=\"tg-nowptr\" style=\"left:0px;top:", ny, "px\"></div>");
        }

        for (var i = start; i <= end + 1; i++) {
            html.push("<div style=\"height: 68px;", i == end ? "border-color:#fff;" : "", "\" class=\"tg-time\"><span>", i, "</span><em>30</em></div>");
        }
        html.push("</td></tr>");
    }
    window.xjCalendar = xjCalendar;
})(window, undefined, jQuery);;