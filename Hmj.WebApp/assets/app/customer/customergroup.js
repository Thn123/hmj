; (function (window, undefined, $) {

    $(document).ready(function () {
             
        var gridopt = {
            url: options.listUrl,
            colModel: [
                    { display: '编号', name: 'ID', width: "0", sortable: false, hide: true, align: 'left', iskey: true },
                    { display: '组名称', name: 'NAME', width: "40%", sortable: false, align: 'left' },
                    { display: '顾客总数', name: 'CUST_COUNT', width: "15%", sortable: false, align: 'left' },
                    { display: '创建用户', name: 'CREATE_USER', width: "15%", sortable: false, align: 'left' },
                    { display: '创建时间', name: 'CREATE_DATE', width: "15%", sortable: false, align: 'left', process: formatDate },
                    { display: '操作', name: 'ID', width: "15%", sortable: false, align: 'center', process: processOp }
            ],
            sortname: "Id",
            sortorder: "ASC",
            title: false,
            rp: 15,
            usepager: true,
            showcheckbox: false
        };

        var xjgrid = new xjGrid("gridlist", gridopt);
        function processOp(value, cell) {
            var ops = [];
            ops.push("&nbsp;<a title='修改用户分组' class='abtn' href='javascript:;'  onclick=\"util.Edit('", value, "')\"><i class='fa fa-edit' ></i>修改</a>");
            ops.push("&nbsp;&nbsp;<a title='删除用户分组' class='abtn' href='javascript:;'  onclick=\"util.Delete('", value, "','", cell[1], "')\"><i class='fa fa-trash-o' ></i>删除</a>");
            return ops.join("");
        }

        function formatDate(value, cell) {
            if (value) {
                return value.split(" ")[0]
            }
            else {
                return "-"
            }
        }

        $("#formQuery").submit(function () {
            xjgrid.Query(this);
            return false;
        });

        $("#btnAdd").click(function (e) {
            window.location.href = options.editUrl;
        });

        $("#btnConfirm").click(function (e) {
            $('#confirmModal').modal('hide');
            var id = $("#hdCurrentId").val();
            $.post(options.deleteUrl, { id: id },
                  function (res) {
                      if (res.status == 0) {
                          _showInfoMessage("操作成功", 'success'); 
                          xjgrid.Reload();
                      }
                      else {
                          _showInfoMessage("操作失败：" + res.message, 'error'); 
                      }
                  },
                  "json"
            );
        })
        util.Edit = function (id) {
            window.location.href = options.editUrl + "/" + id;             
        };
        util.Delete = function (id, name) {
            $("#lbuserName").html(name);
            $("#hdCurrentId").val(id);
            $('#confirmModal').modal('show');
        };
         

        ////根据姓名/电话检索
        //$("#CUST_NAME").select2({
        //    placeholder: "输入姓名、电话查询",
        //    minimumInputLength: 2,
        //    width: 'resolve',
        //    ajax: {
        //        url: options.queryCustomerUrl,
        //        dataType: 'json',
        //        quietMillis: 100,
        //        data: function (term, page) {
        //            return {
        //                q: term, //search term
        //                page_limit: 10, // page size
        //                page: page // page number
        //            };
        //        },
        //        results: function (data, page) {
        //            //var more = (page * 10) < data.total;
        //            var strJson = eval("(" + data.data + ")");
        //            return { results: strJson, more: false };
        //        }
        //    },
        //    formatResult: custFormatResult,
        //    formatSelection: custFormatSelection,
        //    dropdownCssClass: "bigdrop",
        //    formatNoMatches: function (m) {
        //        return "<span style='color:#7B7B7B;font-size:12px;' >未查询到相关顾客信息</span>";
        //    },
        //    escapeMarkup: function (m) { return m; }
        //});


        //function custFormatResult(obj) {
        //    var markup = "<table class='movie-result'><tr>";
        //    if (obj.posters !== undefined && obj.posters.thumbnail !== undefined) {
        //        markup += "<td class='movie-image'><img src='" + obj.posters.thumbnail + "'/></td>";
        //    }
        //    markup += "<td class='movie-info'><div class='movie-title'>" + obj.title + "</div>";
        //    if (obj.critics_consensus !== undefined) {
        //        markup += "<div class='movie-synopsis'>" + obj.critics_consensus + "</div>";
        //    }
        //    else if (obj.synopsis !== undefined) {
        //        markup += "<div class='movie-synopsis'>" + obj.synopsis + "</div>";
        //    }
        //    markup += "</td></tr></table>"
        //    return markup;
        //}

        //function custFormatSelection(obj) {
        //    //$("#custmobile").html(obj.critics_consensus);
        //    //$("#custcard").html("10000102");
        //    //$("#custId").val(obj.id);
        //    $("#newcustname").html(obj.title);
        //    $("#newcustmobile").html(obj.critics_consensus);
        //    $("#divNewCustomer").hide();
        //    $("#divCustomer").show();
        //    return obj.title;
        //}

        //    var cid = $("#hide_Cid").val();
        //    if (cid != null && cid != "") {
        //        $("#divNewCustomer").hide();
        //        $("#divCustomer").show();

        //        //查顾客信息
        //        $.post("/Customer/Edit.do", { id: cid },
        //              function (res) {
        //                  if (res.status == 0) {
        //                      $("#newcustname").html($("#hideCUST_NAME").val());
        //                      $("#newcustmobile").html($("#hide_MOBILE").val());
        //                      $("#divNewCustomer").hide();
        //                      $("#divCustomer").show();
        //                  }
        //                  else {
        //                      return '操作失败';
        //                  }

        //              },
        //              "json"
        //             );
        //    }
        //    else {
        //        $("#divCustomer").hide();
        //        $("#divNewCustomer").show();
        //    }


        
        $('#QueryByGroupName').click(function () {
            $.ajax({
                url: "CustomerGroupList",
                type: "Get",
                success: function (result) {
                }
            });
        });
    });

})(window, undefined, jQuery);