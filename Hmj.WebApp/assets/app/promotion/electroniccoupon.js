; (function (window, undefined, $) {

    $(document).ready(function () {

        //渠道
        function formatQD(value, cell) {
            if (value == 1) {
                return "大众点评网";
            }
            else if (value == 2) {
                return "丁丁网";
            }
            else if (value == 3) {
                return "腾讯微生活";
            }
            else if (value == 4) {
                return "微信";
            }
        }
        //适用项目
        function formatPro(value, cell) {
            if (value == "True") {
                return "所有项目";
            }
            else
                return "部分项目";
        }
        //适用时间
        function formatDay(value, cell) {
            if (value == "True") {
                return "整天";
            }
            else
                return "自定义时间";
        }
        //适用顾客
        function formatGk(value, cell) {
            if (value == 1) {
                return "全部顾客";
            }
            else if (value == 2) {
                return "只限第一次到店顾客";
            }
            else if (value == 3) {
                return "每人只能使用一次";
            }
            else if (value == 4) {
                return "只限非会员";
            }
        }
        //团购类型
        function formatTglx(value, cell) {
            if (value == 1) {
                return "折扣";
            }
            else if (value == 2) {
                return "代金券";
            }
        }

        //渠道优惠券
        var gridopt = {
            url: options.listUrl + "?type=0",
            colModel: [
                    { display: '编号', name: 'ID', width: "0", sortable: false, hide: true, align: 'left', iskey: true },
                    { display: '渠道', name: 'CHANNEL', width: "10%", sortable: false, align: 'left', process: formatQD },
                    { display: '活动名称', name: 'NAME', width: "10%", sortable: false, align: 'left' },
                    { display: '创建时间', name: 'CREATE_DATE', width: "10%", sortable: false, align: 'left', process: formatDate },
                    { display: '有效期', name: 'BEGIN_DATE', width: "20%", sortable: false, align: 'left', process: formatDate1 },
                    { display: '有效期', name: 'END_DATE', width: "0", sortable: false, hide: true, align: 'left' },
                    { display: '优惠折扣', name: 'DISCOUNT_RATE', width: "8%", sortable: false, align: 'left' },
                    { display: '适用项目', name: 'IS_ALLPROD', width: "10%", sortable: false, align: 'left', process: formatPro },
                    { display: '适用顾客', name: 'CUST_GROUP_ID', width: "10%", sortable: false, align: 'left' },
                    { display: '适用时间', name: 'IS_ALLDAY', width: "10%", sortable: false, align: 'left', process: formatDay },
                    { display: '操作', name: 'ID', width: "12%", sortable: false, align: 'center', process: processOp }
            ],
            sortname: "Id",
            sortorder: "ASC",
            title: false,
            rp: 10,
            usepager: true,
            showcheckbox: false
        };
        var xjgrid;

        //团购活动
        var gridopt1 = {
            url: options.listUrl + "?type=1",
            colModel: [
                    { display: '编号', name: 'ID', width: "0", sortable: false, hide: true, align: 'left', iskey: true },
                    { display: '团购类型', name: 'SUB_TYPE', width: "6%", sortable: false, align: 'left', process: formatTglx },
                    { display: '渠道', name: 'CHANNEL', width: "8%", sortable: false, align: 'left', process: formatQD },
                    { display: '活动名称', name: 'NAME', width: "10%", sortable: false, align: 'left' },
                    { display: '创建时间', name: 'CREATE_DATE', width: "10%", sortable: false, align: 'left', process: formatDate },
                    { display: '有效期', name: 'BEGIN_DATE', width: "20%", sortable: false, align: 'left', process: formatDate1 },
                    { display: '有效期', name: 'END_DATE', width: "0", sortable: false, hide: true, align: 'left' },
                    { display: '团购折扣/代金券', name: 'DISCOUNT_RATE', width: "8%", sortable: false, align: 'left' },
                    { display: '适用项目', name: 'IS_ALLPROD', width: "6%", sortable: false, align: 'left', process: formatPro },
                    { display: '适用顾客', name: 'CUST_GROUP_ID', width: "10%", sortable: false, align: 'left', process: formatGk },
                    { display: '适用时间', name: 'IS_ALLDAY', width: "10%", sortable: false, align: 'left', process: formatDay },
                    { display: '操作', name: 'ID', width: "12%", sortable: false, align: 'center', process: processOp }
            ],
            sortname: "Id",
            sortorder: "ASC",
            title: false,
            rp: 10,
            usepager: true,
            showcheckbox: false
        };
        var xjgrid1;

        //买赠活动
        var gridopt2 = {
            url: options.listUrl + "?type=2",
            colModel: [
                    { display: '编号', name: 'ID', width: "0", sortable: false, hide: true, align: 'left', iskey: true },
                    { display: '活动名称', name: 'NAME', width: "12%", sortable: false, align: 'left' },
                    { display: '创建时间', name: 'CREATE_DATE', width: "10%", sortable: false, align: 'left', process: formatDate },
                    { display: '有效期', name: 'BEGIN_DATE', width: "18%", sortable: false, align: 'left', process: formatDate1 },
                    { display: '有效期', name: 'END_DATE', width: "0", sortable: false, hide: true, align: 'left' },
                    { display: '购买项目', name: 'PROD_INFO', width: "15%", sortable: false, align: 'left' },
                    { display: '赠送项目', name: 'PROD_INFO_GIVE', width: "20%", sortable: false, align: 'left' },
                    { display: '适用顾客', name: 'CUST_GROUP_ID', width: "10%", sortable: false, align: 'left', process: formatGk },
                    { display: '操作', name: 'ID', width: "12%", sortable: false, align: 'center', process: processOp }
            ],
            sortname: "Id",
            sortorder: "ASC",
            title: false,
            rp: 10,
            usepager: true,
            showcheckbox: false
        };
        var xjgrid2;

        //自定义促销
        var gridopt3 = {
            url: options.listUrl + "?type=3",
            colModel: [
                    { display: '编号', name: 'ID', width: "0", sortable: false, hide: true, align: 'left', iskey: true },
                    //{ display: '渠道', name: 'CHANNEL', width: "10%", sortable: false, align: 'left', process: formatQD },
                    { display: '活动名称', name: 'NAME', width: "15%", sortable: false, align: 'left' },
                    { display: '创建时间', name: 'CREATE_DATE', width: "10%", sortable: false, align: 'left', process: formatDate },
                    { display: '有效期', name: 'BEGIN_DATE', width: "20%", sortable: false, align: 'left', process: formatDate1 },
                    { display: '有效期', name: 'END_DATE', width: "0", sortable: false, hide: true, align: 'left' },
                    { display: '优惠折扣', name: 'DISCOUNT_RATE', width: "8%", sortable: false, align: 'left' },
                    { display: '适用项目', name: 'IS_ALLPROD', width: "15%", sortable: false, align: 'left', process: formatPro },
                    { display: '适用顾客', name: 'CUST_GROUP_ID', width: "10%", sortable: false, align: 'left', process: formatGk },
                    { display: '适用时间', name: 'IS_ALLDAY', width: "10%", sortable: false, align: 'left', process: formatDay },
                    { display: '操作', name: 'ID', width: "12%", sortable: false, align: 'center', process: processOp }
            ],
            sortname: "Id",
            sortorder: "ASC",
            title: false,
            rp: 10,
            usepager: true,
            showcheckbox: false
        };
        var xjgrid3;

        //发放代金券
        var gridopt4 = {
            url: options.listUrl + "?type=4",
            colModel: [
                    { display: '编号', name: 'ID', width: "0", sortable: false, hide: true, align: 'left', iskey: true },
                    { display: '活动名称', name: 'NAME', width: "30%", sortable: false, align: 'left' },
                    { display: '创建时间', name: 'CREATE_DATE', width: "10%", sortable: false, align: 'left', process: formatDate },
                    { display: '代金券面值', name: 'VOUCHER_VALUE', width: "10%", sortable: false, align: 'left' },
                    { display: '发放顾客', name: 'CUST_GROUP_NAME', width: "20%", sortable: false, align: 'left' },
                    { display: '发放人数', name: 'GRANT_COUNT', width: "10%", sortable: false, align: 'left' },
                    { display: '使用人数', name: 'USER_COUNT', width: "10%", sortable: false, align: 'left' },
                    { display: '操作', name: 'ID', width: "10%", sortable: false, align: 'center', process: processOp }
            ],
            sortname: "Id",
            sortorder: "ASC",
            title: false,
            rp: 10,
            usepager: true,
            showcheckbox: false
        };
        var xjgrid4;


        function loadList() {
            var cuT = $("#TYPE").val();
            if (cuT == 0) {
                $("#titleA").html("<i class=\"fa fa-tags\"></i>&nbsp;渠道优惠券")
                $("#titleB").html("渠道优惠券列表")
                $("#deleteP").html("你确认要删除渠道优惠券 [ <span id=\"lbuserName\"></span>] 吗？")
                xjgrid = new xjGrid("gridlist", gridopt);
            }
            else if (cuT == 1) {
                $("#titleA").html("<i class=\"fa fa-tags\"></i>&nbsp;团购活动")
                $("#titleB").html("团购活动列表")
                $("#deleteP").html("你确认要删除团购活动 [ <span id=\"lbuserName\"></span>] 吗？")
                xjgrid1 = new xjGrid("gridlist", gridopt1);
            }
            else if (cuT == 2) {
                $("#titleA").html("<i class=\"fa fa-tags\"></i>&nbsp;买赠活动")
                $("#titleB").html("买赠活动列表")
                $("#deleteP").html("你确认要删除买赠活动 [ <span id=\"lbuserName\"></span>] 吗？")
                xjgrid2 = new xjGrid("gridlist", gridopt2);
            }
            else if (cuT == 3) {
                $("#titleA").html("<i class=\"fa fa-tags\"></i>&nbsp;自定义活动")
                $("#titleB").html("自定义活动列表")
                $("#deleteP").html("你确认要删除自定义活动 [ <span id=\"lbuserName\"></span>] 吗？")
                xjgrid3 = new xjGrid("gridlist", gridopt3);
            }
            else if (cuT == 4) {
                $("#titleA").html("<i class=\"fa fa-tags\"></i>&nbsp;发放代金券")
                $("#titleB").html("代金券列表")
                $("#deleteP").html("你确认要删除代金券 [ <span id=\"lbuserName\"></span>] 吗？")
                xjgrid4 = new xjGrid("gridlist", gridopt4);
            }

        }
        loadList();
        function processOp(value, cell) {
            var ops = [];
            var cuT = $("#TYPE").val();
            if (cuT != 4)
                ops.push("&nbsp;<a title='编辑' class='abtn' href='javascript:;' onclick=\"util.Edit('", value, "')\"><i class='fa fa-edit' ></i>编辑</a>");

            if (cuT == 0) {
                ops.push("&nbsp;&nbsp;<a title='删除' class='abtn' href='javascript:;'  onclick=\"util.Delete('", value, "','", cell[2], "')\"><i class='fa fa-trash-o' ></i>删除</a>");
            }
            else if (cuT == 1) {
                ops.push("&nbsp;&nbsp;<a title='删除' class='abtn' href='javascript:;'  onclick=\"util.Delete('", value, "','", cell[3], "')\"><i class='fa fa-trash-o' ></i>删除</a>");
            }
            else if (cuT == 2 || cuT == 3 || cuT == 4) {
                ops.push("&nbsp;&nbsp;<a title='删除' class='abtn' href='javascript:;'  onclick=\"util.Delete('", value, "','", cell[1], "')\"><i class='fa fa-trash-o' ></i>删除</a>");
            }
            return ops.join("");
        }

        function formatDate(value, cell) {
            if (value) {
                return value.split(" ")[0].replace("/", "-").replace("/", "-");
            }
            else {
                return "-";
            }
        }
        function formatDate1(value, cell) {
            if (value) {
                return value.split(" ")[0].replace("/", "-").replace("/", "-") + " 至 " + cell[4].split(" ")[0].replace("/", "-").replace("/", "-");
            }
            else {
                return "-";
            }
        }
        util.Edit = function (id) {
            var cuT = $("#TYPE").val();
            if (cuT == 0) {
                window.location.href = options.editUrl + "/" + id;
            }
            else if (cuT == 1) {
                window.location.href = options.editEGCUrl + "/" + id;
            }
            else if (cuT == 2) {
                window.location.href = options.editBGAUrl + "/" + id;
            }
            else if (cuT == 3) {
                window.location.href = options.editELSEUrl + "/" + id;
            }
            else if (cuT == 4) {
                window.location.href = options.editVoucherUrl + "/" + id;
            }
        }
        $("#btnAdd").click(function (e) {
            var cuT = $("#TYPE").val();
            if (cuT == 0) {
                window.location.href = options.editUrl;
            }
            else if (cuT == 1) {
                window.location.href = options.editEGCUrl;
            }
            else if (cuT == 2) {
                window.location.href = options.editBGAUrl;
            }
            else if (cuT == 3) {
                window.location.href = options.editELSEUrl;
            }
            else if (cuT == 4) {
                window.location.href = options.editVoucherUrl;
            }
        });

        $("#formQuery").submit(function () {
            var cuT = $("#TYPE").val();
            if (cuT == 0) {
                xjgrid.Query(this);
            }
            else if (cuT == 1) {
                xjgrid1.Query(this);
            }
            else if (cuT == 2) {
                xjgrid2.Query(this);
            }
            else if (cuT == 3) {
                xjgrid3.Query(this);
            }
            else if (cuT == 4) {
                xjgrid4.Query(this);
            }
            return false;
        });

        util.Delete = function (id, name) {
            $("#lbuserName").html(name);
            $("#hdCurrentId").val(id);
            $('#confirmModal').modal('show');
        };

        $("#btnConfirm").click(function (e) {
            $('#confirmModal').modal('hide');
            var id = $("#hdCurrentId").val();
            $.post(options.deleteUrl + "/" + id, { id: id },
                  function (res) {
                      if (res.status == 0) {
                          _showInfoMessage("操作成功！" , 'success'); 
                          var cuT = $("#TYPE").val();
                          if (cuT == 0) {
                              xjgrid.Reload();
                          }
                          else if (cuT == 1) {
                              xjgrid1.Reload();
                          }
                          else if (cuT == 2) {
                              xjgrid2.Reload();
                          }
                          else if (cuT == 3) {
                              xjgrid3.Reload();
                          }
                          else if (cuT == 4) {
                              xjgrid4.Reload();
                          }
                      }
                      else {
                          _showInfoMessage("操作失败：" + res.message, 'error'); 
                      }
                  },
                  "json"
            );
        });
    });
})(window, undefined, jQuery);