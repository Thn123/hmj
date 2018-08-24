; (function (window, undefined, $) {
        $(document).ready(function () {

            
            //计算
            $("#btnSaveFrm").click(function () {
                var xjgrid = "";
                var store_id = $("#STORE_Id").val();

                var yearmonth = $("#YEAR").val()+$("#MONTH").val();
                $.post(options.queryCommission, {ymonth:yearmonth,storeid:store_id},
                  function (ret) {
                  if (ret.status == 2) {
                      var gridopt = {
                          url: options.listUrl + "?ymonth=" + yearmonth + "&storeid=" + $("#STORE_Id").val(),
                          colModel: [
                                  { display: '员工编号', name: 'EMP_NO', width: "8%", sortable: true, hide: false, align: 'left', iskey: true },
                                  { display: '姓名', name: 'EMP_NAME', width: "10%", sortable: false, align: 'left' },
                                  { display: '年月', name: 'YM', width: "5%", sortable: false, align: 'left' },
                                  { display: '业绩', name: 'PERFORMANCE', width: "5%", sortable: false, align: 'left' },
                                  { display: '提成', name: 'COMMISSION', width: "5%", sortable: false, align: 'left' }

                          ],
                          sortname: "Id",
                          sortorder: "ASC",
                          title: false,
                          rp: 15,
                          usepager: true,
                          showcheckbox: false
                      };

                      xjgrid = new xjGrid("gridlist", gridopt);
                  }
                  else if (ret.status = 0)
                  {
                      alert(ret.message);
                  }
                  else if (ret.status = 1)
                  { alert(ret.message); }
                  else if (ret.status = 3)
                  {
                      alert(ret.message);
                  }
              },
              "json"
             );
            });  
    });
})(window, undefined, jQuery);