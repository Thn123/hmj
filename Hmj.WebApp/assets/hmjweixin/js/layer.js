var layerHtml='';
layerHtml +='<div class="layer" id="tel-error">'+
                '<div class="layer-box">'+
                    '<div class="layer-cont">'+
                        '<div class="layer-border">'+
                            '<div class="layer-text">'+
                                '<p>您填写的推荐人手机号有误，</p>'+
                                '<p>请返回修改。</p>'+
                            '</div>'+
                            '<div class="layer-btn layer-one-btn"><button class="layer-button">确定</button></div>'+
                        '</div>'+
                    '</div>'+
                '</div>'+
            '</div>'+
            /* layer-two-btn */
            '<div class="layer">'+
                '<div class="layer-box">'+
                    '<div class="layer-cont">'+
                        '<div class="layer-text layer-border-none">'+
                                '<p>您确认推荐人是：18012341234</p>'+
                            '</div>'+
                            '<div class="layer-btn layer-two-btn"><button class="layer-button btn-left">返回修改</button><button class="layer-button btn-right">确定</button></div>'+
                        '</div>'+
                    '</div>'+
                '</div>'+
            '</div>'+
            /* layer-no-btn */
            '<div class="layer">'+
                '<div class="layer-box">'+
                    '<div class="layer-cont">'+
                        '<div class="layer-border layer-pd85">'+
                            '<div class="layer-text layer-no-btn">'+
                                '<p>网络延迟，请您稍后再试...</p>'+
                            '</div>'+
                        '</div>'+
                    '</div>'+
                '</div>'+
            '</div>'+
             /* layer-sanhang-text-two-btn */
            '<div class="layer" style="display:block">'+
                    '<div class="layer-box">'+
                        '<div class="layer-cont">'+
                            '<div class="layer-border layer-pd60">'+
                                '<div class="layer-text layer-pd10">'+
                                    '<p>绑定后，您所有积分都将被转换。 共获得华美家xxx积分，并成为华美家xx会员。</p>'+
                                    '<p>共获得华美家xxx积分，并成为华美家xx会员。</p>'+
                                '</div>'+
                                '<div class="layer-btn layer-double-btn"><button class="layer-button btn-left">取消</button><button class="layer-button btn-right">确定</button></div>'+
                            '</div>'+
                        '</div>'+
                    '</div>'+
            '</div>'+
            /* layer-input-modify-telephone */
            '<div class="layer">'+
                '<div class="layer-box">'+
                    '<div class="layer-cont">'+
                        '<div class="layer-border layer-pd40">'+
                            '<div class="layer-text layer-input">'+
                                '<h3>修改手机号码</h3>'+
                                '<p><input type="text" name="old_tel" class="old_tel" value="" placeholder="请输入原手机号码"></p>'+
                                '<p><input type="text" name="new_tel" class="new_tel" value="" placeholder="请输入新的手机号码"></p>'+
                                '<p><input type="text" name="new_code" class="new_code" value="" placeholder=""><em>获取验证码</em></p>'+
                            '</div>'+
                            '<div class="layer-btn layer-two-btn"><button class="layer-button btn-left">取消</button><button class="layer-button btn-right">确定</button></div>'+
                        '</div>'+
                    '</div>'+
                '</div>'+
            '</div>';
$('.body').append(layerHtml); 