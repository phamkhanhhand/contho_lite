


// File: wwwroot/js/custom-input.js

document.addEventListener("DOMContentLoaded", function () {
    // Xử lý các input kiểu 'number' (thêm kiểm tra số hợp lệ)
    const numberInputs = document.querySelectorAll('input[type="number"]');
    numberInputs.forEach(input => {
        input.addEventListener("input", function () {
            if (isNaN(input.value)) {
                alert("Please enter a valid number.");
            }
        });
    });

    // Xử lý các input kiểu 'date' (thêm kiểm tra ngày hợp lệ)
    const dateInputs = document.querySelectorAll('input[type="date"]');
    dateInputs.forEach(input => {
        input.addEventListener("change", function () {
            const selectedDate = new Date(input.value);
            if (selectedDate > new Date()) {
                alert("Please select a valid date in the past.");
            }
        });
    });
});



class KHTextBox extends CTBaseControl {


}