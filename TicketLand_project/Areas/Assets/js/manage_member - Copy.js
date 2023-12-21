// Thêm sự kiện click cho nút "Xác nhận" trong modal
document.getElementById("confirmDelete").addEventListener("click", function () {
   // Lấy seat_id từ hidden input trong modal
   var deleteId = document.getElementById("deleteId").value;

    // Đặt giá trị vào input hidden trong form
    document.getElementById("deleteForm").elements["id"].value = deleteId;

    // Submit form
    $("#deleteForm").submit();
});
