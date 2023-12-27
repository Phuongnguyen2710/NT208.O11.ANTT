/*Sự kiện thêm mới 1 phim*/
$(document).ready(function () {
    // Bắt sự kiện submit của form
    $("#addmovieForm").submit(function (event) {
        // Ngăn chặn hành vi mặc định của form
        event.preventDefault();
        var form = $(this).closest('form');
        // Lấy dữ liệu form
        var moviename = form.find("#movie_name").val();
        var movieDescription = form.find("#movie_description").val();
        var movieTrailer = form.find("#movie_trailer").val();
        var movieCens = form.find("#movie_cens").val();
        var movieGenres = form.find("#movie_genres").val();
        var movieRelease = form.find("#movie_release").val();
        var movieDuration = form.find("#movie_duration").val();
        var movieFormat = form.find("#movie_format").val();
        var moviePoster = form.find("#movie_poster").val();
        var movieActor = form.find("#movie_actor").val();
        var movieDirector = form.find("#movie_director").val();
        var movieStatus = form.find("#movie_status").val();
        var rate = form.find("#rate").val();
        var movieBanner = form.find("#movie_banner").val();
        console.log(moviename);
        // Gửi Ajax request đến action Create trong controller
        $.ajax({
            url: "/Admin/movies/Create",
            type: "POST",
            data: JSON.stringify({
                movie_name: moviename, movie_description: movieDescription, movie_trailer: movieTrailer, movie_cens: movieCens,
                movie_genres: movieGenres, movie_release: movieRelease, movie_duration: movieDuration, movie_format: movieFormat, movie_poster: moviePoster,
                movie_actor: movieActor, movie_director: movieDirector, movie_status: movieStatus, rate: rate, movie_banner: movieBanner
            }),
            contentType: 'application/json; charset=utf-8',
            success: function (response) {
                // Xử lý kết quả từ server
                if (response.success) {
                    // Đóng modal sau khi lưu thành công
                    alert("Success")
                    window.location.reload();

                } else {
                    // Xử lý khi lưu không thành công (hiển thị thông báo lỗi, v.v.)
                    alert("Lỗi")
                    console.log(response.message);
                }
            },
            error: function (error) {
                // Xử lý lỗi (nếu cần)
                alert(error.message)
                console.log(error);
            }
        });
    });
});
/*Kết thúc*/

/*Ajax Xóa*/
$(document).ready(function () {
    $("#DeleteForm button[type='submit']").click(function () {
        // Lấy giá trị id cần xóa
        var form = $(this).closest('form');
        var movieId = parseInt(form.closest('.modal').data('movie-id'));
        console.log(movieId);
        // Gửi Ajax request
        $.ajax({
            url: '/Admin/movies/Delete/' + movieId,
            type: 'POST',
            data: { id: movieId },
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

/*Ajax thay đổi*/
$(document).ready(function () {
    // Bắt sự kiện click nút "Thay đổi" trong modal
    $("#EditmovieForm button[type='submit']").click(function (e) {
        // Ngăn chặn hành vi mặc định của nút submit
        e.preventDefault();

        // Use $(this).closest('form') to target the closest form element
        var form = $(this).closest('form');

        // Access the data attribute from the modal
        var movieId = form.closest('.modal').data('movie-id');
        // Find input values within the form
        var moviename = form.find("#movie_name").val();
        var movieDescription = form.find("#movie_description").val();
        var movieTrailer = form.find("#movie_trailer").val();
        var movieCens = form.find("#movie_cens").val();
        var movieGenres = form.find("#movie_genres").val();
        var movieRelease = moment(form.find("#movie_release").val()).format("YYYY-MM-DDTHH:mm:ss.SSSSSSS");
        var movieFormat = form.find("#movie_format").val();
        var moviePoster = form.find("#movie_poster").val();
        var movieActor = form.find("#movie_actor").val();
        var movieDirector = form.find("#movie_director").val();
        var movieStatus = parseInt(form.find("#movie_status").val());
        var rate = parseFloat(form.find("#rate").val());
        var movieBanner = form.find("#movie_banner").val();
        var movieDuration = form.find("#movie_duration").val();
        // Log values for testing
        console.log(movieDuration);
/*        console.log(movieRelease);*/

        // Gửi Ajax request đến action Edit trong controller
        $.ajax({
            url: "/Admin/movies/Edit/" + movieId,
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                movie_id: movieId, movie_name: moviename, movie_description: movieDescription, movie_trailer: movieTrailer, movie_cens: movieCens ,
                movie_genres: movieGenres, movie_release: movieRelease, movie_duration: movieDuration, movie_format: movieFormat, movie_poster: moviePoster,
                movie_actor: movieActor, movie_director: movieDirector, movie_status: movieStatus, rate: rate, movie_banner: movieBanner 
            }),
            success: function (response) {
                console.log(response);
                if (response.success) {
                    // If the server reports success, close the modal and load rooms
                    $("#EditModal_" + movieId).modal("hide");
                    alert("Thành công");
                    window.location.reload();
                } else {
                    // If the server reports failure, display an error message
                    alert("Lỗi: " + response.message);
                }
            }
        });
    });
});
/*Kết thúc*/


/*Ajax thêm mới*/
$(document).ready(function () {
    // Bắt sự kiện click nút "Thay đổi" trong modal
    $("#addmovieForm button[type='submit']").click(function (e) {
        // Ngăn chặn hành vi mặc định của nút submit
        e.preventDefault();

        // Use $(this).closest('form') to target the closest form element
        var form = $(this).closest('form');

        // Access the data attribute from the modal
        var movieId = form.closest('.modal').data('movie-id');

        // Create a FormData object to handle file uploads
        var formData = new FormData(form[0]);

        // Gửi Ajax request đến action Edit trong controller
        $.ajax({
            url: "/Admin/movies/Create/",
            type: "POST",
            contentType: false, // không sử dụng contentType khi sử dụng FormData
            processData: false, // không xử lý dữ liệu gửi đi
            data: formData,
            success: function (response) {
                console.log(response);
                if (response.success) {
                    // If the server reports success, close the modal and load rooms
                    $("#addmovieModal").modal("hide");
                    alert("Thành công");
                    window.location.reload();
                } else {
                    // If the server reports failure, display an error message
                    alert("Lỗi: " + response.message);
                }
            }
        });
    });
});
/*Kết thúc*/


function ConvertDateFormat(dateString) {
    var parts = dateString.split("-");
    var formattedDate = parts[2] + "-" + parts[1] + "-" + parts[0];
    return formattedDate;
}

var inputDate = document.getElementById("movie_release");
inputDate.value = ConvertDateFormat(inputDate.value);