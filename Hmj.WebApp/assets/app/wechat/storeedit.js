; (function (window, undefined, $) {
    $(document).ready(function () {


        $("#btnSave").click(function (e) {
            $("#frmSave").submit();
        });

        $('#frmSave').validator({
            rules: {
            },
            fields: {
            },
            valid: function (form) {
                FormSubmit(form, function (res) {
                    if (res.status == 0) {
                        _showInfoMessage("操作成功！", 'success');
                        location = "StoreManage.do";
                    }

                    else {
                        _showInfoMessage("操作失败：" + res.message, 'error');
                    }
                })
            }
        });



    });
})(window, undefined, jQuery);


$("#biaozhu").click(function () {
    if ($("#City").val() == "") {
        alert("请输入所属城市");
        return;
    } else {
        window.open("GetLocation.do?nicename=小时光&city=" + $("#City").val() + "&Lng=" + $("#X").val() + "&Lat=" + $("#Y").val() + "&ADDRESS=" + $("#Address").val());
        // $("#biaozhu").attr("href", "GetLocation.do?nicename=小时光&city=" + $("#CITY").val() + "&Lng=" + $("#Lng").val() + "&Lat=" + $("#Lat").val() + "&ADDRESS=" + $("#ADDRESS").val());
    }
});