//检查姓名是否为汉字
function checkName(th) {
    var reg = /^[\u4e00-\u9fa5]{1,30}$/;
    var name = document.getElementById(th.id).value;
    return reg.test(name);
}
//检查性别是否符合规范
function checkSex(th) {
    var reg = /^['男'|'女']?$/;
    var sex = document.getElementById(th.id).value;
    return reg.test(sex);
}
//检查身高是否为符合规范
function checkHeight(th) {
    var reg = /^([1|2]?)([0-9])([0-9])$/;
    var height = document.getElementById(th.id).value;
    return reg.test(height);
}
//检查生日是否符合规范
function checkBirthday(th) {
    var birthday = document.getElementById(th.id).value;
    var reg = /([\d]{4}(((0[13578]|1[02])((0[1-9])|([12][0-9])|(3[01])))|(((0[469])|11)((0[1-9])|([12][1-9])|30))|(02((0[1-9])|(1[0-9])|(2[1-8])))))|((((([02468][048])|([13579][26]))00)|([0-9]{2}(([02468][048])|([13579][26]))))(((0[13578]|1[02])((0[1-9])|([12][0-9])|(3[01])))|(((0[469])|11)((0[1-9])|([12][1-9])|30))|(02((0[1-9])|(1[0-9])|(2[1-9])))))/;
    return reg.test(birthday);
}
//检查教室名是否符合规范
function checkClassName(th) {
    var reg = /^[A-Za-z0-9_\-\u4e00-\u9fa5]{0,30}$/;
    var classname = document.getElementById(th.id).value;
    return reg.test(classname);
}
//检查教室号是否符合规范
function checkClassNumber(th) {
    var reg = /^[A-Za-z0-9_\-\u4e00-\u9fa5]{0,10}$/;
    var classnumber = document.getElementById(th.id).value;
    return reg.test(classnumber);
}
//检查邮箱是否符合规范
function checkEmail(th) {
    var reg = /^\w[-\w.+]*@([A-Za-z0-9][-A-Za-z0-9]+\.)+[A-Za-z]{2,14}$/;
    var email = document.getElementById(th.id).value;
    return reg.test(email);
}
//检查手机号码是否符合规范
function checkTelphone(th) {
    var reg = /((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)/;
    var telphone = document.getElementById(th.id).value;
    return reg.test(telphone);
}