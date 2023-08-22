function changeEventClick() {
    // Get the form element
    var form = document.getElementById("Form");

    // Update the onsubmit attribute to call validateFormAll
    form.onsubmit = function (event) {
        return validateFormAll(event);
    };
}


function validateForm(event) {
    // Lấy giá trị từ các trường nhập liệu
    var fullName = document.getElementById("FullName").value;
    var userName = document.getElementById("UserName").value;
    var Phone = document.getElementById("Phone").value;
    var password = document.getElementById("Password").value;
    var confirmPassword = document.getElementById("Password1").value;

    // Xóa thông báo lỗi cũ
    clearErrorMessages();

    // Kiểm tra tên đăng nhập không chứa ký tự đặc biệt và khoảng trắng, chỉ chứa chữ hoa, chữ thường và số
    var userNamePattern = /^[a-zA-Z0-9]+(?:[._][a-zA-Z0-9]+)*@[a-zA-Z0-9]+\.[a-zA-Z]{2,}$|^[a-zA-Z0-9]+$/; 
    if (!userNamePattern.test(userName)) {
        showError("userNameError", "Tên đăng nhập không hợp lệ! Chỉ chấp nhận chữ hoa, chữ thường, số và dạng gmail!");
        event.preventDefault(); // Ngăn chặn sự kiện submit
        return false;
    }
    // Kiểm tra số điện thoại
    var phoneRegex = /^0\d{9}$/;
    if (!phoneRegex.test(Phone)) {
        showError("phoneError", "Số điện thoại không hợp lệ!");
        event.preventDefault(); // Ngăn chặn sự kiện submit
        return false;
    }



    // Validate thành công, cho phép submit form
    return true;
}
function validateFormAll(event) {
    // Lấy giá trị từ các trường nhập liệu
    var fullName = document.getElementById("FullName").value;
    var userName = document.getElementById("UserName").value;
    var Address = document.getElementById("Address").value;
    var Phone = document.getElementById("Phone").value;
    var password = document.getElementById("Password").value;
    var confirmPassword = document.getElementById("Password1").value;

    // Xóa thông báo lỗi cũ
    clearErrorMessages();

    // Kiểm tra tên đăng nhập không chứa ký tự đặc biệt và khoảng trắng, chỉ chứa chữ hoa, chữ thường và số
    var userNamePattern = /^[a-zA-Z0-9]+(?:[._][a-zA-Z0-9]+)*@[a-zA-Z0-9]+\.[a-zA-Z]{2,}$|^[a-zA-Z0-9]+$/; 
    if (!userNamePattern.test(userName)) {
        showError("userNameError", "Tên đăng nhập không hợp lệ! Chỉ chấp nhận chữ hoa, chữ thường, số và dạng gmail!");
        event.preventDefault(); // Ngăn chặn sự kiện submit
        return false;
    }
    // Kiểm tra số điện thoại
    var phoneRegex = /^0\d{9}$/;
    if (!phoneRegex.test(Phone)) {
        showError("phoneError", "Số điện thoại không hợp lệ!");
        event.preventDefault(); // Ngăn chặn sự kiện submit
        return false;
    }

    // Kiểm tra mật khẩu không chứa khoảng trắng và có độ dài tối thiểu 6 kí tự
    if (password.length < 6 || /\s/.test(password)) {
        showError("passwordError", "Mật khẩu không hợp lệ! Mật khẩu phải có ít nhất 6 kí tự và không được chứa khoảng trắng.");
        event.preventDefault(); // Ngăn chặn sự kiện submit
        return false;
    }

    // Kiểm tra mật khẩu xác nhận trùng khớp
    if (password !== confirmPassword) {
        showError("confirmPasswordError", "Mật khẩu xác nhận không khớp!");
        event.preventDefault(); // Ngăn chặn sự kiện submit
        return false;
    }

    // Validate thành công, cho phép submit form
    return true;
}

function showError(elementId, errorMessage) {
    var errorElement = document.getElementById(elementId);
    errorElement.textContent = errorMessage;
}

function clearErrorMessages() {
    var errorMessages = document.getElementsByClassName("error-message");
    for (var i = 0; i < errorMessages.length; i++) {
        errorMessages[i].textContent = "";
    }
}