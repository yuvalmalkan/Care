function showError(id, message) {
    const errorSpan = document.getElementById(id + "Error");
    if (errorSpan) {
        errorSpan.textContent = message;
    }
}

function clearErrors() {
    const errorSpans = document.querySelectorAll("span[id$='Error']");
    errorSpans.forEach(span => span.textContent = "");
}

function validate() {
    clearErrors();

    let isValid = true;

    const fName = document.getElementById("fName").value.trim();
    if (fName.length < 2) {
        showError("fName", "First name must be at least 2 characters");
        isValid = false;
    }

    const lName = document.getElementById("lName").value.trim();
    if (lName.length < 2) {
        showError("lName", "Last name must be at least 2 characters");
        isValid = false;
    }

    const uName = document.getElementById("uName").value.trim();
    if (uName.length < 4) {
        showError("uName", "Username must be at least 4 characters");
        isValid = false;
    }

    const uPass = document.getElementById("uPass").value;
    const hasUpper = /[A-Z]/.test(uPass);
    const hasLower = /[a-z]/.test(uPass);
    const hasDigit = /\d/.test(uPass);
    if (!hasUpper || !hasLower || !hasDigit) {
        showError("uPass", "Password must include uppercase, lowercase letters, and digits");
        isValid = false;
    }

    const uEmail = document.getElementById("uEmail").value.trim();
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailPattern.test(uEmail)) {
        showError("uEmail", "Invalid email format");
        isValid = false;
    }

    const uMobile = document.getElementById("uMobile").value.trim();
    if (!/^\d{10}$/.test(uMobile)) {
        showError("uMobile", "Mobile number must be exactly 10 digits");
        isValid = false;
    }

    return isValid;
}