

function showPassword() {

    var checkbox_password = document.getElementById("password_checkbox");
    var uPass = document.getElementById("uPass");

    if (checkbox_password.checked) {
        uPass.type = "text";
    }
    else {
        uPass.type = "password";
    }
}

function initilizeControl(controlId) {
    var fcontrol = document.getElementById(controlId);
    var fcontrolError = document.getElementById(controlId + "Error")
    fcontrol.style.borderColor = null;
    fcontrolError.innerText = null;
}

function showError(controlId, errorMessage) {
    var fcontrol = document.getElementById(controlId);
    var fcontrolError = document.getElementById(controlId + "Error");

    fcontrol.style.borderColor = "red";
    fcontrolError.innerHTML = errorMessage;

}

function validate() {

    return true; //Debug Mode

    var fName = fNameOK(); 
    var lName = lNameOK();
    var uEmail = uEmailOK();
    var uPass = uPassOK();
    var uMobile = uMobileOK();
    var uUser = uUserOK();
    var uDate = uDateOK();
    return fName && lName && uEmail && uPass && uMobile && uUser && uDate;
}

function uDateOK() {
    var bDay = document.getElementById("bDay");
    var bMonth = document.getElementById("bMonth");
    var bYear = document.getElementById("bYear");
    var bDayError = document.getElementById("bDayError");

    bDay.style.borderColor = "";
    bDayError.innerHTML = "";

    switch (bMonth.value) {
        case '4' :
        case '6' :
        case '9' :
        case '11' :
            if (parseInt(bDay.value) == 31) {
                bDay.style.borderColor = "red";
                bDayError.innerHTML = "בחודש זה יש 30 יום בלבד";

                return false;
            }
            break;

        case '2':
            if (isLeapYear(parseInt(bYear.value))) {

                if (parseInt(bDay.value) > 29) {
                    bDay.style.borderColor = "red";
                    bDayError.innerHTML = "בחודש זה יש 29 יום בלבד";
                    return false;
                }
            }

            else {
                if (parseInt(bDay.value) > 28) {
                        bDay.style.borderColor = "red";
                        bDayError.innerHTML = "בחודש זה יש 28 יום בלבד";
                        return false;
                }
            }
            
    }

    return true;
}

function isLeapYear(uYear) {

    if (((uYear % 4 == 0) && (uYear % 100 != 0)) || (uYear % 400 == 0)) {
        return true;
    }
    return false;
}

function uEmailOK() {
    var uEmail = document.getElementById("uEmail");
    var uEmailError = document.getElementById("uEmailError");
    initilizeControl("uEmail");
    if (uEmail.value.length == 0) {
        showError("uEmail", "חסר אמייל");
        return false;
    }
    return true;
}

function lNameOK() {
    var lName = document.getElementById("lName");
    var lNameError = document.getElementById("lNameError");
    initilizeControl("lName");
    if (lName.value.length == 0) {
        showError("lName", "חסר שם משפחה");
        return false;
    }
    return true;
}

function uPassOK() {
    var uPass = document.getElementById("uPass");
    initilizeControl("uPass");
    if (uPass.value == "") {
        showError("uPass", "חסר סיסמה");
        return false;
    }

    else if (uPass.value.length < 6) {
        showError("uPass", "הסיסמה חייבת להיות באורך של 6 תווים לפחות");
        return false;
    }

    else {
        var capital_letter = false;
        var small_letter = false;
        var digit = false;

        for (i = 0; i < uPass.value.length; i++) {

            capital_letter = uPass.value.charAt(i) >= 'A' && uPass.value.charAt(i) <= 'Z';

            small_letter = uPass.value.charAt(i) >= 'a' && uPass.value.charAt(i) <= 'z';

            digit = uPass.value.charAt(i) >= '0' && uPass.value.charAt(i) <= '9';

            if (!capital_letter && !small_letter && !digit) {
                showError("uPass", "הסיסמה חייבת להיות באנגלית כולל ספרות");
                return false;
            }
        }
    }

    return true;
}

function fNameOK() {
    var fName = document.getElementById("fName");
    var fNameError = document.getElementById("fNameError");
    initilizeControl("fName");
    if (fName.value.length == 0) {
        showError("fName", "חסר שם פרטי");
        return false;
    }
    return true;
}

function uMobileOK() {
    var uMobile = document.getElementById("uMobile");
    initilizeControl("uMobile");
    var number = uMobile.value + "";
    if (number.length == 0) {
        showError("uMobile", "חסר מספר טלפוון");
        return false;
    }
    else if (number.length != 7) {
        showError("uMobile", "מספר טלפון חייב להיות 7 ספרות");
        return false;
    }
    return true;
}

function uUserOK() {
    var uUser = document.getElementById("uUser");
    var uUserError = document.getElementById("uUserError");
    initilizeControl("uUser");
    if (uUser.value.length == 0) {
        showError("uUser", "חסר שם משתמש");
        return false;
    }
    return true;
}