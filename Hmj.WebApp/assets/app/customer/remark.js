
; (function (window, undefined, $) {
    $(document).ready(function () {

        var submiting = false;
        $('#frmRemark').validator({
            rules: {
            },
            fields: {
                //'#REMARK': 'required',
            },
            valid: function (form) {
                if (submiting) {
                    return;
                }
                submiting = true;
                FormSubmit(form, function (res) {
                    if (res.status == 0) {
                        _showInfoMessage("保存成功！", "success");
                        $("#ID").val(res.data);
                        window.location.reload();
                    }
                    else {
                        //showErrorTip("保存失败！：" + res.message, { left: 400, top: 450 }, true, 5000);
                        _showInfoMessage("保存失败！" + res.message, "error");
                    }
                    submiting = false;
                })
            }
        });
        $("#btnSave").click(function (e) {
            GetFeature();
            $("#frmRemark").submit();
        });

        //获取span标签的值
        //alert($("#FEATURE0").text())
        function GetFeature() {
            var count = $("#count").val();
            var feature = "";
        for(var i=0;i<count;i++)
        {
            var cname = "#CheckBox" + i;
            var sname = "#FEATURE" + i;
          
            var c = $(cname).prop("checked");
            var s = $(sname).text();         
            if (c==true) {              
                feature += s;               
            }          
        }
        $("#FEATURE").val(feature);
        //alert($("#FEATURE").val());
        }
    
    });
})(window, undefined, jQuery);