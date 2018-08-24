; (function (window, undefined, $) {
    $(document).ready(function () {

        var submiting = false;
        $('#formSave').validator({
            valid: function (form) {
                if (submiting) {
                    return;
                }
                submiting = true;
                FormSubmit(form, function (res) {
                    if (res.status > 0) {
                        _showInfoMessage("操作成功！", "success");
                        //
                        window.location.href = "/Customer/View360.do?cid=" + $("#hideCUST_ID").val();
                       
                    }
                    else {
                        _showInfoMessage("操作失败！" + res.message, "error");
                    }
                    submiting = false;
                })
            }
        });

        $('#BEGIN_DATE').datepicker({
            language: "zh-CN",
            format: "yyyy-mm-dd",
            autoclose: true
        }).on('changeDate', function (ev) {
            
        });
        $('#END_DATE').datepicker({
            language: "zh-CN",
            format: "yyyy-mm-dd",
            autoclose: true
        });


        $("#CARD_ID").change(function () {
            var id = $("#CARD_ID").val();
            if(id==""||id==null)
            {
                _showInfoMessage('请选择会员卡级！', 'info');
                return false;
            }
            var prodName =$("#CARD_ID").find("option:selected").text();
            $("#hidePROD_NAME").val(prodName);
            $.post("/Card/Detail.do", { id: id },
                    function (ret) {
                        if (ret.status > 0) {
                            var data = eval(ret.data);
                            $("#BUY_AMT").val(data.PRICE);
                            $("#YH_AMT").val(parseFloat(data.PAR_AMT) - parseFloat(data.PRICE));
                            $("#CASH_AMT").val(data.PRICE);
                            var mon = data.PERIOD;
                            var bdate = new Date();
                            bdate.setMonth(bdate.getMonth() + mon);
                            var edate =bdate.getFullYear() + "-" + (bdate.getMonth()) + "-" + bdate.getDate();
                            $("#END_DATE").val(edate);
                        }
                    },
                    "json"
                   );
        });
        //充值购卡
        $("#btnSave").click(function () {
            var vipcard = $("#CARD_ID").val();
            if (vipcard == "" || vipcard == null) {
                _showInfoMessage("请您先选择会员卡级！", "info");
                return false;
            }
            //充值购卡
            $("#hide_Type").val("buyCard");
            $("#formSave").submit();
        });
        //购买疗程卡
        $("#btnSaveOrder").click(function () {

            //购买疗程卡
            $("#hide_Type").val("buyLCC");
            //疗程名字
            var lCCARD_NAME = $("#LCCARD_PACK").find("option:selected").text();
            $("#hideLCCARD_PACK").val(lCCARD_NAME);
            var totalAmt = $("#spanTotalAmt").html();
            $("#hidePayAmt").val(totalAmt);
            $("#formSave").submit();
        });
        $("#btnReturn").click(function () {
            window.location.href = "/Customer/View360.do?cid=" + $("#hideCUST_ID").val();
        });

        $("#LCCARD_PACK").change(function () {
            showLCCPack();
        });
        function showLCCPack()
        {
            var lccpackId = $("#LCCARD_PACK").val();//疗程卡套餐
            if (lccpackId != null && lccpackId != "") {
                showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 700, top: 450 });
                $.post("/Order/GetLCCPack.do", { lid: lccpackId },
                       function (ret) {
                           if (ret.status > 0) {
                               var data = eval(ret.data);
                               //疗程总价
                               $("#spanTotalAmt").html(data[0].DISCOUNT_PRICE);
                               $("#spanBalanceAmt").html(data[0].DISCOUNT_PRICE);
                               $("#tSpeedyPro tbody").html("");
                               var proListHtml = [];
                               $(data).each(function (i) {
                                   if (i > 0) {
                                       proListHtml.push("<tr class='strip'>");
                                       //proListHtml.push("<td style=\"width: 20%;\"><div style=\"text-align:left;\">", this.ID, "</div></td>");
                                       proListHtml.push("<td style=\"width: 55%;\"><div style=\"text-align:left;\">", this.SERVICE_NAME, "</div></td>");
                                       proListHtml.push("<td style=\"width: 10%;\"><div style=\"text-align:left;\">", this.UNIT_PRICE, "</div></td>");
                                       proListHtml.push("<td style=\"width: 10%;\"><div style=\"text-align:left;\">", this.SERVICE_QTY, "</div></td>");
                                       proListHtml.push("<td style=\"width: 10%;\"><div style=\"text-align:left;\">", this.AMT_PRICE, "</div></td>");
                                       proListHtml.push("<td style=\"width: 15%;\"><div style=\"text-align:left;\">");

                                       if (this.IS_REQUIRED) {
                                           proListHtml.push("<a title='移除' class='abtn' href='javascript:;' onclick=\"util.Remove(this,'", this.ID, "','", this.AMT_PRICE, "')\"><i class='fa fa-trash-o' ></i> 移除</a>");
                                       }
                                       else {
                                           proListHtml.push("<span title='必选疗程项目'>&nbsp;必 选</span>");
                                       }
                                       proListHtml.push("</div></td>");
                                       proListHtml.push("<td>");
                                       proListHtml.push("<input type='hidden' name='proId' value=" + this.ID + " />");
                                       proListHtml.push("<input type='hidden' name='proName' value='"+this.SERVICE_NAME+"' />");
                                       proListHtml.push("<input type='hidden' name='proPrice' value=" + this.UNIT_PRICE + " />");//单价
                                       proListHtml.push("<input type='hidden' name='proQty' value=" + this.SERVICE_QTY + " />");//数量
                                       proListHtml.push("<input type='hidden' name='payAmt' value=" + this.AMT_PRICE + " />");//应付小计金额
                                       proListHtml.push("</td>");
                                       proListHtml.push("</tr>");
                                   }
                               });
                               $(proListHtml.join('')).appendTo("#tSpeedyPro tbody");
                               hideLoadingMsg();
                           }
                       },
                       "json"
                      );
            }
            else {
                $("#tSpeedyPro tbody").html("");
                //疗程总价
                $("#spanTotalAmt").html(0);
                $("#spanBalanceAmt").html(0);
            }
            
        }
        util.Remove = function (obj,id,price)
        {
            var totalAmt = parseFloat($("#spanTotalAmt").html());
            totalAmt = totalAmt - parseFloat(price);
            $("#spanTotalAmt").html(totalAmt);
            $("#spanBalanceAmt").html(totalAmt);
           $(obj).parent("div").parent("td").parent("tr").remove();
        }


        util.keyPress = function (ob) {
            if (!ob.value.match(/^[\+\-]?\d*?\.?\d*?$/))
                ob.value = ob.t_value;
            else ob.t_value = ob.value;
            if (ob.value.match(/^(?:[\+\-]?\d+(?:\.\d+)?)?$/))
                ob.o_value = ob.value;
        }
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
        util.keyUpCash = function (ob) {
            if (!ob.value.match(/^[\+\-]?\d*?\.?\d*?$/))
                ob.value = ob.t_value;
            else ob.t_value = ob.value;
            if (ob.value.match(/^(?:[\+\-]?\d+(?:\.\d+)?)?$/))
                ob.o_value = ob.value;
            try{
                var but_amt = $("#BUY_AMT").val();
                var vipcard = $("#CARD_ID").val();
                if (vipcard == "" || vipcard == null)
                {
                    _showInfoMessage("请您先选择会员卡级！", "info");
                    ob.value = 0;
                    return false;
                }
                var paycard_type = $("#PAYCARD_TYPE").val();
                if (paycard_type == "" || paycard_type == null)
                {
                    _showInfoMessage("请您先选择支付方式！", "info");
                    ob.value = 0;
                    return false;
                }
                if (parseFloat(ob.value) > parseFloat(but_amt))
                {
                    _showInfoMessage("卡付金额不能大于销售金额！", "info");
                    ob.value = but_amt;
                    $("#CASH_AMT").val(0);
                    return false;
                }
                var cash_amt = parseFloat(but_amt) - parseFloat(ob.value);
                if (!isNaN(cash_amt)) {
                    $("#CASH_AMT").val(cash_amt);
                }

            }
            catch(ex)
            {
                _showInfoMessage("金额填写有误！" + ex.message, "error");
            }
        };



        //////********购买疗程**********//////
        util.keyUpCardPay = function (ob) {
            if (!ob.value.match(/^[\+\-]?\d*?\.?\d*?$/))
                ob.value = ob.t_value;
            else ob.t_value = ob.value;
            if (ob.value.match(/^(?:[\+\-]?\d+(?:\.\d+)?)?$/))
                ob.o_value = ob.value;
            try {
                //var but_amt = $("#BUY_AMT").val();
                //var vipcard = $("#CARD_ID").val();
                //if (vipcard == "" || vipcard == null) {
                //    _showInfoMessage("请您先选择会员卡级！", "info");
                //    ob.value = 0;
                //    return false;
                //}
                //var paycard_type = $("#PAYCARD_TYPE").val();
                //if (paycard_type == "" || paycard_type == null) {
                //    _showInfoMessage("请您先选择支付方式！", "info");
                //    ob.value = 0;
                //    return false;
                //}
                //if (parseFloat(ob.value) > parseFloat(but_amt)) {
                //    _showInfoMessage("卡付金额不能大于销售金额！", "info");
                //    ob.value = but_amt;
                //    $("#CASH_AMT").val(0);
                //    return false;
                //}
                //var cash_amt = parseFloat(but_amt) - parseFloat(ob.value);
                //if (!isNaN(cash_amt)) {
                //    $("#CASH_AMT").val(cash_amt);
                //}

                //应付总额
                var totalAmt = parseFloat($("#spanBalanceAmt").html());
                //会员卡支付金额
                var payedMoney = parseFloat(ob.value);
                if (payedMoney > totalAmt)
                {
                    ob.value = totalAmt;
                }


            }
            catch (ex) {
                _showInfoMessage("金额填写有误！" + ex.message, "error");
            }
        };

        util.keyUpCashLCC = function (ob) {
            if (!ob.value.match(/^[\+\-]?\d*?\.?\d*?$/))
                ob.value = ob.t_value;
            else ob.t_value = ob.value;
            if (ob.value.match(/^(?:[\+\-]?\d+(?:\.\d+)?)?$/))
                ob.o_value = ob.value;
            try {
               

            }
            catch (ex) {
                _showInfoMessage("金额填写有误！" + ex.message, "error");
            }
        };
        util.delPay = function delPay(row, price) {
            //删除支付方式 
            $(row).parent("td").parent("tr").remove();

            price = f.floatAdd(parseFloat($("#spanBalanceAmt").html()), parseFloat(price));
            $("#spanBalanceAmt").html(price);
            if (price > 0) {
                $("#btnSubmit").hide();
                $("#btnContPay").show();
                $("#divResAmount").show();


                $("#divPayType").show();
                $("#divPayTypeContent").show();
            }
            else {


                $("#divResAmount").hide();
                $("#btnContPay").hide();
                $("#btnSubmit").show();
            }
        };
        //****************添加付款--储值卡
        $("#btnVIPCARD").click(function () {
            if ($("#VIPCARD_TYPE").val() == "" || $("#VIPCARD_TYPE").val() == undefined) {
                _showInfoMessage('请您先选择会员卡！', 'info');
                return false;
            }
            var payed = $("#payedMoneyVIPCARD").val();
            if (payed == "" || payed == null)
            {
                _showInfoMessage('请您输入卡付金额！', 'info');
                return false;
            }
            //卡内余额
            var cardBalance = $("#VIPCARD_TYPE").find("option:selected").text().split('￥')[1];
           
            if (parseFloat(payed)-parseFloat(cardBalance) > 0) {
                _showInfoMessage('卡内余额不足支付！', 'info');
                return false;
            }
            var cashType = $("#VIPCARD_TYPE").val();
            var cashName = $("#VIPCARD_TYPE").find("option:selected").text();
            var cardNames = cashName.toString().split("|");
            util.addPayList("5", "会员卡支付", cashType, cardNames[0], $("#payedMoneyVIPCARD").val(), $("#payedMoneyVIPCARD").val());
            $("#payedMoneyVIPCARD").val("");
            $("#payMoneyVIPCARD").val("");
            return false;


        });
        ////////////
        //****************现金/
        $("#btnCash").click(function () {
            var payMoney = $("#payedMoney").val();
            if (payMoney == "" || payMoney == null) {
                _showInfoMessage('请您输入付款金额！', 'info');
                return false;
            }
            util.addPayList("0", "现金", "0", "现金", $("#payedMoney").val(), $("#payedMoney").val());
            $("#payedMoney").val("");
        });
        //银联POS
        $("#btnPOSTCARD").click(function () {
            if ($("#POSTCARD_TYPE").val() == "") {
                _showInfoMessage('请您先选择银联POS卡！', 'info');
                return false;
            }
            if ($.trim($("#payMoneyPOSTCARD").val()) == "") {
                _showInfoMessage('请您输入付款金额！', 'info');
                return false;
            }
            var cardType = $("#POSTCARD_TYPE").val();
            var cardName = $("#POSTCARD_TYPE").find("option:selected").text();
            util.addPayList("1", "银联POS", cardType, cardName, $("#payMoneyPOSTCARD").val(), $("#payMoneyPOSTCARD").val());
            $("#payMoneyPOSTCARD").val("");
            return false;
        });

        //应收账款
        $("#btnYSZK").click(function () {
            if ($("#RECEIVABLES").val() == "") {
                _showInfoMessage('请您先选择应收账款！', 'info');
                return false;
            }
            if ($.trim($("#payYSZK").val()) == "") {
                _showInfoMessage('请您输入付款金额！', 'info');
                return false;
            }
            var cardType = $("#RECEIVABLES").val();
            var cardName = $("#RECEIVABLES").find("option:selected").text();
            util.addPayList("2", "应收账款", cardType, cardName, $("#payYSZK").val(), $("#payYSZK").val());
            $("#payYSZK").val("");
            return false;
        });
        //校验优惠券信息 
        $("#btnCheckPROM").click(function () {
            var vocher_no = $("#txtVOUCHAR_NO").val();
            if ($.trim(vocher_no) == "") {
                _showInfoMessage('请您先输入优惠券编码！', 'info');
                return false;
            }
            $.post(options.checkVOCHER_NO, { VOCHER_NO: vocher_no },
                 function (ret) {
                     if (ret.status == 1) {
                         var mes = "礼券是由" + ret.data.STORE_NAME + "发放，面值：" + ret.data.FACE_VALUE + "元";
                         $("#Pro_Voucher").html(mes);
                         $("#Pro_FaceValue").val(ret.data.FACE_VALUE);//优惠券面值
                         $("#Pro_No").val(vocher_no);
                         $("#Pro_Id").val(ret.data.ID);

                     }
                     else if (ret.status == 0) {
                         $("#Pro_Voucher").html("未找到该优惠券信息！请核实编码是否正确");
                         $("#Pro_FaceValue").val(0);//优惠券面值
                     }
                     else if (ret.status == -1) {
                         $("#Pro_Voucher").html(ret.message);
                     }
                 },
                 "json"
                );

        });
        ////*****************优惠券
        //$("#btnPROM").click(function () {
        //    if ($.trim($("#txtVOUCHAR_NO").val()) == "") {
        //        _showInfoMessage('请您先输入优惠券编码！', 'info');
        //        return false;
        //    }
        //    if ($.trim($("#Pro_FaceValue").val()) == 0) {
        //        _showInfoMessage('请您先校验优惠券编码！', 'info');
        //        return false;
        //    }

        //    //var cashType = $("#Pro_FaceValue").val();
        //    //var cashName = $("#Pro_FaceValue").val();
        //    var facevalue = $("#Pro_FaceValue").val();
        //    //如果面值大于应付金额
        //    if (parseFloat(facevalue) > parseFloat($("#spanResAmount").html())) {
        //        facevalue = $("#spanResAmount").html();
        //    }
        //    util.addPayList("3", "代金券", $("#Pro_Id").val(), $("#Pro_No").val(), facevalue, facevalue);
        //    $("#txtVOUCHAR_NO").val("");
        //    $("#Pro_Voucher").html("");


        //});
        //支付列表 deduMoney=抵扣金额
        util.addPayList = function (payType, payTypeName, cashType, cashName, payMoney, deduMoney) {
           
            var price = f.floatSubtract(parseFloat($("#spanBalanceAmt").html()), parseFloat(deduMoney));
            //if (price < 0) {
            //    _showInfoMessage("支付总额不能大于还需支付总额！", "error");
            //    return false;
            //}
            $("#spanBalanceAmt").html(price);
            var html = [];
            html.push('<tr>');
            html.push('<td>' + payTypeName + '</td>');
            html.push('<td>' + cashName + '</td>');
            html.push('<td>￥' + util.formatPrice(deduMoney) + '</td>');
            html.push('<td>￥' + util.formatPrice(payMoney) + '</td>');
            html.push('<td><a title="删除" onclick="util.delPay(this,' + deduMoney + ')" class="abtn" href="javascript:;" ><i class="fa fa-times" ></i></a><td>');
            html.push('<td><input type="hidden" name="payType" value=' + payType + ' />');
            html.push('<input type="hidden" name="cashType" value=' + cashType + ' />');
            html.push('<input type="hidden" name="deduMoney" value=' + deduMoney + ' />');//抵扣金额
            html.push('<input type="hidden" name="payMoney" value=' + payMoney + ' /></td>');//实际支付

            html.push('</tr>');
            $(html.join("")).appendTo("#tablePayList tbody");

            var mess = cashName + "支付￥" + payMoney + "。";
            _showInfoMessage(mess, "success");

            if (price > 0) {
                $("#btnSubmit").hide();
                $("#btnContPay").show();
                $("#divResAmount").show();
            }
            else {
                $("#divResAmount").hide();
                $("#btnContPay").hide();
                $("#btnSubmit").show();

                $("#divPayType").hide();
                $("#divPayTypeContent").hide();
            }

            //$("#divPayType").hide();
            //$("#divPayTypeContent").hide();
            //$("#divBtnSave").show();
        };
        //////********结束**************/////
        //********************//
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

        //价格格式化
        util.formatPrice = function (value) {
            value = value.toString().replace(/\$|\,/g, '');
            if (isNaN(value))
                value = "0";
            sign = (value == (value = Math.abs(value)));
            value = Math.floor(value * 100 + 0.50000000001);
            cents = value % 100;
            value = Math.floor(value / 100).toString();
            if (cents < 10)
                cents = "0" + cents;
            for (var i = 0; i < Math.floor((value.length - (1 + i)) / 3) ; i++)
                value = value.substring(0, value.length - (4 * i + 3)) + ',' +
                value.substring(value.length - (4 * i + 3));
            return (((sign) ? '' : '-') + '' + value);
        };

        //********************//


        //*************充值开始**************//
        util.ChargeAMT = function () {
            alert("aaa");
        };

        util.keyChargePay = function (ob) {
            if (!ob.value.match(/^[\+\-]?\d*?\.?\d*?$/))
                ob.value = ob.t_value;
            else ob.t_value = ob.value;
            if (ob.value.match(/^(?:[\+\-]?\d+(?:\.\d+)?)?$/))
                ob.o_value = ob.value;
        };
        //*************充值结束**************//
    });
})(window, undefined, jQuery);
