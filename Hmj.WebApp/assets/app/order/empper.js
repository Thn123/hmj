; (function (window, undefined, $) {
    $(document).ready(function () {
        //取得门店员工信息
        //查顾客信息
        $.post(options.queryEmployeeBy, null,
              function (data) {
                  if (data.status == 1) {
                      var json = eval(data.data);
                      $(json).each(function () {
                          var li = '<li onclick=\"util.addEmp(this)\" id="li-' + this.ID + '" itemid="' + this.NAME + '"> <div class="content"><div>' + this.ID + '</div><div>' + this.NAME + '</div></li>'
                          $("#ulcontent").append(li);
                      });
                  }
              },
              "json"
             );


        var submiting = false;
        $('#formSave').validator({
            valid: function (form) {
                if (submiting) {
                    return;
                }
                submiting = true;
                FormSubmit(form, function (res) {
                    if (res.status >0) {
                        _showInfoMessage("操作成功！", "success");
                        //跳转列表
                        window.location.href = "/Order/OrderList.do";
                        
                    }
                    else {
                        _showInfoMessage("操作失败！" + res.message, "error");
                    }
                    submiting = false;
                })
            }
        });

       
        $("#tProItems tbody").delegate(".tr", "click", function () {
            var lsrow = $("#lastSelectRow").val();
            if (lsrow != "") {
                $("#tProItems tbody #" + $("#lastSelectRow").val()).css("background-color", "");
            }
            $(this).css("background-color", "#ededed");
            $("#lastSelectRow").val($(this).attr("id"));
        });

        util.addEmp = function (obj) {
            try
            {
                var id = $(obj).attr("id").split('-')[1];
                var name = $(obj).attr("itemid");
                var protrid = $("#lastSelectRow").val();
                if(protrid==""||protrid==null)
                {
                    _showInfoMessage('请您先选择要分配业绩的订单！', 'info');
                    return false;
                }
                var currow = $.trim($("#" + protrid).children("td").eq(2).html());
                var curprice = $("#" + protrid).children("td").eq(1).html().split("￥")[1];
                if (currow == "")
                {
                    var html = [];
                    html.push('<table style="width:100%;">');
                    html.push('<tbody>');
                    html.push('</tbody>');
                    html.push('</table>');
                    $("#" + protrid).children("td").eq(2).html(html.join(''));   
                }

                var curtable = $("#" + protrid).children("td").eq(2).children("table").children("tbody").children("tr");
                var _isaddd = false;
                $(curtable).each(function () {
                    var empid = $(this).children("td").eq(2).find("input[NAME$=empid]").val();
                    if (empid == id)
                    {
                        _isaddd = true;
                    }
                });
                if (_isaddd)
                {
                    _showInfoMessage('该项目已分配员工：' + name, 'info');
                    return false;
                }
                //alert(curtable);

                //员工&业绩
                var objtbody = $("#" + protrid).children("td").eq(2).children("table").children("tbody");
                var objtr = $("#" + protrid).children("td").eq(2).children("table").children("tbody").children("tr");
                if (objtr.html() == undefined) {
                    var empHtml = [];
                    empHtml.push("<tr>");
                    empHtml.push("<td style='width:60px;'>" + name + "</td>");
                    empHtml.push("<td>￥<input onkeypress=\"util.keyPress(this)\" onkeyup=\"util.keyUp(this)\" onblur=\"util.onBlur(this)\" readonly='true' name='performance' class='text inputfirst' maxlength='5'  type='text' value='" + curprice + "' /></td>");
                    empHtml.push("<td><a onclick=\"util.DeleteEmp(this)\">删除</a><input type='hidden' name='proid' value="+protrid+"><input type='hidden' name='empid' value="+id+"></td>");
                    empHtml.push("</tr>");
                    $(empHtml.join('')).appendTo(objtbody);
                }
                else {
                    var price = f.floatDiv(parseFloat(curprice), parseFloat(objtr.length + 1));
                    if (price.toString().indexOf(".") >= 0)
                    {
                        price = price.toFixed(1);
                    }    
                    var empHtml = [];
                    empHtml.push("<tr>");
                    empHtml.push("<td style='width:60px;'>" + name + "</td>");
                    empHtml.push("<td>￥<input onkeypress=\"util.keyPress(this)\" onkeyup=\"util.keyUp(this)\" onblur=\"util.onBlur(this)\" name='performance' class='text' maxlength='5'  type='text' value='" + price + "' /></td>");
                    empHtml.push("<td><a onclick=\"util.DeleteEmp(this)\">删除</a><input type='hidden' name='proid' value=" + protrid + "><input type='hidden' name='empid' value=" + id + "></td>");
                    empHtml.push("</tr>");
                    $(empHtml.join('')).appendTo(objtbody);

                    $(objtr).each(function (i) {
                        if (i > 0) {
                            $(this).children("td").eq(1).find("input[NAME$=performance]").val(price);
                        }
                    });

                    var firstprice = f.floatSubtract(parseFloat(curprice), f.floatMulti(parseFloat(price), parseFloat(objtr.length)).toFixed(1));
                    $(objtr).children("td").eq(1).find("input[NAME$=performance]").val(firstprice);
                }
            }
            catch(ex)
            {
                alert("数据有误："+ex);
            }
        };

        util.DeleteEmp = function (obj) {
            //恢复价格
            var objtr = $(obj).parent("td").parent("tr").parent("tbody").children("tr");
            var curprice = $(obj).parent("td").parent("tr").parent("tbody").parent("table").parent("td").parent("tr").children("td").eq(1).html().split("￥")[1];
            var price = f.floatDiv(parseFloat(curprice), parseFloat(objtr.length-1));
            if (price.toString().indexOf(".") >= 0) {
                price = price.toFixed(1);
            }
            $(objtr).each(function (i) {
                if (i > 0) {
                    $(this).children("td").eq(1).find("input[NAME$=performance]").val(price);
                }
            });
            var firstprice = f.floatSubtract(parseFloat(curprice), f.floatMulti(parseFloat(price), parseFloat(objtr.length-2)).toFixed(1));
            $(objtr).children("td").eq(1).find("input[NAME$=performance]").val(firstprice);
            $(obj).parent("td").parent("tr").remove();
            var curname=$(obj).parent("td").parent("tr").children("td").eq(0).html();
            var firname = $(objtr).children("td").eq(0).html();
            if (curname == firname)
            {
                $(objtr).children("td").parent("tr").eq(0).children("td").eq(1).find("input[NAME$=performance]").val(firstprice);
            }
        };

        util.keyUp = function (ob) {
            try
            {
                if (!ob.value.match(/^[\+\-]?\d*?\.?\d*?$/))
                    ob.value = ob.t_value;
                else ob.t_value = ob.value;
                if (ob.value.match(/^(?:[\+\-]?\d+(?:\.\d+)?)?$/))
                    ob.o_value = ob.value;

                var curprice = $(ob).parent("td").parent("tr").parent("tbody").parent("table").parent("td").parent("tr").children("td").eq(1).html().split("￥")[1];
                var pricetr = $(ob).parent("td").parent("tr").parent("tbody").children("tr");
                var prices = 0;
                $(pricetr).each(function (i) {
                    if (i > 0) {
                        var price = $(this).children("td").eq(1).find("input[NAME$=performance]").val();
                        if (price != "" && price != null && price != undefined) {
                            prices += parseFloat(price);
                        }
                    }
                });
           
                var pricerow = f.floatSubtract(parseFloat(curprice), parseFloat(prices));
                if (parseFloat(pricerow) < 0)
                {
                    _showInfoMessage('员工业绩总和不能大于订单总额！', 'info');
                    ob.value = "";
                    var p = 0;
                    $(pricetr).each(function (i) {
                        if (i > 0) {
                            var pc = $(this).children("td").eq(1).find("input[NAME$=performance]").val();
                            if (pc != "" && pc != null && pc != undefined) {
                                p += parseFloat(pc);
                            }
                        }
                    });
                    var pcrow = f.floatSubtract(parseFloat(curprice), parseFloat(p));
                    $(ob).parent("td").parent("tr").parent("tbody").children("tr").children("td").eq(1).find("input[NAME$=performance]").val(pcrow);
                    return false;
                }
                $(ob).parent("td").parent("tr").parent("tbody").children("tr").children("td").eq(1).find("input[NAME$=performance]").val(pricerow);
            }
            catch (ex)
            {
                alert("数据有误！"+ex);
            }
        };
        util.keyPress = function (ob) {
            if (!ob.value.match(/^[\+\-]?\d*?\.?\d*?$/))
                ob.value = ob.t_value;
            else ob.t_value = ob.value;
            if (ob.value.match(/^(?:[\+\-]?\d+(?:\.\d+)?)?$/))
                ob.o_value = ob.value;
        };
        util.onBlur = function (ob) {
            if (!ob.value.match(/^(?:[\+\-]?\d+(?:\.\d+)?|\.\d*?)?$/))
                ob.value = ob.o_value;
            else {
                if (ob.value.match(/^\.\d+$/))
                    ob.value = 0 + ob.value;
                if (ob.value.match(/^\.$/))
                    ob.value = 0; ob.o_value = ob.value
            };
        };
        //*********加减乘除***********//
        var f = new Object();
        //获取参数精度，如果为整数则精度为0  
        f._getPrecision = function (arg) {
            if (arg.toString().indexOf(".") == -1) {
                return 0;
            } else {
                return arg.toString().split(".")[1].length;
            }
        }
        //获取小数的整数形式  
        f._getIntFromFloat = function (arg) {
            if (arg.toString().indexOf(".") == -1) {
                return arg;
            } else {
                return Number(arg.toString().replace(".", ""));
            }
        }
        //乘法  
        f.floatMulti = function (arg1, arg2) {
            var precision1 = this._getPrecision(arg1);
            var precision2 = this._getPrecision(arg2);
            var tempPrecision = 0;

            tempPrecision += precision1;
            tempPrecision += precision2;
            var int1 = this._getIntFromFloat(arg1);
            var int2 = this._getIntFromFloat(arg2);
            return (int1 * int2) * Math.pow(10, -tempPrecision);
        }
        //加法  
        f.floatAdd = function (arg1, arg2) {
            var precision1 = this._getPrecision(arg1);
            var precision2 = this._getPrecision(arg2);
            var temp = Math.pow(10, Math.max(precision1, precision2));
            return (this.floatMulti(arg1, temp) + this.floatMulti(arg2, temp)) / temp;
        }
        //减法  
        //arg1 被减数  
        //arg2 减数  
        f.floatSubtract = function (arg1, arg2) {
            var precision1 = this._getPrecision(arg1);
            var precision2 = this._getPrecision(arg2);
            var temp = Math.pow(10, Math.max(precision1, precision2));
            return (this.floatMulti(arg1, temp) - this.floatMulti(arg2, temp)) / temp;
        }
        ////除法  
        //arg1 被除数  
        //arg2 除数  
        f.floatDiv = function (arg1, arg2) {
            var precision1 = this._getPrecision(arg1);
            var precision2 = this._getPrecision(arg2);
            var int1 = this._getIntFromFloat(arg1);
            var int2 = this._getIntFromFloat(arg2);
            var result = (int1 / int2) * Math.pow(10, precision2 - precision1);
            return result;
        }
        String.prototype.replaceAll = function (s1, s2) {
            return this.replace(new RegExp(s1, "gm"), s2);
        }

        //********************//

        $("#btnSave").click(function () {
            if (!confirm("您确认要保存该业绩分配吗？"))
            {
                return false;
            }
            $("#formSave").submit();
        });

        $("#btnReturn").click(function () {
            window.location.href = "/Order/OrderList.do";
        });
    });
})(window, undefined, jQuery);