// scripts.js
document.addEventListener("DOMContentLoaded", function () {
    var comboBox = document.getElementById("comboBox");

    // Cập nhật lại danh sách options từ JavaScript
    function updateOptions(options) {
        // Xóa hết các option hiện tại
        comboBox.innerHTML = '';

        // Thêm các option mới vào
        options.forEach(function (option) {
            var optionTag = document.createElement('option');
            optionTag.value = option;
            optionTag.textContent = option;
            comboBox.appendChild(optionTag);
        });
    }

    // Ví dụ cập nhật lại danh sách options từ JavaScript
    updateOptions(["New Option 1", "New Option 2", "New Option 3"]);

    // Thiết lập giá trị đã chọn
    comboBox.value = "New Option 2"; // Thay đổi giá trị đã chọn
});
