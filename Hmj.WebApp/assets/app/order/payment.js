; (function (window, undefined, $) {
    $(document).ready(function () {
        var submiting = false;
        $('#formPaySave').validator({
            valid: function (form) {
                if (submiting) {
                    return;
                }
                submiting = true;
                FormSubmit(form, function (res) {
                    if (res.status >0) {
                        _showInfoMessage("操作成功！", "success");
                        var cid = $("#hide_cid").val();
                        var oid = $("#hide_oid").val();
                        window.location.href = "/Order/OrderB.do?cid="+cid+"&oid="+oid;
                    }
                    else {
                        _showInfoMessage("操作失败！" + res.message, "error");
                    }
                    $("#btnSave").attr("disabled", "");
                    submiting = false;
                })
            }
        });

//****************添加付款--储值卡
$("#btnVIPCARD").click(function () {
    if ($("#VIPCARD_TYPE").val() == "" || $("#VIPCARD_TYPE").val()==undefined) {
        _showInfoMessage('请您先选择会员卡！', 'info');
        return false;
    }
    if ($.trim($("#payMoneyVIPCARD").val()) == "") {
        _showInfoMessage('请您输入抵扣金额！', 'info');
        return false;
    }
    if ($.trim($("#payedMoneyVIPCARD").val()) == "") {
        _showInfoMessage('请您输入卡付金额！', 'info');
        return false;
    }
    if ($.trim($("#payedMoneyVIPCARD").val()) <= 0) {
        _showInfoMessage('卡付金额必须大于0！', 'info');
        return false;
    }
    //卡内余额
    var cardBalance = $("#VIPCARD_TYPE").find("option:selected").text().split('￥')[1];
    var payed = $("#payMoneyVIPCARD").val();
    if(f.floatSubtract(parseFloat(payed),parseFloat(cardBalance))>0)
    {
        _showInfoMessage('卡内余额不足支付！', 'info');
        return false;
    }
    var cashType = $("#VIPCARD_TYPE").val();
    var cashName = $("#VIPCARD_TYPE").find("option:selected").text();
    var cardNames = cashName.toString().split("|");
    util.addPayList("5", "会员卡支付", cashType, cardNames[0], $("#payedMoneyVIPCARD").val(), $("#payMoneyVIPCARD").val());
    $("#payedMoneyVIPCARD").val("");
    $("#payMoneyVIPCARD").val("");
    return false;

});
        //****************现金/
$("#btnCash").click(function () {
    var payMoney = $("#payedMoney").val();
    if (payMoney == "" || payMoney == null)
    {
        _showInfoMessage('请您输入付款金额！', 'info');
        return false;
    }
    util.addPayList("0", "现金", "0", "现金", $("#payedMoney").val(), $("#payedMoney").val());
    $("#payedMoney").val("");
});
//刷卡
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
             else if(ret.status==-1) {
                 $("#Pro_Voucher").html(ret.message);
             }
         },
         "json"
        );

});
        //*****************优惠券
$("#btnPROM").click(function () {
    if ($.trim($("#txtVOUCHAR_NO").val())=="")
    {
        _showInfoMessage('请您先输入优惠券编码！', 'info');
        return false;
    }
    if ($.trim($("#Pro_FaceValue").val()) == 0) {
        _showInfoMessage('请您先校验优惠券编码！', 'info');
        return false;
    }

    //var cashType = $("#Pro_FaceValue").val();
    //var cashName = $("#Pro_FaceValue").val();
    var facevalue = $("#Pro_FaceValue").val();
    //如果面值大于应付金额
    if (parseFloat(facevalue) > parseFloat($("#spanResAmount").html())) {
        facevalue = $("#spanResAmount").html();
    }
    util.addPayList("3", "代金券", $("#Pro_Id").val(), $("#Pro_No").val(), facevalue, facevalue);
    $("#txtVOUCHAR_NO").val("");
    $("#Pro_Voucher").html("");


});
        //储值卡选择事件
$("#VIPCARD_TYPE").change(function () {
    var id = $("#VIPCARD_TYPE").val();
    $.post(options.queryCardDetail, { id: id },
        function (data) {
            if (data.status == 1) {
                $("#hide_VIPCARD_RATE").val(data.data.DISCOUNT_RATE);
            }
        },
        "json"
       );
});

        //支付列表 deduMoney=抵扣金额
