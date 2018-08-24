; (function (window, undefined, $) {

    $(document).ready(function () {

        var submiting = false;
        $('#frmPointsSave').validator({
            rules: {
            },
            fields: {
            },
            valid: function (form) {
                if (submiting) {
                    return;
                }
                submiting = true;
                FormSubmit(form, function (res) {
                    if (res.status > 0) {
                        _showInfoMessage("操作成功！", "success");
                        window.location.href = "/Customer/MemberPoints.do/" + $("#CUST_ID").val();
                    }
                    else {
                        _showInfoMessage("操作失败！" + res.message, "error");
                    }
                    submiting = false;
                })
            }
        });

        $("#btnSave").click(function () {
            if ($("#PRO_TYPE").val() == "" || $("#PRO_TYPE").val() == null)
            {
                _showInfoMessage("请您先选择要兑换的产品！", "success");
                return false;
            }
            var point = $("#PRO_TYPE option:selected").attr("itemid");
            $("#PRO_POINTS").val(point);
            $('#frmPointsSave').submit();
        });
        $("#btnReturn").click(function () {
            window.location.href = "/Customer/View360.do?cid=" + $("#CUST_ID").val();
        });

        $("#PROD_CATEGORY").change(function () {
            var cate_id = $("#PROD_CATEGORY").val();
            var card_points = $("#CARD_POINTS").text();
            $.post(options.queryProd, { id: cate_id, points: card_points },
                 function (res) {
                     $("#PRO_TYPE option").remove();
                     $("#PRO_TYPE").append('<option value="">请选择产品</option>');
                     if (res.status > 0) {
                         var json = eval(res.data);   
                         $(json).each(function () {
                             $("#PRO_TYPE").append("<option value=" + this.ID + " itemid=" + this.POINT + ">" + this.NAME + "&nbsp;&nbsp;[" + this.POINT + "分]</option>");
                         });
                     }
                 },
                 "json"
           );
        });
    });

})(window, undefined, jQuery);