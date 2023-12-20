// Thêm sự kiện click cho nút "Xác nhận" trong modal
document.getElementById("confirmDelete").addEventListener("click", function () {
    // Lấy seat_id từ hidden input trong modal
    var deleteId = document.getElementById("deleteId").value;

    // Đặt giá trị vào input hidden trong form
    document.getElementById("deleteForm").elements["id"].value = deleteId;

    // Submit form
    $("#deleteForm").submit();
});
/*kết thúc*/

/*Ajax Thay đổi*/
$(document).ready(function () {
    // Bắt sự kiện click nút "Thay đổi" trong modal
    $("#EditSeatForm button[type='submit']").click(function (e) {
        // Ngăn chặn hành vi mặc định của nút submit
        e.preventDefault();

        // Use $(this).closest('form') to target the closest form element
        var form = $(this).closest('form');


        var seat_type = form.find("#seat_type").val();
        var roomid = parseInt(form.find("#room_id").val());
        var seat_row = form.find("#seat_row").val();
        var seat_number = parseInt(form.find("#seat_number").val());
        var seat_status = parseInt(form.find("#seat_status").val());

        // Lấy data từ modal
        var seatId = form.closest('.modal').data('seat-id');
        console.log(seatId);
        // Gửi Ajax request đến action Edit trong controller
        $.ajax({
            url: "/Admin/seats/Edit/" + seatId,
            type: "POST",
            contentType: 'application/json; charset=utf-8',

            data: JSON.stringify({ seat_id: seatId, seat_type: seat_type, room_id: roomid, row: seat_row, number: seat_number, seats_status: seat_status }),
            success: function (response) {
                console.log(response);
                if (response.success) {
                    $("#EditModal_" + seatId).modal("hide");
                    // If the server reports success, close the modal and load rooms
                    alert("Thành công");

                } else {
                    // If the server reports failure, display an error message
                    alert("Lỗi: " + response.message);
                }
            }
        });
    });
});
/*kết thúc*/

