/*Sự kiện xóa*/
document.getElementById("confirmDelete").addEventListener("click", function () {
    // Lấy seat_id từ hidden input trong modal
    var deleteId = document.getElementById("deleteId").value;

    // Đặt giá trị vào input hidden trong form
    document.getElementById("deleteForm").elements["id"].value = deleteId;

    // Submit form
    $("#deleteForm").submit();
});
/*Kết thúc*/



/*Sự kiện thêm mới 1 phòng*/
$(document).ready(function () {
    // Bắt sự kiện submit của form
    $("#addRoomForm").submit(function (event) {
        // Ngăn chặn hành vi mặc định của form
        event.preventDefault();

        // Lấy dữ liệu form
        var room_name = $("#_room_name_").val();
        console.log(room_name);
        var room_capacity = $("#capacity").val();
        var _data = JSON.stringify({ room_name: room_name, capacity: room_capacity });

        // Gửi Ajax request đến action Create trong controller
        $.ajax({
            url: "/Admin/rooms/Create",
            type: "POST",
            data: _data,
            contentType: 'application/json; charset=utf-8',
            success: function (response) {
                // Xử lý kết quả từ server
                if (response.success) {
                    // Đóng modal sau khi lưu thành công
                    $("#addRoomModal").modal("hide");
                    // Gọi hàm loadRooms để cập nhật danh sách phòng
                    loadRooms();
                } else {
                    // Xử lý khi lưu không thành công (hiển thị thông báo lỗi, v.v.)
                    alert(response.message)
                }
            },
            error: function (error) {
                // Xử lý lỗi (nếu cần)
                alert(error.message);
            }
        });
    });
});
/*Kết thúc*/

// Hàm load rooms
function loadRooms() {
    // Send Ajax request to get updated room list
    var currentPage = $("#currentPage").data("page");
    $.ajax({
        url: "/Admin/rooms/GetRooms", 
        type: "GET",
        dataType: "html",
        data: { page: currentPage },
        success: function (data) {
            // Update the content of the room list div
            console.log("Load Rooms function called");
            $("#Room_partial").empty().append(data);
            window.location.reload();
        },
        error: function (error) {
            console.log(error);
        }
    });
}
/*Kết thúc*/

/*Ajax thay đổi*/
$(document).ready(function () {
    // Bắt sự kiện click nút "Thay đổi" trong modal
    $("#EditRoomForm button[type='submit']").click(function (e) {
        // Ngăn chặn hành vi mặc định của nút submit
        e.preventDefault();

        // Use $(this).closest('form') to target the closest form element
        var form = $(this).closest('form');

        // Find input values within the form
        var room_name = form.find("#_room_name__").val();
        var room_capacity = form.find("#capacity_").val();

        // Access the data attribute from the modal
        var roomId = form.closest('.modal').data('room-id');

        // Log values for testing
        console.log(room_name);
        console.log(room_capacity);

        // Gửi Ajax request đến action Edit trong controller
        $.ajax({
            url: "/Admin/rooms/Edit/" + roomId,
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ room_id: roomId, room_name: room_name, capacity: room_capacity }),
            success: function (response) {
                console.log(response);
                if (response.success) {
                    // If the server reports success, close the modal and load rooms
                    $("#EditModal_" + roomId).modal("hide");
                    loadRooms();
                    alert("Thành công");
                } else {
                    // If the server reports failure, display an error message
                    alert("Lỗi: " + response.message);
                }
            }
        });
    });
});
/*Kết thúc*/


