
//手机验证
function isPhone(phone) {
    if (phone.length != 11) {
        return false;
    }
    //var myreg = /^0?(13[0-9]|15[0-9]|17[0-9]|18[0-9]|14[0-9])[0-9]{8}$/;
    var myreg = /^1\d{10}$/;
    if (!myreg.test(phone)) return false;
    return true;
}
//倒数
var num = 60;
var timerrr;
var flag = true, flaglogin = true;
var daojishi = document.getElementById('timeid');
function dao() {
    if (num > 0) {
        num -= 1;
        daojishi.innerHTML = num + "s";
        if (num < 10) {
            daojishi.innerHTML = "0" + num + "s";
        }
    }
    if (num == 0) {
        daojishi.innerHTML = "重新获取";
        num = 60;
        flag = true;
        clearInterval(timerrr);
    }
}
//tips
//text要提示的文本，shutdown是否需要关闭，delaytime延迟多久关闭
var tipsflag = true;
function tips(text, shutdown, delaytime,noneBtn,onebtn){
    if (tipsflag == true){
        tipsflag = false;
        //如果传入文本为空，又是立即关闭
        if (text == "" && shutdown == true && delaytime == 0){
            $('#modaltips').html("").hide();
            tipsflag = true;
            return;
        }
        var htmlTips=`
		<div class="layer"  style="display: block">
             <div class="layer-box">
                <div class="layer-cont">
                    <div class="layer-border">
                        <div class="layer-text">
                        </div>
                    </div>
                    <div class="layer-btn">
                        <button class="layer-button leftbutton">返回</button>
                        <button class="layer-button rightbutton">确定</button>
                    </div>
                </div>
            </div>
          </div>
        `;
        $('#modaltips').html("").append(htmlTips).show();
        $('#modaltips').find('.layer-text').html(text);

        if(onebtn==true){
            $("#modaltips .layer").find('.leftbutton').remove();
            $('#modaltips .layer').find('.rightbutton').css("width", '100%');
            
        }
        if(noneBtn==true){
           $("#modaltips .layer").find('.layer-btn').remove();
        }
        //如果弹出框的字超过8个的话就，tips就靠左；
        if (text.trim().length > 8){
            $('#modaltips p').css('text-align', 'left')
        }
        //默认自动关闭
        if (shutdown == undefined)
            shutdown = true;

        //默认延迟2秒关闭
        if (delaytime == undefined)
            delaytime = 2000;

        //如果指定了关闭，延迟时间为0，那么 就立即关闭
        if (shutdown && delaytime == 0) {
            $('#modaltips').fadeOut();
        }
        //如果是自动关闭
        else if (shutdown) {
            setTimeout(function () { $('#modaltips').fadeOut(); }, delaytime);
        }
    }
    tipsflag = true;
}


//只能输入中英文;;
function IsVal(value, name) {
    var returnValue = true;
    if (value.trim() == "") {
        tips("请填写您的" + name,true,2000,true);
        returnValue = false;
        return returnValue;
    }

    var regx = new RegExp("^[A-Za-z\u4e00-\u9fa5]+$");
    if (!regx.test(value.trim())) {
        tips(name + "只能输入中英文", true, 2000, true);
        returnValue = false;
        return returnValue;
    }
    return returnValue;
}

