var max_seats = 0;

// Đặt giá trị mặc định cho DropDownList khi trang được tải
$(document).ready(function () {
    var roomIdFromUrl = new URLSearchParams(window.location.search).get('roomId');

    // Đặt giá trị cho room_Id và kích hoạt sự kiện change
    $("#room_Id").val(roomIdFromUrl);
    $("#room_Capacity").val(roomIdFromUrl);

    // Gửi yêu cầu Ajax để lấy room_capacity từ server
    $.ajax({
        url: '/Admin/seats/GetRoomCapacity',
        type: 'GET',
        data: { roomId: roomIdFromUrl },
        success: function (data) {
            // Cập nhật giá trị cho room_capacity
            $("#room_capacity").val(data);
            initializeSeats(data);      
        },
        error: function () {
            console.error('Failed to fetch room capacity.');
        }
    });
});


// Hàm tạo ghế
function initializeSeats(data) {
    // Lấy thẻ chứa tất cả ghế
    let seats = document.querySelector(".all-seats");

    // Số lượng ghế trong mỗi dãy
    let seatsPerRow = 10;

    // Số lượng dãy
    let numRows = 5;

    // Lặp qua số lượng ghế trong mỗi dãy
    for (let i = 0; i < seatsPerRow * numRows; i++) {
        // Tính toán số hàng (chữ cái)
        let rowLetter = String.fromCharCode('A'.charCodeAt(0) + Math.floor(i / seatsPerRow));

        // Tính toán chữ số cho cột (chữ số)
        let seatNumber = (i % seatsPerRow) + 1;

        // Thêm ghế vào DOM với đánh số theo hàng là A1, A2, A3, ...
        /* VD:  <input type="checkbox" name="tickets" id="A1" /><label for="A1" class="seat"></label> */
        seats.insertAdjacentHTML(
            "beforeend",
            '<input type="checkbox" name="tickets" id="' +
            rowLetter +
            seatNumber +
            '" /><label for="' +
            rowLetter +
            seatNumber +
            '" class="seat"></label>'
        );
    }


    // Lấy tất cả các checkbox
    let checkboxes = document.querySelectorAll('input[name="tickets"]');
    var count_seats = 0;

    var maxSeats = data;
    console.log(data);
    // Thêm sự kiện click cho mỗi checkbox
    checkboxes.forEach(checkbox => {
        checkbox.addEventListener('click', function () {
            // Kiểm tra xem checkbox có được chọn hay không
            if (this.checked) {
                // Kiểm tra xem đã đạt được số lượng ghế tối đa chưa
                if (count_seats < maxSeats) {
                    // Nếu được chọn và số lượng ghế chưa đạt tối đa, thêm class "booked" vào label tương ứng
                    this.nextElementSibling.classList.add('booked');
                    count_seats++;
                } else {
                    // Nếu đã đạt tới số lượng ghế tối đa, không thực hiện thêm/chọn
                    this.checked = false;
                    $("#errorAlert_full_seat").fadeIn().delay(5000).fadeOut();
                }
            } else {
                // Nếu checkbox bị bỏ chọn, loại bỏ class "booked" khỏi label tương ứng và giảm số lượng ghế đã chọn
                this.nextElementSibling.classList.remove('booked');
                count_seats--;
            }
        });
    });
}



// Hàm để lưu thông tin ghế khi nhấn nút SET
function saveSeats() {
    // Tạo một mảng để lưu trữ thông tin về các ghế được chọn
    let selectedSeats = [];

    // Lấy đối tượng phần tử dropdown theo ID (lấy id của phòng)
    let dropdown = document.getElementById("room_Id");

    // Lấy ID của dropdown
    let dropdownId = dropdown.value;
    // Lặp qua tất cả checkbox để xem ghế nào được chọn
    let checkboxes = document.querySelectorAll('input[name="tickets"]:checked');
    checkboxes.forEach(checkbox => {
        // Lấy thông tin về hàng và số ghế từ ID của checkbox
        let seatId = checkbox.id;
        let row = seatId[0];
        let number = seatId.substring(1);
        let status = true;
        let room_id = dropdownId;

        // Thêm thông tin về ghế vào mảng
        selectedSeats.push({ room_id, row, number, status });
    });

    // Hiển thị payload trước khi gửi đi
    console.log('Payload:', selectedSeats);


    // Gửi dữ liệu đến controller bằng Ajax
    $.ajax({
        url: '/Admin/seats/SaveSeats',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(selectedSeats),
        success: function (response) {
            if (response.success) {
                console.log(response.message);
                $("#successAlert").fadeIn().delay(5000).fadeOut();
                // Thực hiện các thao tác khác khi thành công
            } else {
                console.error(response.message);
                $("#errorAlert").fadeIn().delay(5000).fadeOut();
                // Thực hiện các thao tác khác khi có lỗi
            }
        },

    });


    //Hàm load data seat từ controller
    //function loadSeatsData() {
    //    $.ajax({
    //        url: '/Admin/seats/LoadSeatsData_',
    //        type: 'GET',
    //        success: function (data) {
    //            // Xử lý dữ liệu đã tải lên
    //            if (data) {
    //                console.log('Seats data loaded:', data);

    //                // Lặp qua mỗi ghế để set lại cho page
    //                data.seats.forEach(seat => {
    //                    let seatId = `${seat.seatRow}${seat.seatNumber}`;
    //                    let checkbox = document.getElementById(seat.seatId);
    //                    let seatRoomId = document.getElementById(seat.room_id);
    //                    let room_Id = seat.Room.room_id;
    //                    if (seatRoomId === room_Id) {
    //                        if (checkbox && seat.status) {
    //                            checkbox.checked = true;
    //                            checkbox.nextElementSibling.classList.add('booked');
    //                        }
    //                    }
    //                }); 
    //            } else {
    //                console.error('Error loading seats data:', data.message);
    //            }
    //        },
    //        error: function (error) {
    //            console.error('Error loading seats data:', error);
    //        }
    //    });
    }

  
    


