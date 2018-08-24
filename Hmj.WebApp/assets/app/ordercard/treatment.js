; (function (window, undefined, $) {

    $(document).ready(function () {

        var gridopt = {
            url: options.listUrl + "?PROD_TYPE=2",
            colModel: [
                    { display: '编号', name: 'ID', width: "10%", sortable: true, hide: true, align: 'left', iskey: true },
                    { display: '订单号', name: 'ORDER_NO', width: "15%", sortable: false, align: 'left' },
                    { display: '项目名称', name: 'PROD_NAME', width: "15%", sortable: false, align: 'left' },
                    { display: '客户', name: 'CUST_NAME', width: "8%", sortable: false, align: 'left' },
                    { display: '购买日期', name: 'CREATE_DATE', width: "15%", sortable: false, align: 'left', process: formatDate },
                    { display: '购买价格', name: 'PAY_AMT', width: "10%", sortable: false, align: 'left' },
                    { display: '数量', name: 'PROD_CNT', width: "10%", sortable: false, align: 'left' },
                    { display: '状态', name: 'PROD_CNT', width: "10%", sortable: false, align: 'left' },
                    { display: '操作', name: 'ID', width: "20%", sortable: false, align: 'center', process: processOp }
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
            ops.push("&nbsp;<a title='作废订单' class='abtn' href='javascript:;'  onclick=\"util.Edit('", value, "')\"><i class='fa fa-edit' ></i>作废</a>");
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

    });

})(window, undefined, jQuery);