
function validateForm() {

    var x = document.getElementById("formEmail").value;
    var y = document.getElementById("formName").value;
    var z = document.getElementById("formUser").value;
    if (x === "") {
        Swal.fire({
            type: 'error',
            title: 'Lỗi rồi',
            text: 'Có vài thứ bạn chưa điền!',
        })
        return false;
    }
    if (y === "") {
        Swal.fire({
            type: 'error',
            title: 'Lỗi rồi',
            text: 'Có vài thứ bạn chưa điền!',
        })
        return false;
    }
    if (z === "") {
        Swal.fire({
            type: 'error',
            title: 'Lỗi rồi',
            text: 'Có vài thứ bạn chưa điền!',
        })
        return false;
    }
    else
        Swal.fire(
            'Thành Công!',
            'Chúc mừng bạn đã là thành viên!',
            'success'
        )

}
function validateUSER() {
    Swal.fire(
        'Yuppp!',
        'Chúc mừng bạn đã đăng nhập thành công!',
        'success'
    )
}

