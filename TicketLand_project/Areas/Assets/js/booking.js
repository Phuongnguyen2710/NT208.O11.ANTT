function formatCurrency(input) {
    // Lấy giá trị nhập vào và loại bỏ các ký tự không phải số
    var value = input.value.replace(/[^\d]/g, '');

    // Chuyển đổi giá trị thành số tiền
    var formattedValue = parseFloat(value).toLocaleString('vi-VN');

    // Gán giá trị đã được định dạng vào ô input
    input.value = formattedValue + " VND";
}


/* Hàm fitter giá trị lịch chiếu tương ứng movie được chọn */
function filterSchedules() {
    var movieId = document.getElementById("movie_id").value;
    movieId = parseInt(movieId, 10);
    var scheduleDropdown = document.getElementById("schedule_id");

    console.log(movieId);
    console.log(typeof movieId);

    // Xóa tất cả các tùy chọn suất chiếu hiện có
    scheduleDropdown.innerHTML = '<option value="" class="text-center">-- Chọn suất chiếu --</option>';

    if (movieId !== "") {
        // Gọi API hoặc endpoint trên máy chủ để lấy danh sách suất chiếu dựa trên movieId
        fetch(`https://apitikketland.azurewebsites.net/api/schedules?movieId=${movieId}`)
            .then(response => response.json())
            .then(data => {
                // Lọc danh sách suất chiếu để chỉ giữ lại những suất chiếu có movie_id giống với movieId
                var filteredSchedules = data.filter(schedule => schedule.movie_id === movieId);

                // Tạo và thêm các tùy chọn suất chiếu mới dựa trên danh sách lọc được
                filteredSchedules.forEach(schedule => {
                    var option = document.createElement("option");
                    option.value = schedule.schedule_id;
                    option.text = schedule.show_date;
                    scheduleDropdown.appendChild(option);
                });

                console.log(filteredSchedules);
            })
            .catch(error => {
                console.error("Error:", error);
            });
    }
}
/*Kết thúc*/


/*Ajax Xóa*/
$(document).ready(function () {
    $("#DeleteForm button[type='submit']").click(function () {
        // Lấy giá trị id cần xóa
        var form = $(this).closest('form');
        var bookingId = parseInt(form.closest('.modal').data('booking-id'));
        console.log(movieId);
        // Gửi Ajax request
        $.ajax({
            url: '/Admin/bookings/Delete/' + bookingId,
            type: 'POST',
            data: { id: bookingId },
            success: function (result) {
                // Xử lý kết quả sau khi xóa thành công
                console.log(result);

                // Đóng modal nếu xóa thành công
                $('#deleteForm').closest('.modal').modal('hide');
            },
            error: function (error) {
                // Xử lý lỗi nếu có
                console.log(error);
            }
        });
    });
});
/*Kết thúc*/