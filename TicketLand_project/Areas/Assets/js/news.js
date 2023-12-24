/*Ajax edit*/
$(document).ready(function () {
    // Intercept the form submission event
    $("form").submit(function (event) {
        // Prevent the default form submission
        event.preventDefault();

        // Serialize the form data
        var formData = $(this).serialize();

        // Perform an Ajax post
        $.ajax({
            url: '@Url.Action("Edit", "news")', // Thay đổi đường dẫn và tên action phù hợp
            type: 'POST',
            data: formData,
            success: function (result) {
                // Xử lý kết quả sau khi post thành công
                alert("Thành công")
                console.log(result);
            },
            error: function (error) {
                // Xử lý lỗi nếu có
                console.log(error);
            }
        });
    });
});