util.addPayList = function (payType, payTypeName, cashType, cashName, payMoney, deduMoney) {
    var price = f.floatSubtract(parseFloat($("#spanResAmount").html()), parseFloat(deduMoney));
    if (price < 0)
    {
        _showInfoMessage("支付总额不能大于还需支付总额！", "error");
        return false;
    }
    $("#spanResAmount").html(price);
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

util.delPay = function delPay(row, price) {
    //删除支付方式 
    $(row).parent("td").parent("tr").remove();

    price = f.floatAdd(parseFloat($("#spanResAmount").html()), parseFloat(price));
    $("#spanResAmount").html(price);
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
//下单出票
$("#btnSave").click(function () {
    var payway = $("#tablePayList tbody tr").length;
    if (payway <= 0)
    {
        _showInfoMessage("请添加支付！", "info");
        return false;
    }
    var resAmount = parseFloat($("#spanResAmount").html());
    if (resAmount > 0)
    {
        _showInfoMessage("还需支付￥" + resAmount, "info");
        return false;
    }
    $("#btnSave").attr("disabled", "disabled");
    $("#formPaySave").submit();
});
        //返回
$("#btnReturn").click(function () {
    var cid = $("#hide_cid").val();
    var oid = $("#hide_oid").val();
    window.location.href = "/Order/OrderD.do?cid=" + cid + "&oid=" + oid;;
});

util.keyPress = function (ob) {
    if (!ob.value.match(/^[\+\-]?\d*?\.?\d*?$/))
        ob.value = ob.t_value;
    else ob.t_value = ob.value;
    if (ob.value.match(/^(?:[\+\-]?\d+(?:\.\d+)?)?$/))
        ob.o_value = ob.value;
};
//抵扣金额
util.keyUp=function(ob) {
    if ($("#VIPCARD_TYPE").val() == "") {
        _showInfoMessage('请您先选择储值卡！', 'info');
        return false;
    }
    if (!ob.value.match(/^[\+\-]?\d*?\.?\d*?$/))
        ob.value = ob.t_value;
    else ob.t_value = ob.value;
    if (ob.value.match(/^(?:[\+\-]?\d+(?:\.\d+)?)?$/))
        ob.o_value = ob.value;
    //输入的抵扣金额
    var vvalue = "";
    if (ob.value == "" || ob.value == null) {
        vvalue = 0;
    }
    else {
        vvalue = ob.value;
    }

    //alert(ob.value);
    //储值卡扣率
    var vipcardrate = $("#hide_VIPCARD_RATE").val();

    //允许卡支付但不享受折扣金额
    var cardPay = $("#spanCardPay").html();
    //允许卡支付并享受折扣金额
    var cardDistPay = $("#spanCardDistPay").html();

    if (parseFloat(vvalue) > f.floatAdd(parseFloat(cardPay), parseFloat(cardDistPay))) {
        _showInfoMessage('抵扣金额不能大于允许卡付金额！', 'info');
        ob.value = f.floatAdd(parseFloat(cardPay), parseFloat(cardDistPay));
        vvalue = ob.value;
        //return false;
    }

    //实际应付
    var payprice = 0;
    //1.抵扣金额小于享受折扣金额
    if (parseFloat(vvalue) <= parseFloat(cardDistPay)) {
        payprice = f.floatMulti(parseFloat(vvalue), parseFloat(vipcardrate));
    }
        //else if (parseFloat(vvalue) == parseFloat(cardDistPay))
        //{
        //    payprice = f.floatMulti(parseFloat(vvalue), parseFloat(vipcardrate));
        //}
    else if (parseFloat(vvalue) > parseFloat(cardDistPay) && parseFloat(vvalue) <= f.floatAdd(parseFloat(cardDistPay), parseFloat(cardPay))) {
        payprice = f.floatMulti(parseFloat(cardDistPay), parseFloat(vipcardrate));
        payprice = f.floatAdd(parseFloat(payprice), f.floatSubtract(parseFloat(vvalue), parseFloat(cardDistPay)));
    }
    else if (parseFloat(vvalue) > f.floatAdd(parseFloat(cardDistPay), parseFloat(cardPay))) {

    }

    //实际应付
    //var payprice = f.floatMulti(parseFloat(vvalue), parseFloat(vipcardrate));

    $("#payedMoneyVIPCARD").val(payprice);
};
util.keyPress=function(ob) {
    if (!ob.value.match(/^[\+\-]?\d*?\.?\d*?$/))
        ob.value = ob.t_value;
    else ob.t_value = ob.value;
    if (ob.value.match(/^(?:[\+\-]?\d+(?:\.\d+)?)?$/))
        ob.o_value = ob.value;
}
//实际卡付反算抵扣金额
util.keyUpPay=function(ob) {
    if ($("#VIPCARD_TYPE").val() == "") {
        _showInfoMessage('请您先选择储值卡！', 'info');
        return false;
    }
    var vipcard = $('#VIPCARD_TYPE option:selected').text();
    vipcard = vipcard.split("￥")[1];
    if (parseFloat(ob.value) > parseFloat(vipcard)) {
        _showInfoMessage('卡付金额不能大于卡内余额！', 'info');
        ob.value = vipcard;
        return false;
    }

    if (!ob.value.match(/^[\+\-]?\d*?\.?\d*?$/))
        ob.value = ob.t_value;
    else ob.t_value = ob.value;
    if (ob.value.match(/^(?:[\+\-]?\d+(?:\.\d+)?)?$/))
        ob.o_value = ob.value;
    //输入的抵扣金额
    var vvalue = "";
    if (ob.value == "" || ob.value == null) {
        vvalue = 0;
    }
    else {
        vvalue = ob.value;
    }
    //alert(ob.value);
    //储值卡扣率
    var vipcardrate = $("#hide_VIPCARD_RATE").val();

    //允许卡支付但不享受折扣金额
    var cardPay = $("#spanCardPay").html();
    //允许卡支付并享受折扣金额
    var cardDistPay = $("#spanCardDistPay").html();

    //实际应付
    var payprice = 0;
    ////1.卡付金额小于享受折扣金额
    payprice = f.floatDiv(parseFloat(vvalue), parseFloat(vipcardrate));
    if (parseFloat(payprice) > f.floatAdd(parseFloat(cardPay), parseFloat(cardDistPay))) {
        _showInfoMessage('抵扣金额不能大于允许卡付金额！', 'info');

        $("#payMoneyVIPCARD").val(f.floatAdd(parseFloat(cardPay), parseFloat(cardDistPay)));
        ob.value = f.floatMulti(parseFloat(cardDistPay), parseFloat(vipcardrate));
        // vvalue = ob.value;
        return false;
    }
    //实际应付
    $("#payMoneyVIPCARD").val(payprice);
};
        //现金支付
util.keyUpCash = function (ob) {
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

        //********************//


    });
 })(window, undefined, jQuery);